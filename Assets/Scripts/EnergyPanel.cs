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


    public void DistributeEnergy()
    {
        float totalEnergyToDistribute = energyProduction;

        foreach (GameObject building in BuildingManager.instance.buildingList)
        {
            Building b = building.GetComponent<Building>();

            if(b.powerOn)
            {
                if (totalEnergyToDistribute < b.energyConsumption)
                {
                    // We don't have enough energy to satisfy this building's needs
                    b.alocatedEnergy = totalEnergyToDistribute;
                    b.hasEnoughEnergy = false;
                    totalEnergyToDistribute = 0;
                }
                else
                {
                    b.alocatedEnergy = b.energyConsumption;
                    b.hasEnoughEnergy = true;
                    totalEnergyToDistribute -= b.energyConsumption;
                }
            }
            else
            {
                b.alocatedEnergy = 0;
                b.hasEnoughEnergy = false;
            }
        }
    }

    public void UpdateEnergyProductionAndConsumption()
    {
        float totalEnergyProduction = 0;
        float TotalEnergyConsumption = 0;

        foreach (GameObject building in BuildingManager.instance.buildingList)
        {
            if (building.GetComponent<Building>().powerOn)
            {
                if (building.GetComponent<Building>().buildingType.producesEnergy)   // Produces energy
                {
                    totalEnergyProduction += building.GetComponent<PowerPlant>().energyProduction;
                }
                else    // Consumes energy
                {
                    TotalEnergyConsumption += building.GetComponent<Building>().energyConsumption;
                }
            } 
        }

        SetEnergyProduction(totalEnergyProduction);
        SetEnergyConsumption(TotalEnergyConsumption);

        DistributeEnergy();

        UpdateEnergyLevels();
    }
}
