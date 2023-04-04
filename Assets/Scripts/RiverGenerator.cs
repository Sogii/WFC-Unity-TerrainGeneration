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
     [SerializeField] private float _riverSidesWidth;
    private MeshFilter meshFilter;
    private GameObject _river;
    private GameObject _leftRiverSideObject;
    private Mesh _leftRiverSideMesh; 
    private GameObject _rightRiverSideObject;
    private Mesh _rightRiverSideMesh;
    [SerializeField] Material _riverMaterial;
    [SerializeField] Material _riverSideMaterial;
    private void Start(){
        pathCreator = this.gameObject.GetComponent<PathCreator>();
        _riverPath = pathCreator.path;
        _pathLength = _riverPath.length;
        stepSize = (_pathLength - .001f) /_stepCount;
       
        GenerateRiverMesh();
        GenerateRiverSideMesh();
        
    }

    
    private void GenerateRiverMesh(){
        if(_riverMesh == null) _riverMesh = new Mesh();
        else _riverMesh.Clear();
        
        List<Vector3> vertices = new List<Vector3>(); 
        List<Vector3> normals = new List<Vector3>(); 
        float dst = 0;
        while(dst <= _riverPath.length)
        {
            Vector3 point = _riverPath.GetPointAtDistance(dst);
            Vector3 normal = _riverPath.GetNormalAtDistance(dst);            
            Instantiate(Prefab, point, Prefab.transform.rotation ,SplineHolder.transform);
            Instantiate(Prefab, point+ (normal * _riverWidth), Prefab.transform.rotation, SplineHolder.transform);
            Instantiate(Prefab, point+ (normal * -1 * _riverWidth), Prefab.transform.rotation, SplineHolder.transform);

            vertices.Add(point + (normal * _riverWidth));
            normals.Add(normal);
            vertices.Add(point - (normal * _riverWidth));
            normals.Add(-normal);
            
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
        _riverMesh.normals = normals.ToArray();
        _riverMesh.triangles = triangles.ToArray();
        AssignMeshComponents(_river, _riverMesh, "River", _riverMaterial);
    }   

    private void GenerateRiverSideMesh(){
        if(_leftRiverSideMesh == null) _leftRiverSideMesh = new Mesh();
            else _leftRiverSideMesh.Clear();
        if(_rightRiverSideMesh == null) _rightRiverSideMesh = new Mesh();
            else _rightRiverSideMesh.Clear();
        List<Vector3> leftVertices = new List<Vector3>(); 
        List<Vector3> rightVertices = new List<Vector3>(); 
        for (int i = 0; i < _riverMesh.vertices.Length; i++){
            if(i%2 == 0){
                rightVertices.Add(_riverMesh.vertices[i] + (_riverMesh.normals[i] * _riverSidesWidth));
                //leftVertices.Add(_riverMesh.vertices[i] + new Vector3(3*i,0,0));
                leftVertices.Add(_riverMesh.vertices[i]);      
            }
            else{
                rightVertices.Add(_riverMesh.vertices[i]);  
                rightVertices.Add(_riverMesh.vertices[i] + (_riverMesh.normals[i] * _riverSidesWidth));             
            }
        }
        List<int>leftTriangles = new List<int>();
        for (int i = 0; i < (leftVertices.Count/ 2) -1 ; i++){
            
            int index1 = i* 2;
            int index2 = index1+ 1;
            int index3 = index1 + 2;
            int index4 = index1+ 3;

            //Triangle 1
            leftTriangles.Add(index1);
            leftTriangles.Add(index4);
            leftTriangles.Add(index3); 

            //Triangle 2
            leftTriangles.Add(index1);
            leftTriangles.Add(index2);
            leftTriangles.Add(index4);
        }

            List<int>rightTriangles = new List<int>();
        for (int i = 0; i < (rightVertices.Count/ 2) -1 ; i++){
            
            int index1 = i* 2;
            int index2 = index1+ 1;
            int index3 = index1 + 2;
            int index4 = index1+ 3;

            //Triangle 1
            rightTriangles.Add(index1);
            rightTriangles.Add(index4);
            rightTriangles.Add(index3); 

            //Triangle 2
            rightTriangles.Add(index1);
            rightTriangles.Add(index2);
            rightTriangles.Add(index4);
        }

        _leftRiverSideMesh.vertices = leftVertices.ToArray();
        _leftRiverSideMesh.triangles = leftTriangles.ToArray();
        AssignMeshComponents(_leftRiverSideObject, _leftRiverSideMesh, "LeftRiverSide", _riverSideMaterial);

        _rightRiverSideMesh.vertices = rightVertices.ToArray();
        _rightRiverSideMesh.triangles = rightTriangles.ToArray();
        AssignMeshComponents(_rightRiverSideObject, _rightRiverSideMesh, "RightRiverSide", _riverSideMaterial);


    } 

    private void AssignMeshComponents(GameObject gameObject, Mesh meshToAssign, string objectName, Material materialToAssign){
        if(gameObject == null)  gameObject = new GameObject(objectName);
            else{
                Destroy(gameObject);
                new GameObject(objectName);
            }          
        gameObject.AddComponent<MeshFilter>();
        gameObject.AddComponent<MeshRenderer>();
        meshFilter = gameObject.GetComponent<MeshFilter>();
        gameObject.GetComponent<MeshRenderer>().material = materialToAssign;

        meshFilter.sharedMesh = meshToAssign;        
    }
}
