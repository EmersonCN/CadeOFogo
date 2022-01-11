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
using X.PagedList;

namespace CadeOFogo.Controllers
{
  public class HomeController : Controller
  {
    private readonly IConfiguration _configuration;
    private readonly ApplicationDbContext _context;
    private readonly ILogger<HomeController> _logger;
    private readonly IMapProvider _mapProvider;

    private readonly NumberFormatInfo _nfi = new() {NumberDecimalSeparator = ".", NumberDecimalDigits = 5};
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
        .Where(f => f.FocoDataUtc >= DateTime.UtcNow.AddDays(-2)).ToListAsync();

      var resposta = new JsonFocos48ViewModel
      {
        Type = "FeatureCollection",
        Features = focos.Select(foco => new Foco48hViewModel
        {
          Type = "Feature",
          Id = foco.FocoId.ToString(),
          Properties = new Foco48hViewModelProperties
          {
            Resumo = $"<p>Foco detectado em {foco.FocoDataUtc:s} "+
                     $"pelo satélite {foco.Satelite.SateliteNome}.<br /><br />"+
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


    public async Task<IActionResult> Detalhes(int? id)
    {
      if (id == null) return NotFound();

      var foco = await _context.Focos
        .Include(f => f.Estado)
        .Include(f => f.Municipio)
        .Include(f => f.Satelite)
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
        Localidade = $"{foco.Municipio.MunicipioNome}, {foco.Estado.EstadoNome}"
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
                Localidade = $"{foco.Municipio.MunicipioNome}, {foco.Estado.EstadoNome}"
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



        public IActionResult Privacy()
    {
      return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
      return View(new ErrorViewModel {RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier});
    }

    public IActionResult Sobre()
    {
      return View();
    }
  }
}