using System;
using System.ComponentModel.DataAnnotations;
using CadeOFogo.Utilities;
using Microsoft.CodeAnalysis;

namespace CadeOFogo.ViewModels
{
  public class ListaDeFocosViewModel
  {
    public int FocoId { get; set; }

    [Display(Name = "Data e hora UTC do foco", ShortName = "Data/hora UTC")]
    [DataType(DataType.DateTime)]
    [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd HH:mm:ss}", ApplyFormatInEditMode = true)]
    public DateTime FocoDataUtc { get; set; }

    [Display(Name = "Satélite")]
    public string Satelite { get; set; }
    
    [Display(Name = "Coordenadas do foco", ShortName = "Coords")]
    public string Coordenadas { get; set; }

    [Display(Name = "Municipio", ShortName = "Municipio")]
    public string MunicipioNome { get; set; }

    [Display(Name = "Estado", ShortName = "Estado")]
    public string EstadoNome { get; set; }
    
    [Display(Name = "Atendido?")] public bool FocoAtendido { get; set; }
    [Display(Name = "Confirmado?")] public bool FocoConfirmado { get; set; }

    
  }
}