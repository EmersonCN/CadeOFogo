using CadeOFogo.Areas.Cadastros.Models;
using CadeOFogo.Areas.Cadastros.ViewsModels.Equipe;
using CadeOFogo.Data;
using CadeOFogo.Models.Inpe;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using X.PagedList;

namespace CadeOFogo.Areas.Cadastros.Controllers
{
    [Area("Cadastros")]
    public class Equipes : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly int _pagesize;
        private readonly UserManager<ApplicationUser> _userManager;

        public Equipes(ApplicationDbContext context, IConfiguration configuration, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _pagesize = Convert.ToInt32(configuration
            .GetSection("ViewOptions")
            .GetSection("PageSizeCadastro")
            .Value);
            _pagesize = _pagesize != 0 ? _pagesize : 20;

            _userManager = userManager;
        }

        [HttpGet]
        public ActionResult ListarCompanhia(string sigla)
        {
            var lista = new List<Companhia>();
            try
            {
                lista = _context.Companhias.Where(m => m.Batalhao.NomeBatalhao == sigla).ToList();
            }
            catch (Exception ex)
            {

                throw;
            }

            return Json(System.Web.Mvc.JsonRequestBehavior.AllowGet);
        }
        

        public async Task<IActionResult> Index(string keyword, int? pagina)
        {
            var dataset = _context.Equipes
              .Include(p => p.Batalhao)
              .Include(p => p.Companhia)
              .Include(p => p.Pelotao)
              .Include(p => p.ApplicationUser)
              .OrderBy(p => p.EquipeNome);

            if (!string.IsNullOrWhiteSpace(keyword))
            {
                dataset = (IOrderedQueryable<Equipe>)dataset
                  .Where(p => p.EquipeNome.Contains(keyword.ToUpper()));
                ViewBag.keyword = keyword;
            }
            else
            {
                ViewBag.keyword = "";
            }

            var equipeViewModel = await dataset
              .Select(equipe => new EquipeIndexViewModel
              {
                  EquipeId = equipe.EquipeId,
                  EquipeNome = equipe.EquipeNome,
                  NomeBatalhao = equipe.Batalhao.NomeBatalhao,
                  CompanhiaNome = equipe.Companhia.CompanhiaNome,
                  PelotaoNome = equipe.Pelotao.PelotaoNome,
                  Ativa = equipe.Ativa,
                  Nome = equipe.ApplicationUser.NomeCompleto
                  
              }).ToPagedListAsync(pagina ?? 1, _pagesize);

            ViewBag.primeiro = equipeViewModel.FirstItemOnPage;
            ViewBag.ultimo = equipeViewModel.LastItemOnPage;
            ViewBag.total = equipeViewModel.TotalItemCount;

            return View(equipeViewModel);
        }
        public async Task<IActionResult> Create()
        {
            var equipe = new EquipeEditViewModel
            {
                BatalhaoId = 0,
                BatalhaoInputSelect = new SelectList(await _context.Batalhoes
                .OrderBy(c => c.NomeBatalhao).ToListAsync(),
                dataValueField: "BatalhaoId",
                dataTextField: "NomeBatalhao"),

                CompanhiaId = 0,
                CompanhiaInputSelect = new SelectList(await _context.Companhias
                .OrderBy(c => c.CompanhiaNome).ToListAsync(),
                dataValueField: "CompanhiaId",
                dataTextField: "CompanhiaNome"),

                PelotaoId = 0,
                PelotaoInputSelect = new SelectList(await _context.Pelotoes
                .OrderBy(c => c.PelotaoNome).ToListAsync(),
                dataValueField: "PelotaoId",
                dataTextField: "PelotaoNome"),

                ApplicationUserUserId = "0",
                UsuarioInputSelect = new SelectList(await _context.ApplicationUsers
                .OrderBy(c => c.NomeCompleto).ToListAsync(),
                dataValueField: "Id",
                dataTextField: "NomeCompleto"),

            };
            return View(equipe);
        }

        [HttpPost, ActionName("Create")]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> CreateConfirm(EquipeEditViewModel equipe)
        {
            if (equipe.BatalhaoId == 0)
            {
                equipe.BatalhaoInputSelect = new SelectList(await _context.Batalhoes
                  .OrderBy(c => c.NomeBatalhao).ToListAsync(),
                  dataValueField: "BatalhaoId",
                  dataTextField: "NomeBatalhao");
                ModelState.AddModelError("BatalhaoInputSelect", "Selecione um batalhão");
          
                if(equipe.CompanhiaId == 0)
                {
                    equipe.CompanhiaInputSelect = new SelectList(await _context.Companhias
                  .OrderBy(c => c.CompanhiaNome)
                  .ToListAsync(),
                  dataValueField: "CompanhiaId",
                  dataTextField: "CompanhiaNome");
                    ModelState.AddModelError("CompanhiaInputSelect", "Selecione uma companhia");

                    if(equipe.PelotaoId == 0)
                    {
                        equipe.PelotaoInputSelect = new SelectList(await _context.Pelotoes
                        .OrderBy(c => c.PelotaoNome).ToListAsync(),
                         dataValueField: "PelotaoId",
                         dataTextField: "PelotaoNome");
                        ModelState.AddModelError("PelotaoInputSelect", "Selecione uma pelotão");

                        if (equipe.ApplicationUserUserId == "0")
                        {
                            equipe.UsuarioInputSelect = new SelectList(await _context.ApplicationUsers
                            .OrderBy(c => c.NomeCompleto).ToListAsync(),
                            dataValueField: "Id",
                            dataTextField: "NomeCompleto");
                            ModelState.AddModelError("UsuarioInputSelect", "Selecione uma usuario");
                        }
                    }
                }          
                return View(equipe);
            }

            var batalhao = await _context.Batalhoes.FindAsync(equipe.BatalhaoId);
            if (batalhao == null)
                return NotFound();

            var companhia = await _context.Companhias.FindAsync(equipe.CompanhiaId);
            if (companhia == null)
                return NotFound();

            var pelotao = await _context.Pelotoes.FindAsync(equipe.PelotaoId);
            if (pelotao == null)
                return NotFound();

            var user = await _context.ApplicationUsers.FindAsync(equipe.ApplicationUserUserId);
            if (user == null)
                return NotFound();

            var newEquipe = new Equipe
            {
                EquipeId = equipe.EquipeId,
                EquipeNome = equipe.EquipeNome,
                Batalhao = batalhao,
                Companhia = companhia,
                Pelotao = pelotao,
                Ativa = equipe.Ativa,
                ApplicationUser = user
            };
            await _context.Equipes.AddAsync(newEquipe);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return RedirectToAction(nameof(Index));

            var equipe = await _context.Equipes
              .Include(p => p.Batalhao)
              .Include(p => p.Companhia)
              .Include(p => p.Pelotao)
              .Include(p => p.ApplicationUser)
              .FirstOrDefaultAsync(p => p.EquipeId == id);

            if (equipe == null)
                return NotFound();

            var equipeViewModel = new EquipeEditViewModel
            {
                EquipeId = equipe.EquipeId,
                EquipeNome = equipe.EquipeNome,
                BatalhaoId = equipe.BatalhaoId,
                BatalhaoInputSelect = new SelectList(await _context.Batalhoes
                  .OrderBy(c => c.NomeBatalhao).ToListAsync(),
                dataValueField: "BatalhaoId",
                dataTextField: "NomeBatalhao"),
                CompanhiaId = equipe.CompanhiaId,
                CompanhiaInputSelect = new SelectList(await _context.Companhias
                  .OrderBy(c => c.CompanhiaNome).ToListAsync(),
                dataValueField: "CompanhiaId",
                dataTextField: "CompanhiaNome"),
                PelotaoId = equipe.PelotaoId,
                PelotaoInputSelect = new SelectList(await _context.Pelotoes
                .OrderBy(c => c.PelotaoNome).ToListAsync(),
                dataValueField: "PelotaoId",
                 dataTextField: "PelotaoNome"),
                ApplicationUserUserId = equipe.ApplicationUser.Id,
                UsuarioInputSelect = new SelectList(await _context.ApplicationUsers
                .OrderBy(c => c.NomeCompleto).ToListAsync(),
                dataValueField: "Id",
                 dataTextField: "NomeCompleto"),
                Ativa = equipe.Ativa

            };

            return View(equipeViewModel);
        }

        [HttpPost, ActionName("Edit")]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> EditConfirm(int? id, EquipeEditViewModel equipe)
        {
            if ((id == null) || (id != equipe.EquipeId))
                return NotFound();

            if (!ModelState.IsValid)
            {
                var newEquipeEditViewModel = new EquipeEditViewModel
                {
                    EquipeId = equipe.EquipeId,
                    EquipeNome = equipe.EquipeNome,
                    BatalhaoId = equipe.BatalhaoId,
                    BatalhaoInputSelect = new SelectList(await _context.Batalhoes.ToListAsync(),
                   dataValueField: "BatalhaoId",
                   dataTextField: "NomeBatalhao"),
                    CompanhiaId = equipe.CompanhiaId,
                    CompanhiaInputSelect = new SelectList(await _context.Companhias.ToListAsync(),
                dataValueField: "CompanhiaId",
                dataTextField: "CompanhiaNome"),
                    PelotaoId = equipe.PelotaoId,
                    PelotaoInputSelect = new SelectList(await _context.Pelotoes.ToListAsync(),
                dataValueField: "PelotaoId",
                 dataTextField: "PelotaoNome"),
                    ApplicationUserUserId = equipe.ApplicationUserUserId,
                    UsuarioInputSelect = new SelectList(await _context.ApplicationUsers.ToListAsync(),
                dataValueField: "Id",
                 dataTextField: "NomeCompleto"),
                    Ativa = equipe.Ativa

                };
                return View(newEquipeEditViewModel);
            }

            var equipeOriginal = await _context.Equipes
              .Include(p => p.Batalhao)
              .Include(p => p.Companhia)
              .Include(p => p.Pelotao)
              .Include(p => p.ApplicationUser)
              .FirstOrDefaultAsync(p => p.EquipeId == equipe.EquipeId);

            if (equipeOriginal.EquipeNome != equipe.EquipeNome)
                equipeOriginal.EquipeNome = equipe.EquipeNome;

            if (equipeOriginal.BatalhaoId != equipe.BatalhaoId)
            {
                var batalhao = await _context.Batalhoes.FindAsync(equipe.BatalhaoId);

                if (batalhao == null)
                    return NotFound();

                equipeOriginal.Batalhao = batalhao;
            }

            if (equipeOriginal.CompanhiaId != equipe.CompanhiaId)
            {
                var companhia = await _context.Companhias.FindAsync(equipe.CompanhiaId);

                if (companhia == null)
                    return NotFound();

                equipeOriginal.Companhia = companhia;
            }

            if (equipeOriginal.PelotaoId != equipe.PelotaoId)
            {
                var pelotao = await _context.Pelotoes.FindAsync(equipe.PelotaoId);

                if (pelotao == null)
                    return NotFound();

                equipeOriginal.Pelotao = pelotao;
            }

            if (equipeOriginal.ApplicationUser.Id != equipe.ApplicationUserUserId)
            {
                var user = await _context.ApplicationUsers.FindAsync(equipe.ApplicationUserUserId);

                if (user == null)
                    return NotFound();

                equipeOriginal.ApplicationUser = user;
            }

            if (equipeOriginal.Ativa != equipe.Ativa)
                equipeOriginal.Ativa = equipe.Ativa;


            if (_context.Entry(equipeOriginal).State == EntityState.Modified)
            {
                try
                {
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException e)
                {
                    if (!_context.Equipes.Any(p => p.EquipeId == equipeOriginal.EquipeId))
                        return NotFound();
                    Console.WriteLine(e);
                    throw;
                }
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
