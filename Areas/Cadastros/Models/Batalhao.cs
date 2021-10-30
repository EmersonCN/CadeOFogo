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
    public class Batalhao
    {
        [Display(Name = "Código do Batalhão", ShortName = "Cód" )]
        public int BatalhaoId { get; set; }


        [Required(ErrorMessage = "É obrigatório preencher a Nome")]
        [StringLength(maximumLength: 280)]
        [Display(Name = "Batalhão")]
        public string NomeBatalhao { get; set; }
        public ICollection<Pelotao> PelotaoCollection { get; set; }
        public ICollection<Companhia> CompanhiaCollection { get; set; }
        public ICollection<Equipe> EquipeCollection { get; set; }


    }

    public class BatalhaoConfiguration : IEntityTypeConfiguration<Batalhao>
    {
        public void Configure(EntityTypeBuilder<Batalhao> builder)
        {
            builder.HasKey(b => b.BatalhaoId);

            builder.HasMany(c => c.CompanhiaCollection)
                .WithOne(c => c.Batalhao)
                .HasForeignKey(c => c.BatalhaoId);

            builder.HasMany(c => c.PelotaoCollection)
                .WithOne(c => c.Batalhao)
                .HasForeignKey(c => c.BatalhaoId);

            builder.HasMany(c => c.EquipeCollection)
                .WithOne(c => c.Batalhao)
                .HasForeignKey(c => c.BatalhaoId);
          
        }
    }
}
