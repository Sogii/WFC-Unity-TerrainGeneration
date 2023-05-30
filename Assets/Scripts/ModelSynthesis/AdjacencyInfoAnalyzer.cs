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
    private SharedData.TileType[,] ExampleGrid;

    Dictionary<string, Dictionary<SharedData.Direction, HashSet<string>>> adjecencyDictionary = new Dictionary<string, Dictionary<SharedData.Direction, HashSet<string>>>();

    string filePath;

    public Dictionary<string, Dictionary<SharedData.Direction, HashSet<string>>> GetAdjacencyDictionary()
    {
        return adjecencyDictionary;
    }

    void Awake()
    {
        filePath = Path.Combine(Application.persistentDataPath, "Resources/temp.txt");
    }

    public void InitiateExampleGrid()
    {
        AnalyzeExampleGridSize();
        ExampleGrid = new SharedData.TileType[ExampleGridWidth, ExampleGridHeight];
        for (int i = 0; i < ExampleGridWidth; i++)
        {
            for (int j = 0; j < ExampleGridHeight; j++)
            {
                ExampleGrid[i, j] = SharedData.TileType.Water;
            }
        }
    }

    private void AnalyzeExampleGridSize()
    {
        foreach (Transform child in ExampleTilesHolder.transform)
        {
            ExampleGridObject EGO = child.gameObject.GetComponent<ExampleGridObject>();
            if (EGO.GridX > ExampleGridWidth)
            {
                ExampleGridWidth = EGO.GridX;
            }
            if (EGO.GridY > ExampleGridHeight)
            {
                ExampleGridHeight = EGO.GridY;
            }
        }
        Debug.Log("ExampleGridWidth: " + ExampleGridWidth + " ExampleGridHeight: " + ExampleGridHeight);
    }
    public void AssignTilesToExampleGrid()
    {
        foreach (Transform child in ExampleTilesHolder.transform)
        {
            ExampleGridObject EGO = child.gameObject.GetComponent<ExampleGridObject>();
            ExampleGrid[EGO.GridX, EGO.GridY] = EGO.thisTileType;
        }
    }

    public void AnalyzeAdjacency()
    {
        for (int y = 0; y < ExampleGridHeight; y++)
        {
            for (int x = 0; x < ExampleGridWidth; x++)
            {
                string AdjacencyTileType = ExampleGrid[x, y].ToString();
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
                    Coordinate neighbor = UtilityFunctions.GetNeighbourCoordinate((new Coordinate(x, y)), direction);
                    if (neighbor.X >= 0 && neighbor.X < ExampleGridWidth && neighbor.Y >= 0 && neighbor.Y < ExampleGridHeight)
                    {
                        string adjacentTileType = ExampleGrid[neighbor.X, neighbor.Y].ToString();
                        adjecencyDictionary[AdjacencyTileType][direction].Add(adjacentTileType);
                    }
                }
            }
        }
        WriteToFile(adjecencyDictionary);
    }


    // Given an index in the 1D array and a direction, returns the index of the tile in that direction

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
