using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "SharedData", menuName = "ScriptableObjects/SharedData", order = 1)]
public class SharedData : ScriptableObject
{
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

    [SerializeField]
    public enum TileType
    {
        Water = 0,
        URStreetcorner = 1,
        DRStreetcorner = 2,
        DLStreetcorner = 3,
        LUStreetcorner = 4,
        Streethorizontal = 5,
        Streetvertical = 6,
        Grass = 7,
        FillerTile = 8
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
