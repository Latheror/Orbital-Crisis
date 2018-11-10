using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingSlotManager : MonoBehaviour {

    public static BuildingSlotManager instance;
    void Awake()
    {
        if (instance != null) { Debug.LogError("More than one BuildingSlotManager in scene !"); return; }
        instance = this;
    }

    public GameObject mainPlanet;

    public List<GameObject> groundBuildingSlots;
    public List<GameObject> orbitalBuildingSlots;
    public List<GameObject> allBuildingSlots;

    public GameObject buildingSlotsParent;
    public GameObject buildingSlotPrefab;
    public int nbStartBuildingSlots = 10;


    void Start () {
		
	}
	
	void Update () {
		
	}

    public GameObject GetBuildingSlotByID(int id)
    {
        GameObject buildingSlot = null;
        foreach (GameObject slot in allBuildingSlots)
        {
            if(slot.GetComponent<BuildingSlot>().id == id)
            {
                buildingSlot = slot;
                break;
            }
        }
        return buildingSlot;
    }

    public void BuildGroundBuildingSlots()
    {
        Debug.Log("Building building slots.");
        float stepAngle = 2 * Mathf.PI / nbStartBuildingSlots;
        float radius = mainPlanet.transform.localScale.x / 2;

        //Debug.Log("StepAngle: " + stepAngle + " | Rayon: " + radius);

        for (int i = 0; i < nbStartBuildingSlots; i++)
        {
            float angle = stepAngle * i;
            Vector3 pos = new Vector3(Mathf.Cos(angle) * radius, Mathf.Sin(angle) * radius, mainPlanet.transform.position.z);

            GameObject instantiatedSlot = Instantiate(buildingSlotPrefab, pos, Quaternion.identity);
            instantiatedSlot.transform.SetParent(buildingSlotsParent.transform);

            groundBuildingSlots.Add(instantiatedSlot);
            allBuildingSlots.Add(instantiatedSlot);

            instantiatedSlot.GetComponent<BuildingSlot>().id = (100 + i);
            instantiatedSlot.GetComponent<BuildingSlot>().locationType = BuildingManager.BuildingType.BuildingLocationType.Planet;
            instantiatedSlot.GetComponent<BuildingSlot>().SetDefaultColor();
            instantiatedSlot.GetComponent<BuildingSlot>().angleRad = stepAngle * i;
        }

        ResetAllBuildingSlotsColor();
    }

    public void ResetAllBuildingSlotsColor()
    {
        foreach (GameObject slot in allBuildingSlots)
        {
            slot.GetComponent<BuildingSlot>().SetDefaultColor();
        }
    }

    public GameObject FindGroundClosestBuildingSlot(Vector3 pos)
    {
        //Debug.Log("FindClosestBuildingSlot.");
        float minDist = Mathf.Infinity;
        GameObject closestSlot = null;

        foreach (GameObject buildingSlot in groundBuildingSlots)
        {
            float dist = Vector3.Distance(pos, buildingSlot.transform.position);

            if (dist < minDist && !buildingSlot.GetComponent<BuildingSlot>().hasBuilding)
            {
                minDist = dist;
                closestSlot = buildingSlot;
            }
        }

        //Debug.Log("Closest slot found: " + closestSlot + " | Pos: (x=" + closestSlot.transform.position.x + ",y=" + closestSlot.transform.position.y + ")");
        return closestSlot;
    }
}
