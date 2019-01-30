using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MegaCollector : MonoBehaviour {

    public static MegaCollector instance;
    void Awake()
    {
        if (instance != null) { Debug.LogError("More than one MegaCollector in scene !"); return; }
        instance = this;

        InvokeRepeating("UpdateCollectTargets", 0f, 0.5f);
    }

    [Header("Settings")]
    public int collectionStructuresNb = 5;
    public float collectionPointsDistance = 400f;
    public float destroyDistance = 10f;

    public int maxCollectionPointNb = 5;
    public float maxCollectionSpeed = 5f;
    public int startCollectionPointNb = 1;
    public float startCollectionSpeed = 1f;

    public float collectionTimeSpeedFactor = 10f;
    public float energyConsumptionFactor = 100f;

    [Header("Prefabs")]
    public GameObject collectionPointPrefab;

    [Header("Parts")]
    public List<GameObject> collectionPoints;

    [Header("Operation")]
    public bool isUnlocked = false;
    public bool collectionPointsBuilt = false;
    public float power;
    public float energyConsumption;
    public bool hasEnoughEnergy;
    //public bool isActivated;

    public int currentCollectionPointNb = 1;
    public float currentCollectionSpeed = 1;

    void Start()
    {

    }

    private void Update()
    {
        Collect();
    }

    public void Initialize()
    {
        currentCollectionPointNb = startCollectionPointNb;
        currentCollectionSpeed = startCollectionSpeed;

        BuildCollectionPoints();

        ActivateCollectionPoints();
        CalculateEnergyConsumption();

        CollectorControlPanel.instance.ReceiveSettings(currentCollectionSpeed, currentCollectionPointNb, energyConsumption);
    }


    public void BuildCollectionPoints()
    {
        if(! collectionPointsBuilt)
        {
            //DestroyCollectionPoints();

            float stepAngle = (2 * Mathf.PI / collectionStructuresNb);
            //Debug.Log("BuildCollectionPoints | Nb [" + collectionStructuresNb + "] | StepAngle [" + stepAngle + "]");
            for (int i = 0; i < collectionStructuresNb; i++)
            {
                float angle = stepAngle * i;
                float angleDeg = angle * 180 / Mathf.PI;
                Vector3 pos = transform.position + new Vector3(collectionPointsDistance * Mathf.Cos(angle), collectionPointsDistance * Mathf.Sin(angle), 0f);
                GameObject instantiatedCollectionPoint = Instantiate(collectionPointPrefab, pos, Quaternion.Euler(0f, 0f, angleDeg));
                instantiatedCollectionPoint.GetComponent<CollectionPoint>().collectionPointIndex = (i + 1);
                instantiatedCollectionPoint.transform.SetParent(transform);
                collectionPoints.Add(instantiatedCollectionPoint);
            }

            collectionPointsBuilt = true;
            Debug.Log("MegaCollector | CollectionPointsBuilt | Number [" + collectionPoints.Count + "]");
        }
    }

    public void DestroyCollectionPoints()
    {
        foreach (GameObject collectionPoint in collectionPoints)
        {
            Destroy(collectionPoint);
            collectionPoints = new List<GameObject>();
        }
    }

    public void UpdateCollectTargets()
    {
        if(isUnlocked && hasEnoughEnergy)
        {
            //Debug.Log("UpdateCollectTargets");
            foreach (GameObject collectionPoint in collectionPoints)
            {
                CollectionPoint cp = collectionPoint.GetComponent<CollectionPoint>();
                cp.UpdateCollectTarget();
            }
        }
    }

    public void Collect()
    {
        //Debug.Log("MegaCollector | Collect");
        if(isUnlocked && hasEnoughEnergy)
        {
            foreach (GameObject collectionPoint in collectionPoints)
            {
                collectionPoint.GetComponent<CollectionPoint>().Collect();
            }
        }
        else
        {
            foreach (GameObject collectionPoint in collectionPoints)
            {
                collectionPoint.GetComponent<CollectionPoint>().DisableCollectionRay();
            }
        }
    }

    public void Configure(float collectionSpeed, int collectionPointNb)
    {
        Debug.Log("MegaCollector - Configure | CollectionSpeed [" + collectionSpeed + "] | CollectionPointNb [" + collectionPointNb + "]");
        if (collectionSpeed <= maxCollectionSpeed && collectionPointNb <= maxCollectionPointNb)
        {
            currentCollectionSpeed = collectionSpeed;
            currentCollectionPointNb = collectionPointNb;

            CalculateEnergyConsumption();

            UpdateControlPanel();

            ActivateCollectionPoints();
        }
        else
        {
            Debug.LogError("Configure | Settings out of range !");
        }
    }

    public void CalculateEnergyConsumption()
    {
        energyConsumption = Mathf.CeilToInt(currentCollectionPointNb * currentCollectionSpeed * energyConsumptionFactor);
        EnergyPanel.instance.UpdateEnergyProductionAndConsumption();
    }

    public void UpdateControlPanel()
    {
        CollectorControlPanel.instance.ReceiveSettings(currentCollectionSpeed, currentCollectionPointNb, energyConsumption);
    }

    public void ActivateCollectionPoints()
    {
        int collectionPointsToActivate = currentCollectionPointNb;
        Debug.Log("ActivateCollectionPoints | CollectionPointsToActivate [" + collectionPointsToActivate + "] | CollectionPoints Count [" + collectionPoints.Count + "]");
        for (int i = 0; i < collectionPoints.Count; i++)
        {
            if(i < collectionPointsToActivate)
            {
                collectionPoints[i].GetComponent<CollectionPoint>().Activate(true);
            }
            else
            {
                collectionPoints[i].GetComponent<CollectionPoint>().Activate(false);
            }
        }
    }

    public void SetHasEnoughEnergy(bool enough)
    {
        hasEnoughEnergy = enough;
        if(!hasEnoughEnergy)
        {
            ReleaseAllTargets();
        }
    }

    public void ReleaseAllTargets()
    {
        foreach (GameObject cPoint in collectionPoints)
        {
            cPoint.GetComponent<CollectionPoint>().ReleaseTarget();
        }
    }


}
