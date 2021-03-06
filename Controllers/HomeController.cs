using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using CadeOFogo.Data;
using CadeOFogo.Enums;
using CadeOFogo.Interfaces;
using CadeOFogo.Models.Inpe;
using CadeOFogo.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Rotativa;
using SelectPdf;
using X.PagedList;

namespace CadeOFogo.Controllers
{
    public class HomeController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly ApplicationDbContext _context;
        private readonly ILogger<HomeController> _logger;
        private readonly IMapProvider _mapProvider;

        private readonly NumberFormatInfo _nfi = new() { NumberDecimalSeparator = ".", NumberDecimalDigits = 5 };
        private readonly int _pagesize;

        public HomeController(ILogger<HomeController> logger, ApplicationDbContext context, IConfiguration configuration,
          IMapProvider mapProvider)
        {
            _logger = logger;
            _context = context;
            _pagesize = Convert.ToInt32(configuration
              .GetSection("ViewOptions")
              .GetSection("PageSizeCadastro")
              .Value);
            _pagesize = _pagesize != 0 ? _pagesize : 20;
            _configuration = configuration;
            _mapProvider = mapProvider;
        }

        public IActionResult Index()
        {
            ViewBag.TotalFocos = _context.Focos.Count(f => f.FocoDataUtc >= DateTime.UtcNow.AddDays(-2));
            ViewBag.mapa = _mapProvider.DynamicSpotsMap("/Home/Focos48H");

            return View();
        }

        public async Task<JsonResult> Focos48H()
        {
            var focos = await _context.Focos
              .Include(f => f.Municipio)
              .Include(f => f.Estado)
              .Include(f => f.Satelite)
              .Where(f => f.FocoDataUtc >= DateTime.UtcNow.AddDays(-2) && f.MunicipioId != 3802).ToListAsync();

            var resposta = new JsonFocos48ViewModel
            {
                Type = "FeatureCollection",
                Features = focos.Select(foco => new Foco48hViewModel
                {
                    Type = "Feature",
                    Id = foco.FocoId.ToString(),
                    Properties = new Foco48hViewModelProperties
                    {
                        Resumo = $"<p>Foco detectado em {foco.FocoDataUtc:s} " +
                             $"pelo satélite {foco.Satelite.SateliteNome}.<br /><br />" +
                             $"Localização {foco.Coordenadas} ({foco.Municipio.MunicipioNome}).</p>"
                    },
                    Geometry = new ApiInpeFocosGeometry
                    {
                        Type = "Point",
                        Coordinates = new List<decimal>
            {
              foco.FocoLongitude,
              foco.FocoLatitude
            }
                    }
                }).ToList()
            };
            return Json(resposta);
        }

        public async Task<IActionResult> Relatorio(string dataInicio, string dataFinal, bool atendido,
          int? equipe, int? page)
        {
            var provider = CultureInfo.InvariantCulture;

            IQueryable<Foco> dataset = _context.Focos
              .Include(f => f.Municipio)
              .ThenInclude(m => m.Estado)
              .Include(x => x.Equipe)
               .Include(s => s.Satelite)
              .OrderByDescending(f => f.FocoDataUtc)
              .ThenBy(f => f.Municipio.MunicipioNome);

            var inicio = DateTime.UtcNow.AddDays(-2);
            var final = DateTime.UtcNow;

            if (equipe.HasValue && // ha um estado selecionado
                equipe != 0 && // nao eh zero
                await _context.Equipes.AnyAsync(e => e.EquipeId == equipe)) // existe na base de estados
            {
                dataset = dataset.Where(e => e.EquipeId == equipe); // aplica o filtro
                ViewBag.equipe = equipe;
            }
            else
            {
                ViewBag.equipe = "";
            }
            /*
            if (atendido == true)
            {
                dataset = dataset.Where(e => e.FocoAtendido == true); // aplica o filtro
                ViewBag.atendido = atendido;
            }
            else
            {
                ViewBag.atendido = atendido;
            }
            */
            if (!string.IsNullOrEmpty(dataInicio))
                try
                {
                    inicio = DateTime.ParseExact(dataInicio, "yyyy-MM-ddTHH:mm:ss", provider);
                }
                catch (FormatException)
                {
                    inicio = DateTime.UtcNow.AddDays(-2);
                }

            if (!string.IsNullOrEmpty(dataFinal))
                try
                {
                    final = DateTime.ParseExact(dataFinal, "yyyy-MM-ddTHH:mm:ss", provider);
                }
                catch (FormatException)
                {
                    final = DateTime.UtcNow;
                }

            dataset = dataset.Where(f => (f.FocoDataUtc >= inicio) & (f.FocoDataUtc <= final));


            var data = await dataset.ToPagedListAsync(page ?? 1, _pagesize);

            ViewBag.primeiro = data.FirstItemOnPage;
            ViewBag.ultimo = data.LastItemOnPage;
            ViewBag.total = data.TotalItemCount;

            ViewBag.dataInicio = inicio.ToString("s");
            ViewBag.dataFinal = final.ToString("s");
            ViewBag.atendido = atendido;
            ViewBag.equipeInputSelect = new SelectList(await _context.Equipes
                .OrderBy(s => s.EquipeNome)
                .ToListAsync(),
              dataValueField: "EquipeId",
              dataTextField: "EquipeNome",
              selectedValue: ViewBag.equipeInputSelect);


            return View(data);
        }


        public async Task<IActionResult> ListaFocos(string dataInicio, string dataFinal, string municipio, int? satelite,
          int? estado, int? page)
        {
            var provider = CultureInfo.InvariantCulture;

            IQueryable<Foco> dataset = _context.Focos
              .Include(f => f.Municipio)
              .ThenInclude(m => m.Estado)
              .Include(s => s.Satelite)
              .OrderByDescending(f => f.FocoDataUtc)
              .ThenBy(f => f.Municipio.MunicipioNome);

            var inicio = DateTime.UtcNow.AddDays(-2);
            var final = DateTime.UtcNow;

            if (satelite.HasValue && // ha um satelite selecionado
                satelite != 0 && // nao eh zero
                await _context.Satelites.AnyAsync(s => s.SateliteId == satelite)) // existe na base de satelites
            {
                dataset = dataset.Where(s => s.SateliteId == satelite); // aplica o filtro
                ViewBag.satelite = satelite;
            }
            else
            {
                ViewBag.satelite = 0;
            }

            if (estado.HasValue && // ha um estado selecionado
                estado != 0 && // nao eh zero
                await _context.Estados.AnyAsync(e => e.EstadoId == estado)) // existe na base de estados
            {
                dataset = dataset.Where(e => e.EstadoId == estado); // aplica o filtro
                ViewBag.estado = estado;
            }
            else
            {
                ViewBag.estado = 0;
            }

            if (!string.IsNullOrEmpty(dataInicio))
                try
                {
                    inicio = DateTime.ParseExact(dataInicio, "yyyy-MM-ddTHH:mm:ss", provider);
                }
                catch (FormatException)
                {
                    inicio = DateTime.UtcNow.AddDays(-2);
                }

            if (!string.IsNullOrEmpty(dataFinal))
                try
                {
                    final = DateTime.ParseExact(dataFinal, "yyyy-MM-ddTHH:mm:ss", provider);
                }
                catch (FormatException)
                {
                    final = DateTime.UtcNow;
                }

            dataset = dataset.Where(f => (f.FocoDataUtc >= inicio) & (f.FocoDataUtc <= final));

            if (!string.IsNullOrEmpty(municipio))
            {
                dataset = dataset.Where(f => f.Municipio.MunicipioNome.Contains(municipio));
                ViewBag.municipio = municipio;
            }
            else
            {
                ViewBag.municipio = "";
            }

            var data = await dataset.ToPagedListAsync(page ?? 1, _pagesize);

            ViewBag.primeiro = data.FirstItemOnPage;
            ViewBag.ultimo = data.LastItemOnPage;
            ViewBag.total = data.TotalItemCount;

            ViewBag.dataInicio = inicio.ToString("s");
            ViewBag.dataFinal = final.ToString("s");
            ViewBag.SatelitesInputSelect = new SelectList(await _context.Satelites
                .OrderBy(s => s.SateliteNomeINPE)
                .Where(s => s.UltimoFocoUtc > DateTime.MinValue)
                .ToListAsync(),
              dataValueField: "SateliteId",
              dataTextField: "SateliteNome",
              selectedValue: ViewBag.satelite);
            ViewBag.EstadosInputSelect = new SelectList(await _context.Estados
                .OrderBy(e => e.EstadoNome)
                .Where(e => e.UltimoFocoObservadoUtc > DateTime.MinValue)
                .ToListAsync(),
              dataValueField: "EstadoId",
              dataTextField: "EstadoNome",
              selectedValue: ViewBag.estado);

            return View(data);
        }


        public async Task<IActionResult> GerarRelatorio(int? id)
        {
            if (id == null) return NotFound();

            var foco = await _context.Focos
              .Include(f => f.Estado)
              .Include(f => f.Municipio)
              .Include(f => f.Satelite)
              .Include(f => f.StatusDoFoco)
              .Include(f => f.CausadorProvavel)
              .Include(f => f.IndicioInicioFoco)
              .Include(f => f.CausaFogo)
              .Include(f => f.ResponsavelPropriedade)
              .Include(f => f.Equipe)
              .FirstOrDefaultAsync(f => f.FocoId == id);
            if (foco == null) return NotFound();

            var detalheFocoViewModel = new DetalheFocoViewModel
            {
                FocoId = foco.FocoId,
                FocoDataUtc = foco.FocoDataUtc,
                Coordenadas = foco.Coordenadas,
                FocoAtendido = foco.FocoAtendido,
                FocoConfirmado = foco.FocoConfirmado,
                FocoIdInpe = foco.InpeFocoId,
                //SnapshotSatelite = foco.SnapshotSatelite,
                Satelite = foco.Satelite.SateliteNome,
                Localidade = $"{foco.Municipio.MunicipioNome}, {foco.Estado.EstadoNome}",
                Bioma = foco.Bioma,
                Municipi = foco.Municipi,
                PolicialResponsavel = foco.PolicialResponsavel,
                OcorrênciaSIOPM = foco.OcorrênciaSIOPM,
                NºBOPAmb = foco.NºBOPAmb,
                NºTVA = foco.NºTVA,
                RSO = foco.RSO,
                DataAtendimento = foco.DataAtendimento,
                EquipeNome = foco.Equipe.EquipeNome,
                StatusFocoDescricao = foco.StatusDoFoco.StatusFocoDescricao,
                IndicioInicioFocoDescricao = foco.IndicioInicioFoco.IndicioInicioFocoDescricao,
                CausaFogoDescricao = foco.CausaFogo.CausaFogoDescricao,
                CausadorProvavelDescricacao = foco.CausadorProvavel.CausadorProvavelDescricacao,
                ResponsavelPropriedadeDescricao = foco.ResponsavelPropriedade.ResponsavelPropriedadeDescricao,
                PioneiroAPPAreaEmHectares = foco.PioneiroAPPAreaEmHectares,
                InicialAPPAreaEmHectares = foco.InicialAPPAreaEmHectares,
                MedioAPPAreaEmHectares = foco.MedioAPPAreaEmHectares,
                AvancadoAPPAreaEmHectares = foco.AvancadoAPPAreaEmHectares,
                AutoDeInflacaoAmbientalAPP = foco.AutoDeInflacaoAmbientalAPP,
                MultaAPP = foco.MultaAPP,
                Pioneiro = foco.Pioneiro,
                Inicial = foco.Inicial,
                Medio = foco.Medio,
                Avancado = foco.Avancado,
                AutoDeInflacaoAmbiental = foco.AutoDeInflacaoAmbiental,
                MultaR = foco.MultaR,
                Pasto = foco.Pasto,
                Citrus = foco.Citrus,
                Outras = foco.Outras,
                AutoDeInflacaoAmbientalV = foco.AutoDeInflacaoAmbientalV,
                MultaV = foco.MultaV,
                ArvoresIsoladas = foco.ArvoresIsoladas,
                AutoDeInflacaoAmbientalA = foco.AutoDeInflacaoAmbientalA,
                MultaA = foco.MultaA,
                PalhaDeCana = foco.PalhaDeCana,
                CanaDeAcucar = foco.CanaDeAcucar,
                Autorizado = foco.Autorizado,
                AutoDeInflacaoAmbientalL = foco.AutoDeInflacaoAmbientalL,
                MultaL = foco.MultaL,
                PioneiroUC = foco.PioneiroUC,
                InicialUC = foco.InicialUC,
                MedioUC = foco.MedioUC,
                AvancadoUC = foco.AvancadoUC,
                OutrasUC = foco.OutrasUC,
                AutoDeInflacaoAmbientalUC = foco.AutoDeInflacaoAmbientalUC,
                MultaUC = foco.MultaUC,
                PioneiroRL = foco.PioneiroRL,
                InicialRL = foco.InicialRL,
                MedioRL = foco.MedioRL,
                AvancadoRL = foco.AvancadoRL,
                OutrasRL = foco.OutrasRL,
                AutoDeInflacaoAmbientalRL = foco.AutoDeInflacaoAmbientalRL,
                MultaRL = foco.MultaRL,
                Refiscalizacao = foco.Refiscalizacao

            };

            ViewBag.TemReverseGeocode = false;
            var reverseGeocode = _mapProvider.ReverseGeocode(foco.FocoLatitude, foco.FocoLongitude);
            if (reverseGeocode.Status == ReverseGeocodeStatus.OK)
            {
                detalheFocoViewModel.Attribution = reverseGeocode.Attribution;
                detalheFocoViewModel.ReverseGeocode = reverseGeocode.Endereco;
                ViewBag.TemReverseGeocode = true;
            }
            else
            {
                var msg = $"Erro no reverse geocode / latlong: {foco.FocoLatitude.ToString(_nfi)}, {foco.FocoLongitude.ToString(_nfi)} / ";
                msg += $"resposta: {reverseGeocode.Status} / {_mapProvider.providerName()}";
                _logger.LogWarning(msg);
            }

            ViewBag.mapa = _mapProvider.DynamicSingleSpotMap(foco.FocoLatitude, foco.FocoLongitude, "map");

            return View(detalheFocoViewModel);
        }

        /*public ViewAsPdf VisualizarComoPDF()
        {
            return new ViewAsPdf("GerarRelatorio");
            
        }*/

        public ViewAsPdf VisualizarComoPDF()
        {
            return new ViewAsPdf("GerarRelatorio")
            {
                PageSize = Rotativa.Options.Size.A4,
            };
        }

        public async Task<IActionResult> GerarMultiplosRelatorios(int? id)
        {
            if (id == null) return NotFound();

            var foco = await _context.Focos
              .Include(f => f.Estado)
              .Include(f => f.Municipio)
              .Include(f => f.Satelite)
              .Include(f => f.StatusDoFoco)
              .Include(f => f.CausadorProvavel)
              .Include(f => f.IndicioInicioFoco)
              .Include(f => f.CausaFogo)
              .Include(f => f.ResponsavelPropriedade)
              .Include(f => f.Equipe)
              .FirstOrDefaultAsync();
            if (foco == null) return NotFound();

            var detalheFocoViewModel = new DetalheFocoViewModel
            {
                FocoId = foco.FocoId,
                FocoDataUtc = foco.FocoDataUtc,
                Coordenadas = foco.Coordenadas,
                FocoAtendido = foco.FocoAtendido,
                FocoConfirmado = foco.FocoConfirmado,
                FocoIdInpe = foco.InpeFocoId,
                //SnapshotSatelite = foco.SnapshotSatelite,
                Satelite = foco.Satelite.SateliteNome,
                Localidade = $"{foco.Municipio.MunicipioNome}, {foco.Estado.EstadoNome}",
                Bioma = foco.Bioma,
                Municipi = foco.Municipi,
                PolicialResponsavel = foco.PolicialResponsavel,
                OcorrênciaSIOPM = foco.OcorrênciaSIOPM,
                NºBOPAmb = foco.NºBOPAmb,
                NºTVA = foco.NºTVA,
                RSO = foco.RSO,
                DataAtendimento = foco.DataAtendimento,
                EquipeNome = foco.Equipe.EquipeNome,
                StatusFocoDescricao = foco.StatusDoFoco.StatusFocoDescricao,
                IndicioInicioFocoDescricao = foco.IndicioInicioFoco.IndicioInicioFocoDescricao,
                CausaFogoDescricao = foco.CausaFogo.CausaFogoDescricao,
                CausadorProvavelDescricacao = foco.CausadorProvavel.CausadorProvavelDescricacao,
                ResponsavelPropriedadeDescricao = foco.ResponsavelPropriedade.ResponsavelPropriedadeDescricao,
                PioneiroAPPAreaEmHectares = foco.PioneiroAPPAreaEmHectares,
                InicialAPPAreaEmHectares = foco.InicialAPPAreaEmHectares,
                MedioAPPAreaEmHectares = foco.MedioAPPAreaEmHectares,
                AvancadoAPPAreaEmHectares = foco.AvancadoAPPAreaEmHectares,
                AutoDeInflacaoAmbientalAPP = foco.AutoDeInflacaoAmbientalAPP,
                MultaAPP = foco.MultaAPP,
                Pioneiro = foco.Pioneiro,
                Inicial = foco.Inicial,
                Medio = foco.Medio,
                Avancado = foco.Avancado,
                AutoDeInflacaoAmbiental = foco.AutoDeInflacaoAmbiental,
                MultaR = foco.MultaR,
                Pasto = foco.Pasto,
                Citrus = foco.Citrus,
                Outras = foco.Outras,
                AutoDeInflacaoAmbientalV = foco.AutoDeInflacaoAmbientalV,
                MultaV = foco.MultaV,
                ArvoresIsoladas = foco.ArvoresIsoladas,
                AutoDeInflacaoAmbientalA = foco.AutoDeInflacaoAmbientalA,
                MultaA = foco.MultaA,
                PalhaDeCana = foco.PalhaDeCana,
                CanaDeAcucar = foco.CanaDeAcucar,
                Autorizado = foco.Autorizado,
                AutoDeInflacaoAmbientalL = foco.AutoDeInflacaoAmbientalL,
                MultaL = foco.MultaL,
                PioneiroUC = foco.PioneiroUC,
                InicialUC = foco.InicialUC,
                MedioUC = foco.MedioUC,
                AvancadoUC = foco.AvancadoUC,
                OutrasUC = foco.OutrasUC,
                AutoDeInflacaoAmbientalUC = foco.AutoDeInflacaoAmbientalUC,
                MultaUC = foco.MultaUC,
                PioneiroRL = foco.PioneiroRL,
                InicialRL = foco.InicialRL,
                MedioRL = foco.MedioRL,
                AvancadoRL = foco.AvancadoRL,
                OutrasRL = foco.OutrasRL,
                AutoDeInflacaoAmbientalRL = foco.AutoDeInflacaoAmbientalRL,
                MultaRL = foco.MultaRL,
                Refiscalizacao = foco.Refiscalizacao

            };

            ViewBag.TemReverseGeocode = false;
            var reverseGeocode = _mapProvider.ReverseGeocode(foco.FocoLatitude, foco.FocoLongitude);
            if (reverseGeocode.Status == ReverseGeocodeStatus.OK)
            {
                detalheFocoViewModel.Attribution = reverseGeocode.Attribution;
                detalheFocoViewModel.ReverseGeocode = reverseGeocode.Endereco;
                ViewBag.TemReverseGeocode = true;
            }
            else
            {
                var msg = $"Erro no reverse geocode / latlong: {foco.FocoLatitude.ToString(_nfi)}, {foco.FocoLongitude.ToString(_nfi)} / ";
                msg += $"resposta: {reverseGeocode.Status} / {_mapProvider.providerName()}";
                _logger.LogWarning(msg);
            }

            ViewBag.mapa = _mapProvider.DynamicSingleSpotMap(foco.FocoLatitude, foco.FocoLongitude, "map");

            return View(detalheFocoViewModel);
        }

        public async Task<IActionResult> Detalhes(int? id)
        {
            if (id == null) return NotFound();

            var foco = await _context.Focos
              .Include(f => f.Estado)
              .Include(f => f.Municipio)
              .Include(f => f.Satelite)
              .Include(f => f.StatusDoFoco)
              .Include(f => f.CausadorProvavel)
              .Include(f => f.IndicioInicioFoco)
              .Include(f => f.CausaFogo)
              .Include(f => f.ResponsavelPropriedade)
              .Include(f => f.TipoVegetacao)
              .Include(f => f.Equipe)
              .FirstOrDefaultAsync(f => f.FocoId == id);
            if (foco == null) return NotFound();

            var detalheFocoViewModel = new DetalheFocoViewModel
            {
                FocoId = foco.FocoId,
                FocoDataUtc = foco.FocoDataUtc,
                Coordenadas = foco.Coordenadas,
                FocoAtendido = foco.FocoAtendido,
                FocoConfirmado = foco.FocoConfirmado,
                FocoIdInpe = foco.InpeFocoId,
                //SnapshotSatelite = foco.SnapshotSatelite,
                Satelite = foco.Satelite.SateliteNome,
                Localidade = $"{foco.Municipio.MunicipioNome}, {foco.Estado.EstadoNome}",
                Bioma = foco.Bioma,
                Municipi = foco.Municipi,
                PolicialResponsavel = foco.PolicialResponsavel,
                OcorrênciaSIOPM = foco.OcorrênciaSIOPM,
                NºBOPAmb = foco.NºBOPAmb,
                NºTVA = foco.NºTVA,
                RSO = foco.RSO,
                DataAtendimento = foco.DataAtendimento,
                EquipeNome = foco.Equipe.EquipeNome,
                StatusFocoDescricao = foco.StatusDoFoco.StatusFocoDescricao,
                IndicioInicioFocoDescricao = foco.IndicioInicioFoco.IndicioInicioFocoDescricao,
                CausaFogoDescricao = foco.CausaFogo.CausaFogoDescricao,
                CausadorProvavelDescricacao = foco.CausadorProvavel.CausadorProvavelDescricacao,
                ResponsavelPropriedadeDescricao = foco.ResponsavelPropriedade.ResponsavelPropriedadeDescricao,
                TipoVegetacaoDescricao = foco.TipoVegetacao.TipoVegetacaoDescricao,
                PioneiroAPPAreaEmHectares = foco.PioneiroAPPAreaEmHectares,
                InicialAPPAreaEmHectares = foco.InicialAPPAreaEmHectares,
                MedioAPPAreaEmHectares = foco.MedioAPPAreaEmHectares,
                AvancadoAPPAreaEmHectares = foco.AvancadoAPPAreaEmHectares,
                AutoDeInflacaoAmbientalAPP = foco.AutoDeInflacaoAmbientalAPP,
                MultaAPP = foco.MultaAPP,
                Pioneiro = foco.Pioneiro,
                Inicial = foco.Inicial,
                Medio = foco.Medio,
                Avancado = foco.Avancado,
                AutoDeInflacaoAmbiental = foco.AutoDeInflacaoAmbiental,
                MultaR = foco.MultaR,
                Pasto = foco.Pasto,
                Citrus = foco.Citrus,
                Outras = foco.Outras,
                AutoDeInflacaoAmbientalV = foco.AutoDeInflacaoAmbientalV,
                MultaV = foco.MultaV,
                ArvoresIsoladas = foco.ArvoresIsoladas,
                AutoDeInflacaoAmbientalA = foco.AutoDeInflacaoAmbientalA,
                MultaA = foco.MultaA,
                PalhaDeCana = foco.PalhaDeCana,
                CanaDeAcucar = foco.CanaDeAcucar,
                Autorizado = foco.Autorizado,
                AutoDeInflacaoAmbientalL = foco.AutoDeInflacaoAmbientalL,
                MultaL = foco.MultaL,
                PioneiroUC = foco.PioneiroUC,
                InicialUC = foco.InicialUC,
                MedioUC = foco.MedioUC,
                AvancadoUC = foco.AvancadoUC,
                OutrasUC = foco.OutrasUC,
                AutoDeInflacaoAmbientalUC = foco.AutoDeInflacaoAmbientalUC,
                MultaUC = foco.MultaUC,
                PioneiroRL = foco.PioneiroRL,
                InicialRL = foco.InicialRL,
                MedioRL = foco.MedioRL,
                AvancadoRL = foco.AvancadoRL,
                OutrasRL = foco.OutrasRL,
                AutoDeInflacaoAmbientalRL = foco.AutoDeInflacaoAmbientalRL,
                MultaRL = foco.MultaRL,
                Refiscalizacao = foco.Refiscalizacao

            };

            ViewBag.TemReverseGeocode = false;
            var reverseGeocode = _mapProvider.ReverseGeocode(foco.FocoLatitude, foco.FocoLongitude);
            if (reverseGeocode.Status == ReverseGeocodeStatus.OK)
            {
                detalheFocoViewModel.Attribution = reverseGeocode.Attribution;
                detalheFocoViewModel.ReverseGeocode = reverseGeocode.Endereco;
                ViewBag.TemReverseGeocode = true;
            }
            else
            {
                var msg = $"Erro no reverse geocode / latlong: {foco.FocoLatitude.ToString(_nfi)}, {foco.FocoLongitude.ToString(_nfi)} / ";
                msg += $"resposta: {reverseGeocode.Status} / {_mapProvider.providerName()}";
                _logger.LogWarning(msg);
            }

            ViewBag.mapa = _mapProvider.DynamicSingleSpotMap(foco.FocoLatitude, foco.FocoLongitude, "map");

            return View(detalheFocoViewModel);
        }

        public async Task<IActionResult> AdicionarDetalhes(int? id)
        {
            if (id == null) return NotFound();

            var foco = await _context.Focos
              .Include(f => f.Estado)
              .Include(f => f.Municipio)
              .Include(f => f.Satelite)
              .Include(f => f.StatusDoFoco)
              .Include(f => f.CausadorProvavel)
              .Include(f => f.IndicioInicioFoco)
              .Include(f => f.CausaFogo)
              .Include(f => f.ResponsavelPropriedade)
              .Include(f => f.TipoVegetacao)
              .Include(f => f.Equipe)
              .FirstOrDefaultAsync(f => f.FocoId == id);
            if (foco == null) return NotFound();

            var detalheFocoViewModel = new EditFocoViewModel
            {
                FocoId = foco.FocoId,
                FocoAtendido = foco.FocoAtendido,
                FocoConfirmado = foco.FocoConfirmado,
                FocoIdInpe = foco.InpeFocoId,
                Bioma = foco.Bioma,
                Municipi = foco.Municipi,
                PolicialResponsavel = foco.PolicialResponsavel,
                OcorrênciaSIOPM = foco.OcorrênciaSIOPM,
                NºBOPAmb = foco.NºBOPAmb,
                NºTVA = foco.NºTVA,
                RSO = foco.RSO,
                DataAtendimento = foco.DataAtendimento,
                EquipeId = foco.EquipeId,
                Equipe = new SelectList(await _context.Equipes
                  .OrderBy(c => c.EquipeNome).ToListAsync(),
                      dataValueField: "EquipeId",
                     dataTextField: "EquipeNome"),
                StatusFocoId = foco.StatusFocoId,
                StatusDoFoco = new SelectList(await _context.StatusFocos
                  .OrderBy(c => c.StatusFocoDescricao).ToListAsync(),
                      dataValueField: "StatusFocoId",
                     dataTextField: "StatusFocoDescricao"),
                IndicioInicioFocoId = foco.IndicioInicioFocoId,
                IndicioDeInicioDoFoco = new SelectList(await _context.IndiciosInicioFoco
                    .OrderBy(c => c.IndicioInicioFocoDescricao).ToListAsync(),
                      dataValueField: "IndicioInicioFocoId",
                     dataTextField: "IndicioInicioFocoDescricao"),
                CausaFogoId = foco.CausaFogoId,
                CausaFogo = new SelectList(await _context.CausasFogo
                    .OrderBy(c => c.CausaFogoDescricao).ToListAsync(),
                      dataValueField: "CausaFogoId",
                     dataTextField: "CausaFogoDescricao"),
                CausadorProvavelId = foco.CausadorProvavelId,
                CausadorProvavel = new SelectList(await _context.CausadoresProvaveis
                    .OrderBy(c => c.CausadorProvavelDescricacao).ToListAsync(),
                      dataValueField: "CausadorProvavelId",
                     dataTextField: "CausadorProvavelDescricacao"),
                ResponsavelPropriedadeId = foco.ResponsavelPropriedadeId,
                ResponsavelPelaPropriedade = new SelectList(await _context.ResponsaveisPropriedade
                    .OrderBy(c => c.ResponsavelPropriedadeDescricao).ToListAsync(),
                      dataValueField: "ResponsavelPropriedadeId",
                     dataTextField: "ResponsavelPropriedadeDescricao"),
                TipoVegetacaoId = foco.TipoVegetacaoId,
                TipoVegetacao = new SelectList(await _context.TiposVegetacao
                    .OrderBy(c => c.TipoVegetacaoDescricao).ToListAsync(),
                      dataValueField: "TipoVegetacaoId",
                     dataTextField: "TipoVegetacaoDescricao"),
                PioneiroAPPAreaEmHectares = foco.PioneiroAPPAreaEmHectares,
                InicialAPPAreaEmHectares = foco.InicialAPPAreaEmHectares,
                MedioAPPAreaEmHectares = foco.MedioAPPAreaEmHectares,
                AvancadoAPPAreaEmHectares = foco.AvancadoAPPAreaEmHectares,
                AutoDeInflacaoAmbientalAPP = foco.AutoDeInflacaoAmbientalAPP,
                MultaAPP = foco.MultaAPP,
                Pioneiro = foco.Pioneiro,
                Inicial = foco.Inicial,
                Medio = foco.Medio,
                Avancado = foco.Avancado,
                AutoDeInflacaoAmbiental = foco.AutoDeInflacaoAmbiental,
                MultaR = foco.MultaR,
                Pasto = foco.Pasto,
                Citrus = foco.Citrus,
                Outras = foco.Outras,
                AutoDeInflacaoAmbientalV = foco.AutoDeInflacaoAmbientalV,
                MultaV = foco.MultaV,
                ArvoresIsoladas = foco.ArvoresIsoladas,
                AutoDeInflacaoAmbientalA = foco.AutoDeInflacaoAmbientalA,
                MultaA = foco.MultaA,
                PalhaDeCana = foco.PalhaDeCana,
                CanaDeAcucar = foco.CanaDeAcucar,
                Autorizado = foco.Autorizado,
                AutoDeInflacaoAmbientalL = foco.AutoDeInflacaoAmbientalL,
                MultaL = foco.MultaL,
                PioneiroUC = foco.PioneiroUC,
                InicialUC = foco.InicialUC,
                MedioUC = foco.MedioUC,
                AvancadoUC = foco.AvancadoUC,
                OutrasUC = foco.OutrasUC,
                AutoDeInflacaoAmbientalUC = foco.AutoDeInflacaoAmbientalUC,
                MultaUC = foco.MultaUC,
                PioneiroRL = foco.PioneiroRL,
                InicialRL = foco.InicialRL,
                MedioRL = foco.MedioRL,
                AvancadoRL = foco.AvancadoRL,
                OutrasRL = foco.OutrasRL,
                AutoDeInflacaoAmbientalRL = foco.AutoDeInflacaoAmbientalRL,
                MultaRL = foco.MultaRL,
                Refiscalizacao = foco.Refiscalizacao
            };

            ViewBag.TemReverseGeocode = false;
            var reverseGeocode = _mapProvider.ReverseGeocode(foco.FocoLatitude, foco.FocoLongitude);
            if (reverseGeocode.Status == ReverseGeocodeStatus.OK)
            {
                detalheFocoViewModel.Attribution = reverseGeocode.Attribution;
                detalheFocoViewModel.ReverseGeocode = reverseGeocode.Endereco;
                ViewBag.TemReverseGeocode = true;
            }
            else
            {
                var msg = $"Erro no reverse geocode / latlong: {foco.FocoLatitude.ToString(_nfi)}, {foco.FocoLongitude.ToString(_nfi)} / ";
                msg += $"resposta: {reverseGeocode.Status} / {_mapProvider.providerName()}";
                _logger.LogWarning(msg);
            }

            ViewBag.mapa = _mapProvider.DynamicSingleSpotMap(foco.FocoLatitude, foco.FocoLongitude, "map");

            return View(detalheFocoViewModel);
        }

        [HttpPost, ActionName("AdicionarDetalhes")]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> AdicionarDetalhesConfirm(int? id, EditFocoViewModel foco)
        {
            if ((id == null) || (id != foco.FocoId))
                return NotFound();

            if (!ModelState.IsValid)
            {
                var newFocoEditViewModel = new EditFocoViewModel
                {
                    FocoId = foco.FocoId,
                    FocoAtendido = foco.FocoAtendido,
                    FocoConfirmado = foco.FocoConfirmado,
                    Bioma = foco.Bioma,
                    Municipi = foco.Municipi,
                    PolicialResponsavel = foco.PolicialResponsavel,
                    OcorrênciaSIOPM = foco.OcorrênciaSIOPM,
                    NºBOPAmb = foco.NºBOPAmb,
                    NºTVA = foco.NºTVA,
                    RSO = foco.RSO,
                    DataAtendimento = foco.DataAtendimento,
                    EquipeId = foco.EquipeId,
                    Equipe = new SelectList(await _context.Equipes.ToListAsync(),
                      dataValueField: "EquipeId",
                     dataTextField: "EquipeNome"),
                    StatusFocoId = foco.StatusFocoId,
                    StatusDoFoco = new SelectList(await _context.StatusFocos.ToListAsync(),
                      dataValueField: "StatusFocoId",
                     dataTextField: "StatusFocoDescricao"),
                    IndicioInicioFocoId = foco.IndicioInicioFocoId,
                    IndicioDeInicioDoFoco = new SelectList(await _context.IndiciosInicioFoco.ToListAsync(),
                      dataValueField: "IndicioInicioFocoId",
                     dataTextField: "IndicioInicioFocoDescricao"),
                    CausaFogoId = foco.CausaFogoId,
                    CausaFogo = new SelectList(await _context.CausasFogo.ToListAsync(),
                      dataValueField: "CausaFogoId",
                     dataTextField: "CausaFogoDescricao"),
                    CausadorProvavelId = foco.CausadorProvavelId,
                    CausadorProvavel = new SelectList(await _context.CausadoresProvaveis.ToListAsync(),
                      dataValueField: "CausadorProvavelId",
                     dataTextField: "CausadorProvavelDescricacao"),
                    ResponsavelPropriedadeId = foco.ResponsavelPropriedadeId,
                    ResponsavelPelaPropriedade = new SelectList(await _context.ResponsaveisPropriedade.ToListAsync(),
                      dataValueField: "ResponsavelPropriedadeId",
                     dataTextField: "ResponsavelPropriedadeDescricao"),
                    TipoVegetacaoId = foco.TipoVegetacaoId,
                    TipoVegetacao = new SelectList(await _context.TiposVegetacao.ToListAsync(),
                      dataValueField: "TipoVegetacaoId",
                     dataTextField: "TipoVegetacaoDescricao"),
                    PioneiroAPPAreaEmHectares = foco.PioneiroAPPAreaEmHectares,
                    InicialAPPAreaEmHectares = foco.InicialAPPAreaEmHectares,
                    MedioAPPAreaEmHectares = foco.MedioAPPAreaEmHectares,
                    AvancadoAPPAreaEmHectares = foco.AvancadoAPPAreaEmHectares,
                    AutoDeInflacaoAmbientalAPP = foco.AutoDeInflacaoAmbientalAPP,
                    MultaAPP = foco.MultaAPP,
                    Pioneiro = foco.Pioneiro,
                    Inicial = foco.Inicial,
                    Medio = foco.Medio,
                    Avancado = foco.Avancado,
                    AutoDeInflacaoAmbiental = foco.AutoDeInflacaoAmbiental,
                    MultaR = foco.MultaR,
                    Pasto = foco.Pasto,
                    Citrus = foco.Citrus,
                    Outras = foco.Outras,
                    AutoDeInflacaoAmbientalV = foco.AutoDeInflacaoAmbientalV,
                    MultaV = foco.MultaV,
                    ArvoresIsoladas = foco.ArvoresIsoladas,
                    AutoDeInflacaoAmbientalA = foco.AutoDeInflacaoAmbientalA,
                    MultaA = foco.MultaA,
                    PalhaDeCana = foco.PalhaDeCana,
                    CanaDeAcucar = foco.CanaDeAcucar,
                    Autorizado = foco.Autorizado,
                    AutoDeInflacaoAmbientalL = foco.AutoDeInflacaoAmbientalL,
                    MultaL = foco.MultaL,
                    PioneiroUC = foco.PioneiroUC,
                    InicialUC = foco.InicialUC,
                    MedioUC = foco.MedioUC,
                    AvancadoUC = foco.AvancadoUC,
                    OutrasUC = foco.OutrasUC,
                    AutoDeInflacaoAmbientalUC = foco.AutoDeInflacaoAmbientalUC,
                    MultaUC = foco.MultaUC,
                    PioneiroRL = foco.PioneiroRL,
                    InicialRL = foco.InicialRL,
                    MedioRL = foco.MedioRL,
                    AvancadoRL = foco.AvancadoRL,
                    OutrasRL = foco.OutrasRL,
                    AutoDeInflacaoAmbientalRL = foco.AutoDeInflacaoAmbientalRL,
                    MultaRL = foco.MultaRL,
                    Refiscalizacao = foco.Refiscalizacao
                };
                return View(newFocoEditViewModel);
            }

            var focoOriginal = await _context.Focos
              .Include(p => p.Estado)
              .Include(p => p.Municipio)
              .Include(p => p.Satelite)
              .Include(f => f.StatusDoFoco)
              .Include(f => f.CausadorProvavel)
              .Include(f => f.IndicioInicioFoco)
              .Include(f => f.CausaFogo)
              .Include(f => f.ResponsavelPropriedade)
              .Include(f => f.TipoVegetacao)
              .Include(f => f.Equipe)
              .FirstOrDefaultAsync(p => p.FocoId == foco.FocoId);

            if (focoOriginal.Bioma != foco.Bioma)
                focoOriginal.Bioma = foco.Bioma;

            if (focoOriginal.Municipi != foco.Municipi)
                focoOriginal.Municipi = foco.Municipi;

            if (focoOriginal.PolicialResponsavel != foco.PolicialResponsavel)
                focoOriginal.PolicialResponsavel = foco.PolicialResponsavel;

            if (focoOriginal.OcorrênciaSIOPM != foco.OcorrênciaSIOPM)
                focoOriginal.OcorrênciaSIOPM = foco.OcorrênciaSIOPM;

            if (focoOriginal.NºBOPAmb != foco.NºBOPAmb)
                focoOriginal.NºBOPAmb = foco.NºBOPAmb;

            if (focoOriginal.NºTVA != foco.NºTVA)
                focoOriginal.NºTVA = foco.NºTVA;

            if (focoOriginal.RSO != foco.RSO)
                focoOriginal.RSO = foco.RSO;

            if (focoOriginal.DataAtendimento != foco.DataAtendimento)
                focoOriginal.DataAtendimento = foco.DataAtendimento;

            if (focoOriginal.EquipeId != foco.EquipeId)
            {
                var equipe = await _context.Equipes.FindAsync(foco.EquipeId);

                if (equipe == null)
                    return NotFound();

                focoOriginal.Equipe = equipe;
            }

            if (focoOriginal.StatusFocoId != foco.StatusFocoId)
            {
                var statufoco = await _context.StatusFocos.FindAsync(foco.StatusFocoId);

                if (statufoco == null)
                    return NotFound();

                focoOriginal.StatusDoFoco = statufoco;
            }

            if (focoOriginal.IndicioInicioFocoId != foco.IndicioInicioFocoId)
            {
                var iniciofoco = await _context.IndiciosInicioFoco.FindAsync(foco.IndicioInicioFocoId);

                if (iniciofoco == null)
                    return NotFound();

                focoOriginal.IndicioInicioFoco = iniciofoco;
            }

            if (focoOriginal.CausaFogoId != foco.CausaFogoId)
            {
                var causafogo = await _context.CausasFogo.FindAsync(foco.CausaFogoId);

                if (causafogo == null)
                    return NotFound();

                focoOriginal.CausaFogo = causafogo;
            }

            if (focoOriginal.CausadorProvavelId != foco.CausadorProvavelId)
            {
                var causaprovavel = await _context.CausadoresProvaveis.FindAsync(foco.CausadorProvavelId);

                if (causaprovavel == null)
                    return NotFound();

                focoOriginal.CausadorProvavel = causaprovavel;
            }

            if (focoOriginal.ResponsavelPropriedadeId != foco.ResponsavelPropriedadeId)
            {
                var responsavel = await _context.ResponsaveisPropriedade.FindAsync(foco.ResponsavelPropriedadeId);

                if (responsavel == null)
                    return NotFound();

                focoOriginal.ResponsavelPropriedade = responsavel;
            }

            if (focoOriginal.TipoVegetacaoId != foco.TipoVegetacaoId)
            {
                var vegetacao = await _context.TiposVegetacao.FindAsync(foco.TipoVegetacaoId);

                if (vegetacao == null)
                    return NotFound();

                focoOriginal.TipoVegetacao = vegetacao;
            }

            if (focoOriginal.PioneiroAPPAreaEmHectares != foco.PioneiroAPPAreaEmHectares)
                focoOriginal.PioneiroAPPAreaEmHectares = foco.PioneiroAPPAreaEmHectares;

            if (focoOriginal.InicialAPPAreaEmHectares != foco.InicialAPPAreaEmHectares)
                focoOriginal.InicialAPPAreaEmHectares = foco.InicialAPPAreaEmHectares;

            if (focoOriginal.MedioAPPAreaEmHectares != foco.MedioAPPAreaEmHectares)
                focoOriginal.MedioAPPAreaEmHectares = foco.MedioAPPAreaEmHectares;

            if (focoOriginal.AvancadoAPPAreaEmHectares != foco.AvancadoAPPAreaEmHectares)
                focoOriginal.AvancadoAPPAreaEmHectares = foco.AvancadoAPPAreaEmHectares;

            if (focoOriginal.AutoDeInflacaoAmbientalAPP != foco.AutoDeInflacaoAmbientalAPP)
                focoOriginal.AutoDeInflacaoAmbientalAPP = foco.AutoDeInflacaoAmbientalAPP;

            if (focoOriginal.MultaAPP != foco.MultaAPP)
                focoOriginal.MultaAPP = foco.MultaAPP;

            if (focoOriginal.Pioneiro != foco.Pioneiro)
                focoOriginal.Pioneiro = foco.Pioneiro;

            if (focoOriginal.Inicial != foco.Inicial)
                focoOriginal.Inicial = foco.Inicial;

            if (focoOriginal.Medio != foco.Medio)
                focoOriginal.Medio = foco.Medio;

            if (focoOriginal.Avancado != foco.Avancado)
                focoOriginal.Avancado = foco.Avancado;

            if (focoOriginal.AutoDeInflacaoAmbiental != foco.AutoDeInflacaoAmbiental)
                focoOriginal.AutoDeInflacaoAmbiental = foco.AutoDeInflacaoAmbiental;

            if (focoOriginal.MultaR != foco.MultaR)
                focoOriginal.MultaR = foco.MultaR;

            if (focoOriginal.Pasto != foco.Pasto)
                focoOriginal.Pasto = foco.Pasto;

            if (focoOriginal.Citrus != foco.Citrus)
                focoOriginal.Citrus = foco.Citrus;

            if (focoOriginal.Outras != foco.Outras)
                focoOriginal.Outras = foco.Outras;

            if (focoOriginal.AutoDeInflacaoAmbientalV != foco.AutoDeInflacaoAmbientalV)
                focoOriginal.AutoDeInflacaoAmbientalV = foco.AutoDeInflacaoAmbientalV;

            if (focoOriginal.MultaV != foco.MultaV)
                focoOriginal.MultaV = foco.MultaV;

            if (focoOriginal.ArvoresIsoladas != foco.ArvoresIsoladas)
                focoOriginal.ArvoresIsoladas = foco.ArvoresIsoladas;

            if (focoOriginal.AutoDeInflacaoAmbientalA != foco.AutoDeInflacaoAmbientalA)
                focoOriginal.AutoDeInflacaoAmbientalA = foco.AutoDeInflacaoAmbientalA;

            if (focoOriginal.MultaA != foco.MultaA)
                focoOriginal.MultaA = foco.MultaA;

            if (focoOriginal.PalhaDeCana != foco.PalhaDeCana)
                focoOriginal.PalhaDeCana = foco.PalhaDeCana;

            if (focoOriginal.CanaDeAcucar != foco.CanaDeAcucar)
                focoOriginal.CanaDeAcucar = foco.CanaDeAcucar;

            if (focoOriginal.Autorizado != foco.Autorizado)
                focoOriginal.Autorizado = foco.Autorizado;

            if (focoOriginal.AutoDeInflacaoAmbientalL != foco.AutoDeInflacaoAmbientalL)
                focoOriginal.AutoDeInflacaoAmbientalL = foco.AutoDeInflacaoAmbientalL;

            if (focoOriginal.MultaL != foco.MultaL)
                focoOriginal.MultaL = foco.MultaL;

            if (focoOriginal.PioneiroUC != foco.PioneiroUC)
                focoOriginal.PioneiroUC = foco.PioneiroUC;

            if (focoOriginal.InicialUC != foco.InicialUC)
                focoOriginal.InicialUC = foco.InicialUC;

            if (focoOriginal.MedioUC != foco.MedioUC)
                focoOriginal.MedioUC = foco.MedioUC;

            if (focoOriginal.AvancadoUC != foco.AvancadoUC)
                focoOriginal.AvancadoUC = foco.AvancadoUC;

            if (focoOriginal.OutrasUC != foco.OutrasUC)
                focoOriginal.OutrasUC = foco.OutrasUC;

            if (focoOriginal.AutoDeInflacaoAmbientalUC != foco.AutoDeInflacaoAmbientalUC)
                focoOriginal.AutoDeInflacaoAmbientalUC = foco.AutoDeInflacaoAmbientalUC;

            if (focoOriginal.MultaUC != foco.MultaUC)
                focoOriginal.MultaUC = foco.MultaUC;

            if (focoOriginal.PioneiroRL != foco.PioneiroRL)
                focoOriginal.PioneiroRL = foco.PioneiroRL;

            if (focoOriginal.InicialRL != foco.InicialRL)
                focoOriginal.InicialRL = foco.InicialRL;

            if (focoOriginal.MedioRL != foco.MedioRL)
                focoOriginal.MedioRL = foco.MedioRL;

            if (focoOriginal.AvancadoRL != foco.AvancadoRL)
                focoOriginal.AvancadoRL = foco.AvancadoRL;

            if (focoOriginal.OutrasRL != foco.OutrasRL)
                focoOriginal.OutrasRL = foco.OutrasRL;

            if (focoOriginal.AutoDeInflacaoAmbientalRL != foco.AutoDeInflacaoAmbientalRL)
                focoOriginal.AutoDeInflacaoAmbientalRL = foco.AutoDeInflacaoAmbientalRL;

            if (focoOriginal.MultaRL != foco.MultaRL)
                focoOriginal.MultaRL = foco.MultaRL;

            if (focoOriginal.Refiscalizacao != foco.Refiscalizacao)
                focoOriginal.Refiscalizacao = foco.Refiscalizacao;

            if (focoOriginal.FocoAtendido != foco.FocoAtendido)
                focoOriginal.FocoAtendido = foco.FocoAtendido;

            if (focoOriginal.FocoConfirmado != foco.FocoConfirmado)
                focoOriginal.FocoConfirmado = foco.FocoConfirmado;

            if (_context.Entry(focoOriginal).State == EntityState.Modified)
            {
                try
                {
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException e)
                {
                    if (!_context.Focos.Any(p => p.FocoId == focoOriginal.FocoId))
                        return NotFound();
                    Console.WriteLine(e);
                    throw;
                }
            }
            return RedirectToAction(nameof(ListaFocos));
        }



        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public IActionResult Sobre()
        {
            return View();
        }

        public IActionResult GeneratePdf(string html)
        {
            // instantiate a html to pdf converter object
            HtmlToPdf converter = new HtmlToPdf();

            html = html.Replace("start", "<").Replace("end", ">");

            // create a new pdf document converting an url
            PdfDocument doc = converter.ConvertHtmlString(html);
            

            // save pdf document
            byte[] pdffile = doc.Save();

            // close pdf document
            doc.Close();

            return File(pdffile,"application/pdf");
        }

        
    }
}