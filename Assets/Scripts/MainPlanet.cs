using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainPlanet : MonoBehaviour {

    public GameObject mainPlanetEmpty;

    [Header("General")]
    public string planetName = "MainPlanet";
    public float size;
    public int nbStartBuildingSlots = 10;

    [Header("Buildings")]
    public List<GameObject> buildingSlotList;
    public GameObject buildingSlotsParent;
    public GameObject buildingSlotPrefab;
    public List<GameObject> surroundingLevels;
    public int currentSurroundingLevelsShown;

    void Start()
    {
        size = transform.localScale.x;

        BuildBuildingSlots();

        currentSurroundingLevelsShown = 0;
        //InvokeRepeating("AnimateSurroundingLevels", 0f, 0.5f);
    }


    void OnTriggerEnter(Collider other)
    {
        //Debug.Log("Planet collided with : " + other.gameObject.GetComponent<Meteor>());

        if(other.gameObject.tag == "meteor")
        {
            //Debug.Log("Planet hit by a meteor.");
            InfoManager.instance.IncrementMeteorCollisionsValue();
            MeteorsManager.instance.DeleteMeteor(other.gameObject);
        }
    }

    void OnTriggerStay(Collider other)
    {
        //Debug.Log("Planet collided with : " + other.gameObject.GetComponent<Meteor>());
        //InfoManager.instance.IncrementMeteorCollisionsValue();
    }

    public void IncreasePlanetSize()
    {
        Debug.Log("Increasing Planet Size.");
        mainPlanetEmpty.transform.localScale *= 1.1f;
        Camera.main.fieldOfView *= 1.1f;
    }

    public void DecreasePlanetSize()
    {
        Debug.Log("Decreasing Planet Size.");
        mainPlanetEmpty.transform.localScale *= 0.9f;
        Camera.main.fieldOfView *= 0.9f;
    }

    public void BuildBuildingSlots()
    {
        Debug.Log("Building building slots.");
        float stepAngle = 2*Mathf.PI / nbStartBuildingSlots;
        float radius = gameObject.transform.localScale.x / 2;

        //Debug.Log("StepAngle: " + stepAngle + " | Rayon: " + radius);

        for (int i = 0; i < nbStartBuildingSlots; i++)
        {
            float angle = stepAngle * i;
            Vector3 pos = new Vector3(Mathf.Cos(angle) * radius, Mathf.Sin(angle) * radius, transform.position.z);

            GameObject instantiatedSlot = Instantiate(buildingSlotPrefab, pos, Quaternion.identity);
            instantiatedSlot.transform.SetParent(buildingSlotsParent.transform);

            buildingSlotList.Add(instantiatedSlot);

            instantiatedSlot.GetComponent<BuildingSlot>().SetDefaultColor();
            instantiatedSlot.GetComponent<BuildingSlot>().angle = stepAngle * i;
        }

        ResetAllBuildingSlotsColor();
    }


    public void ShowUpToSurroundingLevel(int nb)
    {
        if(nb >= 0)
        {
            if(nb == 0)
            {
                foreach (var level in surroundingLevels)
                {
                    level.SetActive(false);
                }
            }else
            {
                for (int i = 0; i < nb; i++) // < or <= ?
                {
                    surroundingLevels[i].SetActive(true);
                }
                for (int j = nb + 1; j < surroundingLevels.Count; j++)
                {
                    surroundingLevels[j].SetActive(false);
                }
            }
        }
    }

    public void AnimateSurroundingLevels()
    {
        int nbLevels = surroundingLevels.Count;
        //Debug.Log("Animate Surrounding Levels | Nb levels: " + nbLevels + " | Current level: " + currentSurroundingLevelsShown);

        ShowUpToSurroundingLevel(currentSurroundingLevelsShown);

        currentSurroundingLevelsShown = (currentSurroundingLevelsShown + 1) % (nbLevels + 1);
    }

    public GameObject FindClosestBuildingSlot(Vector3 pos)
    {
        Debug.Log("FindClosestBuildingSlot.");
        float minDist = Mathf.Infinity;
        GameObject closestSlot = null;

        foreach (GameObject buildingSlot in buildingSlotList)
        {
            float dist = Vector3.Distance(pos, buildingSlot.transform.position);

            if(dist < minDist)
            {
                minDist = dist;
                closestSlot = buildingSlot;
            }
        }

        Debug.Log("Closest slot found: " + closestSlot + " | Pos: (x=" + closestSlot.transform.position.x + ",y=" + closestSlot.transform.position.y + ")");
        return closestSlot;
    }

    public void ResetAllBuildingSlotsColor()
    {
        foreach (GameObject slot in buildingSlotList)
        {
            slot.GetComponent<BuildingSlot>().SetDefaultColor();
        }
    }
}
