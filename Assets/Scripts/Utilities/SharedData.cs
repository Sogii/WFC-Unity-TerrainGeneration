using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "SharedData", menuName = "ScriptableObjects/SharedData", order = 1)]
public class SharedData : ScriptableObject
{
    public int TileSize = 1;
    public ModelTile[] ModelTiles;
    public enum Direction { North, NorthEast, East, SouthEast, South, SouthWest, West, NorthWest }
    [SerializeField] public enum TileType {Water, URStreetcorner, DRStreetcorner, DLStreetcorner, LUStreetcorner, Streethorizontal, Streetvertical, Grass};
}
   
