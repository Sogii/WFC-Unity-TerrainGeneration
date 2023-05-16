using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "SharedData", menuName = "ScriptableObjects/SharedData", order = 1)]
public class SharedData : ScriptableObject
{
    public int TileSize = 1;
    public ModelTile[] TileTypes;
    public enum Direction { North, East, South, West }
    [SerializeField] public enum TileType {Water, Grass, Fencehorizontal, Streetcorner, Streethorizontal, Streetvertical};
}