using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Building : MonoBehaviour {

    public BuildingManager.BuildingType buildingType;
    public string buildingName;
    public Color buildingColor;
    public GameObject buildingPrefab;
	public List<ResourcesManager.ResourceAmount> buildingPrice;
    public float alocatedEnergy;
    public bool hasEnoughEnergy = false;
    public float buildingSpotAngle = 0f;
    public int currentTier = 1;
    public GameObject rangeCircle;

    public float currentRequiredEnergy = 10f;

    public enum BuildingLocationType {Planet, Disks};
    public BuildingLocationType buildingLocationType;

    public Building(string name)
    {
        this.buildingName = name;
    }

    public List<ResourcesManager.ResourceAmount> GetUpgradeCostsForNextTier()
    {
        Debug.Log("GetUpgradeCostsForNextTier | Current tier: " + currentTier);
        return buildingType.GetUpgradeCostsForTierNb(currentTier + 1);
    }

    public virtual void BuildingTouched(){

        if (buildingType.hasRange)
        {
            DisplayRangeCircle(true);
        }
    }

    public virtual void BuildingDeselected()
    {
        if (buildingType.hasRange)
        {
            DisplayRangeCircle(false);
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
            Debug.LogError("DisplayRangeCircle: No range circle object !");
        }
    }

}
