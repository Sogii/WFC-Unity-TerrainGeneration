using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OutputMesh : MonoBehaviour
{
    public SharedData SharedData;
    private ModelSynthesis2DManager modelSynthesis2DManager;
    private LabelGrid labelGrid;
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
        GenerateMesh();
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
                    // For the sake of simplicity, we're only taking the first label. 
                    // You might want to update this to choose a label in a different way.
                    ModelTile modelTile = labels[0];

                    GameObject tilePrefab = modelTile.gameObject;
                    Vector3 worldPosition = new Vector3(x * tileSize, 0, y * tileSize);
                    GameObject instance = Instantiate(tilePrefab, worldPosition, tilePrefab.transform.rotation);
                    instance.transform.parent = transform;
                }
            }
        }
    }

    public void SpawnCollapsedLabel(Coordinate coordinate, ModelTile modelTile)
    {
        GameObject tilePrefab = modelTile.gameObject;
        Vector3 worldPosition = new Vector3(coordinate.X * tileSize, 0, coordinate.Y * tileSize);
        GameObject instance = Instantiate(tilePrefab, worldPosition, tilePrefab.transform.rotation);
        instance.transform.parent = transform;
    }
}
