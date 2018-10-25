using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerPlant : Building {

    [Header("Settings")]
    public float energyProduction = 50f;

    public PowerPlant()
    {
        //Debug.Log("PowerPlant constructor");
    }

	// Use this for initialization
	void Start () {
        //buildingLocationType = BuildingLocationType.Planet;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

}
