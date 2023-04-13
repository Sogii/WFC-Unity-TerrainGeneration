using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile
{
    public enum TileType { Ground, Water, Forest, Mountain }
    public TileType type;
    public int rotation;

    public Tile(TileType type, int rotation)
    {
        this.type = type;
        this.rotation = rotation;
    }
}

public class Grid : MonoBehaviour
{

}
