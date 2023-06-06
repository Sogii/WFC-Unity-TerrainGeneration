using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OutputMesh : MonoBehaviour
{
    public SharedData SharedData;
    private ModelSynthesis2DManager modelSynthesis2DManager;
    private LabelGrid labelGrid;
    public GameObject[] GrassTilePrefabs;
    private int gridWidth;
    private int gridHeight;
    private float tileSize;


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

                if (labels.Count > 0)
                {
                    ModelTile modelTile = labels[0];
                    GameObject tilePrefab = selectPrefabToSpawn(modelTile);
                   
                    Vector3 worldPosition = new Vector3(x * tileSize, tilePrefab.transform.position.y, y * tileSize);
                     if(modelTile.tileType == SharedData.TileType.Grass)
                    {
                        GameObject instance = Instantiate(tilePrefab, worldPosition, ReturnRandom90degreeAngle());
                        instance.transform.parent = transform;
                    }
                    else
                    {
                        GameObject instance = Instantiate(tilePrefab, worldPosition, tilePrefab.transform.rotation);
                        instance.transform.parent = transform;
                    }
                              
                }
            }
        }
    }

    private GameObject selectPrefabToSpawn(ModelTile modelTile)
    {
        GameObject prefabToSpawn;
        if (modelTile.tileType == SharedData.TileType.Grass)
        {
           prefabToSpawn = GrassTilePrefabs[Random.Range(0, GrassTilePrefabs.Length)];
        }
        else
        {
             prefabToSpawn = modelTile.gameObject;
        }


        return prefabToSpawn;
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
