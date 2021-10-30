using System.Collections.Generic;
using CadeOFogo.Enums;
using CadeOFogo.Interfaces;
using Newtonsoft.Json;

namespace CadeOFogo.Models.MapBox
{
    public class Properties
    {
        [JsonProperty("accuracy")]
        public string Accuracy { get; set; }

        [JsonProperty("wikidata")]
        public string Wikidata { get; set; }

        [JsonProperty("short_code")]
        public string ShortCode { get; set; }
    }

    public class Geometry
    {
        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("coordinates")]
        public List<double> Coordinates { get; set; }
    }

    public class Context
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("wikidata")]
        public string Wikidata { get; set; }

        [JsonProperty("text")]
        public string Text { get; set; }

        [JsonProperty("short_code")]
        public string ShortCode { get; set; }
    }

    public class Feature
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("place_type")]
        public List<string> PlaceType { get; set; }

        [JsonProperty("relevance")]
        public int Relevance { get; set; }

        [JsonProperty("properties")]
        public Properties Properties { get; set; }

        [JsonProperty("text")]
        public string Text { get; set; }

        [JsonProperty("place_name")]
        public string PlaceName { get; set; }

        [JsonProperty("center")]
        public List<double> Center { get; set; }

        [JsonProperty("geometry")]
        public Geometry Geometry { get; set; }

        [JsonProperty("context")]
        public List<Context> Context { get; set; }

        [JsonProperty("bbox")]
        public List<double> Bbox { get; set; }
    }

    public class MapBoxGeocodeRoot
    {
      [JsonProperty("type")]
      public string Type { get; set; }

      [JsonProperty("query")]
      public List<double> Query { get; set; }

      [JsonProperty("features")]
      public List<Feature> Features { get; set; }

      [JsonProperty("attribution")]
      public string Attribution { get; set; }
    }
}