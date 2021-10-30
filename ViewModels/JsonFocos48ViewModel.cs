using System.Collections.Generic;
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
}