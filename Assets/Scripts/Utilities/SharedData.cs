using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "SharedData", menuName = "ScriptableObjects/SharedData", order = 1)]
public class SharedData : ScriptableObject
{
   
   public int XGridSize;
   public int YGridSize;
    public int TileSize = 1;

    [System.Serializable]
    public struct TerrainTileSet
    {
        public TerrainType TerrainType;
        public TileType[] TerrainTileTypes;
    }

    [SerializeField] public TerrainTileSet[] TerrainTileSets;

    public ModelTile[] AllModelTiles;

    public enum Direction
    {
        North, NorthEast, East, SouthEast, South, SouthWest, West, NorthWest
    }

    public enum NonDiagonalDirection
    {
        North, East, South, West
    }

    public enum RoadTiles
    {
        PathUR = 1,
        PathRD = 2,
        PathDL = 3,
        PathUL = 4,
        PathRL = 5,
        PathUD = 6,
        PathX = 9,
        PathRDL = 10,
        PathUDL = 11,
        PathURD = 12,
        PathURL = 13,
        PathU = 14,
        PathR = 15,
        PathD = 16,
        PathL = 17,
    }

    [SerializeField]
    public enum TileType
    {
        Water = 0,
        PathUR = 1,
        PathRD = 2,
        PathDL = 3,
        PathUL = 4,
        PathRL = 5,
        PathUD = 6,
        Grass = 7,
        FillerTile = 8,
        PathX = 9,
        PathRDL = 10,
        PathUDL = 11,
        PathURD = 12,
        PathURL = 13,
        PathU = 14,
        PathR = 15,
        PathD = 16,
        PathL = 17,
    };



    public enum TerrainType
    {
        WaterTerrain, GreeneryTerrain, RiverSide, BufferTerrain
    };

    public ModelTile GetModelTileByTileType(TileType tileType)
    {
        return AllModelTiles[(int)tileType];
    }

    public List<ModelTile> GetModelTilesListByTerrainType(TerrainType terrainType)
    {
        List<ModelTile> modelTiles = new List<ModelTile>();
        foreach (TerrainTileSet terrainTileSet in TerrainTileSets)
        {
            if (terrainTileSet.TerrainType == terrainType)
            {
                foreach (TileType tileType in terrainTileSet.TerrainTileTypes)
                {
                    modelTiles.Add(GetModelTileByTileType(tileType));
                }
            }
        }
        return modelTiles;
    }

    public ModelTile[] GetModelTilesArrayByTerrainType(TerrainType terrainType)
    {
        return GetModelTilesListByTerrainType(terrainType).ToArray();
    }
}
