using CadeOFogo.Models.Inpe;
using CadeOFogo.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CadeOFogo.Controllers
{
    public class UsuariosPapeisController : Controller
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<ApplicationUser> _userManager;

        public UsuariosPapeisController(UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager)
        {
            _roleManager = roleManager;
            _userManager = userManager;
        }

        private async Task<List<string>> GetUserRoles(ApplicationUser user)
        {
            var papeis = await _userManager.GetRolesAsync(user);
            return new List<string>(papeis.OrderBy(p => p.ToString()));
        }

        public async Task<IActionResult> Index()
        {
            var usuarios = await _userManager.Users.ToListAsync();
            var usuariosPapeisViewModel = new List<UsuariosPapeisViewModel>();
            foreach (var usuario in usuarios)
            {
                var vm = new UsuariosPapeisViewModel
                {
                    UserId = usuario.Id,
                    NomeCompleto = usuario.NomeCompleto,
                    UserName = usuario.UserName,
                    Email = usuario.Email,
                    Papeis = await GetUserRoles(usuario)

                };
                usuariosPapeisViewModel.Add(vm);
            }
            var usuarioPapeis = usuariosPapeisViewModel
                .OrderBy(up => up.NomeCompleto);
            return View(usuarioPapeis);
        }

        [HttpGet]
        public async Task<IActionResult> Gerenciar(string userId) 
        {
            ViewBag.userId = userId;
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            { 
                ViewBag.ErrorMessage = $"Usuário com Id = {userId} não foi encontrado";
                return NotFound();
            }
            ViewBag.UserName = user.UserName;
            var model = new List<GerenciarUsuariosPapeisViewModel>();
            foreach (var papel in _roleManager.Roles)
            {
                var upVM = new GerenciarUsuariosPapeisViewModel();

                upVM.PapelId = papel.Id;
                upVM.PapelNome = papel.Name;
                upVM.Selecionado = await _userManager.IsInRoleAsync(user, papel.Name);
                
                model.Add(upVM);
            } 
            return View(model); 
        }

        [HttpPost] 
        public async Task<IActionResult> Gerenciar(List<GerenciarUsuariosPapeisViewModel> model, string userId)
        { 
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                RedirectToAction("Index");
            }
            var roles = await _userManager.GetRolesAsync(user);
            var result = await _userManager.RemoveFromRolesAsync(user, roles);
            if (!result.Succeeded)
            {
                ModelState.AddModelError("", "Erro ao remover os papéis do usuário");
                return View(model);
            }
            result = await _userManager.AddToRolesAsync(user,
                model.Where(x => x.Selecionado)
                .Select(y => y.PapelNome));
            if (!result.Succeeded)
            { 
                ModelState.AddModelError("", "Erro ao adicionar os papéis ao usuário");
                return View(model);
            }
            return RedirectToAction("Index");
        }
    }
}