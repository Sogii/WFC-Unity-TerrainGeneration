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

    public void AddCustomAdjacencyData()
    {
        AddFullyAdjacentTileToLibrary(SharedData.AllModelTiles[8]);


        // PathUR
        AddTileByIndexes(SharedData.AllModelTiles[1],
            new List<SharedData.Direction> { SharedData.Direction.North },
            new List<int> { 2, 3, 6, 10, 11, 12, 16 });
        AddTileByIndexes(SharedData.AllModelTiles[1],
            new List<SharedData.Direction> { SharedData.Direction.East },
            new List<int> { 3, 4, 5, 10, 11, 13, 17 });

        // PathRD
        AddTileByIndexes(SharedData.AllModelTiles[2],
            new List<SharedData.Direction> { SharedData.Direction.East },
            new List<int> { 3, 4, 5, 11, 13, 14, 17 });
        AddTileByIndexes(SharedData.AllModelTiles[2],
            new List<SharedData.Direction> { SharedData.Direction.South },
            new List<int> { 1, 4, 6, 9, 11, 12, 13 });

        // PathDL
        AddTileByIndexes(SharedData.AllModelTiles[3],
            new List<SharedData.Direction> { SharedData.Direction.South },
            new List<int> { 1, 4, 6, 9, 11, 12, 13 });
        AddTileByIndexes(SharedData.AllModelTiles[3],
            new List<SharedData.Direction> { SharedData.Direction.West },
            new List<int> { 1, 2, 3, 10, 12, 14, 16 });

        // PathUL
        AddTileByIndexes(SharedData.AllModelTiles[4],
            new List<SharedData.Direction> { SharedData.Direction.North },
            new List<int> { 2, 3, 6, 10, 11, 12, 16 });
        AddTileByIndexes(SharedData.AllModelTiles[4],
            new List<SharedData.Direction> { SharedData.Direction.West },
            new List<int> { 1, 2, 3, 10, 12, 14, 16 });

        // PathRL
        AddTileByIndexes(SharedData.AllModelTiles[5],
            new List<SharedData.Direction> { SharedData.Direction.East },
            new List<int> { 3, 4, 5, 10, 11, 13, 17 });
        AddTileByIndexes(SharedData.AllModelTiles[5],
            new List<SharedData.Direction> { SharedData.Direction.West },
            new List<int> { 1, 2, 3, 10, 12, 14, 16 });

        // PathUD
        AddTileByIndexes(SharedData.AllModelTiles[6],
            new List<SharedData.Direction> { SharedData.Direction.North },
            new List<int> { 2, 3, 6, 10, 11, 12, 16 });
        AddTileByIndexes(SharedData.AllModelTiles[6],
            new List<SharedData.Direction> { SharedData.Direction.South },
            new List<int> { 1, 4, 6, 9, 11, 12, 13 });

        // PathRDL
        AddTileByIndexes(SharedData.AllModelTiles[10],
            new List<SharedData.Direction> { SharedData.Direction.South },
            new List<int> { 1, 4, 6, 9, 11, 12, 13 });
        AddTileByIndexes(SharedData.AllModelTiles[10],
            new List<SharedData.Direction> { SharedData.Direction.West },
            new List<int> { 1, 2, 3, 10, 12, 14, 16 });
        AddTileByIndexes(SharedData.AllModelTiles[10],
            new List<SharedData.Direction> { SharedData.Direction.East },
            new List<int> { 3, 4, 5, 11, 13, 14, 17 });

        // PathUDL
        AddTileByIndexes(SharedData.AllModelTiles[11],
            new List<SharedData.Direction> { SharedData.Direction.North },
            new List<int> { 2, 3, 6, 10, 11, 12, 16 });
        AddTileByIndexes(SharedData.AllModelTiles[11],
            new List<SharedData.Direction> { SharedData.Direction.South },
            new List<int> { 1, 4, 6, 9, 11, 12, 13 });
        AddTileByIndexes(SharedData.AllModelTiles[11],
            new List<SharedData.Direction> { SharedData.Direction.West },
            new List<int> { 1, 2, 3, 10, 12, 14, 16 });

        // PathURD
        AddTileByIndexes(SharedData.AllModelTiles[12],
            new List<SharedData.Direction> { SharedData.Direction.North },
            new List<int> { 2, 3, 6, 10, 11, 12, 16 });
        AddTileByIndexes(SharedData.AllModelTiles[12],
            new List<SharedData.Direction> { SharedData.Direction.East },
            new List<int> { 3, 4, 5, 11, 13, 14, 17 });
        AddTileByIndexes(SharedData.AllModelTiles[12],
            new List<SharedData.Direction> { SharedData.Direction.South },
            new List<int> { 1, 4, 6, 9, 11, 12, 13 });

        // PathURL
        AddTileByIndexes(SharedData.AllModelTiles[13],
            new List<SharedData.Direction> { SharedData.Direction.North },
            new List<int> { 2, 3, 6, 10, 11, 12, 16 });
        AddTileByIndexes(SharedData.AllModelTiles[13],
            new List<SharedData.Direction> { SharedData.Direction.East },
            new List<int> { 3, 4, 5, 11, 13, 14, 17 });
        AddTileByIndexes(SharedData.AllModelTiles[13],
            new List<SharedData.Direction> { SharedData.Direction.West },
            new List<int> { 1, 2, 3, 10, 12, 14, 16 });

        // PathU
        AddTileByIndexes(SharedData.AllModelTiles[14],
            new List<SharedData.Direction> { SharedData.Direction.North },
            new List<int> { 2, 3, 6, 10, 11, 12, 16 });

        // PathR
        AddTileByIndexes(SharedData.AllModelTiles[15],
            new List<SharedData.Direction> { SharedData.Direction.East },
            new List<int> { 3, 4, 5, 10, 11, 13, 17 });

        // PathD
        AddTileByIndexes(SharedData.AllModelTiles[16],
            new List<SharedData.Direction> { SharedData.Direction.South },
            new List<int> { 1, 4, 6, 9, 11, 12, 13 });

        // PathL
        AddTileByIndexes(SharedData.AllModelTiles[17],
            new List<SharedData.Direction> { SharedData.Direction.West },
            new List<int> { 1, 2, 3, 10, 12, 14, 16 });

        var pathIndexes = new List<int> { 1, 2, 3, 4, 5, 6, 10, 11, 12, 13, 14, 15, 16, 17 };
        var diagonalDirections = new List<SharedData.Direction> { SharedData.Direction.NorthEast, SharedData.Direction.NorthWest, SharedData.Direction.SouthEast, SharedData.Direction.SouthWest };


        //Add paths to grass
        AddTileByIndexes(SharedData.AllModelTiles[7],
            new List<SharedData.Direction> { SharedData.Direction.North },
            new List<int> { 1, 4, 5, 13, 14, 15, 17 });

        AddTileByIndexes(SharedData.AllModelTiles[7],
        new List<SharedData.Direction> { SharedData.Direction.East },
        //all tiles without L in the name
        new List<int> { 1, 2, 6, 12, 14, 15, 16 });

        AddTileByIndexes(SharedData.AllModelTiles[7],
         new List<SharedData.Direction> { SharedData.Direction.South },
         //all tiles without U in the name
         new List<int> { 2, 3, 5, 10, 15, 16, 17 });

        AddTileByIndexes(SharedData.AllModelTiles[7],
         new List<SharedData.Direction> { SharedData.Direction.West },
         //all tiles without R in the name
         new List<int> { 3, 4, 6, 11, 14, 16, 17 });


        AddTileByIndexes(SharedData.AllModelTiles[7],
           new List<SharedData.Direction> { SharedData.Direction.NorthEast },
           //all roadtiles
           new List<int> { 1, 2, 3, 4, 5, 6, 9, 10, 11, 12, 13, 14, 15, 16 });

        AddTileByIndexes(SharedData.AllModelTiles[7],
      new List<SharedData.Direction> { SharedData.Direction.SouthEast },
      //all roadtiles
      new List<int> { 1, 2, 3, 4, 5, 6, 9, 10, 11, 12, 13, 14, 15, 16 });

        AddTileByIndexes(SharedData.AllModelTiles[7],
           new List<SharedData.Direction> { SharedData.Direction.SouthWest },
           //all roadtiles
           new List<int> { 1, 2, 3, 4, 5, 6, 9, 10, 11, 12, 13, 14, 15, 16 });

        AddTileByIndexes(SharedData.AllModelTiles[7],
                 new List<SharedData.Direction> { SharedData.Direction.NorthWest },
                 //all roadtiles
                 new List<int> { 1, 2, 3, 4, 5, 6, 9, 10, 11, 12, 13, 14, 15, 16 });

        foreach (var pathIndex in pathIndexes)
        {
            foreach (var direction in diagonalDirections)
            {
                AddTileByIndexes(SharedData.AllModelTiles[pathIndex],
                    new List<SharedData.Direction> { direction },
                    new List<int> { 7 });
            }
        }

        foreach (var keyValuePair in adjecencyDictionary)
        {
            string key = keyValuePair.Key;
            // Check if the key starts with "Path"
            if (key.StartsWith("Path"))
            {
                Dictionary<SharedData.Direction, HashSet<string>> value = keyValuePair.Value;

                foreach (SharedData.Direction direction in Enum.GetValues(typeof(SharedData.Direction)))
                {
                    // If the direction is not already in the value dictionary or it is there but empty, add "Grass"
                    if (!value.ContainsKey(direction) || value[direction].Count == 0)
                    {
                        if (value.ContainsKey(direction))
                        {
                            value[direction].Add("Grass");
                        }
                        else
                        {
                            value[direction] = new HashSet<string> { "Grass" };
                        }
                    }
                }
            }
        }
        WriteToFile(adjecencyDictionary);
    }



    bool IsTileCompatibleInDirection(ModelTile pathTile, SharedData.Direction direction)
    {
        string pathTileName = pathTile.tileType.ToString();
        switch (direction)
        {
            case SharedData.Direction.North:
                // Check if the tile has a "D" in its name
                if (pathTileName.Contains("D"))
                    return false;
                break;

            case SharedData.Direction.East:
                // Check if the tile has a "L" in its name
                if (pathTileName.Contains("L"))
                    return false;
                break;

            case SharedData.Direction.South:
                // Check if the tile has a "U" in its name
                if (pathTileName.Contains("U"))
                    return false;
                break;

            case SharedData.Direction.West:
                // Check if the tile has a "R" in its name
                if (pathTileName.Contains("R"))
                    return false;
                break;
        }

        return true;
    }

    public void AddFullyAdjacentTileToLibrary(ModelTile tileToAdd)
    {
        // Get the name of the tile to add
        string tileToAddName = tileToAdd.gameObject.name;

        // If this tile doesn't exist in our adjacency dictionary, add it
        if (!adjecencyDictionary.ContainsKey(tileToAddName))
        {
            adjecencyDictionary[tileToAddName] = new Dictionary<SharedData.Direction, HashSet<string>>();
            foreach (SharedData.Direction dir in System.Enum.GetValues(typeof(SharedData.Direction)))
            {
                adjecencyDictionary[tileToAddName][dir] = new HashSet<string>();
            }
        }

        // Now, let's make sure that every tile can be adjacent to this tile
        foreach (var outerEntry in adjecencyDictionary)
        {
            // Get the name of the current tile
            string currentTileName = outerEntry.Key;

            // Add the current tile as an adjacent tile to the tileToAdd
            foreach (SharedData.Direction dir in System.Enum.GetValues(typeof(SharedData.Direction)))
            {
                adjecencyDictionary[tileToAddName][dir].Add(currentTileName);
            }

            // Also add the tileToAdd as an adjacent tile to the current tile
            foreach (SharedData.Direction dir in System.Enum.GetValues(typeof(SharedData.Direction)))
            {
                adjecencyDictionary[currentTileName][dir].Add(tileToAddName);
            }
        }




        WriteToFile(adjecencyDictionary);
    }


    /// <summary>
    /// Adds a tile to the adjacency dictionary in the specified directions and with the specified compatible tiles
    /// </summary>
    /// <param name="tileToAdd">The tile to add to the adjacency dictionary</param>
    /// <param name="CompatibleDirections">The directions in which the tile can be adjacent to other tiles</param>
    /// <param name="CompatibleTiles">The tiles that can be adjacent to the tileToAdd</param>
    public void AddCustomTileToLibrary(ModelTile tileToAdd, List<SharedData.Direction> CompatibleDirections, List<ModelTile> CompatibleTiles)
    {
        string tileToAddName = tileToAdd.gameObject.name;
        // If this tile doesn't exist in our adjacency dictionary, add it
        if (!adjecencyDictionary.ContainsKey(tileToAddName))
        {
            adjecencyDictionary[tileToAddName] = new Dictionary<SharedData.Direction, HashSet<string>>();
            foreach (SharedData.Direction dir in System.Enum.GetValues(typeof(SharedData.Direction)))
            {
                adjecencyDictionary[tileToAddName][dir] = new HashSet<string>();
            }
        }

        foreach (SharedData.Direction dir in CompatibleDirections)
        {
            foreach (ModelTile compatibleTile in CompatibleTiles)
            {
                adjecencyDictionary[tileToAddName][dir].Add(compatibleTile.gameObject.name);
            }
        }
    }
    public void AddTileByIndexes(ModelTile tileToAdd, List<SharedData.Direction> CompatibleDirections, List<int> CompatibleTileIndexes)
    {
        List<ModelTile> CompatibleTiles = new List<ModelTile>();
        foreach (int index in CompatibleTileIndexes)
        {
            CompatibleTiles.Add(SharedData.AllModelTiles[index]);
        }
        AddCustomTileToLibrary(tileToAdd, CompatibleDirections, CompatibleTiles);
    }

}
