using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModelSynthesis2DManager : MonoBehaviour
{
    public static ModelSynthesis2DManager Instance { get; private set; }
    public Texture2D inputTexture;
    public int TileSize;
    public int outputWidth;
    public int outputHeight;

    private InputMesh inputMesh;
    private AdjacencyMatrix adjacencyMatrix;
    public AdjacencyInfoAnalyzer adjacencyInfoAnalyzer;
    public LabelGrid LabelGrid;
    private PropagationManager propagationManager;
    private OutputMesh outputMesh;


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
        Initialize();
        RunModelSynthesis();
        DisplayOutputMesh();
    }

    private void Initialize()
    {
        inputMesh = new InputMesh(inputTexture);
        int[,] adjacencyMatrix = adjacencyInfoAnalyzer.ConstructAdjacencyMatrix();
      //  LabelGrid labelGrid = new LabelGrid(gridWidth, gridHeight, adjacencyMatrix);
       // propagationManager = new PropagationManager(LabelGrid, adjacencyMatrix);
        outputMesh = new OutputMesh();
    }

    private void RunModelSynthesis()
    {
        AssignInitialLabels();
        PropagateConstraints();
    }

    private void AssignInitialLabels()
    {
        LabelGrid.AssignAllPossibleLabels();
    }

    private void PropagateConstraints()
    {
        //propagationManager.Propagate();
    }

    private void DisplayOutputMesh()
    {
      //  outputMesh.Display();
    }
}
