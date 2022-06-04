using CadeOFogo.Data;
using CadeOFogo.Models.Inpe;
using CadeOFogo.Services;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Threading.Tasks;

namespace CadeOFogo
{
  public class Program
  {
        public static async Task Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();
            using (var scope = host.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                try
                {
                    var context = services.GetRequiredService<ApplicationDbContext>();
                    var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();
                    var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
                    await ContextSeed.SeedRolesAsync(userManager, roleManager);
                    await ContextSeed.SeedUberAdminAsync(userManager, roleManager);

                }
                catch (Exception e)
                {
                    System.Console.WriteLine("Problemas ao fazer seed dos papeis");
                }
            }
            host.Run();
        }
// Background tasks
// https://dotnetcorecentral.com/blog/background-tasks/
// https://www.brunobrito.net.br/aspnet-core-background-services/
public static IHostBuilder CreateHostBuilder(string[] args)
    {
            return Host.CreateDefaultBuilder(args)
              .ConfigureWebHostDefaults(webBuilder => { webBuilder.UseStartup<Startup>(); })
         // .ConfigureServices(services => { services.AddHostedService<BootstrapFromInpe>(); });
           .ConfigureServices(services => { services.AddHostedService<ImportFireSpots>(); });
    }
  }
}