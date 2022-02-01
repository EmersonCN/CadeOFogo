﻿using CadeOFogo.Models.Inpe;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace CadeOFogo.Data
{
    public class ContextSeed
    {

        public static async Task SeedRolesAsync(UserManager<ApplicationUser> userManeger
            , RoleManager<IdentityRole> roleManeger)
        {
            await roleManeger.CreateAsync(new IdentityRole(Enums.Roles.AdminBatalhao.ToString()));
            await roleManeger.CreateAsync(new IdentityRole(Enums.Roles.AdminCompanhia.ToString()));
            await roleManeger.CreateAsync(new IdentityRole(Enums.Roles.AdminPelotao.ToString()));
            await roleManeger.CreateAsync(new IdentityRole(Enums.Roles.AdminEquipe.ToString()));
        }

        public static async Task SeedUberAdminAsync(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            var primeiroUsuario = new ApplicationUser
            {
                Id = "1",
                UserName = "EmersonCN",
                Email = "emersoncn2015@gmail.com",
                NomeCompleto = "Emerson Carlos Nogueira",
                EmailConfirmed = true,
                PhoneNumberConfirmed = true,
                PelotaoId = 1
            };

            if (userManager.Users.All(u => u.Id != primeiroUsuario.Id))
            {
                var user = await userManager.FindByEmailAsync(primeiroUsuario.Email);

                if (user == null)
                {
                    await userManager.CreateAsync(primeiroUsuario, "Awesome2021*!");
                    await userManager.AddToRoleAsync(primeiroUsuario, Enums.Roles.AdminBatalhao.ToString());
                    await userManager.AddToRoleAsync(primeiroUsuario, Enums.Roles.AdminCompanhia.ToString());
                    await userManager.AddToRoleAsync(primeiroUsuario, Enums.Roles.AdminPelotao.ToString());
                    await userManager.AddToRoleAsync(primeiroUsuario, Enums.Roles.AdminEquipe.ToString());
                    await userManager.AddToRoleAsync(primeiroUsuario, Enums.Roles.Usuario.ToString());
                }
            }
        }

    }

}