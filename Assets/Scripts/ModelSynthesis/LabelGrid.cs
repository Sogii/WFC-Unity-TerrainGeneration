using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;

public class LabelGrid
{
    public int Width { get; private set; }
    public int Height { get; private set; }
    public List<int>[,] Grid { get; private set; }
    private AdjacencyMatrix adjacencyMatrix;

    public LabelGrid(int width, int height, AdjacencyMatrix adjacencyMatrix)
    {
        Width = width;
        Height = height;
        this.adjacencyMatrix = adjacencyMatrix;
        Grid = new List<int>[width, height];
        InitializeGrid();
    }

    private void InitializeGrid()
    {
        for (int y = 0; y < Height; y++)
        {
            for (int x = 0; x < Width; x++)
            {
                Grid[x, y] = new List<int>();
            }
        }
    }

    public void AssignAllPossibleLabels(List<ModelTile> allmodelTiles)
    {
        int totalLabels = allmodelTiles.Count;
        for (int y = 0; y < Height; y++)
        {
            for (int x = 0; x < Width; x++)
            {
                for (int label = 0; label < totalLabels; label++)
                {
                    Grid[x, y].Add(label);
                }
            }
        }
    }


    public int GetLabelAt(int x, int y)
    {
        if (x >= 0 && x < Width && y >= 0 && y < Height)
        {
            return Grid[x, y][y];
        }
        else
        {
            Debug.LogError($"Invalid coordinates: ({x}, {y}). Grid dimensions: {Width}x{Height}.");
            return -1;
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
                foreach (int label in Grid[x, y])
                {
                    builder.Append($"{label}, ");
                }
                builder.AppendLine();
            }
        }

        Debug.Log(builder.ToString());
    }

}
