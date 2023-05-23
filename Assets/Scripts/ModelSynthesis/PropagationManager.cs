using System;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class PropagationManager
{
    private LabelGrid labelGrid;
    private AdjacencyMatrix adjacencyMatrix;
    private Queue<(int, int, int, int)> queue = new Queue<(int, int, int, int)>();

    private HashSet<(int, int)> processed = new HashSet<(int, int)>();

    public PropagationManager(LabelGrid labelGrid, AdjacencyMatrix adjacencyMatrix)
    {
        this.labelGrid = labelGrid;
        this.adjacencyMatrix = adjacencyMatrix;
    }


    public bool PropagateConstraints()
    {
        while (queue.Count > 0)
        {
            (int x, int y, int originX, int originY) = queue.Dequeue();
            // Create a copy of the list to iterate over.
            List<ModelTile> labels = new List<ModelTile>(labelGrid.GetLabelsAt(x, y));


            if (labelGrid.GetLabelsAt(x, y).Count > 1)
            {
                foreach (ModelTile tile in labels)
                {

                    //Remove non-consistent labels based on neighbours
                    List<ModelTile> tilesToRemove = CheckTileConsistancyInEachDirection2(x, y, tile);

                    foreach (ModelTile tileToRemove in tilesToRemove)
                    {
                        labelGrid.RemoveLabelAt(x, y, tileToRemove);
                        EnqueueNeighbours(x, y);
                    }
                    //  CheckTileConsistancyWithOriginTile(x, y, originX, originY, tile);
                }
            }

        }

        processed.Clear();
        return true;
    }


    public void CollapseCell(int x, int y)
    {
        List<ModelTile> possibleLabels = labelGrid.GetLabelsAt(x, y);
        ModelTile chosenLabel = possibleLabels[UnityEngine.Random.Range(0, possibleLabels.Count)];

        labelGrid.SetLabelsAt(x, y, new List<ModelTile> { chosenLabel });
        Debug.Log($"Collapsed ({x}, {y}) to {chosenLabel.tileType}.");


        foreach (SharedData.Direction direction in Enum.GetValues(typeof(SharedData.Direction)))
        {
            (int nx, int ny) = UtilityFunctions.GetNeighbor(x, y, direction);

            if (nx >= 0 && nx < labelGrid.Width && ny >= 0 && ny < labelGrid.Height)
            {
                queue.Enqueue((nx, ny, x, y));
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
    //4. 

    public List<ModelTile> CheckTileConsistancyInEachDirection2(int x, int y, ModelTile tile)
    {
        List<ModelTile> tilesToRemove = new List<ModelTile>();
        foreach (SharedData.Direction direction in Enum.GetValues(typeof(SharedData.Direction)))
        {
            (int nx, int ny) = UtilityFunctions.GetNeighbor(x, y, direction);

            // Continue to next direction if neighbor is out of bounds.
            if (!(nx >= 0 && nx < labelGrid.Width && ny >= 0 && ny < labelGrid.Height))
                continue;

            // Copy the list of labels at neighbor cell.
            List<ModelTile> neighborTiles = new List<ModelTile>(labelGrid.GetLabelsAt(nx, ny));
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

    public void CheckTileConsistancyInEachDirection(int x, int y, ModelTile tile)
    {
        foreach (SharedData.Direction direction in Enum.GetValues(typeof(SharedData.Direction)))
        {
            (int nx, int ny) = UtilityFunctions.GetNeighbor(x, y, direction);
            if (nx >= 0 && nx < labelGrid.Width && ny >= 0 && ny < labelGrid.Height)
            {
                foreach (ModelTile neighborTile in labelGrid.GetLabelsAt(nx, ny))
                {
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
        List<ModelTile> copiedTiles = new List<ModelTile>(labelGrid.GetLabelsAt(x, y));
        //Check if the ModelTile of the enqueued tile is consistant with the origin tile (tile that caused this tile to be enqued) and figure out the direction in which they need to be checked, remove the lable if they are not consistant
        if (!(x >= 0 && x < labelGrid.Width && y >= 0 && y < labelGrid.Height)) return;
        if (!(originX >= 0 && originX < labelGrid.Width && originY >= 0 && originY < labelGrid.Height)) return;
        if (!(labelGrid.GetLabelsAt(originX, originY).Count == 1)) return;
        if (x == originX && y == originY - 1)
        {
            if (!adjacencyMatrix.CheckAdjacency(tile, labelGrid.GetLabelsAt(originX, originY)[0], SharedData.Direction.North))
            {
                labelGrid.RemoveLabelAt(x, y, tile);
            }
        }
        else if (x == originX - 1 && y == originY)
        {
            if (!adjacencyMatrix.CheckAdjacency(tile, labelGrid.GetLabelsAt(originX, originY)[0], SharedData.Direction.East))
            {
                labelGrid.RemoveLabelAt(x, y, tile);
            }
        }
        else if (x == originX && y == originY + 1)
        {
            if (!adjacencyMatrix.CheckAdjacency(tile, labelGrid.GetLabelsAt(originX, originY)[0], SharedData.Direction.South))
            {
                labelGrid.RemoveLabelAt(x, y, tile);
            }
        }
        else if (x == originX + 1 && y == originY)
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
            (int nx, int ny) = UtilityFunctions.GetNeighbor(x, y, direction);

            if (nx >= 0 && nx < labelGrid.Width && ny >= 0 && ny < labelGrid.Height && !processed.Contains((nx, ny)))
            {
                queue.Enqueue((nx, ny, x, y));
                processed.Add((nx, ny));
            }
        }
    }
}
