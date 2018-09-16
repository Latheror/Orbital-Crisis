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

    public enum BuildingLocationType {Planet, Disks};
    public BuildingLocationType buildingLocationType;

    public Building(string name)
    {
        this.buildingName = name;
    }

}
