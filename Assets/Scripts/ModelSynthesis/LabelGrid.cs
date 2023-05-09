using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    public void AssignAllPossibleLabels()
    {
        int numLabels = adjacencyMatrix.Matrix.GetLength(0);

        for (int y = 0; y < Height; y++)
        {
            for (int x = 0; x < Width; x++)
            {
                for (int label = 0; label < numLabels; label++)
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
        if (Grid[x, y].Count > 0)
        {
            return Grid[x, y][0];
        }
        else
        {
            Debug.LogError($"No label assigned to the cell at coordinates: ({x}, {y}).");
            return -1;
        }
    }
    else
    {
        Debug.LogError($"Invalid coordinates: ({x}, {y}). Grid dimensions: {Width}x{Height}.");
        return -1;
    }
}

}


