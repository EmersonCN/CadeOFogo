using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CadeOFogo.Areas.Cadastros.Models;
using CadeOFogo.Utilities;
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

        [Display(Name = "Bioma")]
        public string Bioma { get; set; }

        [Display(Name = "Municipi")]
        public string Municipi { get; set; }

        [Display(Name = "Policial Responsavel Pelo Atendimento")]
        public string PolicialResponsavel { get; set; }

        [Display(Name = "Ocorrência SIOPM")]
        public string OcorrênciaSIOPM { get; set; }

        [Display(Name = "Nº BOPAmb")]
        public string NºBOPAmb { get; set; }

        [Display(Name = "Nº TVA")]
        public string NºTVA { get; set; }

        [Display(Name = "RSO")]
        public string RSO { get; set; }

        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd HH:mm:ss}", ApplyFormatInEditMode = true)]
        [Display(Name = "Data Atend.")]
        public DateTime DataAtendimento { get; set; }

        public int EquipeId { get; set; }
        [Display(Name ="Equipe")]
        public Equipe Equipe { get; set; }

        public int StatusFocoId { get; set; }
        public StatusFoco StatusDoFoco { get; set; }

        public int IndicioInicioFocoId { get; set; }
        public IndicioInicioFoco IndicioInicioFoco { get; set; }

        public int CausaFogoId { get; set; }
        public CausaFogo CausaFogo { get; set; }

        public int CausadorProvavelId { get; set; }
        public CausadorProvavel CausadorProvavel { get; set; }

        public int ResponsavelPropriedadeId { get; set; }
        public ResponsavelPropriedade ResponsavelPropriedade { get; set; }

        [Display(Name = "Pioneiro (APP) - ÁREA EM HECTARES")]
        public string PioneiroAPPAreaEmHectares { get; set; }

        [Display(Name = "Inicial (APP) - ÁREA EM HECTARES")]
        public string InicialAPPAreaEmHectares { get; set; }

        [Display(Name = "Medio (APP) - ÁREA EM HECTARES")]
        public string MedioAPPAreaEmHectares { get; set; }

        [Display(Name = "Avançado (APP) - ÁREA EM HECTARES")]
        public string AvancadoAPPAreaEmHectares { get; set; }

        [Display(Name = "Auto De Infração Ambiental (Quantidades)")]
        public string AutoDeInflacaoAmbientalAPP { get; set; }

        [Display(Name = "Multa APP")]
        public string MultaAPP { get; set; }

        [Display(Name = "Pioneiro")]
        public string Pioneiro { get; set; }

        [Display(Name = "Inicial")]
        public string Inicial { get; set; }

        [Display(Name = "Médio")]
        public string Medio { get; set; }

        [Display(Name = "Avançado")]
        public string Avancado { get; set; }

        [Display(Name = "Auto De Infração Ambiental (Quantidades)")]
        public string AutoDeInflacaoAmbiental { get; set; }

        [Display(Name = "Multa")]
        public string MultaR { get; set; }

        [Display(Name = "Pasto")]
        public string Pasto { get; set; }

        [Display(Name = "Citrus")]
        public string Citrus { get; set; }

        [Display(Name = "Outras (Eucalipto,Pinus,Etc)")]
        public string Outras { get; set; }

        [Display(Name = "Auto De Infração Ambiental (Quantidades)")]
        public string AutoDeInflacaoAmbientalV { get; set; }

        [Display(Name = "Multa")]
        public string MultaV { get; set; }

        [Display(Name = "Arvores Isoladas")]
        public string ArvoresIsoladas { get; set; }

        [Display(Name = "Auto De Infração Ambiental (Quantidades)")]
        public string AutoDeInflacaoAmbientalA { get; set; }

        [Display(Name = "Multa")]
        public string MultaA { get; set; }

        [Display(Name = "Plaha de Cana")]
        public string PalhaDeCana { get; set; }

        [Display(Name = "Cana-de-Açucar")]
        public string CanaDeAcucar { get; set; }

        [Display(Name = "Autorizado (Sim/Não)")]
        public string Autorizado { get; set; }

        [Display(Name = "Auto De Infração Ambiental (Quantidades)")]
        public string AutoDeInflacaoAmbientalL { get; set; }

        [Display(Name = "Multa")]
        public string MultaL { get; set; }

        [Display(Name = "Pioneiro(UC)")]
        public string PioneiroUC { get; set; }

        [Display(Name = "Inicial(UC)")]
        public string InicialUC { get; set; }

        [Display(Name = "Médio(UC)")]
        public string MedioUC { get; set; }

        [Display(Name = "Avançado(UC)")]
        public string AvancadoUC { get; set; }

        [Display(Name = "Outras(UC) (Eucalipto,Pinus,Etc)")]
        public string OutrasUC { get; set; }

        [Display(Name = "Auto De Infração Ambiental (Quantidades)")]
        public string AutoDeInflacaoAmbientalUC { get; set; }

        [Display(Name = "Multa")]
        public string MultaUC { get; set; }

        [Display(Name = "Pioneiro(RL)")]
        public string PioneiroRL { get; set; }

        [Display(Name = "Inicial(RL)")]
        public string InicialRL { get; set; }

        [Display(Name = "Médio(RL)")]
        public string MedioRL { get; set; }

        [Display(Name = "Avançado(RL)")]
        public string AvancadoRL { get; set; }

        [Display(Name = "Outras(RL) (Eucalipto,Pinus,Etc)")]
        public string OutrasRL { get; set; }

        [Display(Name = "Auto De Infração Ambiental (Quantidades)")]
        public string AutoDeInflacaoAmbientalRL { get; set; }

        [Display(Name = "Multa")]
        public string MultaRL { get; set; }

        [Display(Name = "Refiscalização")]
        public string Refiscalizacao { get; set; }
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

            builder.HasOne(b => b.StatusDoFoco)
                .WithMany(b => b.FocoCollection)
                .OnDelete(DeleteBehavior.NoAction);

            builder.HasOne(b => b.IndicioInicioFoco)
                .WithMany(b => b.FocoCollection)               
                .OnDelete(DeleteBehavior.NoAction);

            builder.HasOne(b => b.CausaFogo)
                .WithMany(b => b.FocoCollection)
                .OnDelete(DeleteBehavior.NoAction);

            builder.HasOne(b => b.CausadorProvavel)
                .WithMany(b => b.FocoCollection)
                .HasForeignKey(b => b.CausadorProvavelId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.HasOne(b => b.ResponsavelPropriedade)
               .WithMany(b => b.FocoCollection)
               .OnDelete(DeleteBehavior.NoAction);

            builder.HasOne(b => b.Equipe)
                .WithMany(b => b.FocoCollection)
                .OnDelete(DeleteBehavior.NoAction);
        }
  }
}