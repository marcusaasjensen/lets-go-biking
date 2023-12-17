package com.ui;
import java.util.Scanner;

public class ItineraryUserInterface {
    private final Scanner scanner;

    public ItineraryUserInterface() {
        this.scanner = new Scanner(System.in);
    }

    public boolean promptToSaveItinerary() {
        System.out.print("Do you want to save the itinerary to a text file? (y/n): ");
        return scanner.nextLine().equalsIgnoreCase("y");
    }

    public String promptFilename() {
        System.out.print("Enter the filename (without extension): ");
        return scanner.nextLine();
    }

    public String promptOrigin() {
        System.out.print("Enter origin (type 'q' to quit): ");
        return scanner.nextLine();
    }

    public String promptDestination() {
        System.out.print("Enter destination: ");
        return scanner.nextLine();
    }
}