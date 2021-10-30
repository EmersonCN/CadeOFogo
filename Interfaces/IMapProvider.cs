using System;
using System.Threading.Tasks;
using CadeOFogo.Models;
using Microsoft.AspNetCore.Html;

namespace CadeOFogo.Interfaces
{
  public interface IMapProvider
  {
    string providerName();
    Task<byte[]> StaticMap(decimal latitude, decimal longitude);
    string DynamicSingleSpotMap (decimal latitude, decimal longitude, string divId);
    ReverseGeocodeResponse ReverseGeocode(decimal latitude, decimal longitude);
    string DynamicSpotsMap(string jsonEndpoint);
  }
}