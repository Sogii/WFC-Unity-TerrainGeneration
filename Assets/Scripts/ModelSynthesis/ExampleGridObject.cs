using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExampleGridObject : MonoBehaviour
{
    public SharedData SharedData;
    public SharedData.TileType thisTileType;
    public int GridX;
    public int GridY;

    void Awake()
    {
       GridX = Mathf.RoundToInt(this.gameObject.transform.localPosition.x);
       GridY = Mathf.RoundToInt(this.gameObject.transform.localPosition.z);    
    }
}
