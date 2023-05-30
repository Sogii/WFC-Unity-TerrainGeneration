using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class UtilityFunctions
{
    /// <summary>
    /// Returns a neighbouring coordinate in the given direction.
    /// The method is used by the propagation manager to navigate the grid and add tiles to the queue.
    /// </summary>
    /// <param name="cord">The coordinate from which to find the neighbour.</param>
    /// <param name="direction">Direction in which to find the neighbouring coordinate.</param>
    /// <returns>Neighbouring coordinate in the given direction if it's one of the main cardinal directions (North, East, South, West). Otherwise, returns an invalid coordinate (-1, -1).</returns>

    public static Coordinate GetNeighbourCoordinate(Coordinate cord, SharedData.Direction direction)
    {
        switch (direction)
        {
            case SharedData.Direction.North: return new Coordinate(cord.X, cord.Y + 1);
            case SharedData.Direction.NorthEast: return new Coordinate(cord.X + 1, cord.Y + 1);
            case SharedData.Direction.East: return new Coordinate(cord.X + 1, cord.Y);
            case SharedData.Direction.SouthEast: return new Coordinate(cord.X + 1, cord.Y - 1);
            case SharedData.Direction.South: return new Coordinate(cord.X, cord.Y - 1);
            case SharedData.Direction.SouthWest: return new Coordinate(cord.X - 1, cord.Y - 1);
            case SharedData.Direction.West: return new Coordinate(cord.X - 1, cord.Y);
            case SharedData.Direction.NorthWest: return new Coordinate(cord.X - 1, cord.Y + 1);
            default: return new Coordinate(-1, -1); //returning invalid coordinates by default
        }
    }

    /// <summary>
    /// Checks if the given coordinate is within the bounds of the grid.
    /// This method is used to prevent out-of-bounds errors when accessing the grid.
    /// </summary>
    /// <param name="coordinate">The coordinate to check.</param>
    /// <param name="labelGrid">The grid in which the coordinate should exist.</param>
    /// <returns>True if the coordinate is within the grid bounds, false otherwise.</returns>
    public static bool IsWithinGridBounds(Coordinate coordinate, LabelGrid labelGrid)
    {
        if (coordinate.X >= 0 && coordinate.X < labelGrid.Width && coordinate.Y >= 0 && coordinate.Y < labelGrid.Height)
        {
            return true;
        }
        else return false;
    }


}
