using System;
using System.Linq;
using System.Threading.Tasks;
using CadeOFogo.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using X.PagedList;

namespace CadeOFogo.Models.Inpe
{
  [Area("Cadastros")]
  public class CausasFogo : Controller
  {
    private readonly ApplicationDbContext _context;
    private readonly int _pagesize;

    public CausasFogo(ApplicationDbContext context, IConfiguration configuration)
    {
      _context = context;
      _pagesize = Convert.ToInt32(configuration
        .GetSection("ViewOptions")
        .GetSection("PageSizeCadastro")
        .Value);
      _pagesize = _pagesize != 0 ? _pagesize : 20;
    }

    // GET: CausasFogo
    public async Task<IActionResult> Index(string keyword, int? page)
    {
      IQueryable<CausaFogo> dataset = _context.CausasFogo
        .OrderBy(c => c.CausaFogoDescricao);

      if (!string.IsNullOrEmpty(keyword))
      {
        dataset = dataset.Where(c =>
          c.CausaFogoDescricao.Contains(keyword));
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

    // GET: CausasFogo/Create
    public IActionResult Create()
    {
      return View();
    }

    // POST: CausasFogo/Create
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("CausaFogoId,CausaFogoDescricao")]
      CausaFogo causaFogo)
    {
      if (ModelState.IsValid)
      {
        _context.Add(causaFogo);
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
      }

      return View(causaFogo);
    }

    // GET: CausasFogo/Edit/5
    public async Task<IActionResult> Edit(int? id)
    {
      if (id == null) return NotFound();

      var causaFogo = await _context.CausasFogo.FindAsync(id);
      if (causaFogo == null) return NotFound();
      return View(causaFogo);
    }

    // POST: CausasFogo/Edit/5
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, [Bind("CausaFogoId,CausaFogoDescricao")]
      CausaFogo causaFogo)
    {
      if (id != causaFogo.CausaFogoId) return NotFound();

      if (ModelState.IsValid)
      {
        try
        {
          _context.Update(causaFogo);
          await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
          if (!CausaFogoExists(causaFogo.CausaFogoId))
            return NotFound();
          throw;
        }

        return RedirectToAction(nameof(Index));
      }

      return View(causaFogo);
    }

    // GET: CausasFogo/Delete/5
    public async Task<IActionResult> Delete(int? id)
    {
      if (id == null) return NotFound();

      var causaFogo = await _context.CausasFogo
        .FirstOrDefaultAsync(m => m.CausaFogoId == id);
      if (causaFogo == null) return NotFound();

      return View(causaFogo);
    }

    // POST: CausasFogo/Delete/5
    [HttpPost]
    [ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
      var causaFogo = await _context.CausasFogo.FindAsync(id);
      _context.CausasFogo.Remove(causaFogo);
      await _context.SaveChangesAsync();
      return RedirectToAction(nameof(Index));
    }

    private bool CausaFogoExists(int id)
    {
      return _context.CausasFogo.Any(e => e.CausaFogoId == id);
    }
  }
}