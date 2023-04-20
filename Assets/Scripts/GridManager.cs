using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    public static GridManager Instance { get; private set; }

    [Header("Gridinfo")]
    public int GridWidth = 50;
    public int GridHeight = 50;
    [SerializeField] private float _tileSize = 1f;
    public Tile[,] TileGrid;

    [Header("TilePrefabs")]
    public GameObject GroundPrefab;
    public GameObject WaterPrefab;
    public GameObject ForestPrefab;
    public GameObject MountainPrefab;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        InnitializeGrid();
        FillGridWithRandomTiles();
        ConvertRiverToGrid();
        InstantiateTiles();
    }


    private void InnitializeGrid()
    {
        TileGrid = new Tile[GridWidth, GridHeight];
    }


    private void ConvertRiverToGrid()
    {
        RiverToGridConverter riverToGridConverter = GetComponent<RiverToGridConverter>();
        riverToGridConverter.InteGrateRiverMesh();
    }

    public Vector3 GridToWorldSpace(int x, int y)
    {
        return new Vector3(x * _tileSize, 0, y * _tileSize);
    }


    void FillGridWithRandomTiles()
    {
        for (int x = 0; x < GridWidth; x++)
        {
            for (int y = 0; y < GridHeight; y++)
            {
                Tile.TileType randomType = GetRandomTileType();
                int randomRotation = GetRandomRotation();
                TileGrid[x, y] = new Tile(randomType, randomRotation);
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
        for (int x = 0; x < GridWidth; x++)
        {
            for (int y = 0; y < GridHeight; y++)
            {
                Tile currentTile = TileGrid[x, y];
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



