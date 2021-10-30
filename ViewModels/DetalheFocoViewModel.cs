using System;
using System.ComponentModel.DataAnnotations;

namespace CadeOFogo.ViewModels
{
  public class DetalheFocoViewModel
  {
    public int FocoId { get; set; }

    [Display(Name = "Coordenadas do foco")]
    public string Coordenadas { get; set; }

    public string Latitude { get; set; }
    public string Longitude { get; set; }

    [Display(Name = "Data e hora UTC do foco")]
    [DataType(DataType.DateTime)]
    [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd HH:mm:ss}", ApplyFormatInEditMode = true)]
    public DateTime FocoDataUtc { get; set; }

    [Display(Name = "Localidade")]
    public string Localidade { get; set; }
    
    [Display(Name = "Foco já atendido")]
    public bool FocoAtendido { get; set; }

    [Display(Name = "Foco confirmado")]
    public bool FocoConfirmado { get; set; }
    
    [Display(Name = "Mapa de localização do foco")]
    public byte[] SnapshotSatelite { get; set; }
    
    [Display(Name = "Satélite")] public string Satelite { get; set; }
    
    [Display(Name = "Identificação do foco no INPE")]
    public string FocoIdInpe { get; set; }
    
    [Display(Name = "Endereço aproximado")]
    public string ReverseGeocode { get; set; }
    
    [Display(Name = "Attribution")]
    public string Attribution { get; set; }

  }
}