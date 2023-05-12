using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "SharedData", menuName = "ScriptableObjects/SharedData", order = 1)]
public class SharedData : ScriptableObject
{
    public int TileSize = 1;
    public List<ModelTile> AllModelTiles = new List<ModelTile>();
     public enum Direction { North, East, South, West }
}