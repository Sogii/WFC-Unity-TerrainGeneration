using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;
using System;
using System.Linq;

public class LabelGrid
{
    public int Width { get; private set; }
    public int Height { get; private set; }
    public List<ModelTile>[,] Grid { get; private set; }
    private AdjacencyMatrix adjacencyMatrix;
    public SharedData sharedData;

    public LabelGrid(int width, int height, AdjacencyMatrix adjacencyMatrix, SharedData sharedData)
    {
        this.sharedData = sharedData;
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


    /// <summary>
    /// Assigns all possible labels to the grid, used to initialize the grid before the propagation process
    /// </summary>
    public void AssignAllPossibleLabels(List<ModelTile> allModelTiles)
    {
        int randomX = UnityEngine.Random.Range(0, Width);
        int randomY = UnityEngine.Random.Range(0, Height);

        int randomX2 = UnityEngine.Random.Range(0, Width);
        int randomY2 = UnityEngine.Random.Range(0, Height);
        for (int y = 0; y < Height; y++)
        {
            for (int x = 0; x < Width; x++)
            {
                if (x == randomX && y == randomY)
                {
                    int randomIndex1 = UnityEngine.Random.Range(1, allModelTiles.Count);
                    int randomIndex2 = UnityEngine.Random.Range(1, allModelTiles.Count);
                    while (randomIndex2 == randomIndex1)
                    {
                        randomIndex2 = UnityEngine.Random.Range(0, allModelTiles.Count);
                    }

                    Grid[x, y].Add(allModelTiles[randomIndex1]);
                    Grid[x, y].Add(allModelTiles[randomIndex2]);
                }
                else if (x == randomX2 && y == randomY2)
                {
                    int randomIndex1 = UnityEngine.Random.Range(1, allModelTiles.Count);
                    int randomIndex2 = UnityEngine.Random.Range(1, allModelTiles.Count);
                    while (randomIndex2 == randomIndex1)
                    {
                        randomIndex2 = UnityEngine.Random.Range(0, allModelTiles.Count);
                    }

                    Grid[x, y].Add(allModelTiles[randomIndex1]);
                    Grid[x, y].Add(allModelTiles[randomIndex2]);
                }
                // Otherwise, assign all possible labels
                else
                {
                    foreach (ModelTile modelTile in allModelTiles)
                    {
                        Grid[x, y].Add(modelTile);
                        Debug.Log("Added modelTile + " + modelTile.tileType.ToString() + " to grid at coordinate " + x + "," + y);
                    }
                }
            }
        }
    }

    public void AssignLabelsBasedOnTerrainTypeGrid(TerrainTypeGrid terrainTypeGrid)
    {
        // int randomIndex1 = UnityEngine.Random.Range(1, Height);
        // int randomIndex2 = UnityEngine.Random.Range(1, Height);
        // //Loop trough each gridcell
        for (int y = 0; y < Height; y++)
        {
            for (int x = 0; x < Width; x++)
            {

                // if (x == randomIndex1 && y == randomIndex2)
                // {
                //     List<ModelTile> modelTilesToAssign = new List<ModelTile>();
                //     modelTilesToAssign.Add(sharedData.ModelTiles[0]);
                //     AssignLabelsFromTerrainGroupAtCoordinate(new Coordinate(x, y),modelTilesToAssign);
                // }
                // else
                // {
                Coordinate cord = new Coordinate(x, y);
                SharedData.TerrainType terrainType = terrainTypeGrid.GetTerrainTypeAt(cord);
                switch (terrainType)
                {
                    case SharedData.TerrainType.GreeneryTerrain:
                        //0 = Nature
                        AssignLabelsFromTerrainGroupAtCoordinate(cord, sharedData.GetModelTilesListByTerrainType(SharedData.TerrainType.GreeneryTerrain));

                        break;

                    case SharedData.TerrainType.WaterTerrain:
                        //1 = Filler
                        AssignLabelsFromTerrainGroupAtCoordinate(cord, sharedData.GetModelTilesListByTerrainType(SharedData.TerrainType.WaterTerrain));
                        break;

                    case SharedData.TerrainType.RiverSide:
                        //1 = Filler
                        AssignLabelsFromTerrainGroupAtCoordinate(cord, sharedData.GetModelTilesListByTerrainType(SharedData.TerrainType.RiverSide));
                        break;

                    case SharedData.TerrainType.BufferTerrain:
                        //1 = Filler
                        AssignLabelsFromTerrainGroupAtCoordinate(cord, sharedData.GetModelTilesListByTerrainType(SharedData.TerrainType.BufferTerrain));
                        break;

                    default:
                        throw new ArgumentException("Invalid terrain type");
                }
                // }

            }
        }
    }

    public void AssignLabelsFromTerrainGroupAtCoordinate(Coordinate cord, List<ModelTile> modelTilesToAsign)
    {
        foreach (ModelTile modelTile in modelTilesToAsign)
        {
            Grid[cord.X, cord.Y].Add(modelTile);
        }
    }

    /// <summary>
    /// Returns a ModelTile List with all the labels at the input coordinate. 
    /// Returns an empty list if the coordinate is outside of the grid boundaries.
    /// </summary> 
    public List<ModelTile> GetLabelsAt(Coordinate cord)
    {
        if (UtilityFunctions.IsWithinGridBounds(cord, this))
        {
            return Grid[cord.X, cord.Y];
        }
        else
        {
            Debug.LogError($"Invalid coordinates: ({cord.X}, {cord.Y}). Grid dimensions: {Width}x{Height}.");
            return new List<ModelTile>(); // Returning an empty list to indicate no valid tiles
        }
    }

    /// <summary>
    ///Replaces the list of labels in the LabelGrid at the given coordinates with the given List of labels
    /// </summary>
    public void SetLabelsAt(Coordinate cords, List<ModelTile> labels)
    {
        if (labels == null || labels.Count == 0)
        {
            throw new ArgumentException("labels must not be null or empty");
        }

        if (UtilityFunctions.IsWithinGridBounds(cords, this))
        {
            //  Debug.Log($"Setting labels at ({cords.X},{cords.Y}) to {labels[0].tileType}.");
            Grid[cords.X, cords.Y] = labels;
        }

        else
        {
            throw new ArgumentOutOfRangeException("Coordinates are outside of the grid boundaries");
        }
    }

    /// <summary>
    ///Prints out comprehensive list of all labels to the debug window
    /// </summary>
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
                    string tileTypeChar = TileTypeToCharacter(modelTile);
                    builder.Append($"{tileTypeChar}, ");
                }
                builder.AppendLine();
            }
        }

        Debug.Log(builder.ToString());
    }


    /// <summary>
    ///Removes the given label at the given coordinates
    /// </summary>
    public void RemoveLabelAt(Coordinate cords, ModelTile modelTileToRemove)
    {
        if (UtilityFunctions.IsWithinGridBounds(cords, this))
        {
            if (Grid[cords.X, cords.Y].Count == 1)
            {
                Debug.LogWarning($"Removing the last label at ({cords.X},{cords.Y}).");
                SetLabelsAt(cords, new List<ModelTile>{sharedData.AllModelTiles[7]});
            }
            else
            {
                // Check if the list at the specified coordinates contains the ModelTile to remove
                if (Grid[cords.X, cords.Y].Contains(modelTileToRemove))
                {
                    // If it does, remove it
                    Grid[cords.X, cords.Y].Remove(modelTileToRemove);
                }
                else
                {
                    Debug.LogWarning($"ModelTile to remove not found at ({cords.X},{cords.Y}).");
                }
            }
        }
        else
        {
            Debug.LogError($"Invalid coordinates: ({cords.X}, {cords.Y}). Grid dimensions: {Width}x{Height}.");
        }
       // PrintGridLabels();
    }

    private string TileTypeToCharacter(ModelTile modelTile)
    {
        switch (modelTile.tileType)
        {
            case SharedData.TileType.Water: return "W";
            case SharedData.TileType.PathUR: return "UR";
            case SharedData.TileType.PathRD: return "DR";
            case SharedData.TileType.PathDL: return "DL";
            case SharedData.TileType.PathUL: return "UL";
            case SharedData.TileType.PathRL: return "RL";
            case SharedData.TileType.PathUD: return "UD";
            case SharedData.TileType.Grass: return "G";
            case SharedData.TileType.FillerTile: return "Fill";
            case SharedData.TileType.PathURD: return "URD";
            case SharedData.TileType.PathRDL: return "RDL";
            case SharedData.TileType.PathURL: return "URL";
            case SharedData.TileType.PathUDL: return "UDL";
            case SharedData.TileType.PathX: return "X";
            case SharedData.TileType.PathL: return "L";
            case SharedData.TileType.PathU: return "U";
            case SharedData.TileType.PathR: return "R";
            case SharedData.TileType.PathD: return "D";


            default: return "?";
        }
    }

}
