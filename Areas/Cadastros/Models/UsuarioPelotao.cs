using CadeOFogo.Models.Inpe;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CadeOFogo.Areas.Cadastros.Models
{
    public class UsuarioPelotao
    {
        public int UsuarioPeltao_Id { get; set; }

        public Pelotao Pelotao { get; set; }

        public int Pelotao_Id { get; set; }

        public ApplicationUser User { get; set; }

        public string User_Id { get; set; }
    }

    public class UsuarioPelotaoConfiguration : IEntityTypeConfiguration<UsuarioPelotao>
    {
        public void Configure(EntityTypeBuilder<UsuarioPelotao> builder)
        {
            builder.HasKey(p => p.UsuarioPeltao_Id);

            builder.HasAlternateKey(x => new {x.Pelotao_Id , x.User_Id });

            builder.HasOne(b => b.Pelotao)
                .WithMany(b => b.UsuariosPelotao)
                .HasForeignKey(b => b.Pelotao_Id)
                .OnDelete(DeleteBehavior.NoAction);

            builder.HasOne(b => b.User)
                .WithMany(b => b.UsuariosPelotao)
                .HasForeignKey(b => b.User_Id)
                .OnDelete(DeleteBehavior.NoAction);

        }
    }
}
