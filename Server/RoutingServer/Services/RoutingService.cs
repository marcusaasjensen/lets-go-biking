using System;
using System.Collections.Generic;
using System.Device.Location;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using RoutingServer.JCDecauxService;
using RoutingServer.Models.OpenRoute;

namespace RoutingServer.Services
{
    public class RoutingService : IRoutingService
    {
        private NominatimService _nominatimService;
        private OpenRouteService _openRouteService;

        public async Task<List<Itinerary>> GetItinerary(string origin, string destination)
        {
            using (var client = new HttpClient())
            {
                _nominatimService = new NominatimService(client);
                _openRouteService = new OpenRouteService(client);

                var originGeoCoordinate = await _nominatimService.PlaceToGeoCoordinateAsync(origin);
                var destinationGeoCoordinate = await _nominatimService.PlaceToGeoCoordinateAsync(destination);

                if(originGeoCoordinate == null || destinationGeoCoordinate == null) return new List<Itinerary>();

                var originCity = await _nominatimService.GetCityFromGeoCoordinateAsync(originGeoCoordinate);
                var destinationCity = await _nominatimService.GetCityFromGeoCoordinateAsync(destinationGeoCoordinate);

                var jCDecauxServiceClient = new JCDecauxServiceClient();

                var nearestOriginStation = await jCDecauxServiceClient.GetNearestAvailableStationFromCoordinateAsync(originGeoCoordinate, originCity, true);
                var nearestDestinationStation = await jCDecauxServiceClient.GetNearestAvailableStationFromCoordinateAsync(destinationGeoCoordinate, destinationCity, false);
                
                jCDecauxServiceClient.Close();

                if (nearestOriginStation == null || nearestDestinationStation == null) return new List<Itinerary>();

                var stepsOriginToDestination = await _openRouteService.GetItinerarySteps(originGeoCoordinate, destinationGeoCoordinate, true);

                // If we can't walk from the origin to the destination, then we can't with a bicycle either.
                
                if (stepsOriginToDestination == null) return new List<Itinerary>();

                var originStationGeoCoordinate = new GeoCoordinate(nearestOriginStation.Position.Latitude, nearestOriginStation.Position.Longitude);
                var destinationStationGeoCoordinate = new GeoCoordinate(nearestDestinationStation.Position.Latitude, nearestDestinationStation.Position.Longitude);

                var stepsToOriginStation = await _openRouteService.GetItinerarySteps(originGeoCoordinate, originStationGeoCoordinate, true);
                var stepsToDestinationStation = await _openRouteService.GetItinerarySteps(originStationGeoCoordinate, destinationStationGeoCoordinate);
                var stepsToFinaleDestination = await _openRouteService.GetItinerarySteps(destinationStationGeoCoordinate, destinationGeoCoordinate, true);


                var bicycleItineraries = new List<Itinerary>
                {
                    stepsToOriginStation,
                    stepsToDestinationStation,
                    stepsToFinaleDestination
                };

                var isWalkingFaster = IsWalkingFasterThanBicycling(stepsOriginToDestination, bicycleItineraries);
                return isWalkingFaster ? new List<Itinerary> { stepsOriginToDestination } : bicycleItineraries;
            }
        }

        private static double TotalDurationOfItineraries(IEnumerable<Itinerary> itineraries) => itineraries.Sum(itinerary => itinerary.TotalDuration);
        private static bool IsWalkingFasterThanBicycling(Itinerary walkingItinerary, IEnumerable<Itinerary> bicycleItineraries)
        {
            var totalWalkingDuration = walkingItinerary.TotalDuration;
            var totalItineraryDuration = TotalDurationOfItineraries(bicycleItineraries);
            return totalWalkingDuration < totalItineraryDuration;
        }
    }
}
