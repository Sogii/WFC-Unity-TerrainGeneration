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
    private Mesh _riverMesh;
    private Mesh _riverSideMesh;
    private MeshFilter meshFilter;
    private GameObject _river;
    private void Start(){
        pathCreator = this.gameObject.GetComponent<PathCreator>();
        _riverPath = pathCreator.path;
        _pathLength = _riverPath.length;
        stepSize = (_pathLength - .001f) /_stepCount;
        _riverMesh = new Mesh();
        GenerateRiverMesh();
        
    }

    
    private void GenerateRiverMesh(){
        List<Vector3> vertices = new List<Vector3>(); 
        float dst = 0;
        while(dst <= _riverPath.length)
        {
            Vector3 point = _riverPath.GetPointAtDistance(dst);
            Vector3 normal = _riverPath.GetNormalAtDistance(dst);            
            Instantiate(Prefab, point, Prefab.transform.rotation ,SplineHolder.transform);
            Instantiate(Prefab, point+ (normal * _riverWidth), Prefab.transform.rotation, SplineHolder.transform);
            Instantiate(Prefab, point+ (normal * -1 * _riverWidth), Prefab.transform.rotation, SplineHolder.transform);
           // vertices.Add(point);
            vertices.Add(point + (normal * _riverWidth));
            vertices.Add(point - (normal * _riverWidth));
            dst += stepSize;
            
        }

        List<int>triangles = new List<int>();  
        for (int i = 0; i < (vertices.Count/ 2) -1 ; i++)
        {
            
            int index1 = i* 2;
            int index2 = index1+ 1;
            int index3 = index1 + 2;
            int index4 = index1+ 3;

            //Triangle 1
            triangles.Add(index1);
            triangles.Add(index4);
            triangles.Add(index3); 

            //Triangle 2
            triangles.Add(index1);
            triangles.Add(index2);
            triangles.Add(index4);
        }
        
        _riverMesh.vertices = vertices.ToArray();
        _riverMesh.triangles = triangles.ToArray();
        AssignMeshComponents();
    }   

    private void GenerateRiverSideMesh(){
        for (int i = 0; i < _riverMesh.vertices.Length; i++){
            if(i%2 == 0){
                _riverMesh.vertices[i] 
            }
            else{

            }
        }
    } 

    private void AssignMeshComponents(){
        _river = new GameObject("River");
        _river.AddComponent<MeshFilter>();
        _river.AddComponent<MeshRenderer>();
        meshFilter = _river.GetComponent<MeshFilter>();
        meshFilter.sharedMesh = _riverMesh;        
    }
}
