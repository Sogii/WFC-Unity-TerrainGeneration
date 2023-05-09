using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdjacencyMatrix
{
    public bool[,] Matrix { get; private set; }

    public AdjacencyMatrix(List<Texture2D> tiles)
    {
        int numTiles = tiles.Count;
        Matrix = new bool[numTiles, numTiles];

        for (int i = 0; i < numTiles; i++)
        {
            for (int j = 0; j < numTiles; j++)
            {
                Matrix[i, j] = CheckAdjacency(tiles[i], tiles[j]);
            }
        }
    }

    private bool CheckAdjacency(Texture2D tileA, Texture2D tileB)
    {
        // Implement your adjacency check logic here.
        // This method should return true if tileA and tileB can be adjacent, false otherwise.

        return false;
    }
}
