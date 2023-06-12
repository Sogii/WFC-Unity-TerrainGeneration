using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class OutputMesh : MonoBehaviour
{
    public TileVariants tileVariants;
    public SharedData SharedData;
    private ModelSynthesis2DManager modelSynthesis2DManager;
    private LabelGrid labelGrid;
    public GameObject[] GrassTilePrefabs;
    private int gridWidth;
    private int gridHeight;
    private float tileSize;
    public VotingResults votingResults;
    List<SharedData.TileType> roadTiles = new List<SharedData.TileType>
        {
        SharedData.TileType.PathUR,
        SharedData.TileType.PathRD,
        SharedData.TileType.PathDL,
        SharedData.TileType.PathUL,
        SharedData.TileType.PathRL,
        SharedData.TileType.PathUD,
        SharedData.TileType.PathRDL,
        SharedData.TileType.PathUDL,
        SharedData.TileType.PathURD,
        SharedData.TileType.PathURL,
        SharedData.TileType.PathX
        };

    public void AssignData()
    {
        modelSynthesis2DManager = ModelSynthesis2DManager.Instance;
        labelGrid = modelSynthesis2DManager.LabelGrid;
        gridWidth = labelGrid.Width;
        gridHeight = labelGrid.Height;
        tileSize = modelSynthesis2DManager.TileSize;
    }

    public void GenerateMesh()
    {
        ClearMesh();
        InstantiateMeshObjects();
    }

    private void ClearMesh()
    {
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }
    }

    private void InstantiateMeshObjects()
    {
        for (int x = 0; x < gridWidth; x++)
        {
            for (int y = 0; y < gridHeight; y++)
            {
                List<ModelTile> labels = labelGrid.GetLabelsAt(new Coordinate(x, y));

                if (labels.Count == 1)
                {
                    ModelTile modelTile = labels[0];
                    GameObject tilePrefab = SelectPrefabToSpawn(modelTile);
                    Vector3 worldPosition = new Vector3(x * tileSize, tilePrefab.transform.position.y, y * tileSize);
                    GameObject instance = Instantiate(tilePrefab, worldPosition, tilePrefab.transform.rotation);
                    instance.transform.parent = transform;
                }
            }
        }
    }

    private GameObject SelectPrefabToSpawn(ModelTile modelTile)
    {
        GameObject prefabToSpawn;
        if (modelTile.tileType == SharedData.TileType.Grass)
        {
            prefabToSpawn = GrassTilePrefabs[Random.Range(0, GrassTilePrefabs.Length)];
        }
        else if (roadTiles.Contains(modelTile.tileType))
        {
            int PathTypeVariantIndex = votingResults.RoundToInt(votingResults.PathType);
            int PathWidthVariantIndex = votingResults.RoundToInt(votingResults.PathWidth);
            prefabToSpawn = LoadPathType(PathTypeVariantIndex, PathWidthVariantIndex, modelTile);
        }
        else
        {
            prefabToSpawn = modelTile.gameObject;
        }

        return prefabToSpawn;
    }

    private GameObject LoadPathType(int PathTypeVariantIndex, int PathWidthVariantIndex, ModelTile modelTile)
    {
       // Debug.Log("PathTypeVariantIndex: " + PathTypeVariantIndex + " PathWidthVariantIndex: " + PathWidthVariantIndex + " modelTile.tileType: " + modelTile.tileType);
        GameObject gameObjectToLoad = tileVariants.PathTilesVariants[PathTypeVariantIndex].pathSizeVariants[PathWidthVariantIndex].pathTerrainVariants[(int)modelTile.tileType].pathTerrainVariant;
        return gameObjectToLoad;
    }

    private Quaternion ReturnRandom90degreeAngle()
    {
        int random = Random.Range(0, 4);
        Quaternion rotation = Quaternion.Euler(0, random * 90, 0);
        return rotation;
    }

    public void SpawnCollapsedLabel(Coordinate coordinate, ModelTile modelTile)
    {
        GameObject tilePrefab = modelTile.gameObject;
        Vector3 worldPosition = new Vector3(coordinate.X * tileSize, 0, coordinate.Y * tileSize);
        GameObject instance = Instantiate(tilePrefab, worldPosition, tilePrefab.transform.rotation);
        instance.transform.parent = transform;
    }
}
