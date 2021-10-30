using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Net.Http;
using CadeOFogo.Services;
using CadeOFogo.Utilities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Newtonsoft.Json;

namespace CadeOFogo.Models.Inpe
{
  public partial class Satelite
  {
    [Display(Name = "Código do satélite", ShortName = "Cód.")]
    public int SateliteId { get; set; }
    
    [StringLength(maximumLength: 80,
      MinimumLength = 2,
      ErrorMessage = "O nome deve ter entre 2 e 80 caracteres")]
    [DataType(DataType.Text)]
    [Display(Name = "Nome do satélite",
      ShortName = "Nome")]
    public string SateliteNome { get; set; }

    [Required(ErrorMessage = "É obrigatório preencher o nome do satélite nas bases do INPE")]
    [StringLength(maximumLength: 15,
      MinimumLength = 2,
      ErrorMessage = "O nome deve ter entre 2 e 15 caracteres")]
    [DataType(DataType.Text)]
    [Display(Name = "Nome do satélite no INPE",
      ShortName = "Nome INPE")]
    public string SateliteNomeINPE { get; set; }

    [Display(Name = "Data e hora UTC do último foco detectado", ShortName = "Data/hora UTC")]
    [Required(ErrorMessage = "A data e hora são obrigatórias")]
    [DataType(DataType.DateTime)]
    [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd HH:mm:ss}", ApplyFormatInEditMode = true)]
    [DataPassado(ErrorMessage = "A data precisa ser anterior ao momento atual")]
    public DateTime UltimoFocoUtc { get; set; }

    public ICollection<Foco> FocosCollection { get; set; }

    public bool Monitorado { get; set; }
  }

  public partial class Satelite
  {
    public static List<ApiInpeSatelite> AtualizaSatelites()
    {
      var httpClient = new HttpClient();
      var uri = new Uri("http://queimadas.dgi.inpe.br/api/focos/?pais_id=33&estado_id=35&satelite=");
      try
      {
        var resposta = httpClient.GetStringAsync(uri).Result;
        var satelitesFromInpe = JsonConvert.DeserializeObject<List<ApiInpeSatelite>>(resposta);
        return satelitesFromInpe;
      }
      catch (Exception e)
      {
        Console.WriteLine(e);
        throw;
      }
    }
  }
  
  // https://json2csharp.com/
  // https://queimadas.dgi.inpe.br/queimadas/dados-abertos/apidoc/
  public class ApiInpeSatelite
  {
    [JsonProperty("satelite")]
    public string Satelite { get; set; }
  }
  
  public class SateliteConfiguration : IEntityTypeConfiguration<Satelite>
  {
    public void Configure(EntityTypeBuilder<Satelite> builder)
    {
      builder.HasKey(s => s.SateliteId);

      builder.HasIndex(s => s.SateliteNomeINPE);

      builder.Property(s => s.SateliteNome)
        .HasMaxLength(80);

      builder.Property(s => s.SateliteNomeINPE)
        .IsRequired()
        .HasMaxLength(15);

      builder.Property(s => s.UltimoFocoUtc)
        .HasPrecision(0);

      builder.Property(s => s.Monitorado)
        .HasDefaultValue(true);

      builder.HasMany(s => s.FocosCollection)
        .WithOne(f => f.Satelite)
        .HasForeignKey(f => f.SateliteId);
    }
  }
}