using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "VotingResults", menuName = "ScriptableObjects/VotingResults", order = 3)]
public class VotingResults : ScriptableObject
{
    public float PathQuantity;
    public float RiverWidth;
    public float WaterFormationGeneral;
    public float RiverSideWaterGeneration;
    public float PathWidth;
    public float PathType;
    public float PathQuality;
    public float WaterColor;
    public float WaterTransparency;
    public float WaterAnimals;
    public float WaterPlants;
    public float RiverSidesPlants;
    public float RiverSidesMaterialType;
    public float RiverFlowSpeed;
    public float TreeDensity;
    public float TreeType;
    public float InfraStructure;
    public float BuildingQuality;


/// <summary>
/// This method is used to calculate the voting results for the path quantity
/// </summary>
///<returns> Returns a int value between 0 and 2 </returns>
    public int RoundToInt(float value)
    {
        int result = Mathf.RoundToInt(Mathf.Abs(value));
        return result;
    }
}
