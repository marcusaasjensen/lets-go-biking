
package com.parsed;

import com.soap.ws.client.generated.Itinerary;

import java.util.ArrayList;
import java.util.List;

public class ParsedItinerary {
    private final List<ParsedStep> allSteps;
    private final double totalDuration;
    private final double totalDistance;

    private final ParsedGeometry geometry;
    public ParsedItinerary(Itinerary itineraryToParse){
        this.totalDistance = itineraryToParse.getTotalDistance();
        this.totalDuration = itineraryToParse.getTotalDuration();
        this.allSteps = new ArrayList<>();
        this.geometry = new ParsedGeometry(itineraryToParse.getGeometry().getValue());

        for(var step : itineraryToParse.getAllSteps().getValue().getStep()){
            allSteps.add(new ParsedStep(step));
        }
    }

    public double getTotalDuration(){
        return this.totalDuration;
    }

    public double getTotalDistance(){
        return this.totalDistance;
    }

    @Override
    public String toString() {
        double distanceToKilometers = totalDistance / 1000.0;

        double durationToMinutes = totalDuration / 60.0;
        String str = getString(durationToMinutes, distanceToKilometers);

        StringBuilder stepsBuilder = new StringBuilder(str);

        for (var step : allSteps) {
            stepsBuilder.append(String.format("     %s.%n", step.toString()));
        }

        return stepsBuilder.toString();
    }

    private String getString(double durationToMinutes, double distanceToKilometers) {
        double durationToHours = durationToMinutes / 60.0;

        String durationToMinutesText = String.format("%.2f", durationToMinutes);
        String durationToHoursText = String.format("%.2f", durationToHours);
        String distanceToKilometersText = String.format("%.2f", distanceToKilometers);
        String totalDistanceToMetersText = String.format("%.2f", totalDistance);

        return String.format("%nDistance: %s km (%s m), Duration: %s hrs (%s min)%n%n   Steps:%n%n",
                distanceToKilometersText,
                totalDistanceToMetersText,
                durationToHoursText,
                durationToMinutesText);
    }

    public ParsedGeometry getGeometry() {
        return geometry;
    }
}
