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
  public class TiposVegetacao : Controller
  {
    private readonly ApplicationDbContext _context;
    private readonly int _pagesize;

    public TiposVegetacao(ApplicationDbContext context, IConfiguration configuration)
    {
      _context = context;
      _pagesize = Convert.ToInt32(configuration
        .GetSection("ViewOptions")
        .GetSection("PageSizeCadastro")
        .Value);
      _pagesize = _pagesize != 0 ? _pagesize : 20;
    }

    // GET: TiposVegetacao
    public async Task<IActionResult> Index(string keyword, int? page)
    {
      IQueryable<TipoVegetacao> dataset = _context.TiposVegetacao
        .OrderBy(c => c.TipoVegetacaoDescricao);

      if (!string.IsNullOrEmpty(keyword))
      {
        dataset = dataset.Where(c =>
          c.TipoVegetacaoDescricao.Contains(keyword));
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

    // GET: TiposVegetacao/Create
    public IActionResult Create()
    {
      return View();
    }

    // POST: TiposVegetacao/Create
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("TipoVegetacaoId,TipoVegetacaoDescricao")]
      TipoVegetacao tipoVegetacao)
    {
      if (ModelState.IsValid)
      {
        _context.Add(tipoVegetacao);
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
      }

      return View(tipoVegetacao);
    }

    // GET: TiposVegetacao/Edit/5
    public async Task<IActionResult> Edit(int? id)
    {
      if (id == null) return NotFound();

      var tipoVegetacao = await _context.TiposVegetacao.FindAsync(id);
      if (tipoVegetacao == null) return NotFound();
      return View(tipoVegetacao);
    }

    // POST: TiposVegetacao/Edit/5
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, [Bind("TipoVegetacaoId,TipoVegetacaoDescricao")]
      TipoVegetacao tipoVegetacao)
    {
      if (id != tipoVegetacao.TipoVegetacaoId) return NotFound();

      if (ModelState.IsValid)
      {
        try
        {
          _context.Update(tipoVegetacao);
          await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
          if (!TipoVegetacaoExists(tipoVegetacao.TipoVegetacaoId))
            return NotFound();
          throw;
        }

        return RedirectToAction(nameof(Index));
      }

      return View(tipoVegetacao);
    }

    // GET: TiposVegetacao/Delete/5
    public async Task<IActionResult> Delete(int? id)
    {
      if (id == null) return NotFound();

      var tipoVegetacao = await _context.TiposVegetacao
        .FirstOrDefaultAsync(m => m.TipoVegetacaoId == id);
      if (tipoVegetacao == null) return NotFound();

      return View(tipoVegetacao);
    }

    // POST: TiposVegetacao/Delete/5
    [HttpPost]
    [ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
      var tipoVegetacao = await _context.TiposVegetacao.FindAsync(id);
      _context.TiposVegetacao.Remove(tipoVegetacao);
      await _context.SaveChangesAsync();
      return RedirectToAction(nameof(Index));
    }

    private bool TipoVegetacaoExists(int id)
    {
      return _context.TiposVegetacao.Any(e => e.TipoVegetacaoId == id);
    }
  }
}