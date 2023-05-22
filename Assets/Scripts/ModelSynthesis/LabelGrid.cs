using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;
using System;

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
                if (x == Width / 2 && y == Height / 2)
                {
                    // If this is the center cell, assign two random labels

                    Grid[x,y].Add(allModelTiles[0]);
                    Grid[x,y].Add(allModelTiles[1]);
                }
                // Otherwise, assign all possible labels
                else
                {
                    foreach (ModelTile modelTile in allModelTiles)
                    {
                        Grid[x, y].Add(modelTile);
                    }
                }
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

    public void SetLabelsAt(int x, int y, List<ModelTile> labels)
    {
        if (labels == null || labels.Count == 0)
        {
            throw new ArgumentException("labels must not be null or empty");
        }

        // Check if the coordinates are within the grid boundaries
        if (x >= 0 && x < Width && y >= 0 && y < Height)
        {
            Debug.Log($"Setting labels at ({x},{y}) to {labels[0].tileType}.");
            Grid[x, y] = labels;
        }
        else
        {
            throw new ArgumentOutOfRangeException("Coordinates are outside of the grid boundaries");
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
                Debug.Log($"Removing {modelTileToRemove.tileType} at ({x},{y}).");
                Grid[x, y].Remove(modelTileToRemove);
                PrintGridLabels();
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
