using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebrisCollectorStation : Building {

    public GameObject debrisCollectorPrefab;
    public List<GameObject> debrisCollectorsList;
    public int maxDebrisCollectorNb = 3;
    public float debrisCollectorInstantiationDistance = 10f;

    public DebrisCollectorStation(string name) :  base(name)
    {
        Debug.Log("DebrisCollectorStation constructor");
    }



    void Start()
    {
        LaunchDebrisCollector();
    }

    public void LaunchDebrisCollector()
    {
        if(debrisCollectorsList.Count < maxDebrisCollectorNb)
        {
            float randomAngle = Random.Range(0, Mathf.PI * 2);
            Vector3 instantiationDeltaPos = new Vector3(debrisCollectorInstantiationDistance * Mathf.Cos(randomAngle), debrisCollectorInstantiationDistance * Mathf.Sin(randomAngle), 0f);
            Vector3 instantiationPos = transform.position + instantiationDeltaPos;

            GameObject instantiatedDebrisCollector = Instantiate(debrisCollectorPrefab, instantiationPos, Quaternion.identity);
            debrisCollectorsList.Add(instantiatedDebrisCollector);
            instantiatedDebrisCollector.transform.SetParent(transform);
        }
        else
        {
            Debug.Log("LaunchDebrisCollector | You already have the maximum number of Debris Collectors !");
        }
    }

	
}
