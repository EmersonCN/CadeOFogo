using System;
using System.Threading.Tasks;
using CadeOFogo.Enums;
using CadeOFogo.Interfaces;
using CadeOFogo.Models;
using Microsoft.AspNetCore.Html;

namespace CadeOFogo.Services
{
  public class NullMapProvider : IMapProvider
  {
    public string providerName()
    {
      return "NullMapProvider";
    }

    public Task<byte[]> StaticMap(decimal latitude, decimal longitude)
    {
      return null;
    }

    public string DynamicSingleSpotMap(decimal latitude, decimal longitude, string divId)
    {
      return string.Empty;
    }
    
    public ReverseGeocodeResponse ReverseGeocode(decimal latitude, decimal longitude)
    {
      return new ReverseGeocodeResponse()
      {
        Attribution = "",
        Endereco = "",
        Status = ReverseGeocodeStatus.NOT_IMPLEMENTED
      };
    }

    public string DynamicSpotsMap(string jsonEndpoint)
    {
      return string.Empty;
    }
  }
}