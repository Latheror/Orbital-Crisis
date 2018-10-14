using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InfrastructureManager : MonoBehaviour {

    public static InfrastructureManager instance;
    void Awake()
    {
        if (instance != null) { Debug.LogError("More than one InfrastructureManager in scene !"); return; }
        instance = this;
    }

    public GameObject selectedBuilding;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void BuildingTouched(GameObject building)
    {
        selectedBuilding = building;
        Debug.Log(selectedBuilding.GetComponent<Building>().buildingType.name + " selected.");

        GameManager.instance.selectionState = GameManager.SelectionState.BuildingSelected;

        // Transmit info to BuildingInfoPanel
        BuildingInfoPanel.instance.BuildingTouched(selectedBuilding);

        // Transmit info to concerned building script
        selectedBuilding.GetComponent<Building>().BuildingTouched();

    }
}
