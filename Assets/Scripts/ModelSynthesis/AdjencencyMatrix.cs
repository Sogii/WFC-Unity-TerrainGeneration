using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

public class AdjacencyMatrix
{
    private Dictionary<ModelTile, Dictionary<ModelTile, List<SharedData.Direction>>> matrix;
    private SharedData sharedData;
    private Dictionary<string, Dictionary<SharedData.Direction, HashSet<string>>> adjacencyDictionary;

    public AdjacencyMatrix(Dictionary<string, Dictionary<SharedData.Direction, HashSet<string>>> adjacencyDictionary, SharedData sharedData)
    {
        this.sharedData = sharedData;
        this.adjacencyDictionary = adjacencyDictionary;
        matrix = ConstructAdjacencyMatrix();
    }

    private Dictionary<ModelTile, Dictionary<ModelTile, List<SharedData.Direction>>> ConstructAdjacencyMatrix()
    {
        var adjacencyMatrix = new Dictionary<ModelTile, Dictionary<ModelTile, List<SharedData.Direction>>>();

        foreach (ModelTile modelTile in sharedData.AllModelTiles)
        {
            adjacencyMatrix[modelTile] = new Dictionary<ModelTile, List<SharedData.Direction>>();
        }

        foreach (KeyValuePair<string, Dictionary<SharedData.Direction, HashSet<string>>> outerEntry in adjacencyDictionary)
        {
            ModelTile i = sharedData.AllModelTiles.First(tile => tile.tileType.ToString() == outerEntry.Key);
            if (i == null)
            {
                Debug.LogError("No ModelTile found matching key: " + outerEntry.Key);
                continue;
            }

            foreach (KeyValuePair<SharedData.Direction, HashSet<string>> innerEntry in outerEntry.Value)
            {
                foreach (string adjacentTileType in innerEntry.Value)
                {
                    ModelTile j = sharedData.AllModelTiles.First(tile => tile.tileType.ToString() == adjacentTileType);

                    if (!adjacencyMatrix[i].ContainsKey(j))
                    {
                        adjacencyMatrix[i][j] = new List<SharedData.Direction>();
                    }
                    adjacencyMatrix[i][j].Add(innerEntry.Key);
                }
            }
        }

        //  LogAdjacencyGrid(adjacencyMatrix);

        return adjacencyMatrix;
    }

    private static void LogAdjacencyGrid(Dictionary<ModelTile, Dictionary<ModelTile, List<SharedData.Direction>>> adjacencyMatrix)
    {
        Debug.Log("Adjacency Matrix: ");
        foreach (KeyValuePair<ModelTile, Dictionary<ModelTile, List<SharedData.Direction>>> outerEntry in adjacencyMatrix)
        {
            string row = outerEntry.Key.tileType + ": ";
            foreach (KeyValuePair<ModelTile, List<SharedData.Direction>> innerEntry in outerEntry.Value)
            {
                row += innerEntry.Key.tileType + " (" + string.Join(", ", innerEntry.Value) + "), ";
            }
            Debug.Log(row);

        }
    }


    /// <summary>
    /// Returns true if the two tiles are adjacent in the given direction in the adjacency matrix 
    /// </summary>
    public bool CheckAdjacency(ModelTile tileType1, ModelTile tileType2, SharedData.Direction direction)
    {
        if (!matrix.ContainsKey(tileType1))
        {
         //   Debug.Log($"Matrix does not contain key for tile type: {tileType1.tileType}.");
            return false;
        }
        if (!matrix[tileType1].ContainsKey(tileType2))
        {
          //  Debug.Log($"Matrix does not contain key for tile type: {tileType2.tileType} in the adjacency list of {tileType1.tileType}.");
            return false;
        }
        if (!matrix[tileType1][tileType2].Contains(direction))
        {
          //  Debug.Log($"{tileType1.tileType} and {tileType2.tileType} are not adjacent in the direction: {direction.ToString()}.");
            return false;
        }

       // Debug.Log($"{tileType1.tileType} and {tileType2.tileType} are adjacent in the direction: {direction.ToString()}.");
        return true;
    }

}
