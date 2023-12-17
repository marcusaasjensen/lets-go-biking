package com.parsed;

import com.soap.ws.client.generated.Geometry;

public class ParsedGeometry {
    private final double[][] coordinates;

    public ParsedGeometry(Geometry geometryToParse) {
        var coordinatesDoubles = geometryToParse.getCoordinates().getValue().getArrayOfdouble();
        this.coordinates = new double[coordinatesDoubles.size()][];
        for (int i = 0; i < coordinatesDoubles.size(); i++) {
            var coordinate = coordinatesDoubles.get(i).getDouble();
            this.coordinates[i] = new double[]{coordinate.get(0), coordinate.get(1)};
        }
    }

    public double[][] getCoordinates() {
        return coordinates;
    }
}
