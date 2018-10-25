using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundPowerPlant : PowerPlant {

    [Header("Tier 2")]
    public float energyProduction_tier_2 = 100f;

    [Header("Tier 3")]
    public float energyProduction_tier_3 = 150f;

    // Use this for initialization
    void Start () {

	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public GroundPowerPlant()
    {
        Debug.Log("GroundPowerPlant constructor");
    }

    public override void ApplyCurrentTierSettings()
    {
        Debug.Log("ApplyCurrentTierSettings | GROUND POWER PLANT | CurrentTier: " + currentTier);
        switch (currentTier)
        {
            case 2:
            {
                energyProduction = energyProduction_tier_2;
                break;

            }
            case 3:
            {
                energyProduction = energyProduction_tier_3;
                break;
            }
        }
    }
}
