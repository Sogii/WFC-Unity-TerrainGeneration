using System;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;


public struct Coordinate
{
    public int X { get; }
    public int Y { get; }

    public Coordinate(int x, int y)
    {
        X = x;
        Y = y;
    }
}

public class PropagationManager
{
    private LabelGrid labelGrid;
    private AdjacencyMatrix adjacencyMatrix;
    private Queue<Coordinate> queue = new Queue<Coordinate>();

    private HashSet<Coordinate> processed = new HashSet<Coordinate>();

    public PropagationManager(LabelGrid labelGrid, AdjacencyMatrix adjacencyMatrix)
    {
        this.labelGrid = labelGrid;
        this.adjacencyMatrix = adjacencyMatrix;
    }




    public bool PropagateGridConstraints()
    {
        while (queue.Count > 0)
        {
            Coordinate propagationCords = queue.Dequeue();
            List<ModelTile> labels = new List<ModelTile>(labelGrid.GetLabelsAt(propagationCords));

            if (labelGrid.GetLabelsAt(propagationCords).Count > 1)
            {
                foreach (ModelTile tile in labels)
                {
                    //Remove non-consistent labels based on neighbours
                    List<ModelTile> tilesToRemove = FindInconsistentTiles(propagationCords, tile);
                    foreach (ModelTile tileToRemove in tilesToRemove)
                    {
                        labelGrid.RemoveLabelAt(propagationCords, tileToRemove);
                        EnqueueNeighbourCoordinates(propagationCords);
                    }
                }
            }
        }
        processed.Clear();
        return true;
    }


    public void CollapseGridCell(Coordinate cord)
    {
        List<ModelTile> possibleLabels = labelGrid.GetLabelsAt(cord);
        List<int> weightedIndices = new List<int>();

        for (int i = 0; i < possibleLabels.Count; i++)
        {
            ModelTile tile = possibleLabels[i];
            float baseWeight = tile.Weight;

            foreach (SharedData.Direction direction in Enum.GetValues(typeof(SharedData.Direction)))
            {
                Coordinate neighbourCord = UtilityFunctions.GetNeighbourCoordinate(cord, direction);
                if (UtilityFunctions.IsWithinGridBounds(neighbourCord, labelGrid))
                {
                    foreach (ModelTile neighbourTile in labelGrid.GetLabelsAt(neighbourCord))
                    {
                        float weight = baseWeight;  // Reset weight for each neighbour tile

                        if (tile.Neighbourweights.ContainsKey(neighbourTile))
                        {
                            weight *= tile.Neighbourweights[neighbourTile];
                            Debug.Log($"Weight for {tile.tileType} and {neighbourTile.tileType} is {weight}.");
                        }

                        int totalWeight = Mathf.RoundToInt(weight);
                        for (int j = 0; j < totalWeight; j++)
                        {
                            weightedIndices.Add(i);
                        }
                    }
                }
            }
        }

        int chosenIndex = weightedIndices[UnityEngine.Random.Range(0, weightedIndices.Count)];
        ModelTile chosenLabel = possibleLabels[chosenIndex];

        labelGrid.SetLabelsAt(cord, new List<ModelTile> { chosenLabel });
        Debug.Log($"Collapsed ({cord.X}, {cord.Y}) to {chosenLabel.tileType}.");

        foreach (SharedData.Direction direction in Enum.GetValues(typeof(SharedData.Direction)))
        {
            Coordinate neighbourCord = UtilityFunctions.GetNeighbourCoordinate(cord, direction);
            if (UtilityFunctions.IsWithinGridBounds(neighbourCord, labelGrid))
            {
                queue.Enqueue((neighbourCord));
            }
        }
    }


    /// <summary>
    /// Checks if a tile is consistent with its neighbors in each direction and removes it if it is not.
    /// </summary>


    //check tile against all neighbours and remove all labels that are not consistent with labels 
    //1. In each direction
    //2. Check for all labels in the tile you want to test consistency for
    //3. Get the neighbours of the tile you want to test consistency for

    public List<ModelTile> FindInconsistentTiles(Coordinate cord, ModelTile tile)
    {
        List<ModelTile> tilesToRemove = new List<ModelTile>();
        foreach (SharedData.Direction direction in Enum.GetValues(typeof(SharedData.Direction)))
        {
            Coordinate neighBourcord = UtilityFunctions.GetNeighbourCoordinate(cord, direction);
            // Continue to next direction if neighbor is out of bounds.
            if (!(UtilityFunctions.IsWithinGridBounds(neighBourcord, labelGrid)))
                continue;

            // Copy the list of labels at neighbor cell.
            List<ModelTile> neighborTiles = new List<ModelTile>(labelGrid.GetLabelsAt(neighBourcord));
            bool isConsistent = false;

            foreach (ModelTile neighborTile in neighborTiles)
            {
                if (adjacencyMatrix.CheckAdjacency(tile, neighborTile, direction))
                {
                    // If the tile is consistent with any neighbor tile, break the loop and keep the tile.
                    isConsistent = true;
                    break;
                }
            }

            // If the tile is not consistent with any of the neighbor's tiles, remove it.
            if (!isConsistent)
            {
                tilesToRemove.Add(tile);
                break;
            }
        }
        return tilesToRemove;
    }

    public void EnqueueNeighbourCoordinates(Coordinate cord)
    {
        foreach (SharedData.Direction direction in Enum.GetValues(typeof(SharedData.Direction)))
        {
            Coordinate neighBourcord = UtilityFunctions.GetNeighbourCoordinate(cord, direction);
            if (UtilityFunctions.IsWithinGridBounds(neighBourcord, labelGrid) && !processed.Contains((neighBourcord)))
            {
                queue.Enqueue((neighBourcord));
                processed.Add((neighBourcord));
            }
        }
    }
}
