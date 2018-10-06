﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebrisCollector : MonoBehaviour {

    [Header("Settings")]
    public int resourcesPerUnitOfSize = 10;
    public float operationDistance = 50f;
    public float movementSpeed = 20f;
    public float rotationSpeed = 20f;

    [Header("Operation")]
    public GameObject debrisTarget = null;
    public bool debrisIsBeingCollected = false;
    public GameObject homeStation = null;


    // Use this for initialization
    void Start () {
        InvokeRepeating("UpdateTarget", 0f, 0.2f);
        debrisIsBeingCollected = false;
	}

    void Update()
    {
        FollowTargetOrRotateAroundStation();
    }
	

    public void UpdateTarget()
    {
        if (GameManager.instance.gameState == GameManager.GameState.Default)
        {
            if (DebrisManager.instance.debrisList.Count > 0 || EnemiesManager.instance.enemyWrecks.Count > 0)
            {
                // DEBUG
                if (debrisIsBeingCollected && (debrisTarget == null))
                {
                    debrisIsBeingCollected = false;
                }

                float minDistance = Mathf.Infinity;
                GameObject closestDesbris = null;

                if (DebrisManager.instance.debrisList.Count > 0)
                {
                    foreach (var debris in DebrisManager.instance.debrisList)
                    {
                        float distance = Vector3.Distance(transform.position, debris.transform.position);
                        if (distance < minDistance)
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
                        if (distance < minDistance)
                        {
                            minDistance = distance;
                            closestDesbris = wreck;
                        }
                    }
                }

                debrisTarget = closestDesbris;
            }
            else
            {
                debrisTarget = null;
            }
        }
    }

    public void FollowTargetOrRotateAroundStation()
    {
        LineRenderer lineRenderer = gameObject.GetComponent<LineRenderer>();
        if((debrisTarget != null) && (GetDistanceBetweenTargetAndHomeStation() < homeStation.GetComponent<DebrisCollectorStation>().range))
        {   
            if(! debrisIsBeingCollected)
            {
                if (DistanceToTarget() > operationDistance)
                {
                   transform.position = Vector3.MoveTowards(transform.position, debrisTarget.transform.position, Time.deltaTime * movementSpeed);
                }
                else
                {
                    lineRenderer.enabled = true;
                    lineRenderer.SetPosition(0, transform.position);
                    lineRenderer.SetPosition(1, debrisTarget.transform.position);

                    StartCoroutine("CollectDebris");
                }
            }
        }
        else
        {
            lineRenderer.enabled = false;
            ComeBackAroundStationAndRotate();
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
        if(debrisTarget != null)
        {
            //Debug.Log("Starting debris collection.");
            debrisIsBeingCollected = true;
            yield return new WaitForSeconds(2.0f);
            if(debrisTarget != null)
            {
                Debris debris = debrisTarget.GetComponent<Debris>();
                if(debris != null)
                {
                    debris.Collect();
                }
                else
                {
                    EnemySpaceship enemySpaceship = debrisTarget.GetComponent<EnemySpaceship>();
                    if (enemySpaceship != null)
                    {
                        enemySpaceship.Collect();
                    }
                    else
                    {
                        Debug.Log("Debris collection error : type of debris unknown !");
                    }
                }  
            }

            debrisIsBeingCollected = false;
            //Debug.Log("Debris collection ended.");
            debrisTarget = null;
        }
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
}
