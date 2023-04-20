using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RiverToGridConverter : MonoBehaviour
{
    private RiverGenerator _riverGenerator;
    private MeshFilter _riverMeshFilter;
    //private Mesh _riverMesh;
    void Start()
    {
        _riverGenerator = gameObject.GetComponent<RiverGenerator>();
        _riverMeshFilter = _riverGenerator.GetComponent<MeshFilter>();
    }

    void Update()
    {

    }

    void InteGrateRiverMesh()
    {
        Vector3[] vertices = _riverMeshFilter.mesh.vertices;

        foreach (Vector3 vertex in vertices)
        {
            //convert vertex from localspace to world space
            Vector3 WorldVertex = _riverMeshFilter.transform.TransformPoint(vertex);
            Vector2Int gridCoords = WorldSpaceToGridSpace(WorldVertex);

             if (gridCoords.x >= 0 && gridCoords.x < GridManager.Instance.GridWidth && gridCoords.y >= 0 && gridCoords.y < GridManager.Instance.GridHeight)
            {
                GridManager.Instance.TileGrid[gridCoords.x, gridCoords.y] = new Tile(Tile.TileType.Water, 0);
            }
        }

    }

    Vector2Int WorldSpaceToGridSpace(Vector3 worldPosition)
    {
        float tileSize = 1.0f; // Adjust this value based on the size of your tiles
        int x = Mathf.RoundToInt(worldPosition.x / tileSize);
        int y = Mathf.RoundToInt(worldPosition.z / tileSize);
        return new Vector2Int(x, y);
    }
}

