using System;
using System.Device.Location;
using System.Globalization;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using RoutingServer.Models.OpenRoute;

namespace RoutingServer.Services
{
    internal class OpenRouteService
    {
        private readonly HttpClient _client;
        private static string _apiKey = "5b3ce3597851110001cf624877e551432d7a4740b51c776073d5b1c3";
        private const string Uri = "https://api.openrouteservice.org/v2/directions/";
        public OpenRouteService(HttpClient client) => _client = client;

        public async Task<Itinerary> GetItinerarySteps(GeoCoordinate origin, GeoCoordinate destination, bool walk = false)
        {
            try
            {
                var query = BuildOpenRouteQuery(origin, destination, walk);
                var response = await _client.GetAsync(query);

                if (!response.IsSuccessStatusCode) throw new OpenRouteException(response);

                var content = await response.Content.ReadAsStringAsync();

                var jsonData = JsonConvert.DeserializeObject<FeatureCollection>(content);

                return new Itinerary(jsonData);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return null;
        }

        private static string BuildOpenRouteQuery(GeoCoordinate origin, GeoCoordinate destination, bool walk = false)
        {
            var travelType = walk ? "foot-walking" : "cycling-road";

            var latitudeOrigin = ConvertToQueryValue(origin.Latitude);
            var longitudeOrigin = ConvertToQueryValue(origin.Longitude);
            var latitudeDestination = ConvertToQueryValue(destination.Latitude);
            var longitudeDestination = ConvertToQueryValue(destination.Longitude);


            DotNetEnv.Env.Load();
            _apiKey = Environment.GetEnvironmentVariable("OPEN_ROUTE_SERVICE_API_KEY");

            var query = $"{Uri}{travelType}?api_key={_apiKey}&start={longitudeOrigin},{latitudeOrigin}&end={longitudeDestination},{latitudeDestination}";
            
            return query;
        }

        private static string ConvertToQueryValue(double value) => value.ToString(CultureInfo.InvariantCulture);
    }
    internal sealed class OpenRouteException : Exception
    {
        public OpenRouteException(HttpResponseMessage response) : base($"Open Route Service API request failed with status code {response.StatusCode}: {response.ReasonPhrase}") { }
    }
}
