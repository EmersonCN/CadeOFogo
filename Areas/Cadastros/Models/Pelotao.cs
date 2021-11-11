using CadeOFogo.Areas.Cadastros.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using X.PagedList;

namespace CadeOFogo.Models.Inpe
{
    public class Pelotao
    {
        [Display(Name = "Código do Pelotão", ShortName = "Cód")]
        public int PelotaoId { get; set; }


        [Required(ErrorMessage = "É obrigatório preencher o Nome")]
        [StringLength(maximumLength: 280)]
        [Display(Name = "Pelotão")]
        public string PelotaoNome { get; set; }
        public virtual Companhia Companhia { get; set; }
        public int CompanhiaId { get; set; }
        public virtual Batalhao Batalhao { get; set; }
        public int BatalhaoId { get; set; }
        public ICollection<Equipe> EquipeCollection { get; set; }
        public ICollection<ApplicationUser> Usuarios { get; set; }


    }

    public class PelotaoConfiguration : IEntityTypeConfiguration<Pelotao>
    {
        public void Configure(EntityTypeBuilder<Pelotao> builder)
        {
            builder.HasKey(p => p.PelotaoId);

            builder.HasOne(b => b.Companhia)
                .WithMany(b => b.PelotaoCollection)
                .HasForeignKey(b => b.CompanhiaId);

            builder.HasOne(b => b.Batalhao)
                .WithMany(b => b.PelotaoCollection)
                .HasForeignKey(b => b.BatalhaoId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.HasMany(b => b.EquipeCollection)
                .WithOne(b => b.Pelotao)
                .HasForeignKey(b => b.PelotaoId);

            builder.HasMany(b => b.Usuarios)
                .WithOne(b => b.Pelotao);

        }
    }
}
