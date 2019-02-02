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
        debrisTarget = null;
	}

    void Update()
    {
        FollowTargetOrRotateAroundStation();
    }
	

    public void UpdateTarget()
    {
        if (GameManager.instance.gameState == GameManager.GameState.Default && homeStation != null && homeStation.GetComponent<DebrisCollectorStation>().hasEnoughEnergy)
        {
            if (!debrisIsBeingCollected)
            {
                bool needNewTarget = false;
                if (debrisTarget == null)
                {
                    needNewTarget = true;
                }
                else
                {
                    // We already have a target
                    // Check if it's in range of the base
                    float distToBase_squared = (debrisTarget.transform.position - homeStation.transform.position).sqrMagnitude;
                    float baseStationRange_squared = Mathf.Pow((homeStation.GetComponent<DebrisCollectorStation>().range),2);

                    bool isInBaseRange = (distToBase_squared <= baseStationRange_squared);
                   
                    if(! isInBaseRange)
                    {
                        needNewTarget = true;
                        FreeTargetDebris();
                    }
                }

                if(needNewTarget)    // Get a new target
                {
                    //Debug.Log("Debris collector | Update Target. No target. | A total of " + DebrisManager.instance.debrisList.Count + " meteors are available.");
                    if (DebrisManager.instance.debrisList.Count > 0 || EnemiesManager.instance.enemyWrecks.Count > 0)
                    {
                        float range = homeStation.GetComponent<DebrisCollectorStation>().range;
                        float minDistance_squared = Mathf.Infinity;
                        GameObject closestDesbris = null;

                        if (DebrisManager.instance.debrisList.Count > 0)
                        {
                            foreach (var debris in DebrisManager.instance.debrisList)
                            {
                                float distance_squared = (debris.transform.position - transform.position).sqrMagnitude;
                                if (distance_squared < minDistance_squared && !IsTargetAlreadyTaken(debris))
                                {
                                    minDistance_squared = distance_squared;
                                    closestDesbris = debris;
                                }
                            }
                        }
                        if (EnemiesManager.instance.enemyWrecks.Count > 0)
                        {
                            foreach (var wreck in EnemiesManager.instance.enemyWrecks)
                            {
                                if (wreck != null)
                                {
                                    float distance_squared = (wreck.transform.position - transform.position).sqrMagnitude;
                                    if (distance_squared < minDistance_squared && !IsTargetAlreadyTaken(wreck))
                                    {
                                        minDistance_squared = distance_squared;
                                        closestDesbris = wreck;
                                    }
                                }
                            }
                        }

                        if(closestDesbris != null)
                        {
                            if (closestDesbris.GetComponent<Debris>() != null)
                            {
                                closestDesbris.GetComponent<Debris>().isBeingTargetedByCollector = true;
                            }
                            else if (closestDesbris.GetComponent<EnemySpaceship>() != null)
                            {
                                closestDesbris.GetComponent<EnemySpaceship>().isBeingTargetedByCollector = true;
                            }

                            debrisTarget = closestDesbris;
                            //Debug.Log("New target chosen");
                        }
                    }
                    else
                    {
                        //Debug.Log("No target found");
                    }
                }
            }
            else
            {
                // Make sure we are not stuck in this state
                if(debrisTarget == null)
                {
                    debrisIsBeingCollected = false;
                }
            }
        }
    }

    public bool IsTargetAlreadyTaken(GameObject target)
    {
        bool alreadyTaken = false;
        if(target.GetComponent<Debris>() != null)
        {
            alreadyTaken = (target.GetComponent<Debris>().isBeingTargetedByCollector || target.GetComponent<Debris>().isBeingCollected);
        }
        else if(target.GetComponent<EnemySpaceship>() != null)
        {
            alreadyTaken = (target.GetComponent<EnemySpaceship>().isBeingCollected || target.GetComponent<EnemySpaceship>().isBeingTargetedByCollector);
        }

        return alreadyTaken;
    }

    public void FollowTargetOrRotateAroundStation()
    {
        if (!debrisIsBeingCollected)
        {
            LineRenderer lineRenderer = gameObject.GetComponent<LineRenderer>();
            float homeStationRange = homeStation.GetComponent<DebrisCollectorStation>().range;
            if ((debrisTarget != null) && (GetDistanceSquaredBetweenTargetAndHomeStation() < homeStationRange* homeStationRange) && homeStation.GetComponent<DebrisCollectorStation>().hasEnoughEnergy)
            {
                if (DistanceSquaredToTarget() > operationDistance*operationDistance)
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
    
    // -- Use DistanceSquaredToTarget() instead
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

    public float DistanceSquaredToTarget()
    {
        if (debrisTarget != null)
        {
            return (debrisTarget.transform.position - transform.position).sqrMagnitude;
        }
        else
        {
            Debug.Log("Using DistanceSquaredToTarget without target...");
            return 0;
        }
    }

    public void RotateAroundStation()
    {
        transform.RotateAround(transform.parent.transform.position, Vector3.forward, Time.deltaTime * rotationSpeed);
    }


    IEnumerator CollectDebris() {
        //Debug.Log("Collect Debris");
        if(debrisTarget != null)
        {
            //Debug.Log("Starting debris collection.");
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
                //Debug.Log("Debris collection error : type of debris unknown !");
            }
        }

        LineRenderer lineRenderer = gameObject.GetComponent<LineRenderer>();
        lineRenderer.enabled = false;
        debrisIsBeingCollected = false;
        //Debug.Log("Debris collection ended.");        
    }

    public void ComeBackAroundStation()
    {
        transform.position = Vector3.MoveTowards(transform.position, transform.parent.transform.position, movementSpeed * Time.deltaTime);
    }

    public void ComeBackAroundStationAndRotate()
    {
        // Check if close enough from home station 
        if((transform.position - homeStation.transform.position).sqrMagnitude > operationDistance * operationDistance)
        {
            ComeBackAroundStation();
            RotateTowardsStation();
        }
        else
        {
            RotateAroundStation();
        }
    }

    // Use GetDistanceSquaredBetweenTargetAndHomeStation instead
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

    public float GetDistanceSquaredBetweenTargetAndHomeStation()
    {
        float distance_squared = Mathf.Infinity;
        if (debrisTarget != null && homeStation != null)
        {
            distance_squared = (debrisTarget.transform.position - homeStation.transform.position).sqrMagnitude;
        }
        else
        {
            Debug.Log("Error: Unable to get distance between home station and target, as some of them are not set.");
        }

        return distance_squared;
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

    public void FreeTargetDebris()
    {
        if (debrisTarget != null)
        {
            if (debrisTarget.GetComponent<Debris>() != null)
            {
                debrisTarget.GetComponent<Debris>().isBeingCollected = false;
                debrisTarget.GetComponent<Debris>().isBeingTargetedByCollector = false;
            }
            else if (debrisTarget.GetComponent<EnemySpaceship>() != null)
            {
                debrisTarget.GetComponent<EnemySpaceship>().isBeingCollected = false;
                debrisTarget.GetComponent<EnemySpaceship>().isBeingTargetedByCollector = false;
            }
            debrisTarget = null;
        }
    }
}
