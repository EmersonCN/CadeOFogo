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
  public class CausadoresProvaveis : Controller
  {
    private readonly ApplicationDbContext _context;
    private readonly int _pagesize;

    public CausadoresProvaveis(ApplicationDbContext context, IConfiguration configuration)
    {
      _context = context;
      _pagesize = Convert.ToInt32(configuration
        .GetSection("ViewOptions")
        .GetSection("PageSizeCadastro")
        .Value);
      _pagesize = _pagesize != 0 ? _pagesize : 20;
    }

    // GET: CausadoresProvaveis
    public async Task<IActionResult> Index(string keyword, int? page)
    {
      IQueryable<CausadorProvavel> dataset = _context.CausadoresProvaveis
        .OrderBy(c => c.CausadorProvavelDescricacao);

      if (!string.IsNullOrEmpty(keyword))
      {
        dataset = dataset.Where(c =>
          c.CausadorProvavelDescricacao.Contains(keyword));
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

    // GET: CausadoresProvaveis/Create
    public IActionResult Create()
    {
      return View();
    }

    // POST: CausadoresProvaveis/Create
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("CausadorProvavelId,CausadorProvavelDescricacao")]
      CausadorProvavel causadorProvavel)
    {
      if (ModelState.IsValid)
      {
        _context.Add(causadorProvavel);
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
      }

      return View(causadorProvavel);
    }

    // GET: CausadoresProvaveis/Edit/5
    public async Task<IActionResult> Edit(int? id)
    {
      if (id == null) return NotFound();

      var causadorProvavel = await _context.CausadoresProvaveis.FindAsync(id);
      if (causadorProvavel == null) return NotFound();
      return View(causadorProvavel);
    }

    // POST: CausadoresProvaveis/Edit/5
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, [Bind("CausadorProvavelId,CausadorProvavelDescricacao")]
      CausadorProvavel causadorProvavel)
    {
      if (id != causadorProvavel.CausadorProvavelId) return NotFound();

      if (ModelState.IsValid)
      {
        try
        {
          _context.Update(causadorProvavel);
          await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
          if (!CausadorProvavelExists(causadorProvavel.CausadorProvavelId))
            return NotFound();
          throw;
        }

        return RedirectToAction(nameof(Index));
      }

      return View(causadorProvavel);
    }

    // GET: CausadoresProvaveis/Delete/5
    public async Task<IActionResult> Delete(int? id)
    {
      if (id == null) return NotFound();

      var causadorProvavel = await _context.CausadoresProvaveis
        .FirstOrDefaultAsync(m => m.CausadorProvavelId == id);
      if (causadorProvavel == null) return NotFound();

      return View(causadorProvavel);
    }

    // POST: CausadoresProvaveis/Delete/5
    [HttpPost]
    [ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
      var causadorProvavel = await _context.CausadoresProvaveis.FindAsync(id);
      _context.CausadoresProvaveis.Remove(causadorProvavel);
      await _context.SaveChangesAsync();
      return RedirectToAction(nameof(Index));
    }

    private bool CausadorProvavelExists(int id)
    {
      return _context.CausadoresProvaveis.Any(e => e.CausadorProvavelId == id);
    }
  }
}