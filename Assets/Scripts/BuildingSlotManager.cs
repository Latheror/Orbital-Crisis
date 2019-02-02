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

    [Header("Operation")]
    public GameObject mainPlanet;
    public List<GameObject> groundBuildingSlots;
    public List<GameObject> orbitalBuildingSlots;
    public List<GameObject> allBuildingSlots;

    [Header("Utility")]
    public GameObject buildingSlotsParent;

    [Header("Prefabs")]
    public GameObject buildingSlotPrefab;

    [Header("Settings")]
    public int nbStartBuildingSlots = 10;

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

            BuildingSlot bs = instantiatedSlot.GetComponent<BuildingSlot>();
            bs.id = (100 + i);
            bs.locationType = BuildingManager.BuildingType.BuildingLocationType.Planet;
            bs.SetDefaultColor();
            bs.angleRad = stepAngle * i;
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
            float dist_squared = (pos - buildingSlot.transform.position).sqrMagnitude;

            if (dist_squared < minDist && !buildingSlot.GetComponent<BuildingSlot>().hasBuilding)
            {
                minDist = dist_squared;
                closestSlot = buildingSlot;
            }
        }

        //Debug.Log("Closest slot found: " + closestSlot + " | Pos: (x=" + closestSlot.transform.position.x + ",y=" + closestSlot.transform.position.y + ")");
        return closestSlot;
    }
}
