using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CadeOFogo.Models.Inpe
{
  public class CausaFogo
  {
    [Display(Name = "Código da causa provável do foco", ShortName = "Cód.")]
    public int CausaFogoId { get; set; }

    [Required(ErrorMessage = "É obrigatório preencher a descrição")]
    [StringLength(maximumLength: 80,
      MinimumLength = 2,
      ErrorMessage = "A descrição deve ter entre 2 e 80 caracteres")]
    [DataType(DataType.Text)]
    [Display(Name = "Descrição da causa provável do foco",
             ShortName = "Causa",
             Prompt = "Descrição...")]
    public string CausaFogoDescricao { get; set; }
  }
  
  public class CausaFogoConfiguration : IEntityTypeConfiguration<CausaFogo>
  {
    public void Configure(EntityTypeBuilder<CausaFogo> builder)
    {
      builder.HasKey(cf => cf.CausaFogoId);

      builder.Property(cf => cf.CausaFogoDescricao)
        .IsRequired()
        .HasMaxLength(80);

      builder.HasData(
        new CausaFogo { CausaFogoId = 1, CausaFogoDescricao = "Desconhecida" },
        new CausaFogo { CausaFogoId = 2, CausaFogoDescricao = "Queima de Cana" },
        new CausaFogo { CausaFogoId = 3, CausaFogoDescricao = "Queima para Limpeza" },
        new CausaFogo { CausaFogoId = 4, CausaFogoDescricao = "Queima de resto de cultura" },
        new CausaFogo { CausaFogoId = 5, CausaFogoDescricao = "Vandalismo" },
        new CausaFogo { CausaFogoId = 6, CausaFogoDescricao = "Raio/Causas naturais" },
        new CausaFogo { CausaFogoId = 7, CausaFogoDescricao = "Descarga elétrica" },
        new CausaFogo { CausaFogoId = 8, CausaFogoDescricao = "Limpeza de controle fitossanitário" },
        new CausaFogo { CausaFogoId = 9, CausaFogoDescricao = "Incidente com colheitadeiras" },
        new CausaFogo { CausaFogoId = 10, CausaFogoDescricao = "Queima Ponto de Apoio" },
        new CausaFogo { CausaFogoId = 11, CausaFogoDescricao = "Terreno Baldio" }
      );
    }
  }
}