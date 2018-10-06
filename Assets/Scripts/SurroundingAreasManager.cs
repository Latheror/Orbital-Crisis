using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SurroundingAreasManager : MonoBehaviour {

    public static SurroundingAreasManager instance;
    void Awake(){ 
        if (instance != null){ Debug.LogError("More than one SurroundingAreasManager in scene !"); return; } instance = this;
    }

    [Header("World")]
    public GameObject mainPlanet;

    [Header("Prefabs")]
    public GameObject satelliteBuildingSlotPrefab;

    [Header("Operation")]
    public int unlockedDisksNb = 1;

    [Header("Disks")]
    public SurroundingDisk[] disks = new SurroundingDisk[3];
    public GameObject firstDisk;
    public GameObject secondDisk;
    public GameObject thirdDisk;

    void Start()
    {
        disks[0] = new SurroundingDisk(firstDisk, 3);
        disks[1] = new SurroundingDisk(secondDisk, 5);
        disks[2] = new SurroundingDisk(thirdDisk, 7);

        BuildDisksBuildingSlots();
        LockAllDisks();
        UnlockDisk(1);
    }

    public void BuildDisksBuildingSlots()
    {
        for (int i = 0; i < disks.Length; i++)
        {
            //Debug.Log("Building Disk " + (i + 1) + " Building Slots");
            BuildDiskBuildingSlots(i);
        }
    }

    public void BuildDiskBuildingSlots(int diskNb)
    {
        float stepAngle = Mathf.PI * 2 / disks[diskNb].buildingSlotsNb;
        float radius = disks[diskNb].diskGO.transform.localScale.x / 2 * 0.85f;

        //Debug.Log("StepAngle: " + stepAngle);

        int nbSlots = disks[diskNb].buildingSlotsNb;

        for (int j = 0; j < nbSlots; j++)
        {
            float angle = stepAngle * j;
            Vector3 pos = new Vector3(Mathf.Cos(angle) * radius, Mathf.Sin(angle) * radius, mainPlanet.transform.position.z);

            GameObject instantiatedSlot = Instantiate(satelliteBuildingSlotPrefab, pos, Quaternion.identity);

            instantiatedSlot.GetComponent<BuildingSlot>().locationType = BuildingManager.BuildingType.BuildingLocationType.Disks;
            instantiatedSlot.GetComponent<BuildingSlot>().SetDefaultColor();
            instantiatedSlot.GetComponent<BuildingSlot>().SetAngle(angle);

            disks[diskNb].diskBuildingSlots.Add(instantiatedSlot);

            instantiatedSlot.transform.SetParent(disks[diskNb].diskGO.transform);
        }
    }

    public void ResetAllSatelliteBuildingSlotsColor()
    {
        for(int i = 0; i < disks.Length; i++)
        {
            foreach (GameObject slot in disks[i].diskBuildingSlots)
            {
                slot.GetComponent<BuildingSlot>().SetDefaultColor();
            }
        }
    }

    // First disk is number 0
    public GameObject FindClosestDiskBuildingSlot(Vector3 pos, int diskNb)
    {
        //Debug.Log("FindClosestDiskBuildingSlot.");
        float minDist = Mathf.Infinity;
        GameObject closestSlot = null;

        SurroundingDisk disk = disks[diskNb];

        foreach (GameObject buildingSlot in disk.diskBuildingSlots)
        {
            float dist = Vector3.Distance(pos, buildingSlot.transform.position);

            if(dist < minDist && !buildingSlot.GetComponent<BuildingSlot>().hasBuilding)
            {
                minDist = dist;
                closestSlot = buildingSlot;
            }
        }

        //Debug.Log("Closest slot found: " + closestSlot + " | Pos: (x=" + closestSlot.transform.position.x + ",y=" + closestSlot.transform.position.y + ")");
        return closestSlot;
    }

    public GameObject FindClosestBuildingSlotInUnlockedDisks(Vector3 pos)
    {
        //Debug.Log("FindClosestDiskBuildingSlot."); 
        float minDist = Mathf.Infinity;
        GameObject closestSlot = null;

        for (int i = 0; i < unlockedDisksNb; i++)
        {
            GameObject closestSpotFromDisk = FindClosestDiskBuildingSlot(pos, i);
            if(closestSpotFromDisk != null && (Vector3.Distance(pos, closestSpotFromDisk.transform.position)) < minDist)
            {
                minDist = (Vector3.Distance(pos, closestSpotFromDisk.transform.position));
                closestSlot = closestSpotFromDisk;
            }
        }

        return closestSlot;
    }

    public void LockAllDisks()
    {
        for (int i = 0; i < (disks.Length); i++)
        {
            disks[i].diskGO.SetActive(false);
            disks[i].unlocked = false;
        }
        unlockedDisksNb = 0;
    }

    public void UnlockNextDisk()
    {
        Debug.Log("UnlockNextDisk | UnlockedDisksNb: " + unlockedDisksNb + " | Disks.Length: " + disks.Length);
        if(unlockedDisksNb < disks.Length){
            UnlockDisk(unlockedDisksNb + 1);
        }
        else
        {
            Debug.Log("No more disks to unlock !");
        }
    }

    public void UnlockDisk(int diskNb)
    {
        disks[diskNb - 1].diskGO.SetActive(true);
        disks[diskNb - 1].unlocked = true;
        unlockedDisksNb++;

        int index = diskNb - 2;
        while(index >= 0)
        {
            //Color newColor = disks[index].diskGO.GetComponent<Renderer>().materials[0].color;
            //newColor.a = 0;
            //disks[index].diskGO.GetComponent<Renderer>().materials[0].color = newColor;
            index--;
        }
    }

    [System.Serializable]
    public class SurroundingDisk
    {
        public GameObject diskGO;
        public List<GameObject> diskBuildingSlots = new List<GameObject>();
        public int buildingSlotsNb;
        public bool unlocked;

        public SurroundingDisk(GameObject diskGameObject, int buildingSlotsAmount)
        {
            diskGO = diskGameObject;
            buildingSlotsNb = buildingSlotsAmount;
            unlocked = false;
        }
    }



}
