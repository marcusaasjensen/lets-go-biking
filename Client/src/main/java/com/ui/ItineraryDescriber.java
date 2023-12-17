
package com.ui;

import com.parsed.ParsedItinerary;

import java.util.List;

public class ItineraryDescriber {
    private ItineraryDescriber(){}
    public static String describeItinerary(String origin, String destination, List<ParsedItinerary> itineraries){
        if(itineraries == null || itineraries.isEmpty()) {
            return "\nUnknown steps.\n";
        }

        return String.format("~ Origin: %s --> Destination: %s ~%n%n", origin, destination) +
                (itineraries.size() == 1 ?
                        describeWalkingItinerary(itineraries.get(0)) : describeBicycleItinerary(itineraries)) +
                "\n\nYou arrive at your destination.\n";
    }

    private static String describeWalkingItinerary(ParsedItinerary walkingItinerary) {
        return String.format("%n-- From origin to destination by walking --%n%s%n", walkingItinerary);
    }

    private static String describeBicycleItinerary(List<ParsedItinerary> bicycleItineraries){
                ParsedItinerary stepsToOriginStation = bicycleItineraries.get(0);
                ParsedItinerary stepsToDestinationStation = bicycleItineraries.get(1);
                ParsedItinerary stepsToDestination = bicycleItineraries.get(2);

                String totalDurationText = String.format("%.2f", (stepsToOriginStation.getTotalDuration() +
                        stepsToDestinationStation.getTotalDuration() + stepsToDestination.getTotalDuration()) / 60.0 / 60.0);
                String totalDistanceText = String.format("%.2f", (stepsToOriginStation.getTotalDistance() +
                        stepsToDestinationStation.getTotalDistance() + stepsToDestination.getTotalDistance()) / 1000.0);

        return String.format("%n%nTotal distance: %s km, Total duration: %s Hrs%n%n",totalDistanceText, totalDurationText) +
                String.format("%n-- From origin to first station --%n%s%n", stepsToOriginStation) +
                String.format("%n-- From first station to finale station --%n%s%n", stepsToDestinationStation) +
                String.format("%n-- From finale station to destination --%n%s%n", stepsToDestination);
    }
}
