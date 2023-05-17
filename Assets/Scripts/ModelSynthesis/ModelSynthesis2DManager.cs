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
        LabelGrid = new LabelGrid(8, 8, AdjacencyMatrix);
        LabelGrid.AssignAllPossibleLabels(SharedData.ModelTiles.ToList());
        LabelGrid.PrintGridLabels();
        PropagationManager = new PropagationManager(LabelGrid, AdjacencyMatrix);
        PropagationManager.PropagateConstraints();
        OutputMesh.AssignData();
        OutputMesh.GenerateMesh();
    }

    private void Initialize()
    {
        // You can add initialization code here
    }

    private void RunModelSynthesis()
    {
        // You can add model synthesis code here
    }

    private void PropagateConstraints()
    {
        // You can add constraint propagation code here
    }

    private void DisplayOutputMesh()
    {
        // outputMesh.Display();
    }
}
