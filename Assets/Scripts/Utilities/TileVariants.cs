using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "TileVariants", menuName = "ScriptableObjects/TileVariants", order = 3)]

public class TileVariants : ScriptableObject
{
    public GameObject[] GrassTilesVariants;
    public PathTiles[] PathTilesVariants;
    public enum PathTerrainVariant
    {
        GreenPath,
        RockPath
    }

    public enum PathSizeVariant
    {
        Small,
        Medium,
        Wide
    }

    [System.Serializable]
    public struct PathTiles
    {
        public PathTerrainVariant pathTerrainVariant;
        public PathSizeVariants[] pathSizeVariants;
    }
    [System.Serializable]
    public struct PathSizeVariants
    {
        public PathSizeVariant pathSizeVariant;
        public PathTerrainVariants[] pathTerrainVariants;
    }
     [System.Serializable]
    public struct PathTerrainVariants
    {
        public SharedData.TileType tileType;
        public GameObject pathTerrainVariant;
    }
}


