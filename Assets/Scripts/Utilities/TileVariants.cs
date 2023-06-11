using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "TileVariants", menuName = "ScriptableObjects/TileVariants", order = 3)]

public class TileVariants : ScriptableObject
{
    public GameObject[] GrassTilesVariants;
    public PathTiles[] PathTilesVariants;


[System.Serializable]
public struct PathTiles
{
    public SharedData.TileType tileType;
    public GameObject[] VariantModels; 
}
}


