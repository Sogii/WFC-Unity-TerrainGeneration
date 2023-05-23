using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class UtilityFunctions
{
    public static (int, int) GetNeighbor(int x, int y, SharedData.Direction direction)
    {
        switch (direction)
        {
            case SharedData.Direction.North: return (x, y + 1);
            case SharedData.Direction.East: return (x + 1, y);
            case SharedData.Direction.South: return (x, y - 1);
            case SharedData.Direction.West: return (x - 1, y);
            default: return(-1, -1);
        }
    }
}
