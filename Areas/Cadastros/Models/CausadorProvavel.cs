using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CadeOFogo.Models.Inpe
{
  public class CausadorProvavel
  {
    [Display(Name = "Código do causador provável do foco", ShortName = "Cód.")]
    public int CausadorProvavelId { get; set; }
    
    [Required(ErrorMessage = "É obrigatório preencher a descrição")]
    [StringLength(maximumLength: 80,
      MinimumLength = 2,
      ErrorMessage = "A descrição deve ter entre 2 e 80 caracteres")]
    [DataType(DataType.Text)]
    [Display(Name = "Descrição do causador provável do foco",
             ShortName = "Causador",
             Prompt = "Descrição...")]
    public string CausadorProvavelDescricacao { get; set; }

        public ICollection<Foco> FocoCollection { get; set; }
    }
  
  public class CausadorProvavelConfiguration : IEntityTypeConfiguration<CausadorProvavel>
  {
    public void Configure(EntityTypeBuilder<CausadorProvavel> builder)
    {
      builder.HasKey(cp => cp.CausadorProvavelId);

      builder.Property(cp => cp.CausadorProvavelDescricacao)
        .IsRequired()
        .HasMaxLength(80);

            builder.HasMany(p => p.FocoCollection)
                        .WithOne(p => p.CausadorProvavel)
                        .HasForeignKey(p => p.CausadorProvavelId);

            builder.HasData(
        new CausadorProvavel {CausadorProvavelId = 1, CausadorProvavelDescricacao = "Funcionário"},
        new CausadorProvavel {CausadorProvavelId = 2, CausadorProvavelDescricacao = "Proprietário"},
        new CausadorProvavel {CausadorProvavelId = 3, CausadorProvavelDescricacao = "Outro Identificado"},
        new CausadorProvavel {CausadorProvavelId = 4, CausadorProvavelDescricacao = "Indeterminado"}
      );
    }
  }
}