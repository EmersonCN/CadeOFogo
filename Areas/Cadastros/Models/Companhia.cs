using CadeOFogo.Areas.Cadastros.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CadeOFogo.Models.Inpe
{
    public class Companhia
    {
        [Display(Name = "Código da Companhia", ShortName = "Cód")]
        public int CompanhiaId { get; set; }


        [Required(ErrorMessage = "É obrigatório preencher o Nome")]
        [StringLength(maximumLength: 280)]
        [Display(Name = "Companhia")]
        public string CompanhiaNome { get; set; }
        public Batalhao Batalhao { get; set; }
        public int BatalhaoId { get; set; }
        public ICollection<Pelotao> PelotaoCollection { get; set; }
        public ICollection<Equipe> EquipeCollection { get; set; }
    }

    public class CompanhiaConfiguration : IEntityTypeConfiguration<Companhia>
    {
        public void Configure(EntityTypeBuilder<Companhia> builder)
        {
            builder.HasKey(c => c.CompanhiaId);

            builder.HasOne(b => b.Batalhao)
                .WithMany(b => b.CompanhiaCollection)
                .HasForeignKey(c => c.BatalhaoId);

            builder.HasMany(p => p.PelotaoCollection)
                .WithOne(p => p.Companhia)
                .HasForeignKey(p => p.CompanhiaId);

            builder.HasMany(p => p.EquipeCollection)
                .WithOne(p => p.Companhia)
                .HasForeignKey(p => p.CompanhiaId);
        }
    }
}
