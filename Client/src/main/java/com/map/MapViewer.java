package com.map;

import com.parsed.ParsedGeometry;
import com.parsed.ParsedItinerary;
import org.jxmapviewer.JXMapViewer;
import org.jxmapviewer.OSMTileFactoryInfo;
import org.jxmapviewer.viewer.DefaultTileFactory;
import org.jxmapviewer.viewer.GeoPosition;
import org.jxmapviewer.viewer.TileFactoryInfo;

import javax.swing.*;
import java.awt.*;
import java.util.ArrayList;
import java.util.List;
import java.util.stream.Collectors;

public class MapViewer {

    private JFrame frame;
    private final JXMapViewer mapViewer;
    private final List<List<GeoPosition>> waypoints;
    private int currentWaypointIndex = 0;

    public MapViewer(List<ParsedItinerary> itineraries) {
        mapViewer = new JXMapViewer();
        TileFactoryInfo info = new OSMTileFactoryInfo();
        DefaultTileFactory tileFactory = new DefaultTileFactory(info);
        mapViewer.setTileFactory(tileFactory);
        tileFactory.setThreadPoolSize(8);

        waypoints = new ArrayList<>();

        int i = 0;
        for (var itinerary : itineraries) {
            waypoints.add(new ArrayList<>());
            ParsedGeometry geometry = itinerary.getGeometry();
            double[][] coordinates = geometry.getCoordinates();

            for (double[] coordinate : coordinates) {
                waypoints.get(i).add(new GeoPosition(coordinate[1], coordinate[0]));
            }
            i++;
        }
    }

    public void showMap() {
        mapViewer.setZoom(13);
        mapViewer.setAddressLocation(waypoints.get(0).get(0));

        frame = new JFrame("Map Itinerary Viewer");

        JButton zoomOutButton = new JButton("Zoom Out");
        JButton zoomInButton = new JButton("Zoom In");
        JButton nextButton = new JButton("Next Step");
        JButton backButton = new JButton("Previous Step");

        zoomOutButton.addActionListener(e -> mapViewer.setZoom(mapViewer.getZoom() + 1));

        zoomInButton.addActionListener(e -> mapViewer.setZoom(mapViewer.getZoom() - 1));

        List<GeoPosition> allWaypoints = waypoints.stream()
                .flatMap(List::stream)
                .collect(Collectors.toList());

        nextButton.addActionListener(e -> {
            if (currentWaypointIndex < allWaypoints.size() - 1) {
                currentWaypointIndex++;
                GeoPosition nextLocation = allWaypoints.get(currentWaypointIndex);
                mapViewer.setAddressLocation(nextLocation);
            }

            if (mapViewer.getZoom() > 1) {
                mapViewer.setZoom(mapViewer.getZoom() - 1);
            }
        });

        backButton.addActionListener(e -> {
            if (currentWaypointIndex > 0) {
                currentWaypointIndex--;
                GeoPosition previousLocation = allWaypoints.get(currentWaypointIndex);
                mapViewer.setAddressLocation(previousLocation);

                if (mapViewer.getZoom() > 1) {
                    mapViewer.setZoom(mapViewer.getZoom() - 1);
                }
            }
        });

        JPanel buttonPanel = new JPanel();
        buttonPanel.add(zoomOutButton);
        buttonPanel.add(zoomInButton);
        buttonPanel.add(backButton);
        buttonPanel.add(nextButton);

        frame.getContentPane().setLayout(new BorderLayout());
        frame.getContentPane().add(buttonPanel, BorderLayout.NORTH);
        frame.getContentPane().add(mapViewer, BorderLayout.CENTER);

        frame.setSize(800, 600);
        frame.setDefaultCloseOperation(JFrame.EXIT_ON_CLOSE);
        frame.setVisible(true);

        var routePainter = new MultiColorRoutePainter(waypoints, List.of(Color.BLUE, Color.GREEN, Color.BLUE));
        mapViewer.setOverlayPainter(routePainter);
    }

    public void closeMap() {
        if (frame != null) {
            frame.dispose();
        }
    }
}
