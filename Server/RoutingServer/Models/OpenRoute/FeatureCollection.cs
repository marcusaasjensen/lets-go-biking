namespace RoutingServer.Models.OpenRoute
{
    using Newtonsoft.Json;
    using System;
    using System.Collections.Generic;
    using System.Runtime.Serialization;

    public class FeatureCollection
    {
        [JsonProperty("features")]
        public List<Feature> Features { get; set; }
    }

    public class Feature
    {

        [JsonProperty("properties")]
        public Properties Properties { get; set; }

        [JsonProperty("geometry")]
        public Geometry Geometry { get; set; }
    }

    [DataContract(Name = "geometry")]
    public class Geometry
    {
        public Geometry(List<List<double>> coordinates) => Coordinates = coordinates;

        [DataMember(Name = "coordinates")]
        public List<List<double>> Coordinates { get; set; }
    }

    public class Properties
    {
        [JsonProperty("segments")]
        public List<Segment> Segments { get; set; }
    }

    public class Segment
    {
        [JsonProperty("distance")]
        public double Distance { get; set; }

        [JsonProperty("duration")]
        public double Duration { get; set; }

        [JsonProperty("steps")]
        public List<Step> Steps { get; set; }
    }

    [DataContract(Name="step")]
    public class Step
    {
        [DataMember(Name = "distance")]
        public double Distance { get; set; }

        [DataMember(Name = "duration")]
        public double Duration { get; set; }

        [DataMember(Name = "instruction")]
        public string Instruction { get; set; }

        [DataMember(Name = "name")]
        public string Name { get; set; }
        public override string ToString() => $"{Instruction} ({Name})";
    }
}
