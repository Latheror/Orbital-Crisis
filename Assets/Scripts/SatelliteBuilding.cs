using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SatelliteBuilding : Building {

	// Use this for initialization
	void Start () {
        buildingLocationType = BuildingLocationType.Disks;
	}
	
    public SatelliteBuilding()
    {
        Debug.Log("SatelliteBuilding constructor");
    }
}
