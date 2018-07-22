using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DebugManager : MonoBehaviour {

    public static DebugManager instance;
    void Awake(){ 
        if (instance != null){ Debug.LogError("More than one MovementsManager in scene !"); return; } instance = this;
    }


    public GameObject debugArea1;
    public GameObject debugArea2;


    public void SetDebugArea1Text(string text)
    {
        debugArea1.GetComponent<Text>().text = text;
    }

    public void DisplayBuildingState()
    {
        SetDebugArea1Text(BuildingManager.instance.buildingState.ToString());
    }

    public void SetDebugArea2Text(string text)
    {
        debugArea2.GetComponent<Text>().text = text;
    }

    //public void DisplayBuildingState()
    //{
    //    SetDebugArea2Text(BuildingManager.instance.buildingState.ToString());
    //}

}
