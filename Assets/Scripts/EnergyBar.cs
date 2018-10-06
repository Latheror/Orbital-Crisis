using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnergyBar : MonoBehaviour {

    [Header("UI")]
    public GameObject[] energyLevels;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void UpdateEnergyBar()
    {
        float production = EnergyPanel.instance.energyProduction;
        float consumption = EnergyPanel.instance.energyConsumption;
        int nbEnergyLevels = energyLevels.Length;

        if(production > consumption)
        {
            float availableEnergy = (production - consumption);
            float energyRatio = availableEnergy / production;

            int nbBarsToDisplay = Mathf.RoundToInt(nbEnergyLevels * energyRatio);
            HideAllEnergyLevels();
            for (int i = 0; i < nbBarsToDisplay; i++)
            {
                energyLevels[i].SetActive(true);
            }
        }
        else
        {
            foreach (GameObject bar in energyLevels)
            {
                bar.SetActive(false);
            }
        }
    }

    public void HideAllEnergyLevels()
    {
        foreach (GameObject level in energyLevels)
        {
            level.SetActive(false);
        }
    }
}
