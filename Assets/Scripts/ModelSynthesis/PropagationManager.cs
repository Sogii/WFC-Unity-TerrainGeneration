using System;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class PropagationManager
{
    private LabelGrid labelGrid;
    private AdjacencyMatrix adjacencyMatrix;
    private Queue<(int, int, int, int)> queue = new Queue<(int, int, int, int)>();

    public PropagationManager(LabelGrid labelGrid, AdjacencyMatrix adjacencyMatrix)
    {
        this.labelGrid = labelGrid;
        this.adjacencyMatrix = adjacencyMatrix;
    }

    public (int, int) GetNeighbor(int x, int y, SharedData.Direction direction)
    {
        switch (direction)
        {
            case SharedData.Direction.North: return (x, y + 1);
            case SharedData.Direction.East: return (x + 1, y);
            case SharedData.Direction.South: return (x, y - 1);
            case SharedData.Direction.West: return (x - 1, y);
            default: throw new ArgumentException("Invalid direction");
        }
    }

    public bool PropagateConstraints()
    {
        while (queue.Count > 0)
        {
            (int x, int y, int originX, int originY) = queue.Dequeue();
            Debug.Log("Dequeued: " + x + ", " + y);
            List<ModelTile> inconsistentLabels = new List<ModelTile>();

            if (labelGrid.GetLabelsAt(x, y).Count > 1)
            {
                foreach (ModelTile tile in new List<ModelTile>(labelGrid.GetLabelsAt(x, y)))
                {

                    //Remove non-consistent labels based on neighbours
                    //CheckTileConsistancyInEachDirection(x, y, tile);
                    CheckTileConsistancyWithOriginTile(x, y, originX, originY, tile);
                    EnqueueNeighbours(x, y);
                }
            }

        }

        return true;
    }


    public void CollapseCell(int x, int y)
    {
        Debug.Log($"Innitaiting CollapseCell, Collapsing ({x}, {y}).");
        List<ModelTile> possibleLabels = labelGrid.GetLabelsAt(x, y);
        Debug.Log($"Possible labels:");
        foreach (ModelTile tile in possibleLabels)
        {
            Debug.Log($"Tile: {tile.tileType}");
        }
        ModelTile chosenLabel = possibleLabels[UnityEngine.Random.Range(0, possibleLabels.Count)];

        labelGrid.SetLabelsAt(x, y, new List<ModelTile> { chosenLabel });
        Debug.Log($"Collapsed ({x}, {y}) to {chosenLabel.tileType}.");
        labelGrid.PrintGridLabels();

        foreach (SharedData.Direction direction in Enum.GetValues(typeof(SharedData.Direction)))
        {
            (int nx, int ny) = GetNeighbor(x, y, direction);

            if (nx >= 0 && nx < labelGrid.Width && ny >= 0 && ny < labelGrid.Height)
            {


                Debug.Log($"Enqueuing ({nx}, {ny}).");
                queue.Enqueue((nx, ny, x, y));
                // if (labelGrid.GetLabelsAt(nx, ny).Count > 1)
                // {
                //     queue.Enqueue((nx, ny));
                // }
            }
        }
    }
    /// <summary>
    /// Checks if a tile is consistent with its neighbors in each direction and removes it if it is not.
    /// </summary>
    public void CheckTileConsistancyInEachDirection(int x, int y, ModelTile tile)
    {
        foreach (SharedData.Direction direction in Enum.GetValues(typeof(SharedData.Direction)))
        {
            (int nx, int ny) = GetNeighbor(x, y, direction);
            if (nx >= 0 && nx < labelGrid.Width && ny >= 0 && ny < labelGrid.Height)
            {
                foreach (ModelTile neighborTile in labelGrid.GetLabelsAt(nx, ny))
                {
                    Debug.Log($"Checking if {tile.tileType} at {x}, {y} is consistent with {neighborTile.tileType} at {nx}, {ny} in direction {direction}");
                    if (!adjacencyMatrix.CheckAdjacency(tile, neighborTile, direction))
                    {
                        labelGrid.RemoveLabelAt(x, y, tile);
                    }
                }
            }
        }
    }

    public void CheckTileConsistancyWithOriginTile(int x, int y, int originX, int originY, ModelTile tile)
    {
        //Check if the ModelTile of the enqueued tile is consistant with the origin tile (tile that caused this tile to be eunqued) and figure out the direction in which they need to be checked, remove the lable if they are not consistant
        if (x == originX && y == originY + 1)
        {
            if (!adjacencyMatrix.CheckAdjacency(tile, labelGrid.GetLabelsAt(originX, originY)[0], SharedData.Direction.North))
            {
                labelGrid.RemoveLabelAt(x, y, tile);
            }
        }
        else if (x == originX + 1 && y == originY)
        {
            if (!adjacencyMatrix.CheckAdjacency(tile, labelGrid.GetLabelsAt(originX, originY)[0], SharedData.Direction.East))
            {
                labelGrid.RemoveLabelAt(x, y, tile);
            }
        }
        else if (x == originX && y == originY - 1)
        {
            if (!adjacencyMatrix.CheckAdjacency(tile, labelGrid.GetLabelsAt(originX, originY)[0], SharedData.Direction.South))
            {
                labelGrid.RemoveLabelAt(x, y, tile);
            }
        }
        else if (x == originX - 1 && y == originY)
        {
            if (!adjacencyMatrix.CheckAdjacency(tile, labelGrid.GetLabelsAt(originX, originY)[0], SharedData.Direction.West))
            {
                labelGrid.RemoveLabelAt(x, y, tile);
            }
        }
    }

    public void EnqueueNeighbours(int x, int y)
    {
        foreach (SharedData.Direction direction in Enum.GetValues(typeof(SharedData.Direction)))
        {
            (int nx, int ny) = GetNeighbor(x, y, direction);

            if (nx >= 0 && nx < labelGrid.Width && ny >= 0 && ny < labelGrid.Height)
            {
                queue.Enqueue((nx, ny, x, y));
            }
        }
    }
}
