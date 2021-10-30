using System.Globalization;
using System.Text;
using System.Threading.Tasks;
using CadeOFogo.Enums;
using CadeOFogo.Interfaces;
using CadeOFogo.Models;
using Microsoft.Extensions.Configuration;

namespace CadeOFogo.Services
{
  public class OpenLayers : IMapProvider
  {
    private readonly IConfigurationSection _configuration;

    private readonly NumberFormatInfo _nfi = new()
    {
      NumberDecimalSeparator = ".",
      NumberDecimalDigits = 6
    };

    public OpenLayers(IConfigurationSection chaves)
    {
      _configuration = chaves;
    }

    public string providerName()
    {
      return "OpenLayers";
    }

    public Task<byte[]> StaticMap(decimal latitude, decimal longitude)
    {
      throw new System.NotImplementedException();
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
      resposta.Append("link.href = \"https://cdn.jsdelivr.net/gh/openlayers/openlayers.github.io@master/en/v6.5.0/css/ol.css\";");
      resposta.Append("document.head.appendChild(link);}");
      resposta.Append("</script>");

      resposta.Append(
        "<script src=\"https://cdn.jsdelivr.net/gh/openlayers/openlayers.github.io@master/en/v6.5.0/build/ol.js\"></script>");

      resposta.Append("<script type=\"text/javascript\">");
      resposta.Append("var map=new ol.Map({");
      resposta.Append($"target: \"{divId}\",");
      resposta.Append("layers:[new ol.layer.Tile({source:new ol.source.OSM()})],");
      resposta.Append("view:new ol.View({center:ol.proj.fromLonLat(");
      resposta.Append($"[{longitude.ToString(_nfi)},{latitude.ToString(_nfi)}]),zoom:15");
      resposta.Append("})});");
      resposta.Append("var layer=new ol.layer.Vector({source:new ol.source.Vector({");
      resposta.Append("features:[new ol.Feature({geometry:new ol.geom.Point");
      resposta.Append($"(ol.proj.fromLonLat([{longitude.ToString(_nfi)},{latitude.ToString(_nfi)}]))");
      resposta.Append("})]})});map.addLayer(layer);");
      
      resposta.Append("</script>");

      return resposta.ToString();
    }

    public ReverseGeocodeResponse ReverseGeocode(decimal latitude, decimal longitude)
    {
      return new ReverseGeocodeResponse
      {
        Attribution = "",
        Endereco = "",
        Status = ReverseGeocodeStatus.NOT_IMPLEMENTED
      };
    }

    public string DynamicSpotsMap(string jsonEndpoint)
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
      resposta.Append("link.href = \"https://cdn.jsdelivr.net/gh/openlayers/openlayers.github.io@master/en/v6.5.0/css/ol.css\";");
      resposta.Append("document.head.appendChild(link);}");
      resposta.Append("</script>");

      resposta.Append(
        "<script src=\"https://cdn.jsdelivr.net/gh/openlayers/openlayers.github.io@master/en/v6.5.0/build/ol.js\"></script>");

      resposta.Append("<script type=\"text/javascript\">");
      resposta.Append("var map=new ol.Map({");
      resposta.Append("target: \"mapFront\",");
      resposta.Append("layers:[new ol.layer.Tile({source:new ol.source.OSM()})],");
      resposta.Append("view:new ol.View({center:ol.proj.fromLonLat(");
      resposta.Append("[-47.909,-22.087]),zoom:7");
      resposta.Append("})});");
      resposta.Append("var layer=new ol.layer.Vector({source:new ol.source.Vector({");
      resposta.Append($"url: \"{jsonEndpoint}\",format: new ol.format.GeoJSON()");
      resposta.Append("})");
      resposta.Append("});map.addLayer(layer);");
      
      resposta.Append("</script>");

      return resposta.ToString();
    }
  }
}