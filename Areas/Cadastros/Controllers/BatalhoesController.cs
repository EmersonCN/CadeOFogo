using CadeOFogo.Data;
using CadeOFogo.Models.Inpe;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Linq;
using System.Threading.Tasks;
using X.PagedList;

namespace CadeOFogo.Areas.Cadastros.Controllers
{
    [Area("Cadastros")]
    public class Batalhoes : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly int _pagesize;

        public Batalhoes(ApplicationDbContext context, IConfiguration configuration)
        {
            _context = context;
            _pagesize = Convert.ToInt32(configuration
              .GetSection("ViewOptions")
              .GetSection("PageSizeCadastro")
              .Value);
            _pagesize = _pagesize != 0 ? _pagesize : 20;
        }

        public async Task<IActionResult> Index(string keyword, int? page)
        {
            IQueryable<Batalhao> dataset = _context.Batalhoes
                .OrderBy(b => b.NomeBatalhao);

            if (!string.IsNullOrEmpty(keyword))
            {
                dataset = dataset.Where(b =>
                  b.NomeBatalhao.Contains(keyword));
                ViewBag.keyword = keyword;
            }
            else
            {
                ViewBag.keyword = "";
            }

            var data = await dataset.ToPagedListAsync(page ?? 1, _pagesize);

            ViewBag.primeiro = data.FirstItemOnPage;
            ViewBag.ultimo = data.LastItemOnPage;
            ViewBag.total = data.TotalItemCount;

            return View(data);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("BatalhaoId,NomeBatalhao")]
        Batalhao batalhao)
        {
            if (ModelState.IsValid)
            {
                _context.Add(batalhao);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            return View(batalhao);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var batalhao = await _context.Batalhoes.FindAsync(id);
            if (batalhao == null) return NotFound();
            return View(batalhao);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("BatalhaoId,NomeBatalhao")]
        Batalhao batalhao)
        {
            if (id != batalhao.BatalhaoId) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(batalhao);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BatalhaoExists(batalhao.BatalhaoId))
                        return NotFound();
                    throw;
                }

                return RedirectToAction(nameof(Index));
            }

            return View(batalhao);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var batalhao = await _context.Batalhoes
              .FirstOrDefaultAsync(m => m.BatalhaoId == id);
            if (batalhao == null) return NotFound();

            return View(batalhao);
        }

        [HttpPost]
        [ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var batalhao = await _context.Batalhoes.FindAsync(id);
            _context.Batalhoes.Remove(batalhao);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BatalhaoExists(int id)
        {
            return _context.Batalhoes.Any(e => e.BatalhaoId == id);
        }



    }
}
