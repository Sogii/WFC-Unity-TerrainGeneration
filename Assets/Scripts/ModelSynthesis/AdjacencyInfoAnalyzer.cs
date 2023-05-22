using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Text;
using System;
public class AdjacencyInfoAnalyzer : MonoBehaviour
{

    public SharedData SharedData;

    public ModelTile[] tiles = new ModelTile[16];

    Dictionary<string, Dictionary<SharedData.Direction, HashSet<string>>> adjecencyDictionary = new Dictionary<string, Dictionary<SharedData.Direction, HashSet<string>>>();

    string filePath;

    public Dictionary<string, Dictionary<SharedData.Direction, HashSet<string>>> GetAdjacencyDictionary()
    {
        return adjecencyDictionary;
    }

    void awake()
    {
        string filePath = Path.Combine(Application.persistentDataPath, "Resources/temp.txt");
    }
    public void AnalyzeAdjacency()
    {
        for (int i = 0; i < tiles.Length; i++)
        {
            string tileType = tiles[i].tileType.ToString();

            // Initialize the dictionaries for this tile type
            if (!adjecencyDictionary.ContainsKey(tileType))
            {
                adjecencyDictionary[tileType] = new Dictionary<SharedData.Direction, HashSet<string>>();
                foreach (SharedData.Direction dir in System.Enum.GetValues(typeof(SharedData.Direction)))
                {
                    adjecencyDictionary[tileType][dir] = new HashSet<string>();
                }
            }

            // Check each direction
            foreach (SharedData.Direction dir in System.Enum.GetValues(typeof(SharedData.Direction)))
            {
                int adjacentIndex = GetAdjacentIndex(i, dir);
                if (adjacentIndex >= 0 && adjacentIndex < tiles.Length)
                {
                    string adjacentTileType = tiles[adjacentIndex].tileType.ToString();
                    adjecencyDictionary[tileType][dir].Add(adjacentTileType);
                }
            }
        }
        WriteToFile(adjecencyDictionary);
    }


    // Given an index in the 1D array and a direction, returns the index of the tile in that direction
    int GetAdjacentIndex(int index, SharedData.Direction dir)
    {
        int x = index % 4;
        int y = index / 4;

        switch (dir)
        {
            case SharedData.Direction.North:
                y++;
                break;
            case SharedData.Direction.East:
                x++;
                break;
            case SharedData.Direction.South:
                y--;
                break;
            case SharedData.Direction.West:
                x--;
                break;
        }

        if (x < 0 || x >= 4 || y < 0 || y >= 4) return -1; // out of bounds

        return y * 4 + x;
    }

    public void WriteToFile(Dictionary<string, Dictionary<SharedData.Direction, HashSet<string>>> dict)
    {
        // Create file path
        string path = Path.Combine(Application.dataPath, "Resources/temp.txt");

        // Use a StringBuilder to create the string to write to the file
        StringBuilder builder = new StringBuilder();

        foreach (KeyValuePair<string, Dictionary<SharedData.Direction, HashSet<string>>> outerEntry in dict)
        {
            builder.AppendLine($"{outerEntry.Key}:");
            foreach (KeyValuePair<SharedData.Direction, HashSet<string>> innerEntry in outerEntry.Value)
            {
                builder.AppendLine($"\t{innerEntry.Key}: {string.Join(", ", innerEntry.Value)}");
            }
        }

        // Write to the file
        File.WriteAllText(path, builder.ToString());
    }
}
