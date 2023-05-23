using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(menuName = "Assign Neighbour Weights")]
public class AssignNeighBourWeights : ScriptableObject
{
    List<SharedData.TileType> roadTiles = new List<SharedData.TileType>
        {
        SharedData.TileType.URStreetcorner,
        SharedData.TileType.DRStreetcorner,
        SharedData.TileType.DLStreetcorner,
        SharedData.TileType.LUStreetcorner,
        SharedData.TileType.Streethorizontal,
        SharedData.TileType.Streetvertical
        };
    public void InitializeNeighbourWeights(ModelTile[] modelTiles)
    {
        foreach (ModelTile tile in modelTiles)
        {
            foreach (ModelTile neighbourTile in modelTiles)
            {
                // Assign weights according to your requirements.
                // This example sets the same weight for all neighbour tiles.
                float weight = 1.0f;

                // Modify this section of code to assign different weights to different tiles.
                if (tile.tileType == SharedData.TileType.Water)
                {
                    if (neighbourTile.tileType == SharedData.TileType.Water)
                    {
                        weight = 1f;
                    }
                }
                else if (tile.tileType == SharedData.TileType.Grass)
                {
                    if (neighbourTile.tileType == SharedData.TileType.Grass)
                    {
                        weight = 1f;
                    }
                }
                else if (roadTiles.Contains(tile.tileType))
                {
                    if (roadTiles.Contains(neighbourTile.tileType))
                    {
                        weight = 3f;
                    }
                }

                tile.Neighbourweights = new Dictionary<ModelTile, float>();
                tile.Neighbourweights.Add(neighbourTile, weight);
            }
        }
    }
}
