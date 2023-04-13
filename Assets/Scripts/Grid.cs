using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile
{
    public enum TileType { Ground, Water, Forest, Mountain }
    public TileType type;
    public int rotation;

    public Tile(TileType type, int rotation)
    {
        this.type = type;
        this.rotation = rotation;
    }
}

public class Grid : MonoBehaviour
{
    [Header("Gridinfo")]
    [SerializeField] private int _gridWidth = 50;
    [SerializeField] private int _gridHeight = 50;
    [SerializeField] private float _tileSize = 1f;
    private Tile[,] _grid;

    [Header("TilePrefabs")]
    public GameObject GroundPrefab;
    public GameObject WaterPrefab;
    public GameObject ForestPrefab;
    public GameObject MountainPrefab;

    void Start()
    {
        _grid = new Tile[_gridWidth, _gridHeight];
        FillGridWithRandomTiles();
        InstantiateTiles();
    }

    public Vector3 GridToWorldSpace(int x, int y)
    {
        return new Vector3(x * _tileSize, 0, y * _tileSize);
    }


    void FillGridWithRandomTiles()
    {
        for (int x = 0; x < _gridWidth; x++)
        {
            for (int y = 0; y < _gridHeight; y++)
            {
                Tile.TileType randomType = GetRandomTileType();
                int randomRotation = GetRandomRotation();
                _grid[x, y] = new Tile(randomType, randomRotation);
            }
        }
    }

    Tile.TileType GetRandomTileType()
    {
        int tileTypeCount = System.Enum.GetValues(typeof(Tile.TileType)).Length;
        int randomIndex = Random.Range(0, tileTypeCount);
        return (Tile.TileType)randomIndex;
    }

    int GetRandomRotation()
    {
        int angleIndex = Random.Range(0, 4); // There are 8 possible 45-degree rotations
        return angleIndex * 90;
    }

    void InstantiateTiles()
    {
        for (int x = 0; x < _gridWidth; x++)
        {
            for (int y = 0; y < _gridHeight; y++)
            {
                Tile currentTile = _grid[x, y];
                GameObject tilePrefab = null;

                // Choose the appropriate prefab based on the tile type
                switch (currentTile.type)
                {
                    case Tile.TileType.Ground:
                        tilePrefab = GroundPrefab;
                        break;
                    case Tile.TileType.Water:
                        tilePrefab = WaterPrefab;
                        break;
                    case Tile.TileType.Forest:
                        tilePrefab = ForestPrefab;
                        break;
                    case Tile.TileType.Mountain:
                        tilePrefab = MountainPrefab;
                        break;
                }

                // Instantiate the prefab and set its position
                if (tilePrefab != null)
                {
                    Vector3 worldPosition = GridToWorldSpace(x, y);
                    GameObject instance = Instantiate(tilePrefab, worldPosition, Quaternion.Euler(0, currentTile.rotation, 0));
                    instance.transform.parent = transform;
                }
            }
        }
    }
}



