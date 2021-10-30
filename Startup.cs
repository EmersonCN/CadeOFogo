using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.EntityFrameworkCore;
using CadeOFogo.Data;
using CadeOFogo.Interfaces;
using CadeOFogo.Models.Inpe;
using CadeOFogo.Services;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;

namespace CadeOFogo
{
  public class Startup
  {
    public Startup(IConfiguration configuration)
    {
      Configuration = configuration;
    }

    public IConfiguration Configuration { get; }

    // This method gets called by the runtime. Use this method to add services to the container.
    public void ConfigureServices(IServiceCollection services)
    {
      services.AddDbContext<ApplicationDbContext>(options =>
        options.UseSqlServer(
          Configuration.GetConnectionString("DefaultConnection")
          ), ServiceLifetime.Transient
        );
      services.AddDatabaseDeveloperPageExceptionFilter();

      services.AddIdentity<ApplicationUser, IdentityRole>(options =>
        {
          options.SignIn.RequireConfirmedAccount = true;
          options.Password.RequiredLength = 6;
          options.Password.RequireDigit = true;
          options.Password.RequireLowercase = true;
          options.Password.RequireUppercase = true;
          options.User.RequireUniqueEmail = true;
        })
        .AddEntityFrameworkStores<ApplicationDbContext>()
        .AddDefaultUI()
        .AddDefaultTokenProviders();
      
      services.AddTransient<IEmailSender, EmailSender>();
      services.AddSingleton<IMapProvider>(provider =>
        //new GoogleMaps(Configuration.GetSection("GoogleMaps"))
        new Mapbox(Configuration.GetSection("MapBox"))
        // new OpenLayers(Configuration.GetSection("OpenLayers"))
      );
      services.Configure<AuthMessageSenderOptions>(Configuration);

      services.AddControllersWithViews();
    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
      if (env.IsDevelopment())
      {
        app.UseDeveloperExceptionPage();
        app.UseMigrationsEndPoint();
      }
      else
      {
        app.UseExceptionHandler("/Home/Error");
        // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
        app.UseHsts();
      }

      app.UseHttpsRedirection();
      app.UseStaticFiles();

      app.UseRouting();

      app.UseAuthentication();
      app.UseAuthorization();
      
      app.UseEndpoints(endpoints =>
      {
        endpoints.MapAreaControllerRoute(
          name : "Cadastros",
          areaName: "Cadastros",
          pattern : "Cadastros/{controller}/{action}/{id?}",
          defaults: new { controller = "Home", action = "Index" }
          );
        endpoints.MapControllerRoute(
          name: "default",
          pattern: "{controller}/{action}/{id?}",
          defaults: new {controller = "Home", action = "ListaFocos"}
          );
        endpoints.MapRazorPages();
      });
    }
  }
}