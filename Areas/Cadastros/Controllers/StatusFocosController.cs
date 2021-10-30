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
  public class StatusFocos : Controller
  {
    private readonly ApplicationDbContext _context;
    private readonly int _pagesize;

    public StatusFocos(ApplicationDbContext context, IConfiguration configuration)
    {
      _context = context;
      _pagesize = Convert.ToInt32(configuration
        .GetSection("ViewOptions")
        .GetSection("PageSizeCadastro")
        .Value);
      _pagesize = _pagesize != 0 ? _pagesize : 20;
    }
  

  // GET: StatusFocos
    public async Task<IActionResult> Index(string keyword, int? page)
    {
      IQueryable<StatusFoco> dataset = _context.StatusFocos
        .OrderBy(c => c.StatusFocoDescricao);

      if (!string.IsNullOrEmpty(keyword))
      {
        dataset = dataset.Where(c =>
          c.StatusFocoDescricao.Contains(keyword));
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

    // GET: StatusFocos/Create
    public IActionResult Create()
    {
      return View();
    }

    // POST: StatusFocos/Create
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("StatusFocoId,StatusFocoDescricao")]
      StatusFoco statusFoco)
    {
      if (ModelState.IsValid)
      {
        _context.Add(statusFoco);
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
      }

      return View(statusFoco);
    }

    // GET: StatusFocos/Edit/5
    public async Task<IActionResult> Edit(int? id)
    {
      if (id == null) return NotFound();

      var statusFoco = await _context.StatusFocos.FindAsync(id);
      if (statusFoco == null) return NotFound();
      return View(statusFoco);
    }

    // POST: StatusFocos/Edit/5
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, [Bind("StatusFocoId,StatusFocoDescricao")]
      StatusFoco statusFoco)
    {
      if (id != statusFoco.StatusFocoId) return NotFound();

      if (ModelState.IsValid)
      {
        try
        {
          _context.Update(statusFoco);
          await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
          if (!StatusFocoExists(statusFoco.StatusFocoId))
            return NotFound();
          throw;
        }

        return RedirectToAction(nameof(Index));
      }

      return View(statusFoco);
    }

    // GET: StatusFocos/Delete/5
    public async Task<IActionResult> Delete(int? id)
    {
      if (id == null) return NotFound();

      var statusFoco = await _context.StatusFocos
        .FirstOrDefaultAsync(m => m.StatusFocoId == id);
      if (statusFoco == null) return NotFound();

      return View(statusFoco);
    }

    // POST: StatusFocos/Delete/5
    [HttpPost]
    [ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
      var statusFoco = await _context.StatusFocos.FindAsync(id);
      _context.StatusFocos.Remove(statusFoco);
      await _context.SaveChangesAsync();
      return RedirectToAction(nameof(Index));
    }

    private bool StatusFocoExists(int id)
    {
      return _context.StatusFocos.Any(e => e.StatusFocoId == id);
    }
  }
}