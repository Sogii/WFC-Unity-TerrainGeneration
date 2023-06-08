using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "SharedData", menuName = "ScriptableObjects/SharedData", order = 1)]
public class SharedData : ScriptableObject
{
    public int TileSize = 1;
    [System.Serializable] public struct TerrainTileSet
    {
        public TerrainType TerrainType;
        public ModelTile[] TerrainModelTiles;
    }
    [SerializeField] public TerrainTileSet[] TerrainTileSets;
    public ModelTile[] ModelTiles;
    public enum Direction { North, NorthEast, East, SouthEast, South, SouthWest, West, NorthWest }
    [SerializeField] public enum TileType { Water, URStreetcorner, DRStreetcorner, DLStreetcorner, LUStreetcorner, Streethorizontal, Streetvertical, Grass, FillerTile };
    public enum TerrainType {WaterTerrain, GreeneryTerrain, RiverSide, BufferTerrain};
}

