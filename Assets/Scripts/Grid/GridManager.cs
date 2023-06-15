using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    [Header("Gridinfo")]
    private int gridWidth;
    private int gridHeight;
    // public SharedData.TerrainType[,] TileGrid;

    [Header("TilePrefabs")]
    public GameObject GroundPrefab;
    public GameObject WaterPrefab;
    public GameObject ForestPrefab;
    public GameObject MountainPrefab;
    public GameObject BrickPrefab;

    [Header("SharedData")]
    public SharedData sharedData;
    private TerrainTypeGrid ModelTerrainTypeGrid;

    public TerrainTypeGrid CreateCatagoryGridFromExampleMesh()
    {
        InnitializeGrid();
        FillGridWithFillerTiles(SharedData.TerrainType.GreeneryTerrain);
        ConvertObjectsToGrid();
        DebugPrintTerrainTypeGrid();
        return ModelTerrainTypeGrid;
        //Send tiles to propagationmanagerscript       
    }

    private void InnitializeGrid()
    {
        gridHeight = sharedData.YGridSize;
        gridWidth = sharedData.XGridSize;
        ModelTerrainTypeGrid = new TerrainTypeGrid(gridWidth, gridHeight);
    }


    private void ConvertObjectsToGrid()
    {
        ObjectToGridConverter objectToGridConverter = this.gameObject.GetComponent<ObjectToGridConverter>();

        objectToGridConverter.IntegrateMeshByName("Buildings", SharedData.TerrainType.BufferTerrain);
        objectToGridConverter.IntegrateMeshByName("Traintracks", SharedData.TerrainType.GreeneryTerrain);
        objectToGridConverter.IntegrateMeshByName("Road", SharedData.TerrainType.GreeneryTerrain);
        objectToGridConverter.IntegrateMeshByName("Urban", SharedData.TerrainType.BufferTerrain);
        objectToGridConverter.IntegrateMeshByName("Water", SharedData.TerrainType.WaterTerrain);
        objectToGridConverter.IntegrateRiverMesh();
    }

    public Vector3 GridToWorldSpace(int x, int y)
    {
        return new Vector3(x * sharedData.TileSize, 0, y * sharedData.TileSize);
    }


    void FillGridWithFillerTiles(SharedData.TerrainType fillerTile)
    {
        for (int x = 0; x < gridWidth; x++)
        {
            for (int y = 0; y < gridHeight; y++)
            {
                ModelTerrainTypeGrid.SetTerrainTypeAt(new Coordinate(x, y), fillerTile);
            }
        }
    }

    public void SetTerrainTypeAt(Coordinate cords, SharedData.TerrainType terrainType)
    {
        ModelTerrainTypeGrid.SetTerrainTypeAt(cords, terrainType);
    }

    public void DebugPrintTerrainTypeGrid()
    {
        Debug.Log("Printing TerrainTypeGrid:");

        string gridString = "";
        for (int y = gridHeight - 1; y >= 0; y--)  // Changed to reverse order to represent top-to-bottom
        {
            string rowString = "";
            for (int x = 0; x < gridWidth; x++)
            {
                // Getting terrain type at given coordinates
                SharedData.TerrainType terrainType = ModelTerrainTypeGrid.GetTerrainTypeAt(new Coordinate(x, y));

                // Convert this terrain type to a character and add it to the row string
                rowString += TerrainTypeToCharacter(terrainType);
            }
            // Append this row to the grid string with a newline
            gridString += rowString + "\n";
        }
        // Print the whole grid
        Debug.Log(gridString);
    }


    private string TerrainTypeToCharacter(SharedData.TerrainType terrainType)
    {
        switch (terrainType)
        {
            case SharedData.TerrainType.GreeneryTerrain: return "G";
            case SharedData.TerrainType.WaterTerrain: return "W";
            case SharedData.TerrainType.BufferTerrain: return "B";
            case SharedData.TerrainType.RiverSide: return "R";
            default: return "?";
        }
    }



    // void InstantiateTiles()
    // {
    //     for (int x = 0; x < GridWidth; x++)
    //     {
    //         for (int y = 0; y < GridHeight; y++)
    //         {
    //             TerrainTile currentTile = TileGrid[x, y];
    //             GameObject tilePrefab = null;

    //             // Choose the appropriate prefab based on the tile type
    //             switch (currentTile.type)
    //             {
    //                 case TerrainTile.TileType.Ground:
    //                     tilePrefab = GroundPrefab;
    //                     break;
    //                 case TerrainTile.TileType.Water:
    //                     tilePrefab = WaterPrefab;
    //                     break;
    //                 case TerrainTile.TileType.Forest:
    //                     tilePrefab = ForestPrefab;
    //                     break;
    //                 case TerrainTile.TileType.Mountain:
    //                     tilePrefab = MountainPrefab;
    //                     break;
    //                 case TerrainTile.TileType.Brick:
    //                     tilePrefab = BrickPrefab;
    //                     break;
    //             }

    //             // Instantiate the prefab and set its position
    //             if (tilePrefab != null)
    //             {
    //                 Vector3 worldPosition = GridToWorldSpace(x, y);
    //                 GameObject instance = Instantiate(tilePrefab, worldPosition, tilePrefab.transform.rotation);
    //                 instance.transform.parent = transform;
    //             }
    //         }
    //     }
    // }
}



