using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Net.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Newtonsoft.Json;

namespace CadeOFogo.Models.Inpe
{
  public partial class Municipio
  {
    public int MunicipioId { get; set; }
    [Display(Name = "Código IBGE do município")]
    public int MunicioIbgeId { get; set; }
    
    [Display(Name = "Nome do município")]
    public string MunicipioNome { get; set; }

    public int EstadoId { get; set; }
    public Estado Estado { get; set; }

    public bool Monitorado { get; set; }

    public DateTime UltimoFocoObservadoUtc { get; set; }

    public ICollection<Foco> ListaDeFocos { get; set; }
  }

  public partial class Municipio
  {
    public static List<ApiInpeMunicipios> AtualizaMunicipios(Estado estado)
    {
      var httpClient = new HttpClient();
      var uri = new Uri(
        $"http://queimadas.dgi.inpe.br/api/auxiliar/municipios?pais_id=33&estado_id={estado.EstadoIdInpe}");
      try
      {
        var resposta = httpClient.GetStringAsync(uri).Result;
        var municipiosFromInpe = JsonConvert.DeserializeObject<List<ApiInpeMunicipios>>(resposta);
        return municipiosFromInpe;
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
  public class ApiInpeMunicipios
  {
    [JsonProperty("pais_name")] public string PaisName { get; set; }

    [JsonProperty("municipio_name")] public string MunicipioName { get; set; }

    [JsonProperty("estado_id")] public int EstadoId { get; set; }

    [JsonProperty("pais_id")] public int PaisId { get; set; }

    [JsonProperty("estado_name")] public string EstadoName { get; set; }

    [JsonProperty("municipio_id")] public int MunicipioId { get; set; }
  }

  public class MunicipioConfiguration : IEntityTypeConfiguration<Municipio>
  {
    public void Configure(EntityTypeBuilder<Municipio> builder)
    {
      builder.HasAlternateKey(m => m.MunicioIbgeId);

      builder.HasKey(m => m.MunicipioId);

      builder.Property(m => m.MunicioIbgeId)
        .IsRequired();

      builder.Property(m => m.MunicipioNome)
        .IsRequired()
        .HasMaxLength(80);

      builder.Property(m => m.Monitorado)
        .HasDefaultValue(true);

      builder.Property(m => m.UltimoFocoObservadoUtc)
        .IsRequired()
        .HasDefaultValue(DateTime.MinValue);

      builder.HasOne(m => m.Estado)
        .WithMany(e => e.MunicipiosCollection)
        .HasForeignKey(m => m.EstadoId);

      builder.HasMany(m => m.ListaDeFocos)
        .WithOne(f => f.Municipio)
        .HasForeignKey(f => f.MunicipioId);
    }
  }
}