using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Text;
using System;

[System.Serializable]
public class ModelTile
{
    public GameObject gameObject;
    public string type;
}
public class AdjacencyInfoAnalyzer : MonoBehaviour
{

    // Assuming you have an enum for directions like this
    public enum Direction { North, East, South, West }
    public string[] tileTypes;

    public ModelTile[] tiles = new ModelTile[16];

    Dictionary<string, Dictionary<Direction, HashSet<string>>> adjecencyDictionary = new Dictionary<string, Dictionary<Direction, HashSet<string>>>();

    string filePath;
    void awake()
    {
        string filePath = Path.Combine(Application.persistentDataPath, "Resources/temp.txt");
    }
    void Start()
    {
        AnalyzeAdjacency();
        WriteToFile(adjecencyDictionary);
    }    

    void AnalyzeAdjacency()
    {
        for (int i = 0; i < tiles.Length; i++)
        {
            string tileType = tiles[i].type;

            // Initialize the dictionaries for this tile type
            if (!adjecencyDictionary.ContainsKey(tileType))
            {
                adjecencyDictionary[tileType] = new Dictionary<Direction, HashSet<string>>();
                foreach (Direction dir in System.Enum.GetValues(typeof(Direction)))
                {
                    adjecencyDictionary[tileType][dir] = new HashSet<string>();
                }
            }

            // Check each direction
            foreach (Direction dir in System.Enum.GetValues(typeof(Direction)))
            {
                int adjacentIndex = GetAdjacentIndex(i, dir);
                if (adjacentIndex >= 0 && adjacentIndex < tiles.Length)
                {
                    string adjacentTileType = tiles[adjacentIndex].type;
                    adjecencyDictionary[tileType][dir].Add(adjacentTileType);
                }
            }
        }
    }


    // Given an index in the 1D array and a direction, returns the index of the tile in that direction
    int GetAdjacentIndex(int index, Direction dir)
    {
        int x = index % 4;
        int y = index / 4;

        switch (dir)
        {
            case Direction.North:
                y++;
                break;
            case Direction.East:
                x++;
                break;
            case Direction.South:
                y--;
                break;
            case Direction.West:
                x--;
                break;
        }

        if (x < 0 || x >= 4 || y < 0 || y >= 4) return -1; // out of bounds

        return y * 4 + x;
    }

    public void WriteToFile(Dictionary<string, Dictionary<AdjacencyInfoAnalyzer.Direction, HashSet<string>>> dict)
    {
        // Create file path
        string path = Path.Combine(Application.dataPath, "Resources/temp.txt");

        // Use a StringBuilder to create the string to write to the file
        StringBuilder builder = new StringBuilder();

        foreach (KeyValuePair<string, Dictionary<AdjacencyInfoAnalyzer.Direction, HashSet<string>>> outerEntry in dict)
        {
            builder.AppendLine($"{outerEntry.Key}:");
            foreach (KeyValuePair<AdjacencyInfoAnalyzer.Direction, HashSet<string>> innerEntry in outerEntry.Value)
            {
                builder.AppendLine($"\t{innerEntry.Key}: {string.Join(", ", innerEntry.Value)}");
            }
        }

        // Write to the file
        File.WriteAllText(path, builder.ToString());
    }

    public int[,] ConstructAdjacencyMatrix()
    {
        int tileTypeCount = tileTypes.Length;
        int[,] adjacencyMatrix = new int[tileTypeCount, tileTypeCount];

        // Initialize matrix with zeros
        for (int i = 0; i < tileTypeCount; i++)
        {
            for (int j = 0; j < tileTypeCount; j++)
            {
                adjacencyMatrix[i, j] = 0;
            }
        }

        // Iterate over adjacencyDictionary and set adjacencyMatrix[i, j] = 1 if tile type i can be adjacent to tile type j
        foreach (KeyValuePair<string, Dictionary<Direction, HashSet<string>>> outerEntry in adjecencyDictionary)
        {
            int i = Array.IndexOf(tileTypes, outerEntry.Key);
            foreach (KeyValuePair<Direction, HashSet<string>> innerEntry in outerEntry.Value)
            {
                foreach (string adjacentTileType in innerEntry.Value)
                {
                    int j = Array.IndexOf(tileTypes, adjacentTileType);
                    adjacencyMatrix[i, j] = 1;
                }
            }
        }

        return adjacencyMatrix;
    }

}
