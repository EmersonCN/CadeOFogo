using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using CadeOFogo.Models.Inpe;
using Microsoft.AspNetCore.Identity;
using CadeOFogo.Areas.Cadastros;
using CadeOFogo.Areas.Cadastros.Models;
using CadeOFogo.Areas.Cadastros.ViewsModels.Equipe;

namespace CadeOFogo.Data
{
  public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
  {
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    public DbSet<CausadorProvavel> CausadoresProvaveis { get; set; }
    public DbSet<CausaFogo> CausasFogo { get; set; }
    public DbSet<IndicioInicioFoco> IndiciosInicioFoco { get; set; }
    public DbSet<ResponsavelPropriedade> ResponsaveisPropriedade { get; set; }
    public DbSet<StatusFoco> StatusFocos { get; set; }
    public DbSet<TipoVegetacao> TiposVegetacao { get; set; }
    public DbSet<Foco> Focos { get; set; }
    public DbSet<Satelite> Satelites { get; set; }
    public DbSet<Municipio> Municipios { get; set; }
    public DbSet<Estado> Estados { get; set; }
    public DbSet<Batalhao> Batalhoes { get; set; }
    public DbSet<Companhia> Companhias { get; set; }
    public DbSet<Pelotao> Pelotoes { get; set; }
    public DbSet<Equipe> Equipes { get; set; }
    public DbSet<ApplicationUser> ApplicationUsers { get; set; }

        //public DbSet<AreaComum> AreaComums { get; set; }
        //public DbSet<AreaPreservacaoPermanente> AreasPreservacaoPermanente { get; set; }
        //public DbSet<Arvore> Arvores { get; set; }
        //public DbSet<CanaDeAcucar> CanasDeAcucar { get; set; }
        //public DbSet<Ocorrencia> Ocorrencias { get; set; }
        //public DbSet<ReservaLegal> ReservasLegais { get; set; }
        //public DbSet<UnidadeDeConservacao> UnidadesDeConservacao { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
      base.OnConfiguring(optionsBuilder);
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
      base.OnModelCreating(builder);
    
      builder.ApplyConfiguration(new CausadorProvavelConfiguration());
      builder.ApplyConfiguration(new CausaFogoConfiguration());
      builder.ApplyConfiguration(new IndicioInicioFocoConfiguration());
      builder.ApplyConfiguration(new ResponsavelPropriedadeConfiguration());
      builder.ApplyConfiguration(new StatusFocoConfiguration());
      builder.ApplyConfiguration(new TipoVegetacaoConfiguration());
      builder.ApplyConfiguration(new SateliteConfiguration());
      builder.ApplyConfiguration(new FocoConfiguration());
      builder.ApplyConfiguration(new MunicipioConfiguration());
      builder.ApplyConfiguration(new EstadoConfiguration());
      builder.ApplyConfiguration(new BatalhaoConfiguration());
      builder.ApplyConfiguration(new CompanhiaConfiguration());
      builder.ApplyConfiguration(new PelotaoConfiguration());
      builder.ApplyConfiguration(new EquipeConfiguration());
      builder.ApplyConfiguration(new ApplicationUserConfiguration());
            builder.Entity<Batalhao>().HasData(
                  new Batalhao
                  {
                      BatalhaoId = 1,
                      NomeBatalhao = "4º Batalhão de Polícia Ambiental",
                  }
              );
            builder.Entity<Companhia>().HasData(
                  new Companhia
                  {
                      CompanhiaId = 1,
                      BatalhaoId = 1,
                      CompanhiaNome = "1º Companhia de São José do Rio Preto",
                  }
              );
            builder.Entity<Pelotao>().HasData(
                  new Pelotao
                  {
                      PelotaoId = 1,
                      CompanhiaId = 1,
                      BatalhaoId = 1,
                      PelotaoNome = "1º Pelotão de São José do Rio Preto",
                  }
              );

            builder.Entity<Equipe>().HasData(
                 new EquipeEditViewModel
                 {
                     EquipeId = 1,
                     EquipeNome = "Norte ",
                     PelotaoId = 1,
                     CompanhiaId = 1,
                     BatalhaoId = 1,
                     Ativa = true,
                     ApplicationUserUserId = "1"
                 }
             );

        }
  }
}