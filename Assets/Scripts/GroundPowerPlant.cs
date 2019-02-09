using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundPowerPlant : PowerPlant {

    [Header("Tier 2")]
    public float energyProduction_tier_2 = 100f;

    [Header("Tier 3")]
    public float energyProduction_tier_3 = 150f;

    public override void ApplyCurrentTierSettings()
    {
        //Debug.Log("ApplyCurrentTierSettings | GROUND POWER PLANT | CurrentTier: " + currentTier);
        switch (currentTier)
        {
            case 2:
            {
                baseEnergyProduction = energyProduction_tier_2;
                effectiveEnergyProduction = baseEnergyProduction;
                break;

            }
            case 3:
            {
                baseEnergyProduction = energyProduction_tier_3;
                effectiveEnergyProduction = baseEnergyProduction;
                break;
            }
        }
    }
}
