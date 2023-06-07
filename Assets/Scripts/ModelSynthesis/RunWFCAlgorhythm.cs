using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RunWFCAlgorhythm : MonoBehaviour
{
    public ModelSynthesis2DManager ModelSynthesis2DManager;
    private PropagationManager PropagationManager;
    private LabelGrid LabelGrid;
    private OutputMesh OutputMesh;
    private float tileGenerationDelay = 0.01f;

    public void AssignScripts()
    {
        PropagationManager = ModelSynthesis2DManager.PropagationManager;
        LabelGrid = ModelSynthesis2DManager.LabelGrid;
        OutputMesh = ModelSynthesis2DManager.OutputMesh;   
    }
    public void InnitiateWFCAlgorhythm(bool InstantlyGenerate)
    {
        if (InstantlyGenerate)
        {
            RunWFCAlgorithmInstant();
        }
        else
        {
            StartCoroutine(RunWFCAlgorithmCoroutine());
            
        }
        OutputMesh.GenerateMesh();
    }


    public void RunWFCAlgorithmInstant()
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
        }
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
            yield return new WaitForSeconds(tileGenerationDelay);  // adjust delay time as needed
        }
    }
    private bool IsFullyCollapsed()
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
    private Vector2Int FindCellWithLeastLabels()
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
            return cellWithLeastLabels;
        }

        // If no cell with more than 1 label was found, return a random cell.
        int randomX = UnityEngine.Random.Range(0, LabelGrid.Width);
        int randomY = UnityEngine.Random.Range(0, LabelGrid.Height);
       // Debug.Log("No cell with more than 1 label was found. Returning random cell: " + new Vector2Int(randomX, randomY));
        return new Vector2Int(randomX, randomY);
    }
}
