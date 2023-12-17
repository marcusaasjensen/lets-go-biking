package com.util;

import java.io.FileWriter;
import java.io.IOException;

public class SaveUtility {
    private SaveUtility() {
        throw new IllegalStateException("Utility class");
    }
    public static void saveToFile(String directory, String filename, String str) {
        try (FileWriter writer = new FileWriter(directory + filename)) {
            writer.write(str);
            System.out.println("Saved to file: " + filename);
        } catch (IOException e) {
            System.out.println("Error saving itinerary to file: " + e.getMessage());
        }
    }
}