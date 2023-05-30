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
            //Prevents propagation of labels if there is only one label in the cell
            if (labelGrid.GetLabelsAt(propagationCords).Count > 1)
            {
                //Loops trough each label in the cell
                foreach (ModelTile tile in labels)
                {
                    //Checks if the label is consistent with its neighbours & adds it to the tilestoremove list if it is not
                    List<ModelTile> tilesToRemove = FindInconsistentTiles(propagationCords, tile);

                    //removes all wrong labels in one go & enqueues the neighbours of the cell
                    foreach (ModelTile tileToRemove in tilesToRemove)
                    {
                        labelGrid.RemoveLabelAt(propagationCords, tileToRemove);
                        EnqueueNeighbourCoordinates(propagationCords);
                        if (labelGrid.GetLabelsAt(propagationCords).Count == 1)
                        {
                            OutputMesh outputMesh = ModelSynthesis2DManager.Instance.OutputMesh;
                            outputMesh.SpawnCollapsedLabel(propagationCords, labelGrid.GetLabelsAt(propagationCords)[0]);
                        }
                    }
                }
            }
        }
        processed.Clear();
        return true;
    }

    /// <summary>
    /// Collapses a cell in the grid to a single label by 
    /// </summary>
    public void CollapseGridCell(Coordinate cord)
    {
      //  labelGrid.PrintGridLabels();
        List<ModelTile> possibleLabels = labelGrid.GetLabelsAt(cord);
        List<int> weightedIndices = new List<int>();

        //Loop trough each possible label at the cell
        for (int i = 0; i < possibleLabels.Count; i++)
        {
            //Grabs tile and baseweight of the tile (baseweight can be adjusted in the shared data script)
            ModelTile tile = possibleLabels[i];
            float baseWeight = tile.Weight;

            //Loops trough each neighbour of the tile and adjusts the weight based on the neighbourweights
            foreach (SharedData.Direction direction in Enum.GetValues(typeof(SharedData.Direction)))
            {
                //Grabs the neighbour of the tile
                Coordinate neighbourCord = UtilityFunctions.GetNeighbourCoordinate(cord, direction);
                if (UtilityFunctions.IsWithinGridBounds(neighbourCord, labelGrid))
                {
                    foreach (ModelTile neighbourTile in labelGrid.GetLabelsAt(neighbourCord))
                    {
                        float weight = baseWeight;

                        //If the neighbourweight dictionary contains the neighbour, multiply the weight by the neighbourweight
                        if (tile.Neighbourweights.ContainsKey(neighbourTile))
                        {
                   //         Debug.Log($"Weight of {tile.tileType} is {weight} and neighbourweight of {neighbourTile.tileType} is {tile.Neighbourweights[neighbourTile]}.");
                            weight *= tile.Neighbourweights[neighbourTile];


                        }

                        int totalWeight = Mathf.RoundToInt(weight);
                   //     Debug.Log($"Total weight is {totalWeight}.");

                        //Adds the index of the tile to the weightedindices list an equal amount of times as the totalWeight of the tile. 
                        for (int j = 0; j < totalWeight; j++)
                        {
                            weightedIndices.Add(i);
                        }
                    }
                }
            }
        }
     //   Debug.Log($"Weighted indices are {string.Join(", ", weightedIndices)}.");
        int chosenIndex = weightedIndices[UnityEngine.Random.Range(0, weightedIndices.Count)];
      //  Debug.Log($"Chosen index is {chosenIndex}.");
        ModelTile chosenLabel = possibleLabels[chosenIndex];

        labelGrid.SetLabelsAt(cord, new List<ModelTile> { chosenLabel });
        OutputMesh outputMesh = ModelSynthesis2DManager.Instance.OutputMesh;
        outputMesh.SpawnCollapsedLabel(cord, chosenLabel);
     //   Debug.Log($"Collapsed ({cord.X}, {cord.Y}) to {chosenLabel.tileType}.");

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
    /// Returns a list of neighbour tiles that are inconsistent with the given tile.
    /// </summary>

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
