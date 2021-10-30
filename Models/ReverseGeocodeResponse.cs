using CadeOFogo.Enums;

namespace CadeOFogo.Models
{
  public class ReverseGeocodeResponse
  {
    public string Attribution { get; set; }
    public string Endereco { get; set; }
    public ReverseGeocodeStatus Status { get; set; }

  }
}