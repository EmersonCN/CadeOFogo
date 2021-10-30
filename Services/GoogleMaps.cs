using System;
using System.Globalization;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using CadeOFogo.Enums;
using CadeOFogo.Interfaces;
using CadeOFogo.Models;
using CadeOFogo.Models.GoogleMaps;
using CadeOFogo.Utilities;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace CadeOFogo.Services
{
  public class GoogleMaps : IMapProvider
  {
    private readonly IConfigurationSection _configuration;
    private readonly NumberFormatInfo _nfi = new NumberFormatInfo
    {
      NumberDecimalSeparator = ".",
      NumberDecimalDigits = 6
    };

    public GoogleMaps(IConfigurationSection chaves)
    {
      _configuration = chaves;
    }

    public string providerName()
    {
      return "GoogleMaps";
    }

    // http://bacaj.azurewebsites.net/asp-net-mvc-and-google-street-view-static-maps-apis/
    public async Task<byte[]> StaticMap(decimal latitude, decimal longitude)
    {
      try
      {
        var endereco = "https://maps.googleapis.com/maps/api/staticmap?zoom=15&size=600x300&scale=2&" +
                       "maptype=hybrid&markers=color:red%7Csize:tiny%7C" + latitude.ToString(_nfi) + "%2C" +
                       longitude.ToString(_nfi) + "&key=" + _configuration["Static"];

        var url = new Uri(GoogleSignedUrl.Sign(endereco, _configuration["SecretKey"]));

        var httpClient = new HttpClient();

        var response = httpClient.GetAsync(url).Result;
        response.EnsureSuccessStatusCode();
        await response.Content.LoadIntoBufferAsync();

        return response.Content.ReadAsByteArrayAsync().Result;
      }
      catch
      {
        return null;
      }
    }

    public string DynamicSingleSpotMap(decimal latitude, decimal longitude, string divId)
    {

      var resposta = new StringBuilder();
      
      resposta.Append("<script>function initMap() {");
      resposta.Append($"const map=new google.maps.Map(document.getElementById(\"{divId}\"),");
      resposta.Append("{zoom:15,scaleControl:true,center:{");
      resposta.Append($"lat:{latitude.ToString(_nfi)},lng:{longitude.ToString(_nfi)}");
      resposta.Append("}});");
      resposta.Append($"const image=\"{_configuration["IconeQueimada"]}\";");
      resposta.Append("const foco=new google.maps.Marker({");
      resposta.Append("position:{");
      resposta.Append($"lat:{latitude.ToString(_nfi)},lng:{longitude.ToString(_nfi)}");
      resposta.Append("},map,icon:image});}");
      resposta.Append("</script>");
      resposta.Append(
        $"<script src=\"https://maps.googleapis.com/maps/api/js?key={_configuration["JavaScriptApi"]}&callback=initMap\" async defer></script>");
      return resposta.ToString();
    }
    
    public ReverseGeocodeResponse ReverseGeocode(decimal latitude, decimal longitude)
    {
      var httpClient = new HttpClient();
      var resposta = new ReverseGeocodeResponse();
      var erro = false;
      var googleGeocodeRoot = new GoogleGeocodeRoot();

      var uri = new Uri("https://maps.googleapis.com/maps/api/geocode/json?latlng=" +
                        $"{latitude.ToString(_nfi)},{longitude.ToString(_nfi)}" +
                        $"&result_type=street_address|route&key={_configuration["Geocode"]}");

      try
      {
        var r = httpClient.GetStringAsync(uri).Result;
        googleGeocodeRoot = JsonConvert.DeserializeObject<GoogleGeocodeRoot>(r);
      }
      catch (Exception e)
      {
        Console.WriteLine(e);
        resposta.Status = ReverseGeocodeStatus.ERROR;
        erro = true;
      }

      if (erro)
        return resposta;
      
      var achou = false;
      switch (googleGeocodeRoot.Status)
      {
        case "OK":
          resposta.Status = ReverseGeocodeStatus.OK;
          foreach (var result in googleGeocodeRoot.Results)
          {
            if (!(result.Types.Contains("route") || result.Types.Contains("street_address")))
              continue;
            achou = true;
            resposta.Endereco = result.FormattedAddress;
            resposta.Attribution = "Built with Google Maps.";
            resposta.Status = ReverseGeocodeStatus.OK;
            break;
          }
          break;
        case "OVER_QUERY_LIMIT":
          resposta.Status = ReverseGeocodeStatus.OVERQUOTA;
          break;
        case "ZERO_RESULTS":
          resposta.Status = ReverseGeocodeStatus.NOT_FOUND;
          break;
        default:
          resposta.Status = ReverseGeocodeStatus.ERROR;
          break;
      }
      
      if (!achou)
        resposta.Status = ReverseGeocodeStatus.NOT_FOUND;
      
      return resposta;
    }

    public string DynamicSpotsMap(string jsonEndpoint)
    {
      var resposta = new StringBuilder();
        
      resposta.Append("<script>");
      resposta.Append("function initMap(){");
      resposta.Append("let map = new google.maps.Map(document.getElementById(\"mapFront\"),");
      resposta.Append("{zoom:7,mapTypeId:\"terrain\",");
      resposta.Append("scaleControl:true,");
      resposta.Append("center:{lat:-22.087,lng:-47.909}");
      resposta.Append("});");
      // https://gist.github.com/geog4046instructor/fc472ec499502f3e9a76
      // Add a GeoJSON layer with a popup showing feature attributes with the Google Maps API
      resposta.Append($"map.data.loadGeoJson(\"{jsonEndpoint}\");");
      resposta.Append("var infowindow=new google.maps.InfoWindow();");
      resposta.Append("map.data.addListener('click',function(e){");
      resposta.Append("let place=e.feature.getProperty(\"resumo\");");
      resposta.Append("infowindow.setContent(place);infowindow.setPosition(e.feature.getGeometry().get());");
      resposta.Append("infowindow.setOptions({pixelOffset:new google.maps.Size(0,-30)});infowindow.open(map);");
      resposta.Append("});");
      resposta.Append("}</script>");
      resposta.Append(
        $"<script src=\"https://maps.googleapis.com/maps/api/js?key={_configuration["JavaScriptApi"]}&callback=initMap\" async defer></script>");

      return resposta.ToString();
    }
  }
  
  // https://developers.google.com/maps/documentation/maps-static/get-api-key
  public struct GoogleSignedUrl
  {
    public static string Sign(string url, string keyString)
    {
      var encoding = new ASCIIEncoding();

      // converting key to bytes will throw an exception, need to replace '-' and '_' characters first.
      var usablePrivateKey = keyString.Replace("-", "+").Replace("_", "/");
      var privateKeyBytes = Convert.FromBase64String(usablePrivateKey);

      var uri = new Uri(url);
      var encodedPathAndQueryBytes = encoding.GetBytes(uri.LocalPath + uri.Query);

      // compute the hash
      var algorithm = new HMACSHA1(privateKeyBytes);
      var hash = algorithm.ComputeHash(encodedPathAndQueryBytes);

      // convert the bytes to string and make url-safe by replacing '+' and '/' characters
      var signature = Convert.ToBase64String(hash).Replace("+", "-").Replace("/", "_");

      // Add the signature to the existing URI.
      return uri.Scheme + "://" + uri.Host + uri.LocalPath + uri.Query + "&signature=" + signature;
    }
  }
}