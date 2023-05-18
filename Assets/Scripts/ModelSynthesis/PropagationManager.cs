using System;
using System.Collections.Generic;
using UnityEngine;

public class PropagationManager
{
    private LabelGrid labelGrid;
    private AdjacencyMatrix adjacencyMatrix;
    private Queue<(int, int)> queue = new Queue<(int, int)>();

    public PropagationManager(LabelGrid labelGrid, AdjacencyMatrix adjacencyMatrix)
    {
        this.labelGrid = labelGrid;
        this.adjacencyMatrix = adjacencyMatrix;
    }

    public (int, int) GetNeighbor(int x, int y, SharedData.Direction direction)
    {
        switch (direction)
        {
            case SharedData.Direction.North: return (x, y - 1);
            case SharedData.Direction.East: return (x + 1, y);
            case SharedData.Direction.South: return (x, y + 1);
            case SharedData.Direction.West: return (x - 1, y);
            default: throw new ArgumentException("Invalid direction");
        }
    }

    public bool PropagateConstraints()
    {
        while (queue.Count > 0)
        {
            (int x, int y) = queue.Dequeue();

            foreach (ModelTile tile in new List<ModelTile>(labelGrid.GetLabelsAt(x, y)))
            {
                bool isConsistent = false;

                foreach (SharedData.Direction direction in Enum.GetValues(typeof(SharedData.Direction)))
                {
                    (int nx, int ny) = GetNeighbor(x, y, direction);

                    if (nx >= 0 && nx < labelGrid.Width && ny >= 0 && ny < labelGrid.Height)
                    {
                        foreach (ModelTile neighborTile in labelGrid.GetLabelsAt(nx, ny))
                        {
                            if (adjacencyMatrix.CheckAdjacency(tile, neighborTile, direction))
                            {
                                isConsistent = true;
                                break;
                            }
                        }

                        if (!isConsistent)
                        {
                            break;
                        }
                    }
                }

                if (!isConsistent)
                {
                    labelGrid.RemoveLabelAt(x, y, tile);

                    if (labelGrid.GetLabelsAt(x, y).Count == 0)
                    {
                        return false;
                    }

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

        return true;
    }

    public void CollapseCell(int x, int y)
    {
        List<ModelTile> possibleLabels = labelGrid.GetLabelsAt(x, y);

        ModelTile chosenLabel = possibleLabels[UnityEngine.Random.Range(0, possibleLabels.Count)];

        labelGrid.SetLabelsAt(x, y, new List<ModelTile> { chosenLabel });

        foreach (SharedData.Direction direction in Enum.GetValues(typeof(SharedData.Direction)))
        {
            (int nx, int ny) = GetNeighbor(x, y, direction);

            if (nx >= 0 && nx < labelGrid.Width && ny >= 0 && ny < labelGrid.Height)
            {
                queue.Enqueue((nx, ny));
            }
        }
    }

    public void AddTileToQueue(int x, int y)
    {
        queue.Enqueue((x, y));
    }
}
