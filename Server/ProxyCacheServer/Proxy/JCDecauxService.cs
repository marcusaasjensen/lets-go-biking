using System;
using System.Collections.Generic;
using System.Device.Location;
using System.Linq;
using System.Net.Http;
using System.ServiceModel;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace ProxyCacheServer.Proxy
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single, IncludeExceptionDetailInFaults = true)]
    public class JCDecauxService : IJCDecauxService
    {
        private string _apiKey;
        private HttpClient _client;
        private const string BaseUri = "https://api.jcdecaux.com/vls/v1/";
        private const int CacheDuration = 10;
        private const string AllStationCacheKey = "all";

        public async Task<Station> GetNearestAvailableStationFromCoordinateAsync(GeoCoordinate addressCoordinate, string city, bool isOrigin = false)
        {
            _client = new HttpClient();

            var standOption = isOrigin ? StandOption.GetBike : StandOption.ReturnBike;

            try
            {
                var allStations = await GetStationsFromContractAsync(city);
                var availableStations = FilterAvailableStations(allStations, standOption);
                var nearestStation = GetNearestStationFromCoordinate(availableStations, addressCoordinate);

                return nearestStation ?? throw new NullStationException();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

                Console.WriteLine("Can't find contract associated with address.");
                Console.WriteLine("Try to get the nearest station from all stations.");

                return await TryGetNearestAvailableStationFromAllAsync(addressCoordinate, standOption);
            }
        }

        private async Task<Station> TryGetNearestAvailableStationFromAllAsync(GeoCoordinate coordinate, StandOption option)
        {
            try
            {
                var allStations = await GetAllExistingStationsAsync();
                var availableStations = FilterAvailableStations(allStations, option);
                var nearestStation = GetNearestStationFromCoordinate(availableStations, coordinate);

                return nearestStation ?? throw new NullStationException();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.WriteLine("Can't find any available stations.");
                return null;
            }
        }

        private static IEnumerable<Station> FilterAvailableStations(IEnumerable<Station> allStations, StandOption option = StandOption.GetBike)
            => (from station in allStations
                let standAvailable = option == StandOption.GetBike ? station.HasBikes() : station.IsFull()
                where station.IsStandOpened() && standAvailable
                select station).ToList();

        private static Station GetNearestStationFromCoordinate(IEnumerable<Station> stations, GeoCoordinate coordinate)
        {
            var smallestDistance = double.MaxValue;
            Station nearestStation = null;

            foreach (var station in stations)
            {
                var stationCoordinate = new GeoCoordinate(station.Position.Latitude, station.Position.Longitude);
                var distanceToStation = coordinate.GetDistanceTo(stationCoordinate);

                if (!(distanceToStation < smallestDistance)) continue;

                nearestStation = station;
                smallestDistance = distanceToStation;
            }

            return nearestStation;
        }

        private async Task<T> GetFromJCDecauxQueryAsync<T>(string query) where T : class
        {
            var response = await _client.GetAsync(query);

            if (!response.IsSuccessStatusCode) throw new JCDecauxException(response);

            var content = await response.Content.ReadAsStringAsync();

            var jsonData = JsonConvert.DeserializeObject<T>(content);
            return jsonData;
        }

        private async Task<Station[]> GetStationsFromCacheOrApiAsync(string uri, string cacheKey)
        {
            var allStations = Cache.CacheUtility.GetFromCache<Station[]>(cacheKey);

            if (allStations != null) return allStations;

            DotNetEnv.Env.Load();
            _apiKey = Environment.GetEnvironmentVariable("JCDECAUX_API_KEY");
            allStations = await GetFromJCDecauxQueryAsync<Station[]>(uri);

            Cache.CacheUtility.SetToCache(cacheKey, allStations, CacheDuration);

            return allStations;
        }

        private async Task<Station[]> GetAllExistingStationsAsync() => await GetStationsFromCacheOrApiAsync($"{BaseUri}stations?apiKey={_apiKey}", AllStationCacheKey);
        private async Task<Station[]> GetStationsFromContractAsync(string city) => await GetStationsFromCacheOrApiAsync($"{BaseUri}stations?apiKey={_apiKey}&contract={city}", city);

    }

    internal sealed class JCDecauxException : Exception
    {
        public JCDecauxException(HttpResponseMessage response) : base($"JCDecaux API request failed with status code {response.StatusCode}: {response.ReasonPhrase}"){ }
    }

    internal sealed class NullStationException : Exception
    {
        public NullStationException() : base("Can't find any available station.") { }
    }
}
