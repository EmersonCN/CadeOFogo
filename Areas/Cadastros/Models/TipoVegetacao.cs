using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CadeOFogo.Models.Inpe
{
  public class TipoVegetacao
  {
    [Display(Name = "Código do tipo de vegetação", ShortName = "Cód.")]
    public int TipoVegetacaoId { get; set; }

    [Required(ErrorMessage = "É obrigatório preencher o tipo de vegetação")]
    [StringLength(maximumLength: 80,
      MinimumLength = 2,
      ErrorMessage = "O tipo deve ter entre 2 e 80 caracteres")]
    [DataType(DataType.Text)]
    [Display(Name = "Tipo de vegetação",
      ShortName = "Tipo veg.",
      Prompt = "Tipo de vegetação...")]
    public string TipoVegetacaoDescricao { get; set; }

        public ICollection<Foco> FocoCollection { get; set; }
    }
  
  public class TipoVegetacaoConfiguration : IEntityTypeConfiguration<TipoVegetacao>
  {
    public void Configure(EntityTypeBuilder<TipoVegetacao> builder)
    {
      builder.HasKey(tv => tv.TipoVegetacaoId);

      builder.Property(tv => tv.TipoVegetacaoDescricao)
        .IsRequired()
        .HasMaxLength(80);

            builder.HasMany(p => p.FocoCollection)
                       .WithOne(p => p.TipoVegetacao)
                       .HasForeignKey(p => p.TipoVegetacaoId);

            builder.HasData(
        new TipoVegetacao {TipoVegetacaoId = 1, TipoVegetacaoDescricao = "Vegetação pioneira ou demais formas de vegetação natural"},
        new TipoVegetacao {TipoVegetacaoId = 2, TipoVegetacaoDescricao = "Vegetação nativa secundária em estágio inicial de regeneração"},
        new TipoVegetacao {TipoVegetacaoId = 3, TipoVegetacaoDescricao = "Vegetação nativa secundária em estágio médio de regeneração"},
        new TipoVegetacao {TipoVegetacaoId = 4, TipoVegetacaoDescricao = "Vegetação nativa secundária em estágio avançado de regeneração"},
        new TipoVegetacao {TipoVegetacaoId = 5, TipoVegetacaoDescricao = "Vegetação nativa primária"},
        new TipoVegetacao {TipoVegetacaoId = 6, TipoVegetacaoDescricao = "Vegetação exótica"}
      );
    }
  }
}
