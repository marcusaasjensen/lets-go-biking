using System;
using System.Device.Location;
using System.Globalization;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using Newtonsoft.Json;
using RoutingServer.Models.Nominatim;

namespace RoutingServer.Services
{
    internal class NominatimService
    {
        private const string BaseUri = "https://nominatim.openstreetmap.org";
        private const string UserAgent = "User-Agent";
        private const string QueryOption = "limit=1";

        private readonly HttpClient _client;

        public NominatimService(HttpClient client)
        {
            _client = client;

            //Required for the fair use of Nominatim API

            if (_client.DefaultRequestHeaders.Contains(UserAgent)) return;
            _client.DefaultRequestHeaders.Add(UserAgent, "LetsGoBiking");
        }

        private static string BuildNominatimQuery(string place)
        {
            var formattedPlace = HttpUtility.UrlEncode(place);
            var query = $"{BaseUri}/search?q={formattedPlace}&format=json&{QueryOption}";
            return query;
        }

        private static string BuildReverseNominatimQuery(GeoCoordinate coordinate)
        {
            var query = $"{BaseUri}/reverse?lat={coordinate?.Latitude.ToString(CultureInfo.InvariantCulture)}&lon={coordinate?.Longitude.ToString(CultureInfo.InvariantCulture)}&format=json";
            return query;
        }

        public async Task<GeoCoordinate> PlaceToGeoCoordinateAsync(string place)
        {
            try
            {
                var query= BuildNominatimQuery(place);
                var geoCoordinate = await GetGeoCoordinateFromQueryAsync(query);
                return geoCoordinate;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return null;
        }

        private async Task<GeoCoordinate> GetGeoCoordinateFromQueryAsync(string query)
        {
            var response = await _client.GetAsync(query);

            if (!response.IsSuccessStatusCode) throw new NominatimException(response);

            var contentPlace = await response.Content.ReadAsStringAsync();

            var jsonData = JsonConvert.DeserializeObject<Place[]>(contentPlace);

            if (jsonData == null || jsonData.Length == 0) throw new EmptyAddressDataException();

            var jsonPlace = jsonData[0];

            var geoCoordinate = new GeoCoordinate
            {
                Latitude = double.Parse(jsonPlace.Lat, NumberStyles.Any, CultureInfo.InvariantCulture),
                Longitude = double.Parse(jsonPlace.Lon, NumberStyles.Any, CultureInfo.InvariantCulture)
            };

            var place = jsonPlace.Address?.City ?? jsonPlace.Address?.Village ?? jsonPlace.Address?.Town;

            return geoCoordinate;
        }

        public async Task<string> GetCityFromGeoCoordinateAsync(GeoCoordinate coordinate)
        { 
            try
            {
                var query = BuildReverseNominatimQuery(coordinate);
                var city = await GetCityFromQueryAsync(query);
                return city;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return null;
        }

        private async Task<string> GetCityFromQueryAsync(string query)
        {
            var response = await _client.GetAsync(query);

            if (!response.IsSuccessStatusCode) throw new NominatimException(response);

            var contentPlace = await response.Content.ReadAsStringAsync();

            var jsonData = JsonConvert.DeserializeObject<Place>(contentPlace) ?? throw new EmptyAddressDataException();

            var place = jsonData.Address?.City ?? jsonData.Address?.Village ?? jsonData.Address?.Town;
            return place;
        }

    }

    internal sealed class NominatimException : Exception
    {
        public NominatimException(HttpResponseMessage response) : base($"Nominatim API request failed with status code {response.StatusCode}: {response.ReasonPhrase}") { }
    }

    internal sealed class EmptyAddressDataException : Exception
    {
        public EmptyAddressDataException() : base($"Can't get any information from address."){}
    }
}
