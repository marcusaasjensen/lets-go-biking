using Newtonsoft.Json;
namespace RoutingServer.Models.Nominatim
{
    public class AddressDetails
    {
        [JsonProperty("city")]
        public string City { get; set; }

        [JsonProperty("village")]
        public string Village{ get; set; }

        [JsonProperty("town")]
        public string Town { get; set; }
    }
}
