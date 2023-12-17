using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Runtime.Serialization;

namespace RoutingServer.Models.OpenRoute
{
    [DataContract]
    public class Itinerary
    {
        public Itinerary(FeatureCollection itineraryData)
        {
            TotalDuration = 0;
            TotalDistance = 0;
            AllSteps = new List<Step>();

            var coordinates = itineraryData.Features[0].Geometry.Coordinates;
            Geometry = new Geometry(coordinates);

            foreach (var segment in itineraryData.Features[0].Properties.Segments)
            {
                TotalDuration += segment.Duration;
                TotalDistance += segment.Distance;
                AllSteps.AddRange(segment.Steps);
            }
        }

        [DataMember]
        public double TotalDistance { get; set; }
        [DataMember]
        public double TotalDuration { get; set; }
        [DataMember]
        public List<Step> AllSteps { get; set; }
        
        [DataMember]
        public Geometry Geometry { get; set; }

        public override string ToString()
        {
            var distanceToKilometers = Math.Round(TotalDistance / 1000, 2);

            var durationToMinutes = TotalDuration / 60;
            var durationToMinutesRounded = Math.Round(durationToMinutes);
            var durationToHours = Math.Round(durationToMinutes / 60, 2);

            var str = $"\nDistance: {distanceToKilometers.ToString(CultureInfo.InvariantCulture)} km ({TotalDistance.ToString(CultureInfo.InvariantCulture)} m), " +
                      $"Duration: {durationToHours.ToString(CultureInfo.InvariantCulture)} hrs ({durationToMinutesRounded.ToString(CultureInfo.InvariantCulture)} min)\n\n   Steps:\n\n";
            return AllSteps.Aggregate(str, (current, step) => current + $"     {step}.\n");
        }
    }
}
