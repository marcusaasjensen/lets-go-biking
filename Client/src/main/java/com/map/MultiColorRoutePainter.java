package com.map;

import org.jxmapviewer.JXMapViewer;
import org.jxmapviewer.viewer.GeoPosition;
import org.jxmapviewer.painter.Painter;

import java.awt.*;
import java.util.List;

public class MultiColorRoutePainter implements Painter<JXMapViewer> {

    private List<List<GeoPosition>> waypoints;
    private List<Color> colors;

    public MultiColorRoutePainter(List<List<GeoPosition>> waypoints, List<Color> colors) {
        this.waypoints = waypoints;
        this.colors = colors;
    }

    @Override
    public void paint(Graphics2D g, JXMapViewer map, int w, int h) {
        for (int i = 0; i < waypoints.size(); i++) {
            RoutePainter routePainter = new RoutePainter(waypoints.get(i), colors.get(i));
            routePainter.paint(g, map, w, h);
        }
    }
}

