using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanetExpansionManager : MonoBehaviour {

    [Header("World")]
    public GameObject mainPlanetGO;
    public MainPlanet mainPlanet;

    [Header("Operation")]
    public List<GameObject> buildingSlots;

    [Header("Prefabs")]
    public GameObject buildingSlotPrefab;


    public GameObject buildingSlotsParent;

	// Use this for initialization
	void Start () {
        mainPlanet = mainPlanetGO.GetComponent<MainPlanet>();
        buildingSlots = BuildingSlotManager.instance.allBuildingSlots;
	}


    public void AddBuildingSlot()
    {
        int oldNbSlots = buildingSlots.Count;
        Debug.Log("AddBuildingSlot | There is " + oldNbSlots + " building slots currently.");

        float newStepAngle = 2 * Mathf.PI / (oldNbSlots + 1);
        Debug.Log("AddBuildingSlot | NewStepAngle is " + newStepAngle + ".");
        float radius = mainPlanetGO.transform.localScale.x / 2;

        for (int i = 0; i < oldNbSlots; i++)
        {
            float angle = newStepAngle * i;
            Vector3 newPos = new Vector3(Mathf.Cos(angle) * radius, Mathf.Sin(angle) * radius, transform.position.z);

            buildingSlots[i].transform.position = newPos;
            buildingSlots[i].GetComponent<BuildingSlot>().angleRad = angle;
        }

        // Instantiate a last one
        float lastAngle = newStepAngle * (oldNbSlots);
        Vector3 newSlotPos = new Vector3(Mathf.Cos(lastAngle) * radius, Mathf.Sin(lastAngle) * radius, mainPlanetGO.transform.position.z);

        GameObject newInstantiatedSlot = Instantiate(buildingSlotPrefab, newSlotPos, Quaternion.identity);
        newInstantiatedSlot.transform.SetParent(buildingSlotsParent.transform);

        BuildingSlotManager.instance.groundBuildingSlots.Add(newInstantiatedSlot);
        BuildingSlotManager.instance.allBuildingSlots.Add(newInstantiatedSlot);

        newInstantiatedSlot.GetComponent<BuildingSlot>().SetDefaultColor();
        newInstantiatedSlot.GetComponent<BuildingSlot>().angleRad = lastAngle;
       

    }
   




}
