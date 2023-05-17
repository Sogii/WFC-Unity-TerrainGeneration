using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class AdjacencyMatrix
{
    private int[,] matrix;
    private SharedData sharedData;
    private Dictionary<string, Dictionary<SharedData.Direction, HashSet<string>>> adjacencyDictionary;

    public AdjacencyMatrix(Dictionary<string, Dictionary<SharedData.Direction, HashSet<string>>> adjacencyDictionary, SharedData sharedData)
    {
        this.sharedData = sharedData;
        this.adjacencyDictionary = adjacencyDictionary;
        matrix = ConstructAdjacencyMatrix();
    }


    private int[,] ConstructAdjacencyMatrix()
    {
        int tileTypeCount = sharedData.TileTypes.Length;
        int[,] adjacencyMatrix = new int[tileTypeCount, tileTypeCount];

        // Initialize matrix with zeros
        for (int i = 0; i < tileTypeCount; i++)
        {
            for (int j = 0; j < tileTypeCount; j++)
            {
                adjacencyMatrix[i, j] = 0;
            }
        }

        // Populate adjacencyMatrix based on sharedData's adjacencyDictionary
        foreach (KeyValuePair<string, Dictionary<SharedData.Direction, HashSet<string>>> outerEntry in adjacencyDictionary)
        {
            int i = Array.FindIndex(sharedData.TileTypes, tile => tile.tileType.ToString() == outerEntry.Key);
            foreach (KeyValuePair<SharedData.Direction, HashSet<string>> innerEntry in outerEntry.Value)
            {
                foreach (string adjacentTileType in innerEntry.Value)
                {
                    int j = Array.FindIndex(sharedData.TileTypes, tile => tile.tileType.ToString() == adjacentTileType);
                    if (i >= 0 && j >= 0)
                        adjacencyMatrix[i, j] = 1;
                }
            }
        }

        // Debugging statement to print the adjacency matrix
       // LogAdjacencyGrid(tileTypeCount, adjacencyMatrix);

        return adjacencyMatrix;
    }

    private static void LogAdjacencyGrid(int tileTypeCount, int[,] adjacencyMatrix)
    {
        Debug.Log("Adjacency Matrix: ");
        for (int i = 0; i < tileTypeCount; i++)
        {
            string row = "";
            for (int j = 0; j < tileTypeCount; j++)
            {
                row += adjacencyMatrix[i, j].ToString() + " ";
            }
            Debug.Log(row);
        }
    }

    public bool CheckAdjacency(int label1, int label2)
    {
        // Verify the labels are within bounds
        if (label1 < 0 || label1 >= matrix.GetLength(0) || label2 < 0 || label2 >= matrix.GetLength(1))
        {
            return false;
        }

        return matrix[label1, label2] == 1;
    }
}


