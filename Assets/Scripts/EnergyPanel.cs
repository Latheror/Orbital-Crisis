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
        energyProductionText.text = Mathf.RoundToInt(energyProduction).ToString();
        EnergyConsumptionColorIndication();
        energyBar.GetComponent<EnergyBar>().UpdateEnergyBar();
    }

    public void UpdateEnergyConsumptionDisplay()
    {
        energyConsumptionText.text = Mathf.RoundToInt(energyConsumption).ToString();
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

        // Regular Buildings
        foreach (GameObject building in BuildingManager.instance.buildingList)
        {
            Building b = building.GetComponent<Building>();

            if(b.powerOn)
            {
                if (totalEnergyToDistribute < b.energyConsumption)
                {
                    // We don't have enough energy to satisfy this building's needs
                    b.alocatedEnergy = totalEnergyToDistribute;
                    b.SetHasEnoughEnergy(false);
                    totalEnergyToDistribute = 0;
                }
                else
                {
                    b.alocatedEnergy = b.energyConsumption;
                    b.SetHasEnoughEnergy(true);
                    totalEnergyToDistribute -= b.energyConsumption;
                }
            }
            else
            {
                b.alocatedEnergy = 0;
                b.SetHasEnoughEnergy(false);
            }
        }

        // MegaStructures
        // Planetary Shield
        if(PlanetaryShield.instance != null && PlanetaryShield.instance.isUnlocked)
        {
            if(totalEnergyToDistribute > PlanetaryShield.instance.energyConsumption && (PlanetaryShield.instance.energyConsumption != 0))
            {
                PlanetaryShield.instance.hasEnoughEnergy = true;
                totalEnergyToDistribute -= PlanetaryShield.instance.energyConsumption;
            }
            else
            {
                PlanetaryShield.instance.hasEnoughEnergy = false;
            }
        }

        // Mega Collector
        if (MegaCollector.instance != null && MegaCollector.instance.isUnlocked)
        {
            if (totalEnergyToDistribute > MegaCollector.instance.energyConsumption && (MegaCollector.instance.energyConsumption != 0))
            {
                MegaCollector.instance.SetHasEnoughEnergy(true);
                totalEnergyToDistribute -= MegaCollector.instance.energyConsumption;
            }
            else
            {
                MegaCollector.instance.SetHasEnoughEnergy(false);
            }
        }
    }

    public void UpdateEnergyProductionAndConsumption()
    {
        float totalEnergyProduction = 0;
        float totalEnergyConsumption = 0;

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
                    totalEnergyConsumption += building.GetComponent<Building>().energyConsumption;
                }
            } 
        }

        // MegaStructures
        // Planetary Shield
        if (PlanetaryShield.instance != null && PlanetaryShield.instance.isUnlocked)
        {
            totalEnergyConsumption += PlanetaryShield.instance.energyConsumption;
        }

        // Mega Collector
        if (MegaCollector.instance != null && MegaCollector.instance.isUnlocked)
        {
            totalEnergyConsumption += MegaCollector.instance.energyConsumption;
        }

        // Dyson Sphere
        if (DysonSphere.instance != null && DysonSphere.instance.isUnlocked && DysonSphere.instance.isActivated)
        {
            totalEnergyProduction += DysonSphere.instance.currentEnergyProduction;
        }

        SetEnergyProduction(totalEnergyProduction);
        SetEnergyConsumption(totalEnergyConsumption);

        DistributeEnergy();

        UpdateEnergyLevels();
    }
}
