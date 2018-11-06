using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingSlotManager : MonoBehaviour {

    public static BuildingSlotManager instance;
    void Awake()
    {
        if (instance != null) { Debug.LogError("More than one BuildingSlotManager in scene !"); return; }
        instance = this;
    }

    public List<GameObject> buildingSlots;

    void Start () {
		
	}
	
	void Update () {
		
	}

    public GameObject GetBuildingSlotByID(int id)
    {
        GameObject buildingSlot = null;
        foreach (GameObject slot in buildingSlots)
        {
            if(slot.GetComponent<BuildingSlot>().id == id)
            {
                buildingSlot = slot;
                break;
            }
        }
        return buildingSlot;
    }
}
