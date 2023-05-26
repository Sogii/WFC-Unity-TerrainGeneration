using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Text;
using System;
public class AdjacencyInfoAnalyzer : MonoBehaviour
{

    public SharedData SharedData;
    public int ExampleGridWidth = 10;
    public int ExampleGridHeight = 10;
    public GameObject ExampleTilesHolder;
    private SharedData.TileType[,] ExampleTiles;

    Dictionary<string, Dictionary<SharedData.Direction, HashSet<string>>> adjecencyDictionary = new Dictionary<string, Dictionary<SharedData.Direction, HashSet<string>>>();

    string filePath;

    public Dictionary<string, Dictionary<SharedData.Direction, HashSet<string>>> GetAdjacencyDictionary()
    {
        return adjecencyDictionary;
    }

    void Awake()
    {
        ExampleTiles = new SharedData.TileType[ExampleGridWidth, ExampleGridHeight];
        for (int i = 0; i < ExampleGridWidth; i++)
        {
            for (int j = 0; j < ExampleGridHeight; j++)
            {
                ExampleTiles[i, j] = SharedData.TileType.Water;
            }

        }
        filePath = Path.Combine(Application.persistentDataPath, "Resources/temp.txt");
    }

    void Start()
    {

    }

    public void FilloutExampleGrid()
    {
        foreach (Transform child in ExampleTilesHolder.transform)
        {
            ExampleGridObject EGO = child.gameObject.GetComponent<ExampleGridObject>();
            ExampleTiles[EGO.GridX, EGO.GridY] = EGO.thisTileType;
        }


    }

    public void AnalyzeAdjacency()
    {
        for (int y = 0; y < ExampleGridHeight; y++)
        {
            for (int x = 0; x < ExampleGridWidth; x++)
            {
                string AdjacencyTileType = ExampleTiles[x, y].ToString();
                if (!adjecencyDictionary.ContainsKey(AdjacencyTileType))
                {
                    adjecencyDictionary[AdjacencyTileType] = new Dictionary<SharedData.Direction, HashSet<string>>();
                    foreach (SharedData.Direction dir in System.Enum.GetValues(typeof(SharedData.Direction)))
                    {
                        adjecencyDictionary[AdjacencyTileType][dir] = new HashSet<string>();
                    }
                }

                foreach (SharedData.Direction direction in Enum.GetValues(typeof(SharedData.Direction)))
                {
                    Coordinate neighbor = UtilityFunctions.GetNeighbourcoordinate((new Coordinate(x, y)), direction);
                    if (neighbor.X >= 0 && neighbor.X < ExampleGridWidth && neighbor.Y >= 0 && neighbor.Y < ExampleGridHeight)
                    {
                        string adjacentTileType = ExampleTiles[neighbor.X, neighbor.Y].ToString();
                        adjecencyDictionary[AdjacencyTileType][direction].Add(adjacentTileType);
                    }
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
