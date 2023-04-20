using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectToGridConverter : MonoBehaviour
{
    private GameObject _river;
    private MeshFilter _riverMeshFilter;

    private GameObject _leftRiverBank;
    private MeshFilter _leftRiverBankMeshFilter;
    private GameObject _rightRiverBank;
    private MeshFilter _rightRiverBankMeshFilter;
    private int _samplePoints = 25;
    void Start()
    {
        AssignComponents();
    }

    private void AssignComponents()
    {
        _river = GameObject.Find("River");
        _riverMeshFilter = _river.GetComponent<MeshFilter>();

        _rightRiverBank = GameObject.Find("RightRiverSide");
        _rightRiverBankMeshFilter = _rightRiverBank.GetComponent<MeshFilter>();

        _leftRiverBank = GameObject.Find("LeftRiverSide");
        _leftRiverBankMeshFilter = _leftRiverBank.GetComponent<MeshFilter>();
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

            for (int j = 0; j < _samplePoints; j++)
            {
                float t = (float)j / (_samplePoints - 1);
                Vector3 samplePoint = Vector3.Lerp(vertex1, vertex2, t);

                Vector2Int gridCoords = WorldSpaceToGridSpace(samplePoint);

                if (gridCoords.x >= 0 && gridCoords.x < GridManager.Instance.GridWidth && gridCoords.y >= 0 && gridCoords.y < GridManager.Instance.GridHeight)
                {
                    GridManager.Instance.TileGrid[gridCoords.x, gridCoords.y] = new Tile(Tile.TileType.Water, 0);
                }
            }
        }

    }

    public void InteGrateRiverBankMesh()
    {
        Vector3[] leftVertices = _leftRiverBankMeshFilter.mesh.vertices;

        for (int i = 0; i < leftVertices.Length; i += 2)
        {
            Vector3 vertex1 = _leftRiverBankMeshFilter.transform.TransformPoint(leftVertices[i]);
            Vector3 vertex2 = _leftRiverBankMeshFilter.transform.TransformPoint(leftVertices[i + 1]);

            for (int j = 0; j < _samplePoints; j++)
            {
                float t = (float)j / (_samplePoints - 1);
                Vector3 samplePoint = Vector3.Lerp(vertex1, vertex2, t);

                Vector2Int gridCoords = WorldSpaceToGridSpace(samplePoint);

                if (gridCoords.x >= 0 && gridCoords.x < GridManager.Instance.GridWidth && gridCoords.y >= 0 && gridCoords.y < GridManager.Instance.GridHeight)
                {
                    GridManager.Instance.TileGrid[gridCoords.x, gridCoords.y] = new Tile(Tile.TileType.Ground, 0);
                }
            }
        }

        Vector3[] rightVertices = _rightRiverBankMeshFilter.mesh.vertices;

        for (int i = 0; i < rightVertices.Length; i += 2)
        {
            Vector3 vertex1 = _leftRiverBankMeshFilter.transform.TransformPoint(rightVertices[i]);
            Vector3 vertex2 = _leftRiverBankMeshFilter.transform.TransformPoint(rightVertices[i + 1]);

            for (int j = 0; j < _samplePoints; j++)
            {
                float t = (float)j / (_samplePoints - 1);
                Vector3 samplePoint = Vector3.Lerp(vertex1, vertex2, t);

                Vector2Int gridCoords = WorldSpaceToGridSpace(samplePoint);

                if (gridCoords.x >= 0 && gridCoords.x < GridManager.Instance.GridWidth && gridCoords.y >= 0 && gridCoords.y < GridManager.Instance.GridHeight)
                {
                    GridManager.Instance.TileGrid[gridCoords.x, gridCoords.y] = new Tile(Tile.TileType.Ground, 0);
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

