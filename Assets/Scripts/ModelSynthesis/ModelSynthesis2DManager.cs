using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

public class ModelSynthesis2DManager : MonoBehaviour
{
    public static ModelSynthesis2DManager Instance { get; private set; }
    public int TileSize;

    public AssignNeighBourWeights assignNeighbourWeights;
    public AdjacencyMatrix AdjacencyMatrix;
    public AdjacencyInfoAnalyzer AdjacencyInfoAnalyzer;
    public LabelGrid LabelGrid;
    public PropagationManager PropagationManager;
    public OutputMesh OutputMesh;
    public SharedData SharedData;


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

        assignNeighbourWeights.InitializeNeighbourWeights(SharedData.ModelTiles);
        assignNeighbourWeights.DebugPrintNeighbourWeights(SharedData.ModelTiles);
        AdjacencyInfoAnalyzer.FilloutExampleGrid();
        AdjacencyInfoAnalyzer.AnalyzeAdjacency();
        AdjacencyMatrix = new AdjacencyMatrix(AdjacencyInfoAnalyzer.GetAdjacencyDictionary(), SharedData);
        LabelGrid = new LabelGrid(15, 15, AdjacencyMatrix);
        LabelGrid.AssignAllPossibleLabels(SharedData.ModelTiles.ToList());
        LabelGrid.PrintGridLabels();
        PropagationManager = new PropagationManager(LabelGrid, AdjacencyMatrix);

        // Trigger the WFC algorithm
        OutputMesh.AssignData();
        StartCoroutine(RunWFCAlgorithmCoroutine());
        //RunWFCAlgorithm();


        LabelGrid.PrintGridLabels();
        // OutputMesh.GenerateMesh();

    }

    IEnumerator RunWFCAlgorithmCoroutine()
    {
        while (!IsFullyCollapsed())
        {
            // Find the cell with the least number of possible labels
            Vector2Int cellWithLeastLabels = FindCellWithLeastLabels();

            // Collapse that cell
            PropagationManager.CollapseGridCell(new Coordinate(cellWithLeastLabels.x, cellWithLeastLabels.y));

            // Propagate constraints
            bool success = PropagationManager.PropagateGridConstraints();

            if (!success)
            {
                Debug.LogError("A cell with no possible labels was found. Aborting...");
                break;
            }

            // Wait for a short duration before running the next iteration
            yield return new WaitForSeconds(0.05f);  // adjust delay time as needed
        }
    }

    // Method that contains the main loop of the WFC algorithm
    // void RunWFCAlgorithm()
    // {
    //     while (!IsFullyCollapsed())
    //     {
    //         // Find the cell with the least number of possible labels
    //         Vector2Int cellWithLeastLabels = FindCellWithLeastLabels();


    //         // Collapse that cell
    //         PropagationManager.CollapseGridCell(new Coordinate(cellWithLeastLabels.x, cellWithLeastLabels.y));
    //         // Propagate constraints
    //         bool success = PropagationManager.PropagateGridConstraints();
    //         // LabelGrid.PrintGridLabels();
    //         if (!success)
    //         {
    //             Debug.LogError("A cell with no possible labels was found. Aborting...");
    //             break;
    //         }
    //     }
    // }

    // Determine if every cell in the label grid has been fully collapsed (only one label remains)
    bool IsFullyCollapsed()
    {
        for (int x = 0; x < LabelGrid.Width; x++)
        {
            for (int y = 0; y < LabelGrid.Height; y++)
            {
                if (LabelGrid.GetLabelsAt(new Coordinate(x, y)).Count > 1)
                {
                    return false;
                }
            }
        }

        return true;
    }

    // Find the cell with the least number of possible labels
    public Vector2Int FindCellWithLeastLabels()
    {
        int leastLabels = int.MaxValue;
        Vector2Int cellWithLeastLabels = new Vector2Int(-1, -1);
        bool foundCell = false;

        for (int x = 0; x < LabelGrid.Width; x++)
        {
            for (int y = 0; y < LabelGrid.Height; y++)
            {
                List<ModelTile> labels = LabelGrid.GetLabelsAt(new Coordinate(x, y));
                if (labels.Count > 1 && labels.Count < leastLabels)
                {
                    leastLabels = labels.Count;
                    cellWithLeastLabels = new Vector2Int(x, y);
                    foundCell = true;
                }
            }
        }

        // If we found a cell with the least labels, return it.
        if (foundCell)
        {
            Debug.Log("Found cell with least labels: " + cellWithLeastLabels);
            return cellWithLeastLabels;
        }

        // If no cell with more than 1 label was found, return a random cell.
        int randomX = UnityEngine.Random.Range(0, LabelGrid.Width);
        int randomY = UnityEngine.Random.Range(0, LabelGrid.Height);
        Debug.Log("No cell with more than 1 label was found. Returning random cell: " + new Vector2Int(randomX, randomY));
        return new Vector2Int(randomX, randomY);
    }



}
