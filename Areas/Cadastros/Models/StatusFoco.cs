using System.ComponentModel.DataAnnotations;
using System.ComponentModel.Design.Serialization;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CadeOFogo.Models.Inpe
{
  public class StatusFoco
  {
    [Display(Name = "Código da situação do foco", ShortName = "Cód.")]
    public int StatusFocoId { get; set; }
    
    [Required(ErrorMessage = "É obrigatório preencher a situação")]
    [StringLength(maximumLength: 80,
      MinimumLength = 2,
      ErrorMessage = "A situação deve ter entre 2 e 80 caracteres")]
    [DataType(DataType.Text)]
    [Display(Name = "Situação do foco",
      ShortName = "Situação",
      Prompt = "Situação do foco...")]
    public string StatusFocoDescricao { get; set; }
  }
  
  public class StatusFocoConfiguration : IEntityTypeConfiguration<StatusFoco>
  {
    public void Configure(EntityTypeBuilder<StatusFoco> builder)
    {
      builder.HasKey(sf => sf.StatusFocoId);

      builder.Property(sf => sf.StatusFocoDescricao)
        .IsRequired()
        .HasMaxLength(80);

      builder.HasData(
        new StatusFoco {StatusFocoId = 1, StatusFocoDescricao = "Não encontrado"},
        new StatusFoco {StatusFocoId = 2, StatusFocoDescricao = "Sem nexo causalidade"},
        new StatusFoco {StatusFocoId = 3, StatusFocoDescricao = "Não autorizado"},
        new StatusFoco {StatusFocoId = 4, StatusFocoDescricao = "Autorizado"},
        new StatusFoco {StatusFocoId = 5, StatusFocoDescricao = "Autorizado desacordo"}
      );
    }
  }
}



  
  

