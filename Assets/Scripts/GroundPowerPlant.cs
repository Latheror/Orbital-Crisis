using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundPowerPlant : PowerPlant {

	// Use this for initialization
	void Start () {
        InitializeEnergyContribution();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public GroundPowerPlant(string name) :  base(name)
    {
        Debug.Log("GroundPowerPlant constructor");
    }
}
