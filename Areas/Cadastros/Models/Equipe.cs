using CadeOFogo.Models.Inpe;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CadeOFogo.Areas.Cadastros.Models
{
    public class Equipe
    {
        public int EquipeId { get; set; }

        public string EquipeNome { get; set; }

        public Batalhao Batalhao { get; set; }

        public int BatalhaoId { get; set; }
       
        public Companhia Companhia { get; set; }

        public int CompanhiaId { get; set; }

        public Pelotao Pelotao { get; set; }

        public int PelotaoId { get; set; }

        public bool Ativa { get; set; }

        public ApplicationUser ApplicationUser { get; set; }

    }

   

    public class EquipeConfiguration : IEntityTypeConfiguration<Equipe>
    {
        public void Configure(EntityTypeBuilder<Equipe> builder)
        {
            builder.HasKey(c => c.EquipeId);

            builder.HasOne(c => c.Batalhao)
                   .WithMany(c => c.EquipeCollection)
                   .HasForeignKey(c => c.BatalhaoId)
                   .OnDelete(DeleteBehavior.NoAction);

            builder.HasOne(c => c.Companhia)
                .WithMany(c => c.EquipeCollection)
                .HasForeignKey(c => c.CompanhiaId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.HasOne(c => c.Pelotao)
                .WithMany(c => c.EquipeCollection)
                .HasForeignKey(c => c.PelotaoId)
                .OnDelete(DeleteBehavior.NoAction);


        }
    }
}
