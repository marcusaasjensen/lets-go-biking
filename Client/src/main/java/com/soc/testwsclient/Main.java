package com.soc.testwsclient;

import com.map.MapViewer;
import com.parsed.ParsedItinerary;
import com.soap.ws.client.generated.IRoutingService;
import com.soap.ws.client.generated.Itinerary;
import com.soap.ws.client.generated.RoutingService;
import com.ui.ItineraryDescriber;
import com.ui.ItineraryUserInterface;
import com.util.SaveUtility;

import java.util.ArrayList;
import java.util.List;

public class Main {
    public static void main(String[] args) {
        System.out.println("\nWelcome to... Lets Go Biking!\n");

        ItineraryUserInterface ui = new ItineraryUserInterface();

        MapViewer map = null;

        while (true) {
            String origin = ui.promptOrigin();

            if (origin.equalsIgnoreCase("q")) {
                break;
            }

            String destination = ui.promptDestination();

            try {
                RoutingService routingService = new RoutingService();
                IRoutingService proxy = routingService.getBasicHttpBindingIRoutingService();

                List<Itinerary> itinerary = proxy.getItinerary(origin, destination).getItinerary();

                List<ParsedItinerary> parsedItineraries = new ArrayList<>();

                for (var it : itinerary)
                    parsedItineraries.add(new ParsedItinerary(it));

                String itineraryString = ItineraryDescriber.describeItinerary(origin, destination, parsedItineraries);

                System.out.println("\n\n" + itineraryString);

                if(!itinerary.isEmpty()){
                    if(map != null) map.closeMap();
                    map = new MapViewer(parsedItineraries);
                    map.showMap();
                }

                if (itinerary.isEmpty()) {
                    System.out.println("No itineraries found. Please, enter valid addresses.");
                } else {
                    if (ui.promptToSaveItinerary()) {
                        String filename = ui.promptFilename();
                        SaveUtility.saveToFile("../", filename + ".txt", itineraryString);
                    }
                }

            } catch (Exception e) {
                System.out.println(e.getMessage());
                System.out.println("Lets Go Biking servers are inactive. Please try again later.");
            }
        }
        if(map != null)
            map.closeMap();

        System.out.println("Thank you for using Lets Go Biking!");
    }
}
