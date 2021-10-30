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
  public class IndiciosInicioFogo : Controller
  {
    private readonly ApplicationDbContext _context;
    private readonly int _pagesize;

    public IndiciosInicioFogo(ApplicationDbContext context, IConfiguration configuration)
    {
      _context = context;
      _pagesize = Convert.ToInt32(configuration
        .GetSection("ViewOptions")
        .GetSection("PageSizeCadastro")
        .Value);
      _pagesize = _pagesize != 0 ? _pagesize : 20;
    }

    // GET: IndiciosInicioFogo
    public async Task<IActionResult> Index(string keyword, int? page)
    {
      IQueryable<IndicioInicioFoco> dataset = _context.IndiciosInicioFoco
        .OrderBy(c => c.IndicioInicioFocoDescricao);

      if (!string.IsNullOrEmpty(keyword))
      {
        dataset = dataset.Where(c =>
          c.IndicioInicioFocoDescricao.Contains(keyword));
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

    // GET: IndiciosInicioFogo/Create
    public IActionResult Create()
    {
      return View();
    }

    // POST: IndiciosInicioFogo/Create
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("IndicioInicioFocoId,IndicioInicioFocoDescricao")]
      IndicioInicioFoco indicioInicioFoco)
    {
      if (ModelState.IsValid)
      {
        _context.Add(indicioInicioFoco);
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
      }

      return View(indicioInicioFoco);
    }

    // GET: IndiciosInicioFogo/Edit/5
    public async Task<IActionResult> Edit(int? id)
    {
      if (id == null) return NotFound();

      var indicioInicioFoco = await _context.IndiciosInicioFoco.FindAsync(id);
      if (indicioInicioFoco == null) return NotFound();
      return View(indicioInicioFoco);
    }

    // POST: IndiciosInicioFogo/Edit/5
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, [Bind("IndicioInicioFocoId,IndicioInicioFocoDescricao")]
      IndicioInicioFoco indicioInicioFoco)
    {
      if (id != indicioInicioFoco.IndicioInicioFocoId) return NotFound();

      if (ModelState.IsValid)
      {
        try
        {
          _context.Update(indicioInicioFoco);
          await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
          if (!IndicioInicioFocoExists(indicioInicioFoco.IndicioInicioFocoId))
            return NotFound();
          throw;
        }

        return RedirectToAction(nameof(Index));
      }

      return View(indicioInicioFoco);
    }

    // GET: IndiciosInicioFogo/Delete/5
    public async Task<IActionResult> Delete(int? id)
    {
      if (id == null) return NotFound();

      var indicioInicioFoco = await _context.IndiciosInicioFoco
        .FirstOrDefaultAsync(m => m.IndicioInicioFocoId == id);
      if (indicioInicioFoco == null) return NotFound();

      return View(indicioInicioFoco);
    }

    // POST: IndiciosInicioFogo/Delete/5
    [HttpPost]
    [ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
      var indicioInicioFoco = await _context.IndiciosInicioFoco.FindAsync(id);
      _context.IndiciosInicioFoco.Remove(indicioInicioFoco);
      await _context.SaveChangesAsync();
      return RedirectToAction(nameof(Index));
    }

    private bool IndicioInicioFocoExists(int id)
    {
      return _context.IndiciosInicioFoco.Any(e => e.IndicioInicioFocoId == id);
    }
  }
}