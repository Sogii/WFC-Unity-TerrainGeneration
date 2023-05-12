using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LabelGrid
{
    public int Width { get; private set; }
    public int Height { get; private set; }
    public int[,] Grid { get; private set; }
    private AdjacencyMatrix adjacencyMatrix;

    public LabelGrid(int width, int height, AdjacencyMatrix adjacencyMatrix, int[,] initialGrid)
    {
        Width = width;
        Height = height;
        this.adjacencyMatrix = adjacencyMatrix;
        Grid = initialGrid;
    }

    private void InitializeGrid()
    {
        for (int y = 0; y < Height; y++)
        {
            for (int x = 0; x < Width; x++)
            {
                Grid[x, y] = -1; // Initialize with an invalid value
            }
        }
    }

    public void AssignAllPossibleLabels()
    {
        // int numLabels = adjacencyMatrix.Matrix.GetLength(0);

        // for (int y = 0; y < Height; y++)
        // {
        //     for (int x = 0; x < Width; x++)
        //     {
        //         for (int label = 0; label < numLabels; label++)
        //         {
        //             Grid[x, y].Add(label);
        //         }
        //     }
        // }
    }

    public int GetLabelAt(int x, int y)
    {
        if (x >= 0 && x < Width && y >= 0 && y < Height)
        {
            return Grid[x, y];
        }
        else
        {
            Debug.LogError($"Invalid coordinates: ({x}, {y}). Grid dimensions: {Width}x{Height}.");
            return -1;
        }
    }

}


