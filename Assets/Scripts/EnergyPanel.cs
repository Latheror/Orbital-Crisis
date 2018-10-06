using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class EnergyPanel : MonoBehaviour {

    public static EnergyPanel instance;
    void Awake(){ 
        if (instance != null){ Debug.LogError("More than one EnergyPanel in scene !"); return; } instance = this;
    }

    [Header("Settings")]
    public float energyProduction;
    public float energyConsumption;

    [Header("UI")]
    public TextMeshProUGUI energyProductionText;
    public TextMeshProUGUI energyConsumptionText;
    public GameObject energyBar;

	// Use this for initialization
	void Start () {
        InitializeEnergyLevels();
	}

    public void SetEnergyProduction(float production)
    {
        energyProduction = production;
        UpdateEnergyProductionDisplay();
    }

    public void SetEnergyConsumption(float consumptioon)
    {
        energyConsumption = consumptioon;
        UpdateEnergyConsumptionDisplay();
    }

    public void IncreaseEnergyProduction(float deltaProd)
    {
        energyProduction += deltaProd;
        UpdateEnergyProductionDisplay();
    }

    public void IncreaseEnergyConsumption(float deltaProd)
    {
        energyConsumption += deltaProd;
        UpdateEnergyConsumptionDisplay();
    }

    public void UpdateEnergyProductionDisplay()
    {
        energyProductionText.text = energyProduction.ToString();
        EnergyConsumptionColorIndication();
        energyBar.GetComponent<EnergyBar>().UpdateEnergyBar();
    }

    public void UpdateEnergyConsumptionDisplay()
    {
        energyConsumptionText.text = energyConsumption.ToString();
        EnergyConsumptionColorIndication();
        energyBar.GetComponent<EnergyBar>().UpdateEnergyBar();
    }

    public void UpdateEnergyLevels()
    {
        UpdateEnergyProductionDisplay();
        UpdateEnergyConsumptionDisplay();
    }

    public void InitializeEnergyLevels()
    {
        SetEnergyProduction(0);
        SetEnergyConsumption(0);
    }

    public void EnergyConsumptionColorIndication()
    {
        if(energyConsumption > energyProduction || energyProduction == 0)
        {
            // We don't have enough energy
            energyConsumptionText.color = Color.red;
        }
        else
        {
            energyConsumptionText.color = Color.green;
        }
    }


    public void UpdateEnergyNeedsDisplay()
    {
        float totalEnergyNeeded = 0f;

        foreach (GameObject building in BuildingManager.instance.buildingList)
        {
            totalEnergyNeeded += building.GetComponent<Building>().buildingType.requiredEnergy;
        }

        SetEnergyConsumption(totalEnergyNeeded);
    }

    public void DistributeEnergy()
    {
        float totalEnergyToDistribute = energyProduction;

        foreach (GameObject building in BuildingManager.instance.buildingList)
        {
            Building b = building.GetComponent<Building>();
            if(totalEnergyToDistribute < b.buildingType.requiredEnergy)
            {
                // We don't have enough energy to satisfy this building's needs
                b.alocatedEnergy = totalEnergyToDistribute;
                b.hasEnoughEnergy = false;
                totalEnergyToDistribute = 0;
            }
            else
            {
                b.alocatedEnergy = b.buildingType.requiredEnergy;
                b.hasEnoughEnergy = true;
                totalEnergyToDistribute -= b.buildingType.requiredEnergy;
            }
        }
    }
}
