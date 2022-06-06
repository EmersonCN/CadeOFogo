using CadeOFogo.Data;
using CadeOFogo.Interfaces;
using CadeOFogo.Models.Inpe;
using CadeOFogo.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace CadeOFogo.Controllers
{
    [Route("Controllers/[controller]")]
    public class CadeOFogoController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly ApplicationDbContext _context;
        private readonly ILogger<HomeController> _logger;
        private readonly IMapProvider _mapProvider;

        public CadeOFogoController(ILogger<HomeController> logger, ApplicationDbContext context, IConfiguration configuration,
          IMapProvider mapProvider)
        {
            _logger = logger;
            _context = context;
            _configuration = configuration;
            _mapProvider = mapProvider;
        }

        [HttpGet("{datainicio},{dataFinal}")]
        public JsonResult GetFoco(string dataInicio, string dataFinal)
        {
            var provider = CultureInfo.InvariantCulture;

            var inicio = DateTime.UtcNow.AddDays(-2);
            var final = DateTime.UtcNow;

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

            var dataset = _context.Focos
                .Include(f => f.Municipio)
                .Include(f => f.Estado)
                .Include(f => f.Satelite)
                .Where(x => x.FocoConfirmado == true)
                .Where(x => x.FocoDataUtc >= inicio & x.FocoDataUtc <= final);

            if (dataset == null)
                return null;

            var foco = new JsonFocosInpeViewModel
            {
                Focos = dataset.Select(foco => new JsonFocosInpeVerificadosViewModel
                {
                    FocoId = foco.FocoId.ToString(),
                    FocoDataUtc = foco.FocoDataUtc.ToString(),
                    Satelite = foco.Satelite.SateliteNome,
                    EstadoNome = foco.Estado.EstadoNome,
                    MunicipioNome = foco.Municipio.MunicipioNome,
                    Coordenadas = new List<decimal>
                    {
                      foco.FocoLongitude,
                      foco.FocoLatitude
                     },
                    FocoConfirmado = foco.FocoConfirmado
                }).ToList()
            };
            return Json(foco);
        }        
    }
}

