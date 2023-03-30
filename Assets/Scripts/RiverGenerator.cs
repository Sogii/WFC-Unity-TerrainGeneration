using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PathCreation.Utility;
using PathCreation.Examples;
using PathCreation;
public class RiverGenerator : MonoBehaviour
{
    public GameObject Prefab;
    public GameObject SplineHolder;
    [SerializeField] private int _stepCount;
    private float  _pathLength;
    private float stepSize;
    private PathCreator pathCreator;

    private void Start(){
        pathCreator = this.gameObject.GetComponent<PathCreator>();
        VertexPath riverPath = pathCreator.path;
        _pathLength = riverPath.length;
        float stepSize = _pathLength/_stepCount;
    }

    
    private void GenerateRiverMesh(){
        
    }

}
