using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class UtilityFunctions
{
   public static Coordinate GetNeighbourcoordinate(Coordinate cord, SharedData.Direction direction)
{
    switch (direction)
    {
        case SharedData.Direction.North: return new Coordinate(cord.X, cord.Y + 1);
        case SharedData.Direction.East: return new Coordinate(cord.X + 1, cord.Y);
        case SharedData.Direction.South: return new Coordinate(cord.X, cord.Y - 1);
        case SharedData.Direction.West: return new Coordinate(cord.X - 1, cord.Y);
        default: return new Coordinate(-1, -1); //returning invalid coordinates by default
    }
}

    public static bool IsWithinGridBounds(Coordinate coordinate, LabelGrid labelGrid)
    {
        if (coordinate.X >= 0 && coordinate.X < labelGrid.Width && coordinate.Y >= 0 && coordinate.Y < labelGrid.Height)
        {
            return true;
        }
        else return false;
    }


}
