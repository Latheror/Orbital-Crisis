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
    public float alocatedEnergy;
    public bool hasEnoughEnergy = false;
    public float buildingSpotAngleDeg = 0f;
    public float buildingSpotAngleRad = 0f;
    public int currentTier = 1;
    public GameObject rangeCircle;
    public GameObject rangeIndicator;
    public GameObject rangeIndicatorStart;
    public float range = 100f;
    public bool hasAngleRange = true;
    public float angleRange = 3f;
    public int rangeIndicatorNbPoints = 10;
    public float energyConsumption = 10f;
    public GameObject buildingSpot;
    public List<string> tags;

    public enum BuildingLocationType {Planet, Disks};
    public BuildingLocationType buildingLocationType;

    public Building(){}

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
        if(currentTier < buildingType.maxTier)
        {
            //Debug.Log("Can go to next tier");
            currentTier++;
            ApplyCurrentTierSettings();
            SpaceportInfoPanel.instance.ImportInfo();

            // Refresh info panels, range indicator...
            DisplayRangeIndicator(true);
            BuildingInfoPanel.instance.SetInfo();

            EnergyPanel.instance.UpdateEnergyProductionAndConsumption();
        }
        else
        {
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
