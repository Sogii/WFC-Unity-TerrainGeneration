using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class ModelTile
{
    public GameObject gameObject;
    public SharedData.TileType tileType;
    public float Weight = 1f;
    public Dictionary<ModelTile ,float> Neighbourweights;
}
