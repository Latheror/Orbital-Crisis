using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerPlant : Building {

    public float energyProduction = 50f;

    public PowerPlant(string name) :  base(name)
    {
        //Debug.Log("PowerPlant constructor");
    }

	// Use this for initialization
	void Start () {
        InitializeEnergyContribution();
        //buildingLocationType = BuildingLocationType.Planet;
	}
	
	// Update is called once per frame
	void Update () {
		
	}



    public void InitializeEnergyContribution()
    {
        EnergyPanel.instance.IncreaseEnergyProduction(energyProduction);
        EnergyPanel.instance.DistributeEnergy();
    }
}
