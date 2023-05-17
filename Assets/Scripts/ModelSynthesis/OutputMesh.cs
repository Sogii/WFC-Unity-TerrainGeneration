using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OutputMesh : MonoBehaviour
{
   // private GameObject[] _tilePrefabs;
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
        for (int x = 0; x < gridWidth; x++)
        {
            for (int y = 0; y < gridHeight; y++)
            {
              //  int label = labelGrid.GetLabelAt(x, y);
               // Debug.Log(label);
            //     GameObject tilePrefab = SharedData.TileTypes[label].gameObject;
            //     Vector3 worldPosition = new Vector3(x * tileSize, 0, y * tileSize);
            //     GameObject instance = Instantiate(tilePrefab, worldPosition, tilePrefab.transform.rotation);
            //     instance.transform.parent = transform;
            }
        }
    }
}
