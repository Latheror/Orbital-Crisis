using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SatelliteSolarStation : PowerPlant {

    //public float energyProduction = 10f;

	// Use this for initialization
	void Start () {
        InitializeEnergyContribution();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public SatelliteSolarStation(string name) :  base(name)
    {
        //Debug.Log("SatelliteSolarStation constructor");
        buildingLocationType = BuildingLocationType.Disks;
    }
}
