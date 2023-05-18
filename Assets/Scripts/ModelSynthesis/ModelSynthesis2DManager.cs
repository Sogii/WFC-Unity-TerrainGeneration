using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

public class ModelSynthesis2DManager : MonoBehaviour
{
    public static ModelSynthesis2DManager Instance { get; private set; }
    public int TileSize;

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
        AdjacencyInfoAnalyzer.AnalyzeAdjacency();
        AdjacencyMatrix = new AdjacencyMatrix(AdjacencyInfoAnalyzer.GetAdjacencyDictionary(), SharedData);
        LabelGrid = new LabelGrid(5, 5, AdjacencyMatrix);
        LabelGrid.AssignAllPossibleLabels(SharedData.ModelTiles.ToList());
        LabelGrid.PrintGridLabels();
        PropagationManager = new PropagationManager(LabelGrid, AdjacencyMatrix);

        // Trigger the WFC algorithm
        RunWFCAlgorithm();

      //  OutputMesh.AssignData();
       // OutputMesh.GenerateMesh();
        LabelGrid.PrintGridLabels();
    }

    // Method that contains the main loop of the WFC algorithm
    void RunWFCAlgorithm()
    {
        while (!IsFullyCollapsed())
        {
            // Find the cell with the least number of possible labels
            Vector2Int cellWithLeastLabels = FindCellWithLeastLabels();

            // Collapse that cell
            PropagationManager.CollapseCell(cellWithLeastLabels.x, cellWithLeastLabels.y);

            // Propagate constraints
            bool success = PropagationManager.PropagateConstraints();

            if (!success)
            {
                Debug.LogError("A cell with no possible labels was found. Aborting...");
                break;
            }
        }
    }

    // Determine if every cell in the label grid has been fully collapsed (only one label remains)
    bool IsFullyCollapsed()
    {
        for (int x = 0; x < LabelGrid.Width; x++)
        {
            for (int y = 0; y < LabelGrid.Height; y++)
            {
                if (LabelGrid.GetLabelsAt(x, y).Count > 1)
                {
                    return false;
                }
            }
        }

        return true;
    }

    // Find the cell with the least number of possible labels
    Vector2Int FindCellWithLeastLabels()
    {
        Vector2Int minPos = new Vector2Int(-1, -1);
        int minCount = int.MaxValue;

        for (int x = 0; x < LabelGrid.Width; x++)
        {
            for (int y = 0; y < LabelGrid.Height; y++)
            {
                int count = LabelGrid.GetLabelsAt(x, y).Count;

                if (count < minCount)
                {
                    minCount = count;
                    minPos = new Vector2Int(x, y);
                }
            }
        }

        return minPos;
    }
}
