using System.Device.Location;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace ProxyCacheServer.Proxy
{
    [ServiceContract]
    public interface IJCDecauxService
    {
        [OperationContract]
        Task<Station> GetNearestAvailableStationFromCoordinateAsync(GeoCoordinate coordinate, string city, bool isOrigin = false);
    }

    [DataContract]
    public class Station
    {
        [DataMember(Name = "name")]
        public string Name { get; set; }

        [DataMember(Name = "position")]
        public Position Position { get; set; }

        [DataMember(Name = "status")]
        public string Status { get; set; }

        [DataMember(Name = "available_bike_stands")]
        public int AvailableBikeStands { get; set; }

        [DataMember(Name = "available_bikes")]
        public int AvailableBikes { get; set; }

        public bool IsStandOpened() => Status == "OPEN";
        public bool IsFull() => AvailableBikeStands == 0;
        public bool HasBikes() => AvailableBikes > 0;
    }

    [DataContract]
    public class Position
    {
        [DataMember(Name = "lat")]
        public double Latitude { get; set; }

        [DataMember(Name = "lng")]
        public double Longitude { get; set; }
    }

    [DataContract]
    public enum StandOption
    {
        [EnumMember]
        GetBike,
        [EnumMember]
        ReturnBike
    }

}
