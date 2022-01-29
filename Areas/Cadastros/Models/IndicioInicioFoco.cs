using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CadeOFogo.Models.Inpe
{
  public class IndicioInicioFoco
  {
    [Display(Name = "Código do indício de início do foco", ShortName = "Cód.")]
    public int IndicioInicioFocoId { get; set; }
    
    
    [Required(ErrorMessage = "É obrigatório preencher a descrição")]
    [StringLength(maximumLength: 80,
      MinimumLength = 2,
      ErrorMessage = "A descrição deve ter entre 2 e 80 caracteres")]
    [DataType(DataType.Text)]
    [Display(Name = "Descrição do indício de início do foco",
      ShortName = "Indício",
      Prompt = "Descrição...")]
    public string IndicioInicioFocoDescricao { get; set; }

        public ICollection<Foco> FocoCollection { get; set; }
    }
  
  public class IndicioInicioFocoConfiguration : IEntityTypeConfiguration<IndicioInicioFoco>
  {
    public void Configure(EntityTypeBuilder<IndicioInicioFoco> builder)
    {
      builder.HasKey(iif => iif.IndicioInicioFocoId);

      builder.Property(iif => iif.IndicioInicioFocoDescricao)
        .IsRequired()
        .HasMaxLength(80);

            builder.HasMany(p => p.FocoCollection)
                        .WithOne(p => p.IndicioInicioFoco)
                        .HasForeignKey(p => p.IndicioInicioFocoId);

            builder.HasData(
        new IndicioInicioFoco {IndicioInicioFocoId = 1, IndicioInicioFocoDescricao = "Dentro da Propriedade"},
        new IndicioInicioFoco {IndicioInicioFocoId = 2, IndicioInicioFocoDescricao = "Rodovia/Estrada"},
        new IndicioInicioFoco {IndicioInicioFocoId = 3, IndicioInicioFocoDescricao = "Indeterminado"}
      );
    }
  }
}