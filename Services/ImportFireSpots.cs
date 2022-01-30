using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using CadeOFogo.Data;
using CadeOFogo.Interfaces;
using CadeOFogo.Models.Inpe;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace CadeOFogo.Services
{
  public class ImportFireSpots : IHostedService, IDisposable
  {
    private readonly ILogger<ImportFireSpots> _logger;
    private Timer _timer;
    private readonly IConfiguration _configuration;
    private readonly IServiceProvider _service;
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly IMapProvider _mapProvider;

    public ImportFireSpots(ILogger<ImportFireSpots> logger,
                           IServiceProvider serviceProvider,
                           IConfiguration configuration,
                           IServiceScopeFactory scopeFactory,
                           IMapProvider mapProvider)
    {
      _logger = logger;
      _service = serviceProvider;
      _configuration = configuration;
      _scopeFactory = scopeFactory;
      _mapProvider = mapProvider;
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
      _logger.LogInformation("Servico de importação de focos iniciado");
      
      _timer = new Timer(ImportarFocosDeCalor, null, TimeSpan.Zero, 
        TimeSpan.FromHours(20));

      return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
      _logger.LogInformation("Servico de importação de focos finalizado");
      _timer?.Change(Timeout.Infinite, 0);
      return Task.CompletedTask;
    }

    public void Dispose()
    {
      _timer?.Dispose();
    }

    private void ImportarFocosDeCalor(object state)
    {
      _logger.LogInformation("Importando focos de calor...");
      var totalDeFocos = 0;
      using var scope = _scopeFactory.CreateScope();
      var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
      var httpClient = new HttpClient();

      var estados = dbContext.Estados.Where(e => e.Monitorado).ToList();
      var satelites = dbContext.Satelites.Where(s => s.Monitorado).ToList();


      // Para cada estado
      string msg;
      foreach (var estado in estados)
      {
        msg = $"Recuperando os focos de {estado.EstadoNome}";
        _logger.LogInformation(msg);

        var uri = new Uri($"http://queimadas.dgi.inpe.br/api/focos/?pais_id=33");
        List<ApiInpeFocos> focosFromInpe;
        try
        {
          var resposta = httpClient.GetStringAsync(uri).Result;
          focosFromInpe = JsonConvert.DeserializeObject<List<ApiInpeFocos>>(resposta);
        }
        catch (Exception e)
        {
          Console.WriteLine(e);
          _logger.LogCritical("Erro no mapeamento do Json dos focos do INPE para a classe ApiInpeFocos");
          throw;
        }

        msg = $"Importando {focosFromInpe.Count} foco para {estado.EstadoNome}";
        _logger.LogInformation(msg);
        
        foreach (var foco in focosFromInpe)
        {
          // Foco ainda nao foi importado
          if (dbContext.Focos.FirstOrDefault(f => f.InpeFocoId == foco.Id) == null)
          {
            var municipio = dbContext.Municipios
              .Include(m => m.Estado)
              .FirstOrDefault(m => m.EstadoId == estado.EstadoId &&
                                   m.MunicipioNome == foco.Properties.Municipio);
            if (municipio == null)
            {
              msg = $"Impossivel importar foco {foco.Id} por falta do municipio {foco.Properties.Municipio} na base local";
              _logger.LogCritical(msg);
              continue;
            }

            if (!municipio.Monitorado)
            {
              msg = $"Descartando foco {foco.Id} no municipio {foco.Properties.Municipio} - municipio nao monitorado";
              _logger.LogDebug(msg);
              continue;
            }

            var satelite = satelites.FirstOrDefault(s =>
              s.SateliteNomeINPE.ToLowerInvariant().Trim() == foco.Properties.Satelite.ToLowerInvariant().Trim());
            if (satelite == null)
            {
              msg = $"Impossivel importar foco {foco.Id} por falta do satelite {foco.Properties.Satelite} na base local";
              _logger.LogCritical(msg);
              continue;
            }

            if (!satelite.Monitorado)
            {
              msg = $"Descartando foco {foco.Id} do satelite {foco.Properties.Satelite} - satelite nao monitorado";
              _logger.LogDebug(msg);
              continue;
            }

            var novoFoco = new Foco
            {
              FocoAtendido = false,
              FocoDataUtc = foco.Properties.DataHoraGmt,
              InpeFocoId = foco.Id,
              FocoLatitude = Convert.ToDecimal(foco.Properties.Latitude),
              FocoLongitude = Convert.ToDecimal(foco.Properties.Longitude),
              Estado = estado,
              Municipio = municipio,
              Satelite = satelite,
              SnapshotSatelite = null,
              DataSnapshot = DateTime.MinValue,
              SnapshotProvider = null,
              CausadorProvavelId = 4,
              CausaFogoId = 1,
              ResponsavelPropriedadeId = 1,
              StatusFocoId = 1,
              IndicioInicioFocoId = 3,
              EquipeId = 1
            };
            
            if (_configuration.GetValue<bool>("PrefetchImages"))
            {
              msg = $"Fazendo prefetch da imagem para o foco {foco.Id} a partir do {_mapProvider.providerName()}";
              _logger.LogInformation(msg);
              novoFoco.SnapshotSatelite = _mapProvider.StaticMap(
                Convert.ToDecimal(foco.Properties.Latitude),
                Convert.ToDecimal(foco.Properties.Longitude)).Result;
              novoFoco.DataSnapshot = DateTime.UtcNow;
              novoFoco.SnapshotProvider = _mapProvider.providerName();
            }

            dbContext.Focos.Add(novoFoco);
            dbContext.SaveChanges();

            // Atualizar na cidade, no estado e no satelite o timestamp do ultimo foco observado
            if (municipio.UltimoFocoObservadoUtc < novoFoco.FocoDataUtc)
              municipio.UltimoFocoObservadoUtc = novoFoco.FocoDataUtc;

            if (estado.UltimoFocoObservadoUtc < novoFoco.FocoDataUtc)
              estado.UltimoFocoObservadoUtc = novoFoco.FocoDataUtc;

            if (satelite.UltimoFocoUtc < novoFoco.FocoDataUtc)
              satelite.UltimoFocoUtc = novoFoco.FocoDataUtc;

            dbContext.SaveChanges();
            msg = $"Cadastrando foco {foco.Id}";
            totalDeFocos = totalDeFocos + 1;
            _logger.LogDebug(msg);
          }
          else
          {
            msg = $"Foco {foco.Id} ja cadastrado";
            _logger.LogDebug(msg);
          }
        }
      }

      msg = $"Total de focos importados: {totalDeFocos.ToString()}";
      _logger.LogInformation(msg);
      _logger.LogInformation("Aguardando próximo ciclo de importação de focos");
    }
  }
}