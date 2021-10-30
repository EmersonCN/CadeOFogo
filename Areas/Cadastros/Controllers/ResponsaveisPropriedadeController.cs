using System;
using System.Linq;
using System.Threading.Tasks;
using CadeOFogo.Data;
using CadeOFogo.Models.Inpe;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using X.PagedList;

namespace CadeOFogo.Controllers
{
  [Area("Cadastros")]
  public class ResponsaveisPropriedade : Controller
  {
    private readonly ApplicationDbContext _context;
    private readonly int _pagesize;

    public ResponsaveisPropriedade(ApplicationDbContext context, IConfiguration configuration)
    {
      _context = context;
      _pagesize = Convert.ToInt32(configuration
        .GetSection("ViewOptions")
        .GetSection("PageSizeCadastro")
        .Value);
      _pagesize = _pagesize != 0 ? _pagesize : 20;
    }

    // GET: ResponsaveisPropriedade
    public async Task<IActionResult> Index(string keyword, int? page)
    {
      IQueryable<ResponsavelPropriedade> dataset = _context.ResponsaveisPropriedade
        .OrderBy(c => c.ResponsavelPropriedadeDescricao);

      if (!string.IsNullOrEmpty(keyword))
      {
        dataset = dataset.Where(c =>
          c.ResponsavelPropriedadeDescricao.Contains(keyword));
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

    // GET: ResponsaveisPropriedade/Create
    public IActionResult Create()
    {
      return View();
    }

    // POST: ResponsaveisPropriedade/Create
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("ResponsavelPropriedadeId,ResponsavelPropriedadeDescricao")]
      ResponsavelPropriedade responsavelPropriedade)
    {
      if (ModelState.IsValid)
      {
        _context.Add(responsavelPropriedade);
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
      }

      return View(responsavelPropriedade);
    }

    // GET: ResponsaveisPropriedade/Edit/5
    public async Task<IActionResult> Edit(int? id)
    {
      if (id == null) return NotFound();

      var responsavelPropriedade = await _context.ResponsaveisPropriedade.FindAsync(id);
      if (responsavelPropriedade == null) return NotFound();
      return View(responsavelPropriedade);
    }

    // POST: ResponsaveisPropriedade/Edit/5
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, [Bind("ResponsavelPropriedadeId,ResponsavelPropriedadeDescricao")]
      ResponsavelPropriedade responsavelPropriedade)
    {
      if (id != responsavelPropriedade.ResponsavelPropriedadeId) return NotFound();

      if (ModelState.IsValid)
      {
        try
        {
          _context.Update(responsavelPropriedade);
          await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
          if (!ResponsavelPropriedadeExists(responsavelPropriedade.ResponsavelPropriedadeId))
            return NotFound();
          throw;
        }

        return RedirectToAction(nameof(Index));
      }

      return View(responsavelPropriedade);
    }

    // GET: ResponsaveisPropriedade/Delete/5
    public async Task<IActionResult> Delete(int? id)
    {
      if (id == null) return NotFound();

      var responsavelPropriedade = await _context.ResponsaveisPropriedade
        .FirstOrDefaultAsync(m => m.ResponsavelPropriedadeId == id);
      if (responsavelPropriedade == null) return NotFound();

      return View(responsavelPropriedade);
    }

    // POST: ResponsaveisPropriedade/Delete/5
    [HttpPost]
    [ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
      var responsavelPropriedade = await _context.ResponsaveisPropriedade.FindAsync(id);
      _context.ResponsaveisPropriedade.Remove(responsavelPropriedade);
      await _context.SaveChangesAsync();
      return RedirectToAction(nameof(Index));
    }

    private bool ResponsavelPropriedadeExists(int id)
    {
      return _context.ResponsaveisPropriedade.Any(e => e.ResponsavelPropriedadeId == id);
    }
  }
}