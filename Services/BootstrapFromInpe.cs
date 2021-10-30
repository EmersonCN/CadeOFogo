using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CadeOFogo.Data;
using CadeOFogo.Models.Inpe;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace CadeOFogo.Services
{
  public class BootstrapFromInpe : IHostedService, IDisposable
  {
    private readonly IConfiguration _configuration;
    private readonly ILogger<BootstrapFromInpe> _logger;
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly IServiceProvider _service;

    public BootstrapFromInpe(ILogger<BootstrapFromInpe> logger,
      IServiceProvider serviceProvider,
      IConfiguration configuration,
      IServiceScopeFactory scopeFactory)
    {
      _logger = logger;
      _service = serviceProvider;
      _configuration = configuration;
      _scopeFactory = scopeFactory;
    }

    public void Dispose()
    {
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
      string msg;

      _logger.LogInformation("Iniciando bootstrap a partir das bases do INPE");

      // https://stackoverflow.com/questions/48368634/how-should-i-inject-a-dbcontext-instance-into-an-ihostedservice
      using var scope = _scopeFactory.CreateScope();
      var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

      _logger.LogDebug("- Importando base de satélites");
      var satelitesInpe = Satelite.AtualizaSatelites();
      foreach (var sateliteInpe in satelitesInpe)
        if (context.Satelites
          .FirstOrDefault(s => s.SateliteNomeINPE == sateliteInpe.Satelite) == null)
          context.Satelites.Add(
            new Satelite
            {
              SateliteNome = sateliteInpe.Satelite,
              SateliteNomeINPE = sateliteInpe.Satelite,
              Monitorado = true,
              UltimoFocoUtc = DateTime.MinValue
            });

      msg = $"Foram importados {satelitesInpe.Count} satélites";
      _logger.LogDebug(msg);
      context.SaveChanges();

      _logger.LogDebug("- Importando base de estados");
      var estadosInpe = Estado.AtualizaEstados();
      foreach (var estadoInpe in estadosInpe)
        if (context.Estados
          .FirstOrDefault(s => s.EstadoIdInpe == estadoInpe.EstadoId) == null)
        {
          msg = $"Adicionando estado {estadoInpe.EstadoName}";
          _logger.LogDebug(msg);
          var estado = new Estado
          {
            EstadoNome = estadoInpe.EstadoName,
            EstadoIdInpe = estadoInpe.EstadoId,
            Monitorado = true,
            UltimoFocoObservadoUtc = DateTime.MinValue
          };
          context.Estados.Add(estado);
          context.SaveChanges();

          var municipiosInpe = Municipio.AtualizaMunicipios(estado);
          foreach (var municipioInpe in municipiosInpe)
            if (context.Municipios
              .FirstOrDefault(s => s.MunicioIbgeId == municipioInpe.MunicipioId) == null)
              context.Municipios.Add(new Municipio
              {
                Estado = estado,
                MunicioIbgeId = municipioInpe.MunicipioId,
                MunicipioNome = municipioInpe.MunicipioName,
                Monitorado = true,
                UltimoFocoObservadoUtc = DateTime.MinValue
              });
          msg = $"Adicionados {municipiosInpe.Count} municipios no estado {estado.EstadoNome}";
          _logger.LogDebug(msg);
          context.SaveChanges();
        }

      return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
      _logger.LogInformation("Bootstrap a partir das bases do INPE concluído");
      return Task.CompletedTask;
    }
  }
}