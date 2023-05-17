using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;

public class LabelGrid
{
    public int Width { get; private set; }
    public int Height { get; private set; }
    public List<SharedData.TileType>[,] Grid { get; private set; }
    private AdjacencyMatrix adjacencyMatrix;

    public LabelGrid(int width, int height, AdjacencyMatrix adjacencyMatrix)
    {
        Width = width;
        Height = height;
        this.adjacencyMatrix = adjacencyMatrix;
        Grid = new List<SharedData.TileType>[width, height];
        InitializeGrid();
    }

    private void InitializeGrid()
    {
        for (int y = 0; y < Height; y++)
        {
            for (int x = 0; x < Width; x++)
            {
                Grid[x, y] = new List<SharedData.TileType>();
            }
        }
    }

    public void AssignAllPossibleLabels(List<ModelTile> allModelTiles)
    {
        for (int y = 0; y < Height; y++)
        {
            for (int x = 0; x < Width; x++)
            {
                foreach (ModelTile modelTile in allModelTiles)
                {
                    Grid[x, y].Add(modelTile.tileType);
                }
            }
        }
    }

   public SharedData.TileType GetLabelAt(int x, int y)
    {
        if (x >= 0 && x < Width && y >= 0 && y < Height)
        {
            return Grid[x, y][y];
        }
        else
        {
            Debug.LogError($"Invalid coordinates: ({x}, {y}). Grid dimensions: {Width}x{Height}.");
            return SharedData.TileType.None; // Assuming None is a valid TileType that indicates no tile
        }
    }

    public void PrintGridLabels()
    {
        StringBuilder builder = new StringBuilder();
        for (int y = 0; y < Height; y++)
        {
            for (int x = 0; x < Width; x++)
            {
                builder.Append($"({x},{y}): ");
                foreach (SharedData.TileType label in Grid[x, y])
                {
                    builder.Append($"{label}, ");
                }
                builder.AppendLine();
            }
        }

        Debug.Log(builder.ToString());
    }
}
