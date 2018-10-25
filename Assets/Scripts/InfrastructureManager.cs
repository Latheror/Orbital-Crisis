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
    public GameObject previouslySelectedBuilding;

    public List<GameObject> recyclingStationsList = new List<GameObject>();

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

        GameManager.instance.ChangeSelectionState(GameManager.SelectionState.BuildingSelected);

        if (previouslySelectedBuilding != null)
        {
            previouslySelectedBuilding.GetComponent<Building>().BuildingDeselected();
        }

        // Transmit info to BuildingInfoPanel
        BuildingInfoPanel.instance.BuildingTouched(selectedBuilding);

        // Transmit info to concerned building script
        selectedBuilding.GetComponent<Building>().BuildingTouched();

        // Take actions based on building "Tags" list
        BuildingTagsActions(selectedBuilding);

        previouslySelectedBuilding = selectedBuilding;
    }

    public bool UpgradeBuildingRequest(GameObject building)
    {
        bool requestAccepted = false;
        Debug.Log("UpgradeBuildingRequest");
        if (ResourcesManager.instance.CanPayUpgradeCosts(building.GetComponent<Building>()))
        {
            if(ResourcesManager.instance.PayUpgradeCosts(building.GetComponent<Building>()))
            {
                Debug.Log("UpgradeBuilding : Can pay upgrade");
                building.GetComponent<Building>().UpgradeToNextTier();
                requestAccepted = true;
            }
        }
        else
        {
            Debug.Log("UpgradeBuilding : Can't pay upgrade !");
        }
        return requestAccepted;
    }

    public void DestroyBuilding(GameObject building)
    {
        building.GetComponent<Building>().DestroyBuildingSpecificActions();

        building.GetComponent<Building>().buildingSpot.GetComponent<BuildingSlot>().RemoveBuilding();

        BuildingManager.instance.buildingList.Remove(building);

        // Building type lists
        if(building.GetComponent<Building>().buildingType.name == "Recycling Station")
        {
            recyclingStationsList.Remove(building);
        }

        EnergyPanel.instance.UpdateEnergyProductionAndConsumption();

        GameManager.instance.ChangeSelectionState(GameManager.SelectionState.Default);

        Destroy(building);
    }

    public void BuildingTagsActions(GameObject building)
    {
        Debug.Log("BuildingTagsActions");
        if(building.GetComponent<Building>().HasTag("spaceport"))
        {
            Debug.Log("Spaceport Touched !");

            SpaceportInfoPanel.instance.SpaceportTouched(building);
        }
    }
}
