using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class ModelSynthesis2DManager : MonoBehaviour
{
    public static ModelSynthesis2DManager Instance { get; private set; }
    private int tileSize;
    private int XCordWidth = 0;
    private int YCordWidth = 0;
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
        XCordWidth = SharedData.XGridSize;
        YCordWidth = SharedData.YGridSize;
        tileSize = SharedData.TileSize;
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
       // GridManager.CreateCatagoryGridFromExampleMesh();
    }

    private void AnalyzeWFCExampleGrid()
    {
        assignNeighbourWeights.InitializeNeighbourWeights(SharedData.AllModelTiles);
        assignNeighbourWeights.InitializeNeighbourWeights(SharedData.GetModelTilesArrayByTerrainType(SharedData.TerrainType.GreeneryTerrain));
        assignNeighbourWeights.InitializeNeighbourWeights(SharedData.GetModelTilesArrayByTerrainType(SharedData.TerrainType.BufferTerrain));
        //  assignNeighbourWeights.DebugPrintNeighbourWeights(SharedData.ModelTiles);
        AdjacencyInfoAnalyzer.InitiateExampleGrid();
        AdjacencyInfoAnalyzer.AssignTilesToExampleGrid();
        AdjacencyInfoAnalyzer.AnalyzeAdjacency();
        AdjacencyInfoAnalyzer.AddCustomAdjacencyData();
    }

    private void SetupWFCGrids()
    {
        AdjacencyMatrix = new AdjacencyMatrix(AdjacencyInfoAnalyzer.GetAdjacencyDictionary(), SharedData);
        LabelGrid = new LabelGrid(XCordWidth, YCordWidth, AdjacencyMatrix, SharedData);

        LabelGrid.AssignLabelsBasedOnTerrainTypeGrid(GridManager.CreateCatagoryGridFromExampleMesh());
        //   LabelGrid.AssignAllPossibleLabels(SharedData.ModelTiles.ToList());
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
