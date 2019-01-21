using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SatelliteSolarStation : PowerPlant {

    [Header("Tier 2")]
    public float energyProduction_tier_2 = 100f;

    [Header("Tier 3")]
    public float energyProduction_tier_3 = 150f;

    public override void ApplyCurrentTierSettings()
    {
        Debug.Log("ApplyCurrentTierSettings | SATELLITE SOLAR STATION | CurrentTier: " + currentTier);
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
