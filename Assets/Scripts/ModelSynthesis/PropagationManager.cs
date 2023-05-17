using System.Collections.Generic;
using System;
using UnityEngine;

public class PropagationManager
{
    private LabelGrid labelGrid;
    private AdjacencyMatrix adjacencyMatrix;

    // Assuming that these are set up somewhere...
    public PropagationManager(LabelGrid labelGrid, AdjacencyMatrix adjacencyMatrix)
    {
        this.labelGrid = labelGrid;
        this.adjacencyMatrix = adjacencyMatrix;
    }

    public void PropagateConstraints()
    {
        // Create a queue to hold all tiles that need to be checked.
        Queue<(int x, int y)> queue = new Queue<(int x, int y)>();

        // Initially, every tile needs to be checked, so add them all to the queue.
        for (int y = 0; y < labelGrid.Height; y++)
        {
            for (int x = 0; x < labelGrid.Width; x++)
            {
                queue.Enqueue((x, y));
            }
        }

        // While there are still tiles to check...
        while (queue.Count > 0)
        {
            // Dequeue a tile to check.
            (int x, int y) = queue.Dequeue();

            Debug.Log($"Queue count: {queue.Count}, LabelGrid at ({x}, {y}): {labelGrid.GetLabelsAt(x, y)}");

            // For each tile type that this tile could be...
            foreach (ModelTile tile in new List<ModelTile>(labelGrid.GetLabelsAt(x, y)))  // We create a copy of the list to avoid modifying it while iterating over it.
            {
                // Assume that this tile is not consistent with its neighbors.
                bool isConsistent = false;

                // For each direction...
                foreach (SharedData.Direction direction in Enum.GetValues(typeof(SharedData.Direction)))
                {
                    // Get the neighbor in that direction.
                    (int nx, int ny) = GetNeighbor(x, y, direction);

                    // If the neighbor is within the grid...
                    if (nx >= 0 && nx < labelGrid.Width && ny >= 0 && ny < labelGrid.Height)
                    {
                        // For each tile type that the neighbor could be...
                        foreach (ModelTile neighborTile in labelGrid.GetLabelsAt(nx, ny))
                        {
                            // If this tile type and the neighbor tile type are allowed to be neighbors in this direction...
                            if (adjacencyMatrix.CheckAdjacency(tile, neighborTile, direction))
                            {
                                // Then this tile type is consistent with its neighbors.
                                isConsistent = true;
                                break;
                            }
                        }
                    }

                    // If we've found that this tile type is consistent, we can stop checking.
                    if (isConsistent) break;
                }

                // If this tile type is not consistent with its neighbors...
                if (!isConsistent)
                {
                    // Remove this tile type from the possibilities for this cell.
                    labelGrid.RemoveLabelAt(x, y, tile);

                    // Add all neighbors to the queue to be checked, since they might be affected by this change.
                    foreach (SharedData.Direction direction in Enum.GetValues(typeof(SharedData.Direction)))
                    {
                        (int nx, int ny) = GetNeighbor(x, y, direction);
                        if (nx >= 0 && nx < labelGrid.Width && ny >= 0 && ny < labelGrid.Height)
                        {
                            queue.Enqueue((nx, ny));
                        }
                    }
                }
            }
        }
    }

    private (int, int) GetNeighbor(int x, int y, SharedData.Direction direction)
    {
        switch (direction)
        {
            case SharedData.Direction.North:
                return (x, y - 1);
            case SharedData.Direction.South:
                return (x, y + 1);
            case SharedData.Direction.East:
                return (x + 1, y);
            case SharedData.Direction.West:
                return (x - 1, y);
            default:
                return (x, y);
        }
    }
}
