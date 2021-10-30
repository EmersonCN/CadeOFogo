using System;
using System.Globalization;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using CadeOFogo.Enums;
using CadeOFogo.Interfaces;
using CadeOFogo.Models;
using CadeOFogo.Models.MapBox;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace CadeOFogo.Services
{
  public class Mapbox : IMapProvider
  {
    private readonly IConfigurationSection _configuration;

    private readonly NumberFormatInfo _nfi = new()
    {
      NumberDecimalSeparator = ".",
      NumberDecimalDigits = 6
    };

    public Mapbox(IConfigurationSection chaves)
    {
      _configuration = chaves;
    }

    public string providerName()
    {
      return "MapBox";
    }

    public async Task<byte[]> StaticMap(decimal latitude, decimal longitude)
    {
      try
      {
        // https://docs.mapbox.com/api/maps/static-images/
        var endereco = "https://api.mapbox.com/styles/v1/mapbox/satellite-streets-v11/static/pin-s" +
                       $"({longitude.ToString(_nfi)},{latitude.ToString(_nfi)})/" +
                       $"{longitude.ToString(_nfi)},{latitude.ToString(_nfi)},12,0/600x300@2x?" +
                       "access_token=" + _configuration["ApiKey"];

        var url = new Uri(endereco);

        var httpClient = new HttpClient
        {
          MaxResponseContentBufferSize = 1 * 1024 * 1024    // 1MB
        };
        
        var response = await httpClient.GetAsync(url);
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

      // https://stackoverflow.com/questions/19673398/inserting-script-and-link-tag-inside-the-header-tag/19673429
      // https://developer.mozilla.org/en-US/docs/Web/HTML/Element/link
      // <link stylesheet> dentro do body, antes do map
      resposta.Append("<script>");
      resposta.Append("if(!document.getElementById(\"mapCssLink\")) {");
      resposta.Append("var link = document.createElement('link');");
      resposta.Append("link.id = \"mapCssLink\";");
      resposta.Append("link.rel = \"stylesheet\";");
      resposta.Append("link.href = \"https://api.mapbox.com/mapbox-gl-js/v2.2.0/mapbox-gl.css\";");
      resposta.Append("document.head.appendChild(link);}");
      resposta.Append("</script>");

      // https://docs.mapbox.com/mapbox-gl-js/api/
      resposta.Append("<script src=\"https://api.mapbox.com/mapbox-gl-js/v2.2.0/mapbox-gl.js\"></script>");
      resposta.Append("<script>");
      resposta.Append($"mapboxgl.accessToken=\"{_configuration["ApiKey"]}\";");
      resposta.Append(
        $"var map = new mapboxgl.Map({{\"container\":\"{divId}\",\"style\":\"mapbox://styles/mapbox/satellite-streets-v11\",");
      resposta.Append($"\"center\":[{longitude.ToString(_nfi)},{latitude.ToString(_nfi)}],");
      resposta.Append("\"zoom\":15});");
      resposta.Append("map.addControl(new mapboxgl.ScaleControl({position:\"bottom-right\"}));");
      resposta.Append("map.addControl(new mapboxgl.NavigationControl());");
      resposta.Append("var marker1 = new mapboxgl.Marker()");
      resposta.Append($".setLngLat([{longitude.ToString(_nfi)},{latitude.ToString(_nfi)}])");
      resposta.Append(".addTo(map)");
      resposta.Append("</script>");
      return resposta.ToString();
    }

    public ReverseGeocodeResponse ReverseGeocode(decimal latitude, decimal longitude)
    {
      // https://docs.mapbox.com/api/search/geocoding/
      var httpClient = new HttpClient();
      var resposta = new ReverseGeocodeResponse();
      var achou = false;

      var uri = new Uri("https://api.mapbox.com/geocoding/v5/mapbox.places/" +
                        $"{longitude.ToString(_nfi)},{latitude.ToString(_nfi)}" +
                        $".json?access_token={_configuration["ApiKey"]}");
      try
      {
        var r = httpClient.GetStringAsync(uri).Result;
        var mapBoxGeocodeRoot = JsonConvert.DeserializeObject<MapBoxGeocodeRoot>(r);
        foreach (var feature in mapBoxGeocodeRoot.Features)
        {
          if (!feature.Id.Contains("address"))
            continue;
          resposta.Endereco = feature.PlaceName;
          resposta.Attribution = mapBoxGeocodeRoot.Attribution;
          resposta.Status = ReverseGeocodeStatus.OK;
          achou = true;
          break;
        }
      }
      catch (Exception e)
      {
        Console.WriteLine(e);
        resposta.Status = ReverseGeocodeStatus.ERROR;
      }

      if (!achou) resposta.Status = ReverseGeocodeStatus.NOT_FOUND;

      return resposta;
    }

    public string DynamicSpotsMap(string jsonEndpoint)
    {
      var resposta = new StringBuilder();

      // https://stackoverflow.com/questions/19673398/inserting-script-and-link-tag-inside-the-header-tag/19673429
      // https://developer.mozilla.org/en-US/docs/Web/HTML/Element/link
      // <link stylesheet> dentro do body, antes do map
      resposta.Append("<script>");
      resposta.Append("if(!document.getElementById(\"mapCssLink\")){");
      resposta.Append("var link=document.createElement('link');");
      resposta.Append("link.id=\"mapCssLink\";");
      resposta.Append("link.rel=\"stylesheet\";");
      resposta.Append("link.href=\"https://api.mapbox.com/mapbox-gl-js/v2.2.0/mapbox-gl.css\";");
      resposta.Append("document.head.appendChild(link);}");
      resposta.Append("</script>");

      // https://docs.mapbox.com/mapbox-gl-js/api/
      resposta.Append("<script src=\"https://api.mapbox.com/mapbox-gl-js/v2.2.0/mapbox-gl.js\"></script>");
      resposta.Append("<script>");
      resposta.Append($"mapboxgl.accessToken=\"{_configuration["ApiKey"]}\";");

      resposta.Append(
        "var map = new mapboxgl.Map({\"container\":\"mapFront\",\"style\":\"mapbox://styles/mapbox/satellite-streets-v11\",");
      resposta.Append("\"center\":[-47.909,-22.087],");
      resposta.Append("\"zoom\":6});");
      resposta.Append("map.addControl(new mapboxgl.ScaleControl({position:\"bottom-right\"}));");
      resposta.Append("map.addControl(new mapboxgl.NavigationControl());");
      resposta.Append("map.on(\"load\",function(){");
      resposta.Append("map.addSource(\"focos\",{type: \"geojson\",");
      resposta.Append($"data: \"{jsonEndpoint}\"");
      resposta.Append("});");
      // https://docs.mapbox.com/mapbox-gl-js/example/external-geojson/
      resposta.Append("map.addLayer({\"id\":\"focos\",");
      resposta.Append("\"type\":\"circle\",");
      resposta.Append("\"source\":\"focos\",");
      resposta.Append("\"paint\":{\"circle-radius\":8,\"circle-stroke-width\":2,");
      resposta.Append("\"circle-color\":\"red\",\"circle-stroke-color\":\"white\"");
      resposta.Append("}});");
      resposta.Append("map.on('click','focos',function(e){");
      resposta.Append("var c=e.features[0].geometry.coordinates.slice();");
      resposta.Append("var desc=e.features[0].properties.resumo;");
      resposta.Append("while(Math.abs(e.lngLat.lng-c[0])>180){");
      resposta.Append("c[0]+=e.lngLat.lng>c[0]?360:-360;}");
      resposta.Append("new mapboxgl.Popup().setLngLat(c).setHTML(desc).addTo(map);");
      resposta.Append("});");
      resposta.Append("map.on('mouseenter','focos',function(){map.getCanvas().style.cursor='pointer';});");
      resposta.Append("map.on('mouseleave','focos',function(){map.getCanvas().style.cursor='';});");
      resposta.Append("});");
      resposta.Append("</script>");
      
      return resposta.ToString();
    }
  }
}