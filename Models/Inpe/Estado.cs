using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Net.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Newtonsoft.Json;

namespace CadeOFogo.Models.Inpe
{
  public partial class Estado // POCO class para o EF
  {
    public int EstadoId { get; set; }
    
    [Display(Name = "Nome do Estado")]
    public string EstadoNome { get; set; }
    public int EstadoIdInpe { get; set; }
    public DateTime UltimoFocoObservadoUtc { get; set; }

    public bool Monitorado { get; set; }

    public ICollection<Municipio> MunicipiosCollection { get; set; }

    public ICollection<Foco> ListaDeFocos { get; set; }
  }

  public partial class Estado
  {
    public static List<ApiInpeEstados> AtualizaEstados()
    {
      var httpClient = new HttpClient();
      var uri = new Uri("http://queimadas.dgi.inpe.br/api/auxiliar/estados?pais_id=33");
      try
      {
        var resposta = httpClient.GetStringAsync(uri).Result;
        var estadosFromInpe = JsonConvert.DeserializeObject<List<ApiInpeEstados>>(resposta);
        return estadosFromInpe;
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
  public class ApiInpeEstados
  {
    [JsonProperty("pais_id")] public int PaisId { get; set; }

    [JsonProperty("pais_name")] public string PaisName { get; set; }

    [JsonProperty("estado_id")] public int EstadoId { get; set; }

    [JsonProperty("estado_name")] public string EstadoName { get; set; }
  }

  public class EstadoConfiguration : IEntityTypeConfiguration<Estado>
  {
    public void Configure(EntityTypeBuilder<Estado> builder)
    {
      builder.HasKey(e => e.EstadoId);

      builder.Property(e => e.EstadoNome)
        .IsRequired()
        .HasMaxLength(80);

      builder.Property(e => e.Monitorado)
        .HasDefaultValue(true);

      builder.HasMany(m => m.MunicipiosCollection)
        .WithOne(m => m.Estado)
        .HasForeignKey(m => m.EstadoId)
        .OnDelete(DeleteBehavior.Cascade);

      builder.HasMany(e => e.ListaDeFocos)
        .WithOne(f => f.Estado)
        .HasForeignKey(f => f.EstadoId);
    }
  }
}