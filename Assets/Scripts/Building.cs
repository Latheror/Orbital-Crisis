using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Building : MonoBehaviour {

    [Header("Building Common")]
    public BuildingManager.BuildingType buildingType;
    public string buildingName;
    public Color buildingColor;
    public GameObject buildingPrefab;
    public List<ResourcesManager.ResourceAmount> buildingPrice;
    public List<string> tags;

    [Header("Building Spot")]
    public GameObject buildingSpot;
    public float buildingSpotAngleDeg = 0f;
    public float buildingSpotAngleRad = 0f;

    [Header("Range")]
    public float range = 100f;
    public bool hasAngleRange = true;
    public float angleRange = 3f;
    public int rangeIndicatorNbPoints = 10;
    public GameObject rangeCircle;
    public GameObject rangeIndicator;
    public GameObject rangeIndicatorStart;

    [Header("Energy")]
    public float energyConsumption = 10f;
    public bool powerOn = true;
    public float alocatedEnergy;
    public bool hasEnoughEnergy = false;

    [Header("Level")]
    public int currentTier = 1;
    public bool maxUpgradeLevelReached = false;

    [Header("Bonuses")]
    public float populationBonus = 0f;

    public enum BuildingLocationType {Planet, Disks};
    public BuildingLocationType buildingLocationType;

    [Header("UI")]
    public GameObject powerMissingCanvas;

    public Building(){}

    private void Start()
    {
        maxUpgradeLevelReached = false;
        powerOn = true;
    }

    public List<ResourcesManager.ResourceAmount> GetUpgradeCostsForNextTier()
    {
        //Debug.Log("GetUpgradeCostsForNextTier | Current tier: " + currentTier);
        return buildingType.GetUpgradeCostsForTierNb(currentTier + 1);
    }

    public void BuildingTouched(){

        if (buildingType.hasRange)
        {
            //DisplayRangeCircle(true);
            DisplayRangeIndicator(true);
        }
    }

    public virtual void BuildingDeselected()
    {
        if (buildingType.hasRange)
        {
            //DisplayRangeCircle(false);
            DisplayRangeIndicator(false);
        }
    }

    public void DisplayRangeCircle(bool display)
    {
        if(rangeCircle != null)
        {
            rangeCircle.SetActive(display);
        }
        else
        {
            Debug.Log("DisplayRangeCircle: No range circle object !");
        }
    }

    public void DisplayRangeIndicator(bool display)
    {
        if (rangeIndicator != null && rangeIndicatorStart != null)
        {
            LineRenderer lr = rangeIndicator.GetComponent<LineRenderer>();
            if (display)
            {
                float radius = range;
                //Debug.Log("DisplayRangeIndicator | Radius: " + radius + " | BuildingSpotAngleRad: " + buildingSpotAngleRad + " | RangeIndicatorNbPoints: " + rangeIndicatorNbPoints);

                lr.SetColors(buildingColor, buildingColor);

                if (hasAngleRange)  // Ground Towers
                {
                    float totalAngle = angleRange;
                    float anglePortion = totalAngle / rangeIndicatorNbPoints;

                    rangeIndicator.GetComponent<LineRenderer>().positionCount = rangeIndicatorNbPoints + 3;

                    // Set Line Renderer points
                    rangeIndicator.GetComponent<LineRenderer>().SetPosition(0, rangeIndicatorStart.transform.position);     // Origin

                    for (int i = 0; i <= rangeIndicatorNbPoints; i++)
                    {
                        float angle_i = (i * anglePortion) - (totalAngle / 2) + buildingSpotAngleRad;
                        //Debug.Log("Angle [" + i + "] : " + angle_i);
                        Vector3 pos_i = new Vector3(gameObject.transform.position.x + radius * Mathf.Cos(angle_i), gameObject.transform.position.y + radius * Mathf.Sin(angle_i), gameObject.transform.position.z);
                        rangeIndicator.GetComponent<LineRenderer>().SetPosition(i + 1, pos_i);
                    }

                    rangeIndicator.GetComponent<LineRenderer>().SetPosition(rangeIndicatorNbPoints + 2, rangeIndicatorStart.transform.position);     // End

                    lr.enabled = true;
                } 
                else    // Satellites
                {
                    float totalAngle = Mathf.PI * 2;
                    float anglePortion = totalAngle / rangeIndicatorNbPoints;

                    rangeIndicator.GetComponent<LineRenderer>().positionCount = rangeIndicatorNbPoints + 1;

                    for (int i = 0; i <= rangeIndicatorNbPoints; i++)
                    {
                        float angle_i = (i * anglePortion);
                        //Debug.Log("Angle [" + i + "] : " + angle_i);
                        Vector3 pos_i = new Vector3(gameObject.transform.position.x + radius * Mathf.Cos(angle_i), gameObject.transform.position.y + radius * Mathf.Sin(angle_i), gameObject.transform.position.z);
                        rangeIndicator.GetComponent<LineRenderer>().SetPosition(i, pos_i);
                    }

                    lr.enabled = true;
                }
            }
            else
            {
                lr.enabled = false;
            }
        }
        else
        {
            Debug.Log("Can't display range indicator. Objects not referenced.");
        }
    }

    public void UpgradeToNextTier()
    {
        if (currentTier < buildingType.maxTier)
        {
            //Debug.Log("Can go to next tier");
            currentTier++;
            ApplyCurrentTierSettings();

            // Refresh info panels, range indicator...
            DisplayRangeIndicator(true);

            maxUpgradeLevelReached = (currentTier < buildingType.maxTier) ? false : true;

            BuildingInfoPanel.instance.SetInfo();

            EnergyPanel.instance.UpdateEnergyProductionAndConsumption();

            if (buildingType.name == "Spaceport")
            {
                SpaceportInfoPanel.instance.ImportInfo();
            }
        }
        else
        {
            maxUpgradeLevelReached = (currentTier < buildingType.maxTier) ? false : true;
            //Debug.Log("Max tier already reached !");
        }
    }

    public void UpgradeToTier(int tier)
    {
        if (tier <= buildingType.maxTier)
        {
            Debug.Log("Upgrading to tier [" + tier + "]");
            currentTier = tier;
            ApplyCurrentTierSettings();

            EnergyPanel.instance.UpdateEnergyProductionAndConsumption();
        }
        else
        {
            Debug.Log("Can't upgrade to tier [" + tier + "], max is [" + buildingType.maxTier + "]");
        }

        maxUpgradeLevelReached = (currentTier < buildingType.maxTier) ? false : true;
    }

    public bool HasTag(string searchedTag)
    {
        bool hasTag = false;
        foreach (string tag in tags)
        {
            if(tag == searchedTag)
            {
                hasTag = true;
                break;
            }
        }
        return hasTag;
    }

    public virtual void ApplyCurrentTierSettings(){}

    public virtual void DestroyBuildingSpecificActions(){}

    public float GetBuildingStatValue(BuildingStat buildingStat)
    {
        float statValue = 0;

        switch (buildingStat.statType)
        {
            case BuildingStat.StatType.energyConsumption:
            {
                statValue = energyConsumption;
                break;
            }
            case BuildingStat.StatType.damagePower:
            {
                if(gameObject.GetComponent<Turret>() != null)
                {
                    statValue = gameObject.GetComponent<Turret>().power;
                }
                else if (gameObject.GetComponent<ShockSatellite>() != null)
                {
                    statValue = gameObject.GetComponent<ShockSatellite>().damagePower;
                }
                else if (gameObject.GetComponent<StormSatellite>() != null)
                {
                    statValue = gameObject.GetComponent<StormSatellite>().damagePower;
                }
                break;
            }
            case BuildingStat.StatType.range:
            {
                statValue = range;
                break;
            }
            case BuildingStat.StatType.energyProduction:
            {
                if (gameObject.GetComponent<PowerPlant>() != null)
                {
                    statValue = gameObject.GetComponent<PowerPlant>().effectiveEnergyProduction;    // Changed from base to effectiveEnergyProduction, to take population bonus into account
                }
                break;
            }
            case BuildingStat.StatType.freezingPower:
            {
                if (gameObject.GetComponent<FreezingTurret>() != null)
                {
                    statValue = gameObject.GetComponent<FreezingTurret>().freezingFactor;
                }
                break;
            }
            case BuildingStat.StatType.healingPower:
            {
                if (gameObject.GetComponent<HealingTurret>() != null)
                {
                    statValue = gameObject.GetComponent<HealingTurret>().healingPower;
                }
                break;
            }
            case BuildingStat.StatType.miningSpeed:
            {
                if (gameObject.GetComponent<MineBuilding>() != null)
                {
                    statValue = gameObject.GetComponent<MineBuilding>().productionDelay;
                }
                break;
            }
        }

        return statValue;
    }

    public void PowerSwitch()
    {
        powerOn = !powerOn;
        Debug.Log("Switch power to: " + ((powerOn)?"On":"Off"));
        EnergyPanel.instance.UpdateEnergyProductionAndConsumption();
        BuildingInfoPanel.instance.SetInfo();
    }

    public virtual void SetHasEnoughEnergy(bool enough)
    {
        hasEnoughEnergy = enough;
        if(powerMissingCanvas != null)
        {
            powerMissingCanvas.SetActive(! enough);
        }
    }

    public class BuildingStat
    {
        public enum StatType { energyConsumption, range, energyProduction, damagePower, freezingPower, healingPower, miningSpeed };

        public StatType statType;
        public Sprite statImage;

        public BuildingStat(StatType statType)
        {
            this.statType = statType;
            string statsImagesPath = "Images/Stats/";
            switch(statType)
            {
                case StatType.damagePower:
                {
                    statImage = Resources.Load<Sprite>(statsImagesPath + "laser_beam");
                    break;
                }
                case StatType.energyConsumption:
                {
                    statImage = Resources.Load<Sprite>(statsImagesPath + "power_minus");
                    break;
                }
                case StatType.energyProduction:
                {
                    statImage = Resources.Load<Sprite>(statsImagesPath + "power_plus");
                    break;
                }
                case StatType.freezingPower:
                {
                    statImage = Resources.Load<Sprite>(statsImagesPath + "snowflake");
                    break;
                }
                case StatType.healingPower:
                {
                    statImage = Resources.Load<Sprite>(statsImagesPath + "green_cross");
                    break;
                }
                case StatType.range:
                {
                    statImage = Resources.Load<Sprite>(statsImagesPath + "range_indicator");
                    break;
                }
                case StatType.miningSpeed:
                {
                    statImage = Resources.Load<Sprite>(statsImagesPath + "gear");
                    break;
                }
            }
        }
    }

    public class StatInfo
    {

        public StatInfo()
        {

        }
    }

    [Serializable]
    public class BuildingData
    {
        public int buildingTypeID;
        public int buildingSlotID;
        public int tier;

        public BuildingData(Building b)
        {
            buildingTypeID = b.buildingType.id;
            buildingSlotID = b.buildingSpot.GetComponent<BuildingSlot>().id;
            tier = b.currentTier;
        }
    }
}
