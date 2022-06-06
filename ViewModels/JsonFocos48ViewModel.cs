using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using CadeOFogo.Models.Inpe;
using Newtonsoft.Json;

namespace CadeOFogo.ViewModels
{
  public class JsonFocos48ViewModel
  {
    [JsonProperty("type")] public string Type { get; set; }
    [JsonProperty("features")]public List<Foco48hViewModel> Features { get; set; }
  }

  public class Foco48hViewModel
  {
    [JsonProperty("type")] public string Type { get; set; }
    [JsonProperty("id")] public string Id { get; set; }
    [JsonProperty("geometry")] public ApiInpeFocosGeometry Geometry { get; set; }
    [JsonProperty("properties")] public Foco48hViewModelProperties Properties { get; set; }
  }

  public class Foco48hViewModelProperties
  {
    [JsonProperty("resumo")] public string Resumo { get; set; }
  }

    public class JsonFocosInpeViewModel
    {
        [JsonProperty("focos")] public List<JsonFocosInpeVerificadosViewModel> Focos { get; set; }
    }

    public class JsonFocosInpeVerificadosViewModel
   {
        [JsonProperty("focoId")] public string FocoId { get; set; }
        [JsonProperty("data")] public string FocoDataUtc { get; set; }
        [JsonProperty("satelite")] public string Satelite { get; set; }
        [JsonProperty("coordenadas")] public List<decimal> Coordenadas { get; set; }
        [JsonProperty("municipio")]public string MunicipioNome { get; set; }
        [JsonProperty("estado")] public string EstadoNome { get; set; }
        [JsonProperty("confirmado")] public bool FocoConfirmado { get; set; }
    }
}