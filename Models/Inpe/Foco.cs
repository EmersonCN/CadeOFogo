using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using CadeOFogo.Utilities;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Newtonsoft.Json;

namespace CadeOFogo.Models.Inpe
{
  public class Foco
  {
    public int FocoId { get; set; }

    [Display(Name = "Longitude", ShortName = "Long.")]
    [Required(ErrorMessage = "A longitude é obrigatória")]
    public decimal FocoLongitude { get; set; }

    [Display(Name = "Latitude", ShortName = "Lat.")]
    [Required(ErrorMessage = "A latitude é obrigatória")]
    public decimal FocoLatitude { get; set; }

    [Display(Name = "Data e hora UTC do foco", ShortName = "Data/hora UTC")]
    [Required(ErrorMessage = "A data e hora são obrigatórias")]
    [DataType(DataType.DateTime)]
    [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd HH:mm:ss}", ApplyFormatInEditMode = true)]
    [DataPassado(ErrorMessage = "A data precisa ser anterior ao momento atual")]
    public DateTime FocoDataUtc { get; set; }

    [Display(Name = "Foco já atendido", ShortName = "Atendido")]
    [Required(ErrorMessage = "É obrigatório indicar se o foco foi atendido")]
    public bool FocoAtendido { get; set; }

    [Display(Name = "Foco confirmado", ShortName = "Confirmado")]
    [Required(ErrorMessage = "É obrigatório indicar se o foco foi confirmado")]
    public bool FocoConfirmado { get; set; }

    public int SateliteId { get; set; }
    public Satelite Satelite { get; set; }

    public int MunicipioId { get; set; }
    public Municipio Municipio { get; set; }

    public int EstadoId { get; set; }
    public Estado Estado { get; set; }

    public byte[] SnapshotSatelite { get; set; }
    public DateTime DataSnapshot { get; set; }
    public string SnapshotProvider { get; set; }
    public string InpeFocoId { get; set; }

    [NotMapped]
    [Display(Name = "Coordenadas")]
    public string Coordenadas
    {
      get
      {
        var lat = (double) this.FocoLatitude;
        var lon = (double) this.FocoLongitude;

        string latDir = (this.FocoLatitude >= 0 ? "N" : "S");
        lat = (double) Math.Abs(this.FocoLatitude);
        double latMinPart = ((lat - Math.Truncate(lat) / 1) * 60);
        double latSecPart = ((latMinPart - Math.Truncate(latMinPart) / 1) * 60);
        var latitude = $"{Math.Truncate(lat)}º {Math.Truncate(latMinPart)}' {latSecPart:N2}\" {latDir}";

        string lonDir = (lon >= 0 ? "E" : "W");
        lon = Math.Abs(lon);
        double lonMinPart = ((lon - Math.Truncate(lon) / 1 ) * 60);
        double lonSecPart = ((lonMinPart - Math.Truncate(lonMinPart) / 1) * 60);
        var longitude = $"{Math.Truncate(lon)}º {Math.Truncate(lonMinPart)}' {lonSecPart:N2}\" {lonDir}";

        return $"{latitude}, {longitude}";
      }
    }
  }
  
  // https://json2csharp.com/
  // https://queimadas.dgi.inpe.br/queimadas/dados-abertos/apidoc/
  public class ApiInpeFocos
  {
    [JsonProperty("geometry")] public ApiInpeFocosGeometry Geometry { get; set; }
    [JsonProperty("properties")] public ApiInpeFocosProperties Properties { get; set; }
    [JsonProperty("type")] public string Type { get; set; }
    [JsonProperty("id")] public string Id { get; set; }
    [JsonProperty("geometry_name")] public string GeometryName { get; set; }
  }

  public class ApiInpeFocosProperties
  {
    [JsonProperty("risco_fogo")] public object RiscoFogo { get; set; }
    [JsonProperty("longitude")] public double Longitude { get; set; }
    [JsonProperty("data_hora_gmt")] public DateTime DataHoraGmt { get; set; }
    [JsonProperty("precipitacao")] public object Precipitacao { get; set; }
    [JsonProperty("latitude")] public double Latitude { get; set; }
    [JsonProperty("pais")] public string Pais { get; set; }
    [JsonProperty("numero_dias_sem_chuva")]
    public object NumeroDiasSemChuva { get; set; }
    [JsonProperty("estado")] public string Estado { get; set; }
    [JsonProperty("municipio")] public string Municipio { get; set; }
    [JsonProperty("satelite")] public string Satelite { get; set; }
  }

  public class ApiInpeFocosGeometry
  {
    [JsonProperty("type")] public string Type { get; set; }
    [JsonProperty("coordinates")] public List<decimal> Coordinates { get; set; }
  }

  public class FocoConfiguration : IEntityTypeConfiguration<Foco>
  {
    public void Configure(EntityTypeBuilder<Foco> builder)
    {
      builder.HasKey(f => f.FocoId);

      builder.HasAlternateKey(f => new {f.FocoLatitude, f.FocoLongitude, f.FocoDataUtc, f.SateliteId});

      builder.Property(f => f.FocoLongitude)
        .IsRequired()
        .HasPrecision(13, 8)
        .HasColumnType("decimal(13, 8)");

      builder.Property(f => f.FocoLatitude)
        .IsRequired()
        .HasPrecision(13, 8)
        .HasColumnType("decimal(13, 8)");

      builder.Property(f => f.FocoDataUtc)
        .IsRequired()
        .HasPrecision(0);

      builder.Property(f => f.DataSnapshot)
        .HasPrecision(0);

      builder.Property(f => f.SnapshotProvider)
        .HasMaxLength(40);

      builder.Property(f => f.SateliteId)
        .IsRequired();

      builder.Property(f => f.InpeFocoId)
        .IsRequired()
        .HasMaxLength(80);

      builder.Property(f => f.FocoAtendido)
        .IsRequired()
        .HasDefaultValue(false);

      builder.Property(f => f.FocoConfirmado)
        .IsRequired()
        .HasDefaultValue(true);

      builder.HasOne(f => f.Satelite)
        .WithMany(s => s.FocosCollection)
        .HasForeignKey(f => f.SateliteId);

      builder.HasOne(f => f.Municipio)
        .WithMany(m => m.ListaDeFocos)
        .OnDelete(DeleteBehavior.NoAction);

      builder.HasOne(f => f.Estado)
        .WithMany(e => e.ListaDeFocos)
        .OnDelete(DeleteBehavior.NoAction);
    }
  }
}