using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebrisCollectorStation : Building {

    [Header("Settings")]
    public int maxDebrisCollectorNb = 3;
    public float debrisCollectorInstantiationDistance = 10f;
    public float collectionTime = 0.5f;

    [Header("Tier 2")]
    public float collectionTime_tier_2 = 0.4f;
    public float range_tier_2 = 200f;
    public float energyConsumption_tier_2 = 25;

    [Header("Tier 3")]
    public float collectionTime_tier_3 = 0.3f;
    public float range_tier_3 = 300f;
    public float energyConsumption_tier_3 = 40;

    [Header("Operation")]
    public List<GameObject> debrisCollectorsList = new List<GameObject>();

    [Header("Prefabs")]
    public GameObject debrisCollectorPrefab;

    void Start()
    {
        LaunchDebrisCollector();
    }

    public void LaunchDebrisCollector()
    {
        if(debrisCollectorsList.Count < maxDebrisCollectorNb)
        {
            if(debrisCollectorPrefab != null)
            {
                //Debug.Log("Debris Collector has been assigned to station.");
                float randomAngle = Random.Range(0, Mathf.PI * 2);
                Vector3 instantiationDeltaPos = new Vector3(debrisCollectorInstantiationDistance * Mathf.Cos(randomAngle), debrisCollectorInstantiationDistance * Mathf.Sin(randomAngle), 0f);
                Vector3 instantiationPos = transform.position + instantiationDeltaPos;

                GameObject instantiatedDebrisCollector = Instantiate(debrisCollectorPrefab, instantiationPos, Quaternion.identity);
                DebrisCollector debrisC = instantiatedDebrisCollector.GetComponent<DebrisCollector>();
                debrisC.homeStation = gameObject;
                debrisC.collectionTime = (collectionTime * (1 - populationBonus));
                debrisCollectorsList.Add(instantiatedDebrisCollector);
                instantiatedDebrisCollector.transform.SetParent(transform);
            }
            else
            {
                //Debug.Log("Error : Debris Collector Prefab hasn't been assigned to station.");
            }

        }
        else
        {
            Debug.Log("LaunchDebrisCollector | You already have the maximum number of Debris Collectors !");
        }
    }

    public override void ApplyCurrentTierSettings()
    {
        //Debug.Log("ApplyCurrentTierSettings | LASER TURRET | CurrentTier: " + currentTier);
        switch (currentTier)
        {
            case 2:
            {
                range = range_tier_2;
                collectionTime = collectionTime_tier_2;
                energyConsumption = energyConsumption_tier_2;
                break;

            }
            case 3:
            {
                range = range_tier_3;
                collectionTime = collectionTime_tier_2;
                energyConsumption = energyConsumption_tier_3;
                break;
            }
        }

        LaunchDebrisCollector();
        UpdateCollectorsSettings();

    }

    public void UpdateCollectorsSettings()
    {
        foreach (GameObject debrisCollector in debrisCollectorsList)
        {
            debrisCollector.GetComponent<DebrisCollector>().collectionTime = collectionTime;
        }
    }


}
