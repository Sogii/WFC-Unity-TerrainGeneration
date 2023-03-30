using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RiverBedGenerator : MonoBehaviour
{
    public RoadMeshCreator roadMeshCreator;
    private Mesh extractedRiverMesh;
    
    void Start()
    {
        extractedRiverMesh = roadMeshCreator.RiverMesh;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
