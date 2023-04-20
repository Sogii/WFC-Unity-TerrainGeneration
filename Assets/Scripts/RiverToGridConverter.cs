using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RiverToGridConverter : MonoBehaviour
{
    //private RiverGenerator _riverGenerator;
    private GameObject _river;
    private MeshFilter _riverMeshFilter;
    private int riverSamplePoints = 5;
    //private Mesh _riverMesh;
    void Start()
    {
        _river = GameObject.Find("River");
        _riverMeshFilter = _river.GetComponent<MeshFilter>();
    }

    void Update()
    {

    }

    public void InteGrateRiverMesh()
    {
        Vector3[] vertices = _riverMeshFilter.mesh.vertices;

        for (int i = 0; i < vertices.Length; i += 2)
        {
            Vector3 vertex1 = _riverMeshFilter.transform.TransformPoint(vertices[i]);
            Vector3 vertex2 = _riverMeshFilter.transform.TransformPoint(vertices[i + 1]);

            for (int j = 0; j < riverSamplePoints; j++)
            {
                float t = (float)j / (riverSamplePoints - 1);
                Vector3 samplePoint = Vector3.Lerp(vertex1, vertex2, t);

                Vector2Int gridCoords = WorldSpaceToGridSpace(samplePoint);

                if (gridCoords.x >= 0 && gridCoords.x < GridManager.Instance.GridWidth && gridCoords.y >= 0 && gridCoords.y < GridManager.Instance.GridHeight)
                {
                    GridManager.Instance.TileGrid[gridCoords.x, gridCoords.y] = new Tile(Tile.TileType.Water, 0);
                }
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

