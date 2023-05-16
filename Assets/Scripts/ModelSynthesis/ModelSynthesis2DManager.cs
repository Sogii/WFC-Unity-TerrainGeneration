using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModelSynthesis2DManager : MonoBehaviour
{
    public static ModelSynthesis2DManager Instance { get; private set; }
    public int TileSize;
    //  public int outputWidth;
    //  public int outputHeight;

    // private InputMesh inputMesh;
    public AdjacencyMatrix AdjecencyMatrix;
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
        int[,] adjecencyMatrix = AdjacencyInfoAnalyzer.ConstructAdjacencyMatrix();
       // LabelGrid = new LabelGrid(8, 8, AdjecencyMatrix);
        // PropagationManager = new PropagationManager(LabelGrid, AdjecencyMatrix);
        // PropagationManager.PropagateConstraints();
        // OutputMesh.AssignData();
        // OutputMesh.GenerateMesh();
    }

    private void Initialize()
    {


    }

    private void RunModelSynthesis()
    {

    }

    private void PropagateConstraints()
    {

    }

    private void DisplayOutputMesh()
    {
        //  outputMesh.Display();
    }
}
