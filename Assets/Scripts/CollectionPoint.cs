using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectionPoint : MonoBehaviour {

    [Header("Parts")]
    public GameObject activationIndicator;

    [Header("Resources")]
    public Material activationOnIndicatorMaterial;
    public Material activationOffIndicatorMaterial;

    [Header("Operation")]
    public int collectionPointIndex;
    public GameObject lineRendererOrigin;
    public bool isActivated = false;
    public GameObject targetDebris;
    public bool isCollecting = false;

    public GameObject GetClosestAvailableDebris()
    {
        //Debug.Log("CollectionPoint | GetClosestAvailableDebris");
        GameObject closestDebris = null;
        float minDist = Mathf.Infinity;

        // Meteor Debris
        foreach (GameObject debris in DebrisManager.instance.debrisList)
        {
            float dist_squared = (transform.position - debris.transform.position).sqrMagnitude;
            if((dist_squared < minDist) && (dist_squared < Mathf.Pow(MegaCollector.instance.collectionPointsDistance,2)) && (!debris.GetComponent<Debris>().isBeingCollected) && (!debris.GetComponent<Debris>().isBeingTargetedByCollector))
            {
                minDist = dist_squared;
                closestDebris = debris;
            }
        }

        // Enemy spaceship debris
        foreach (GameObject enemyWreck in EnemiesManager.instance.enemyWrecks)
        {
            if(enemyWreck != null)
            {
                float dist_squared = (transform.position - enemyWreck.transform.position).sqrMagnitude;
                if ((dist_squared < minDist) && (dist_squared < Mathf.Pow(MegaCollector.instance.collectionPointsDistance,2)) && (!enemyWreck.GetComponent<EnemySpaceship>().isBeingCollected) && (!enemyWreck.GetComponent<EnemySpaceship>().isBeingTargetedByCollector))
                {
                    minDist = dist_squared;
                    closestDebris = enemyWreck;
                }
            }
        }

        return closestDebris;
    }

    public void UpdateCollectTarget()
    {
        //Debug.Log("CollectionPoint UpdateCollectTarget | IsCollecting [" + isCollecting + "] | Target [" + targetDebris + "]");
        if (!isCollecting) // CollectionPoint isn't collecting a debris at this moment
        {
            if (targetDebris != null)
            {
                FreeTargetDebris();
            }
            else
            {
                targetDebris = GetClosestAvailableDebris();
                if(targetDebris != null)
                {
                    if (targetDebris.GetComponent<Debris>() != null)
                    {
                        targetDebris.GetComponent<Debris>().isBeingTargetedByCollector = true;
                    }
                    else if (targetDebris.GetComponent<EnemySpaceship>() != null)
                    {
                        targetDebris.GetComponent<EnemySpaceship>().isBeingTargetedByCollector = true;
                    }
                    isCollecting = true;
                    //Debug.Log("CollectionPoint | IsCollecting [TRUE]");
                }            
            }
        }
    }

    public void FreeTargetDebris()
    {
        if(targetDebris != null)
        {
            if (targetDebris.GetComponent<Debris>() != null)
            {
                targetDebris.GetComponent<Debris>().isBeingCollected = false;
                targetDebris.GetComponent<Debris>().isBeingTargetedByCollector = false;
            }
            else if (targetDebris.GetComponent<EnemySpaceship>() != null)
            {
                targetDebris.GetComponent<EnemySpaceship>().isBeingCollected = false;
                targetDebris.GetComponent<EnemySpaceship>().isBeingTargetedByCollector = false;
            }
            targetDebris = null;
            isCollecting = false;
        }
    }


    public void Collect()
    {
        LineRenderer lr = GetComponent<LineRenderer>();

        //Debug.Log("CollectionPoint [" + collectionPointIndex + "] | Collect | Target [" + targetDebris + "]");
        if (isActivated)
        {
            if (targetDebris != null)
            {
                // Line Renderer update
                lr.SetPosition(0, lineRendererOrigin.transform.position);
                lr.SetPosition(1, targetDebris.transform.position);
                lr.enabled = true;

                // Move Debris towards collection point
                targetDebris.transform.position = Vector3.MoveTowards(targetDebris.transform.position, transform.position, MegaCollector.instance.currentCollectionSpeed * MegaCollector.instance.collectionTimeSpeedFactor * Time.deltaTime);

                // Check if debris is in range
                if ((targetDebris.transform.position - transform.position).sqrMagnitude <= Mathf.Pow(MegaCollector.instance.destroyDistance,2)) // Using distance squared -> Less computation
                {
                    //Debug.Log("Collecting Debris !");
                    if (targetDebris.GetComponent<Debris>() != null)
                    {
                        targetDebris.GetComponent<Debris>().Collect();
                    }
                    else if (targetDebris.GetComponent<EnemySpaceship>() != null)
                    {
                        targetDebris.GetComponent<EnemySpaceship>().Collect();
                    }
                    targetDebris = null;
                    isCollecting = false;
                }
            }
            else
            {
                isCollecting = false;
                lr.enabled = false;
            }
        }
        else
        {
            lr.enabled = false;
        }
    }

    public bool IsTargetAlreadyTaken(GameObject target)
    {
        bool alreadyTaken = false;
        if (target.GetComponent<Debris>() != null)
        {
            alreadyTaken = (target.GetComponent<Debris>().isBeingTargetedByCollector || target.GetComponent<Debris>().isBeingCollected);
        }
        else if (target.GetComponent<EnemySpaceship>() != null)
        {
            alreadyTaken = (target.GetComponent<EnemySpaceship>().isBeingCollected || target.GetComponent<EnemySpaceship>().isBeingCollected);
        }

        return alreadyTaken;
    }

    public void Activate(bool activate)
    {
        //Debug.Log("CollectionPoint [" + collectionPointIndex + "] | Activating [" + activate + "]");
        isActivated = activate;
        SetActivationMaterial();
    }

    public void SetActivationMaterial()
    {
        activationIndicator.GetComponent<Renderer>().material = (isActivated) ? activationOnIndicatorMaterial : activationOffIndicatorMaterial;
    }

    public void DisableCollectionRay()
    {
        GetComponent<LineRenderer>().enabled = false;
    }

    public void ReleaseTarget()
    {
        if(targetDebris != null)
        {
            if(targetDebris.GetComponent<Debris>() != null)
            {
                targetDebris.GetComponent<Debris>().isBeingCollected = false;
            }
            else if (targetDebris.GetComponent<EnemySpaceship>() != null)
            {
                targetDebris.GetComponent<EnemySpaceship>().isBeingCollected = false;
            }
        }
    }
}
