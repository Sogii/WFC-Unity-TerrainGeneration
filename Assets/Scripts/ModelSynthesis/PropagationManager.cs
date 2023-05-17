using System.Collections.Generic;
using System;
using UnityEngine;
using static SharedData;

public class PropagationManager
{
    private LabelGrid labelGrid;
    private AdjacencyMatrix adjacencyMatrix;
    public SharedData sharedData;

    // Assuming that these are set up somewhere...
    public PropagationManager(LabelGrid labelGrid, AdjacencyMatrix adjacencyMatrix)
    {
        this.labelGrid = labelGrid;
        this.adjacencyMatrix = adjacencyMatrix;
    }

    public void PropagateConstraints()
    {
        // Create a queue to hold all labels that need to be checked.
        Queue<(int x, int y)> queue = new Queue<(int x, int y)>();

        // Initially, every label needs to be checked, so add them all to the queue.
        for (int y = 0; y < labelGrid.Height; y++)
        {
            for (int x = 0; x < labelGrid.Width; x++)
            {
                queue.Enqueue((x, y));
            }
        }

        // While there are still labels to check...
        while (queue.Count > 0)
        {
            // Dequeue a label to check.
            (int x, int y) = queue.Dequeue();

            Debug.Log($"Queue count: {queue.Count}, LabelGrid at ({x}, {y}): {labelGrid.GetLabelAt(x, y)}");

            // For each label that this label could be...
            foreach (int label in labelGrid.Grid[x, y])  // We create a copy of the list to avoid modifying it while iterating over it.
            {
                // Assume that this label is not consistent with its neighbors.
                bool isConsistent = false;

                // For each direction...
                foreach (SharedData.Direction direction in Enum.GetValues(typeof(SharedData.Direction)))
                {
                    // Get the neighbor in that direction.
                    (int nx, int ny) = GetNeighbor(x, y, direction);

                    // If the neighbor is within the grid...
                    if (nx >= 0 && nx < labelGrid.Width && ny >= 0 && ny < labelGrid.Height)
                    {
                        // For each label that the neighbor could be...
                        foreach (int neighborLabel in labelGrid.Grid[nx, ny])
                        {
                            // If this label and the neighbor label are allowed to be neighbors in this direction...
                            // if (adjacencyMatrix.CheckAdjacency(label, neighborLabel))
                            // {
                            //     // Then this label is consistent with its neighbors.
                            //     isConsistent = true;
                            //     break;
                            // }
                        }
                    }

                    // If we've found that this label is consistent, we can stop checking.
                    if (isConsistent) break;
                }

                // If this label is not consistent with its neighbors...
                if (!isConsistent)
                {
                    // Remove this label from the possibilities for this cell.
                 //   labelGrid.Grid[x, y].Remove(label);

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
            case Direction.North:
                return (x, y - 1);
            case Direction.South:
                return (x, y + 1);
            case Direction.East:
                return (x + 1, y);
            case Direction.West:
                return (x - 1, y);
            default:
                return (x, y);
        }
    }
    
}
