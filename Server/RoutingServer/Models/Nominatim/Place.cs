using Newtonsoft.Json;

namespace RoutingServer.Models.Nominatim
{
    public class Place
    {
        [JsonProperty("lat")]
        public string Lat { get; set; }

        [JsonProperty("lon")]
        public string Lon { get; set; }

        [JsonProperty("address")]
        public AddressDetails Address { get; set; }
    }


}
