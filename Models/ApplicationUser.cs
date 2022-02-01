using CadeOFogo.Areas.Cadastros.Models;
using CadeOFogo.Models.Inpe;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;

namespace CadeOFogo.Models.Inpe
{
  public class ApplicationUser : IdentityUser
  {
    public string NomeCompleto { get; set; }
   
    public byte[] FotoPerfil { get; set; }

    public Pelotao Pelotao { get; set; }

    public int PelotaoId { get; set; }

        public ICollection<Equipe> EquipeCollection { get; set; }


    }
}

public class ApplicationUserConfiguration : IEntityTypeConfiguration<ApplicationUser>
{
    public void Configure(EntityTypeBuilder<ApplicationUser> builder)
    {
        builder.HasOne(b => b.Pelotao)
                .WithMany(b => b.Usuarios)
                .HasForeignKey(b => b.PelotaoId);
    }
}