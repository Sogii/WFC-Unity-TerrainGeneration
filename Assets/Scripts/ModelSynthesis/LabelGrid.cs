using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;

public class LabelGrid
{
    public int Width { get; private set; }
    public int Height { get; private set; }
    public List<ModelTile>[,] Grid { get; private set; }
    private AdjacencyMatrix adjacencyMatrix;

    public LabelGrid(int width, int height, AdjacencyMatrix adjacencyMatrix)
    {
        Width = width;
        Height = height;
        this.adjacencyMatrix = adjacencyMatrix;
        Grid = new List<ModelTile>[width, height];
        InitializeGrid();
    }

    private void InitializeGrid()
    {
        for (int y = 0; y < Height; y++)
        {
            for (int x = 0; x < Width; x++)
            {
                Grid[x, y] = new List<ModelTile>();
            }
        }
    }

    public void AssignAllPossibleLabels(List<ModelTile> allModelTiles)
    {
        for (int y = 0; y < Height; y++)
        {
            for (int x = 0; x < Width; x++)
            {
                Grid[x, y] = new List<ModelTile>(allModelTiles);
            }
        }
    }

    public List<ModelTile> GetLabelsAt(int x, int y)
    {
        if (x >= 0 && x < Width && y >= 0 && y < Height)
        {
            return Grid[x, y];
        }
        else
        {
            Debug.LogError($"Invalid coordinates: ({x}, {y}). Grid dimensions: {Width}x{Height}.");
            return new List<ModelTile>(); // Returning an empty list to indicate no valid tiles
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
                foreach (ModelTile modelTile in Grid[x, y])
                {
                    builder.Append($"{modelTile.tileType}, ");
                }
                builder.AppendLine();
            }
        }

        Debug.Log(builder.ToString());
    }

    public void RemoveLabelAt(int x, int y, ModelTile modelTileToRemove)
    {
        if (x >= 0 && x < Width && y >= 0 && y < Height)
        {
            // Check if the list at the specified coordinates contains the ModelTile to remove
            if (Grid[x, y].Contains(modelTileToRemove))
            {
                // If it does, remove it
                Grid[x, y].Remove(modelTileToRemove);
            }
            else
            {
                Debug.LogWarning($"ModelTile to remove not found at ({x},{y}).");
            }
        }
        else
        {
            Debug.LogError($"Invalid coordinates: ({x}, {y}). Grid dimensions: {Width}x{Height}.");
        }
    }

}
