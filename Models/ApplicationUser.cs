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

    public ICollection<UsuarioPelotao> UsuariosPelotao { get; set; }

    public ICollection<Equipe> EquipeCollection { get; set; }


    }
}

public class ApplicationUserConfiguration : IEntityTypeConfiguration<ApplicationUser>
{
    public void Configure(EntityTypeBuilder<ApplicationUser> builder)
    {
        builder.HasMany(b => b.UsuariosPelotao)
                .WithOne(b => b.User)
                .HasForeignKey(b => b.User_Id);
    }
}