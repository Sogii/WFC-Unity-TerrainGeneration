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
            tile.Neighbourweights = new Dictionary<ModelTile, float>();

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
                        weight = 16;
                    }
                }
                else if (tile.tileType == SharedData.TileType.Grass)
                {
                    if (neighbourTile.tileType == SharedData.TileType.Grass)
                    {
                        weight = 2f;
                    }
                    else if (neighbourTile.tileType == SharedData.TileType.Streetvertical | neighbourTile.tileType == SharedData.TileType.Streethorizontal)
                    {
                        weight = 3f;
                    }
                    else if (neighbourTile.tileType == SharedData.TileType.Water)
                    {
                        weight = 0.1f;
                    }
                }
                else if (roadTiles.Contains(tile.tileType))
                {
                    if (roadTiles.Contains(neighbourTile.tileType))
                    {
                        if (neighbourTile.tileType == SharedData.TileType.Streethorizontal | neighbourTile.tileType == SharedData.TileType.Streetvertical)
                        {
                            weight = 10f;
                        }
                        else
                        {
                            weight = 0.1f;
                        }
                    }
                    else if (tile.tileType == SharedData.TileType.Grass)
                    {
                        weight = 0.1f;
                    }
                }


                tile.Neighbourweights.Add(neighbourTile, weight);
            }
        }
    }

    public void DebugPrintNeighbourWeights(ModelTile[] modelTiles)
    {
        foreach (ModelTile tile in modelTiles)
        {
            Debug.Log($"Tile {tile.tileType} neighbour weights:");
            foreach (KeyValuePair<ModelTile, float> entry in tile.Neighbourweights)
            {
                Debug.Log($"Neighbour {entry.Key.tileType}, weight: {entry.Value}");
            }
        }
    }

}
