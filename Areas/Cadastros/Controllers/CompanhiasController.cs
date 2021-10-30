using CadeOFogo.Areas.Cadastros.ViewsModels;
using CadeOFogo.Data;
using CadeOFogo.Models.Inpe;
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
    public class Companhias : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly int _pagesize;

        public Companhias(ApplicationDbContext context, IConfiguration configuration)
        {
            _context = context;
            _pagesize = Convert.ToInt32(configuration
            .GetSection("ViewOptions")
            .GetSection("PageSizeCadastro")
            .Value);
            _pagesize = _pagesize != 0 ? _pagesize : 20;
        }


        public async Task<IActionResult> Index(string keyword, int? pagina)
        {
            var dataset = _context.Companhias
              .Include(p => p.Batalhao)
              .OrderBy(p => p.CompanhiaNome);

            if (!string.IsNullOrWhiteSpace(keyword))
            {
                dataset = (IOrderedQueryable<Companhia>)dataset
                  .Where(p => p.CompanhiaNome.Contains(keyword.ToUpper()));
                ViewBag.keyword = keyword;
            }
            else
            {
                ViewBag.keyword = "";
            }

            var companhiaViewModel = await dataset
              .Select(companhia => new CompanhiaIndexViewModel
              {
                  CompanhiaId = companhia.CompanhiaId,
                  CompanhiaNome = companhia.CompanhiaNome,
                  NomeBatalhao = companhia.Batalhao.NomeBatalhao
              }).ToPagedListAsync(pagina ?? 1, _pagesize);

            ViewBag.primeiro = companhiaViewModel.FirstItemOnPage;
            ViewBag.ultimo = companhiaViewModel.LastItemOnPage;
            ViewBag.total = companhiaViewModel.TotalItemCount;

            return View(companhiaViewModel);
        }

        public async Task<IActionResult> Create()
        {
            var companhia = new CompanhiaEditViewModel
            {
                BatalhaoId = 0,
                BatalhaoInputSelect = new SelectList(await _context.Batalhoes
                  .OrderBy(c => c.NomeBatalhao).ToListAsync(),
                dataValueField: "BatalhaoId",
                dataTextField: "NomeBatalhao")
            };
            return View(companhia);
        }

        [HttpPost, ActionName("Create")]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> CreateConfirm(CompanhiaEditViewModel companhia)
        {
            if (companhia.BatalhaoId == 0)
            {
                companhia.BatalhaoInputSelect = new SelectList(await _context.Batalhoes
                  .OrderBy(c => c.NomeBatalhao).ToListAsync(),
                  dataValueField: "BatalhaoId",
                  dataTextField: "NomeBatalhao");
                ModelState.AddModelError("BatalhaoInputSelect", "Selecione um batalhão");
                return View(companhia);
            }

            var batalhao = await _context.Batalhoes.FindAsync(companhia.BatalhaoId);
            if (batalhao == null)
                return NotFound();
            var newcompanhia = new Companhia
            {
                CompanhiaId = companhia.CompanhiaId,
                CompanhiaNome = companhia.CompanhiaNome,
                Batalhao = batalhao
            };
            await _context.Companhias.AddAsync(newcompanhia);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return RedirectToAction(nameof(Index));

            var companhia = await _context.Companhias
              .Include(p => p.Batalhao)
              .FirstOrDefaultAsync(p => p.CompanhiaId == id);

            if (companhia == null)
                return NotFound();

            var companhiaViewModel = new CompanhiaEditViewModel
            {
                CompanhiaId = companhia.CompanhiaId,
                CompanhiaNome = companhia.CompanhiaNome,
                BatalhaoId = companhia.BatalhaoId,
                BatalhaoInputSelect = new SelectList(await _context.Batalhoes
                  .OrderBy(c => c.NomeBatalhao).ToListAsync(),
                dataValueField: "BatalhaoId",
                dataTextField: "NomeBatalhao")
            };

            return View(companhiaViewModel);
        }

        [HttpPost, ActionName("Edit")]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> EditConfirm(int? id, CompanhiaEditViewModel companhia)
        {
            if ((id == null) || (id != companhia.CompanhiaId))
                return NotFound();

            if (!ModelState.IsValid)
            {
                var newCompanhiaEditViewModel = new CompanhiaEditViewModel
                {
                    CompanhiaId = companhia.CompanhiaId,
                    CompanhiaNome = companhia.CompanhiaNome,
                    BatalhaoId = companhia.BatalhaoId,
                    BatalhaoInputSelect = new SelectList(await _context.Batalhoes.ToListAsync(),
                   dataValueField: "BatalhaoId",
                   dataTextField: "NomeBatalhao")
                  
                };
                return View(newCompanhiaEditViewModel);
            }

            var companhiaOriginal = await _context.Companhias
              .Include(p => p.Batalhao)
              .FirstOrDefaultAsync(p => p.CompanhiaId == companhia.CompanhiaId);

            if (companhiaOriginal.CompanhiaNome != companhia.CompanhiaNome)
                companhiaOriginal.CompanhiaNome = companhia.CompanhiaNome;

            if (companhiaOriginal.BatalhaoId != companhia.BatalhaoId)
            {
                var batalhao = await _context.Batalhoes.FindAsync(companhia.BatalhaoId);

                if (batalhao == null)
                    return NotFound();

                companhiaOriginal.Batalhao = batalhao;
            }

            if (_context.Entry(companhiaOriginal).State == EntityState.Modified)
            {
                try
                {
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException e)
                {
                    if (!_context.Companhias.Any(p => p.CompanhiaId == companhiaOriginal.CompanhiaId))
                        return NotFound();
                    Console.WriteLine(e);
                    throw;
                }
            }
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
                return NotFound();

            var companhia = await _context.Companhias
              .Include(p => p.Batalhao)
              .FirstOrDefaultAsync(p => p.CompanhiaId == id);

            if (companhia == null)
                return NotFound();

            var companhiaViewModel = new CompanhiaDetailsViewModel
            {
                CompanhiaId = companhia.CompanhiaId,
                CompanhiaNome = companhia.CompanhiaNome,
                NomeBatalhao = companhia.Batalhao.NomeBatalhao
            };
            return View(companhiaViewModel);
        }

        [HttpPost, ActionName("Delete")]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> DeleteConfirm(int? id)
        {
            if (id == null)
                return NotFound();

            var companhia = await _context.Companhias.FindAsync(id);
            if (companhia == null)
                return NotFound();

            _context.Remove(companhia);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException e)
            {
                if (!await _context.Companhias.AnyAsync(p => p.CompanhiaId == id))
                    return NotFound();
                Console.WriteLine(e);
                throw;
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
