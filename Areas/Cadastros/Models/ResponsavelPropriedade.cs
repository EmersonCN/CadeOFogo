using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CadeOFogo.Models.Inpe
{
  public class ResponsavelPropriedade
  {
    [Display(Name = "Código do responsável pela propriedade", ShortName = "Cód.")]
    public int ResponsavelPropriedadeId { get; set; }

    [Required(ErrorMessage = "É obrigatório preencher o responsável")]
    [StringLength(maximumLength: 80,
      MinimumLength = 2,
      ErrorMessage = "O responsável deve ter entre 2 e 80 caracteres")]
    [DataType(DataType.Text)]
    [Display(Name = "Responsável pela propriedade",
      ShortName = "Responsável",
      Prompt = "Responsável...")]
    public string ResponsavelPropriedadeDescricao { get; set; }

        public ICollection<Foco> FocoCollection { get; set; }
    }
  
  public class ResponsavelPropriedadeConfiguration : IEntityTypeConfiguration<ResponsavelPropriedade>
  {
    public void Configure(EntityTypeBuilder<ResponsavelPropriedade> builder)
    {
      builder.HasKey(rp => rp.ResponsavelPropriedadeId);

      builder.Property(rp => rp.ResponsavelPropriedadeDescricao)
        .IsRequired()
        .HasMaxLength(80);

            builder.HasMany(p => p.FocoCollection)
                        .WithOne(p => p.ResponsavelPropriedade)
                        .HasForeignKey(p => p.ResponsavelPropriedadeId);

            builder.HasData(
        new ResponsavelPropriedade {ResponsavelPropriedadeId = 1, ResponsavelPropriedadeDescricao = "Não Informado no TVA/BO"},
        new ResponsavelPropriedade {ResponsavelPropriedadeId = 2, ResponsavelPropriedadeDescricao = "Proprietário Fornecedor"},
        new ResponsavelPropriedade {ResponsavelPropriedadeId = 3, ResponsavelPropriedadeDescricao = "Usina Arrendatário/Proprietária"},
        new ResponsavelPropriedade {ResponsavelPropriedadeId = 4, ResponsavelPropriedadeDescricao = "Empresa Arrendatário"},
        new ResponsavelPropriedade {ResponsavelPropriedadeId = 5, ResponsavelPropriedadeDescricao = "Prefeitura"},
        new ResponsavelPropriedade {ResponsavelPropriedadeId = 6, ResponsavelPropriedadeDescricao = "Proprietário (Não Usina)"}
      );
    }
  }
}
