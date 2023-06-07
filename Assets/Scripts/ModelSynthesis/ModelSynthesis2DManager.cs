using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class ModelSynthesis2DManager : MonoBehaviour
{
    public static ModelSynthesis2DManager Instance { get; private set; }
    public int TileSize;
    [SerializeField] private int XCord = 0;
    [SerializeField] private int YCord = 0;
    [SerializeField] private bool InstantlyGenerate = false;

    public AssignNeighBourWeights assignNeighbourWeights;
    public AdjacencyMatrix AdjacencyMatrix;
    public AdjacencyInfoAnalyzer AdjacencyInfoAnalyzer;
    public LabelGrid LabelGrid;
    public PropagationManager PropagationManager;
    public OutputMesh OutputMesh;
    public SharedData SharedData;
    public RunWFCAlgorhythm runWFCAlgorhythm;
    public GridManager GridManager;


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
        GenerateLayoutGrid();
        //Analyzes the neighbourweight libary and prints them, asigns the tiles to the example grid and analyzes the adjacency of all the tiles
        AnalyzeWFCExampleGrid();
        //Innitiates the adjacencyMatrix with the generated adjacency dictionary, creates a labelgrid and assigns all possible labels to the grid
        SetupWFCGrids();
        //Innitiates the propagation manager and assigns the data to the output mesh
        InitiateWFCAlgorhythm();
    }

    private void GenerateLayoutGrid()
    {
        GridManager.CreateCatagoryGridFromExampleMesh();
    }

    private void AnalyzeWFCExampleGrid()
    {
        assignNeighbourWeights.InitializeNeighbourWeights(SharedData.ModelTiles);
      //  assignNeighbourWeights.DebugPrintNeighbourWeights(SharedData.ModelTiles);
        AdjacencyInfoAnalyzer.InitiateExampleGrid();
        AdjacencyInfoAnalyzer.AssignTilesToExampleGrid();
        AdjacencyInfoAnalyzer.AnalyzeAdjacency();
    }

    private void SetupWFCGrids()
    {
        AdjacencyMatrix = new AdjacencyMatrix(AdjacencyInfoAnalyzer.GetAdjacencyDictionary(), SharedData);
        LabelGrid = new LabelGrid(XCord, YCord, AdjacencyMatrix);
        LabelGrid.AssignAllPossibleLabels(SharedData.ModelTiles.ToList());
        LabelGrid.PrintGridLabels();
    }

    private void InitiateWFCAlgorhythm()
    {
        PropagationManager = new PropagationManager(LabelGrid, AdjacencyMatrix);
        OutputMesh.AssignData();
        runWFCAlgorhythm.AssignScripts();
        runWFCAlgorhythm.InnitiateWFCAlgorhythm(InstantlyGenerate);
    }
}
