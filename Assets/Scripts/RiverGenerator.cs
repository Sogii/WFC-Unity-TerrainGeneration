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
    [SerializeField] private float _riverWidth;
    private float  _pathLength;
    private float stepSize;
    private VertexPath _riverPath;
    private PathCreator pathCreator;

    private void Start(){
        pathCreator = this.gameObject.GetComponent<PathCreator>();
        _riverPath = pathCreator.path;
        _pathLength = _riverPath.length;
        stepSize = _pathLength/_stepCount;
        GenerateRiverMesh();
    }

    
    private void GenerateRiverMesh(){
        float dst = 0;
        while(dst < _riverPath.length){
            Vector3 point = _riverPath.GetPointAtDistance(dst);
            Instantiate(Prefab, point, Prefab.transform.rotation ,SplineHolder.transform);
            dst += stepSize; 
        }   
    }

    

}
