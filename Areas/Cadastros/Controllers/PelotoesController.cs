using CadeOFogo.Areas.Cadastros.Models;
using CadeOFogo.Areas.Cadastros.ViewsModels.Pelotao;
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
    public class Pelotoes : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly int _pagesize;
        private readonly UserManager<ApplicationUser> _userManager;

        public Pelotoes(ApplicationDbContext context, IConfiguration configuration, UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
            _context = context;
            _pagesize = Convert.ToInt32(configuration
            .GetSection("ViewOptions")
            .GetSection("PageSizeCadastro")
            .Value);
            _pagesize = _pagesize != 0 ? _pagesize : 20;
        }

        private async Task<List<ApplicationUser>> GetUser(Pelotao pelotao)
        {
            var users = new List<ApplicationUser>();
            var usuarios = _context.UsuarioPelotao.Where(x => x.Pelotao_Id == pelotao.PelotaoId).ToList();
            foreach(var usuario in usuarios)
            {
                var user = await _userManager.Users.Where(x => x.Id == usuario.User_Id).FirstOrDefaultAsync();
                users.Add(user);
            }
            return users;
        }

        public async Task<IActionResult> Index(string keyword, int? pagina)
        {
            var dataset = _context.Pelotoes
              .Include(p => p.Companhia)
              .Include(d => d.Batalhao)
              .OrderBy(p => p.PelotaoNome);

            if (!string.IsNullOrWhiteSpace(keyword))
            {
                dataset = (IOrderedQueryable<Pelotao>)dataset
                  .Where(p => p.PelotaoNome.Contains(keyword.ToUpper()));
                ViewBag.keyword = keyword;
            }
            else
            {
                ViewBag.keyword = "";
            }

            var pelotaoViewModel = await dataset
              .Select(pelotao => new PelotaoIndexViewModel
              {
                  PelotaoId = pelotao.PelotaoId,
                  PelotaoNome = pelotao.PelotaoNome,
                  CompanhiaNome = pelotao.Companhia.CompanhiaNome,
                  NomeBatalhao = pelotao.Batalhao.NomeBatalhao

              }).ToPagedListAsync(pagina ?? 1, _pagesize);

            ViewBag.primeiro = pelotaoViewModel.FirstItemOnPage;
            ViewBag.ultimo = pelotaoViewModel.LastItemOnPage;
            ViewBag.total = pelotaoViewModel.TotalItemCount;

            return View(pelotaoViewModel);
        }

        public async Task<IActionResult> Create()
        {
            var pelotao = new PelotaoEditViewModel
            {
                CompanhiaId = 0,
                CompanhiaInputSelect = new SelectList(await _context.Companhias
                  .OrderBy(c => c.CompanhiaNome).ToListAsync(),
                dataValueField: "CompanhiaId",
                dataTextField: "CompanhiaNome"),

                BatalhaoId = 0,
                BatalhaoInputSelect = new SelectList(await _context.Batalhoes
                  .OrderBy(c => c.NomeBatalhao).ToListAsync(),
                dataValueField: "BatalhaoId",
                dataTextField: "NomeBatalhao")

            };
            return View(pelotao);
        }

        [HttpPost, ActionName("Create")]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> CreateConfirm(PelotaoEditViewModel pelotao)
        {
            if (pelotao.CompanhiaId == 0)
            {
                pelotao.CompanhiaInputSelect = new SelectList(await _context.Companhias
                  .OrderBy(c => c.CompanhiaNome).ToListAsync(),
                  dataValueField: "CompanhiaId",
                  dataTextField: "CompanhiaNome");
                ModelState.AddModelError("CompanhiaInputSelect", "Selecione uma companhia");
                return View(pelotao);
            }

            if (pelotao.BatalhaoId == 0)
            {
                pelotao.BatalhaoInputSelect = new SelectList(await _context.Batalhoes
                 .OrderBy(c => c.NomeBatalhao).ToListAsync(),
                 dataValueField: "BatalhaoId",
                 dataTextField: "NomeBatalhao");
                ModelState.AddModelError("BatalhaoInputSelect", "Selecione uma batalhão");
                return View(pelotao);
            }

            var companhia = await _context.Companhias.FindAsync(pelotao.CompanhiaId);
            if (companhia == null)
                return NotFound();

            var batalhao = await _context.Batalhoes.FindAsync(pelotao.BatalhaoId);
            if (batalhao == null)
                return NotFound();
            var newpelotao = new Pelotao
            {
                PelotaoId = pelotao.PelotaoId,
                PelotaoNome = pelotao.PelotaoNome,
                Companhia = companhia,
                Batalhao = batalhao

            };
            await _context.Pelotoes.AddAsync(newpelotao);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return RedirectToAction(nameof(Index));

            var pelotao = await _context.Pelotoes
              .Include(p => p.Companhia)
              .Include(p => p.Batalhao)
              .FirstOrDefaultAsync(p => p.PelotaoId == id);

            if (pelotao == null)
                return NotFound();

            var pelotaoViewModel = new PelotaoEditViewModel
            {
                PelotaoId = pelotao.PelotaoId,
                PelotaoNome = pelotao.PelotaoNome,
                CompanhiaId = pelotao.CompanhiaId,
                CompanhiaInputSelect = new SelectList(await _context.Companhias
                  .OrderBy(c => c.CompanhiaNome).ToListAsync(),
                dataValueField: "CompanhiaId",
                dataTextField: "CompanhiaNome"),
                BatalhaoId = pelotao.BatalhaoId,
                BatalhaoInputSelect = new SelectList(await _context.Batalhoes
                  .OrderBy(c => c.NomeBatalhao).ToListAsync(),
                dataValueField: "BatalhaoId",
                dataTextField: "NomeBatalhao")

            };

            return View(pelotaoViewModel);
        }

        [HttpPost, ActionName("Edit")]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> EditConfirm(int? id, PelotaoEditViewModel pelotao)
        {
            if ((id == null) || (id != pelotao.PelotaoId))
                return NotFound();

            if (!ModelState.IsValid)
            {
                var newPelotaoEditViewModel = new PelotaoEditViewModel
                {
                    PelotaoId = pelotao.PelotaoId,
                    PelotaoNome = pelotao.PelotaoNome,
                    CompanhiaId = pelotao.CompanhiaId,
                    CompanhiaInputSelect = new SelectList(await _context.Companhias.ToListAsync(),
                   dataValueField: "CompanhiaId",
                   dataTextField: "CompanhiaNome"),
                    BatalhaoId = pelotao.BatalhaoId,
                    BatalhaoInputSelect = new SelectList(await _context.Batalhoes.ToListAsync(),
                   dataValueField: "BatalhaoId",
                   dataTextField: "NomeBatalhao")

                };
                return View(newPelotaoEditViewModel);
            }

            var pelotaoOriginal = await _context.Pelotoes
              .Include(p => p.Companhia)
              .Include(p => p.Batalhao)
              .FirstOrDefaultAsync(p => p.PelotaoId == pelotao.PelotaoId);

            if (pelotaoOriginal.PelotaoNome != pelotao.PelotaoNome)
                pelotaoOriginal.PelotaoNome = pelotao.PelotaoNome;

            if (pelotaoOriginal.CompanhiaId != pelotao.CompanhiaId)
            {
                var companhia = await _context.Companhias.FindAsync(pelotao.CompanhiaId);

                if (companhia == null)
                    return NotFound();

                pelotaoOriginal.Companhia = companhia;
            }

            if (pelotaoOriginal.BatalhaoId != pelotao.BatalhaoId)
            {
                var batalhao = await _context.Batalhoes.FindAsync(pelotao.BatalhaoId);

                if (batalhao == null)
                    return NotFound();

                pelotaoOriginal.Batalhao = batalhao;
            }

            if (_context.Entry(pelotaoOriginal).State == EntityState.Modified)
            {
                try
                {
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException e)
                {
                    if (!_context.Pelotoes.Any(p => p.PelotaoId == pelotaoOriginal.PelotaoId))
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

            var pelotao = await _context.Pelotoes
              .Include(p => p.Companhia)
              .FirstOrDefaultAsync(p => p.PelotaoId == id);

            if (pelotao == null)
                return NotFound();

            var pelotaoViewModel = new PelotaoDetailsViewModel
            {
                PelotaoId = pelotao.PelotaoId,
                PelotaoNome = pelotao.PelotaoNome,
                CompanhiaNome = pelotao.Companhia.CompanhiaNome
            };
            return View(pelotaoViewModel);
        }

        [HttpPost, ActionName("Delete")]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> DeleteConfirm(int? id)
        {
            if (id == null)
                return NotFound();

            var pelotao = await _context.Pelotoes.FindAsync(id);
            if (pelotao == null)
                return NotFound();

            _context.Remove(pelotao);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException e)
            {
                if (!await _context.Pelotoes.AnyAsync(p => p.PelotaoId == id))
                    return NotFound();
                Console.WriteLine(e);
                throw;
            }
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Detalhes(int id)
        {
            if (id == null)
                return NotFound();

            var pelotao = await _context.Pelotoes
              .FirstOrDefaultAsync(p => p.PelotaoId == id);

            if (pelotao == null)
                return NotFound();

            var pelotaoViewModel = new PelotaoDetailsViewModel
            {
                PelotaoId = pelotao.PelotaoId,
                PelotaoNome = pelotao.PelotaoNome,
                Usuarios = await GetUser(pelotao)
             };
            return View(pelotaoViewModel);
        }


        [HttpGet]
        public async Task<IActionResult> Gerenciar(int? pelotaoId)
        {

            if (pelotaoId == null)
                return RedirectToAction(nameof(Index));

            var pelotao = await _context.Pelotoes
              .FirstOrDefaultAsync(p => p.PelotaoId == pelotaoId);

            if (pelotao == null)
                return NotFound();

            var model = new List<GerenciarUsuariosViewModel>();
            var usuarios = _userManager.Users.ToList();
            foreach (var user in usuarios)
            {
                var usuarioPelotao = _context.UsuarioPelotao.Where(x => x.Pelotao_Id == pelotaoId && x.User_Id == user.Id).FirstOrDefault();
                if (user.UsuariosPelotao == usuarioPelotao)
                {
                    var upVM2 = new GerenciarUsuariosViewModel
                    {
                        UserId = user.Id,
                        NomeCompleto = user.NomeCompleto,
                        Selecionado = false
                    };

                    model.Add(upVM2);
                }
                else
                {
                    var upVM = new GerenciarUsuariosViewModel
                    {
                        UserId = user.Id,
                        NomeCompleto = user.NomeCompleto,
                        Selecionado = true
                    };

                    model.Add(upVM);
                }
                
            }
            return View(model);
        }
        
        [HttpPost]
        public async Task<IActionResult> Gerenciar(List<GerenciarUsuariosViewModel> model, int? pelotaoId)
        {
            
            var pelotao = await _context.Pelotoes.FindAsync(pelotaoId);
            if (pelotao == null)
            {
                return RedirectToAction(nameof(Index));
            }

            foreach(var policial in model)
            {
                if (policial.Selecionado)
                {
                    var p = await _context.ApplicationUsers.FindAsync(policial.UserId);
                    var up = _context.UsuarioPelotao.Where(x => x.Pelotao_Id == pelotao.PelotaoId && x.User_Id == p.Id).FirstOrDefault();
                    if (up != null)
                    {
                        continue;
                    }
                    else
                    {
                        var usuarioPelotao = new UsuarioPelotao()
                        {
                            User_Id = p.Id,
                            Pelotao_Id = pelotao.PelotaoId
                        };
                        _context.UsuarioPelotao.Add(usuarioPelotao);
                    }                    
                }
                else
                {
                    var p = await _context.ApplicationUsers.FindAsync(policial.UserId);
                    var up = _context.UsuarioPelotao.Where(x => x.Pelotao_Id == pelotao.PelotaoId && x.User_Id == p.Id).FirstOrDefault();
                    if (up == null)
                    {
                        continue;
                    }
                    else
                    {
                        _context.UsuarioPelotao.Remove(up);
                    }
                }

                _context.SaveChanges();
            }
            return RedirectToAction(nameof(Index));

        }
    }
}
