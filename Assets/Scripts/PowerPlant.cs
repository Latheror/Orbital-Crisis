using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerPlant : Building {

    [Header("Settings")]
    public float baseEnergyProduction = 50f;

    public float effectiveEnergyProduction = 50f;

    public void UpdatePopulationBonusEffectsOnProduction()
    {
        effectiveEnergyProduction = baseEnergyProduction * (1 + populationBonus);

        // TODO: Refresh Building Info panel in case building is selected
    }

}
