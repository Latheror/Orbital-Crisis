using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebrisCollector : MonoBehaviour {

    [Header("Settings")]
    public int resourcesPerUnitOfSize = 10;
    public float operationDistance = 20;
    public float movementSpeed = 20f;
    public float rotationSpeed = 20f;
    public float collectionTime = 0.5f;
    public GameObject shootingPoint;

    [Header("Operation")]
    public GameObject debrisTarget = null;
    public bool debrisIsBeingCollected = false;
    public GameObject homeStation = null;


    // Use this for initialization
    void Start () {
        InvokeRepeating("UpdateTarget", 0f, 0.2f);
        debrisIsBeingCollected = false;
        debrisTarget = null;
	}

    void Update()
    {
        FollowTargetOrRotateAroundStation();
    }
	

    public void UpdateTarget()
    {
        if (GameManager.instance.gameState == GameManager.GameState.Default && !debrisIsBeingCollected)
        {
            Debug.Log("Debris collector | Update Target. No target. | A total of " + DebrisManager.instance.debrisList.Count + " meteors are available.");
            if (DebrisManager.instance.debrisList.Count > 0 || EnemiesManager.instance.enemyWrecks.Count > 0)
            {
                float minDistance = Mathf.Infinity;
                GameObject closestDesbris = null;

                if (DebrisManager.instance.debrisList.Count > 0)
                {
                    foreach (var debris in DebrisManager.instance.debrisList)
                    {
                        float distance = Vector3.Distance(transform.position, debris.transform.position);
                        if (distance < minDistance && !IsTargetAlreadyTaken(debris))
                        {
                            minDistance = distance;
                            closestDesbris = debris;
                        }
                    }
                }
                if (EnemiesManager.instance.enemyWrecks.Count > 0)
                {
                    foreach (var wreck in EnemiesManager.instance.enemyWrecks)
                    {
                        float distance = Vector3.Distance(transform.position, wreck.transform.position);
                        if (distance < minDistance && !IsTargetAlreadyTaken(wreck))
                        {
                            minDistance = distance;
                            closestDesbris = wreck;
                        }
                    }
                }

                debrisTarget = closestDesbris;
                Debug.Log("New target chosen");
            }
            else
            {
                Debug.Log("No target found");
            }
        }
    }

    public bool IsTargetAlreadyTaken(GameObject target)
    {
        bool alreadyTaken = false;
        foreach (GameObject recyclingStation in InfrastructureManager.instance.recyclingStationsList)
        {
            foreach (GameObject collector in recyclingStation.GetComponent<DebrisCollectorStation>().debrisCollectorsList)
            {
                if ((collector != gameObject) && collector.GetComponent<DebrisCollector>().debrisTarget == target)
                {
                    alreadyTaken = true;
                    break;
                }
            }
        }
        return alreadyTaken;
    }

    public void FollowTargetOrRotateAroundStation()
    {
        if (!debrisIsBeingCollected)
        {
            LineRenderer lineRenderer = gameObject.GetComponent<LineRenderer>();
            if ((debrisTarget != null) && (GetDistanceBetweenTargetAndHomeStation() < homeStation.GetComponent<DebrisCollectorStation>().range))
            {
                if (DistanceToTarget() > operationDistance)
                {
                    transform.position = Vector3.MoveTowards(transform.position, debrisTarget.transform.position, Time.deltaTime * movementSpeed);
                    RotateTowardsTarget();
                }
                else
                {
                    lineRenderer.enabled = true;
                    lineRenderer.SetPosition(0, shootingPoint.transform.position);
                    lineRenderer.SetPosition(1, debrisTarget.transform.position);

                    StartCoroutine("CollectDebris");
                }
            }
            else
            {
                lineRenderer.enabled = false;
                ComeBackAroundStationAndRotate();
            }
        }
    }

    public float DistanceToTarget()
    {
        if(debrisTarget != null)
        {
            return Vector3.Distance(debrisTarget.transform.position, transform.position);
        }
        else
        {
            Debug.Log("Using DistanceToTarget without target...");
            return 0;
        }
    }

    public void RotateAroundStation()
    {
        transform.RotateAround(transform.parent.transform.position, Vector3.forward, Time.deltaTime * rotationSpeed);
    }


    IEnumerator CollectDebris() {
        Debug.Log("Collect Debris");
        if(debrisTarget != null)
        {
            Debug.Log("Starting debris collection.");
            debrisIsBeingCollected = true;
            yield return new WaitForSeconds(collectionTime);

            if(debrisTarget != null && debrisTarget.GetComponent<Debris>())  // Meteor
            {
                debrisTarget.GetComponent<Debris>().Collect();
                debrisTarget = null;
            }
            else if (debrisTarget != null && debrisTarget.GetComponent<EnemySpaceship>())    // Spaceship wreck
            {
                debrisTarget.GetComponent<EnemySpaceship>().Collect();
                debrisTarget = null;
            }
            else
            {
                Debug.Log("Debris collection error : type of debris unknown !");
            }
        }

        LineRenderer lineRenderer = gameObject.GetComponent<LineRenderer>();
        lineRenderer.enabled = false;
        debrisIsBeingCollected = false;
        Debug.Log("Debris collection ended.");        
    }

    public void ComeBackAroundStation()
    {
        transform.position = Vector3.MoveTowards(transform.position, transform.parent.transform.position, movementSpeed * Time.deltaTime);
    }

    public void ComeBackAroundStationAndRotate()
    {
        if(Vector3.Distance(transform.position, transform.parent.transform.position) > operationDistance)
        {
            ComeBackAroundStation();
            RotateTowardsStation();
        }
        else
        {
            RotateAroundStation();
        }
    }

    public float GetDistanceBetweenTargetAndHomeStation()
    {
        float distance = Mathf.Infinity;
        if(debrisTarget != null && homeStation != null)
        {
            distance = Vector3.Distance(debrisTarget.transform.position, homeStation.transform.position);
        }
        else
        {
            Debug.Log("Error: Unable to get distance between home station and target, as some of them are not set.");
        }

        return distance;  
    }

    public void RotateTowardsTarget()
    {
        if (debrisTarget != null)
        {
            Vector3 targetDir = debrisTarget.transform.position - transform.position;
            float rotationStep = rotationSpeed * Time.deltaTime;
            Vector3 newDir = Vector3.RotateTowards(transform.forward, targetDir, rotationStep, 0.0f);
            transform.rotation = Quaternion.LookRotation(newDir);
        }
    }

    public void RotateTowardsStation()
    {
        if (debrisTarget != null)
        {
            Vector3 stationDir = homeStation.transform.position - transform.position;
            float rotationStep = rotationSpeed * Time.deltaTime;
            Vector3 newDir = Vector3.RotateTowards(transform.forward, stationDir, rotationStep, 0.0f);
            transform.rotation = Quaternion.LookRotation(newDir);
        }
    }
}
