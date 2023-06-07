using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainTypeGrid
{
    public int Width { get; private set; }
    public int Height { get; private set; }
    private SharedData.TerrainType[,] grid;

    public TerrainTypeGrid(int width, int height)
    {
        Width = width;
        Height = height;
        grid = new SharedData.TerrainType[width, height];
    }

    public void SetTerrainTypeAt(Coordinate cords, SharedData.TerrainType terrainType)
    {
        grid[cords.X, cords.Y] = terrainType;
    }

    public SharedData.TerrainType GetTerrainTypeAt(Coordinate coordinate)
    {
        return grid[coordinate.X, coordinate.Y];
    }

}
