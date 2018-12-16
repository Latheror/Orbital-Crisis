using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectionPoint : MonoBehaviour {

    public int collectionPointIndex;
    public bool isActivated = false;
    public GameObject targetDebris;
    public bool isCollecting = false;
    public GameObject lineRendererOrigin;
    public GameObject activationIndicator;

    public Material activationOnIndicatorMaterial;
    public Material activationOffIndicatorMaterial;

    public GameObject GetClosestAvailableDebris()
    {
        GameObject closestDebris = null;
        float minDist = Mathf.Infinity;

        foreach (GameObject debris in DebrisManager.instance.debrisList)
        {
            float dist = Vector3.Distance(transform.position, debris.transform.position);
            if((dist < minDist) && (dist < MegaCollector.instance.collectionPointsDistance) && (!debris.GetComponent<Debris>().isBeingCollected))
            {
                minDist = dist;
                closestDebris = debris;
            }
        }
        return closestDebris;
    }

    public void UpdateCollectTarget()
    {
        if (!isCollecting) // CollectionPoint isn't collecting a debris at this moment
        {
            targetDebris = GetClosestAvailableDebris();
            if(targetDebris != null)
            {
                targetDebris.GetComponent<Debris>().isBeingCollected = true;
                isCollecting = true;
            }
            else
            {
                isCollecting = false;
            }
        }
    }


    public void Collect()
    {
        LineRenderer lr = GetComponent<LineRenderer>();

        if (isActivated && targetDebris != null)
        {
            // Line Renderer update
            lr.SetPosition(0, lineRendererOrigin.transform.position);
            lr.SetPosition(1, targetDebris.transform.position);
            lr.enabled = true;

            // Move Debris towards collection point
            targetDebris.transform.position = Vector3.MoveTowards(targetDebris.transform.position, transform.position, MegaCollector.instance.currentCollectionSpeed * MegaCollector.instance.collectionTimeSpeedFactor * Time.deltaTime);

            // Check if debris is in range
            if (Vector3.Distance(targetDebris.transform.position, transform.position) <= MegaCollector.instance.destroyDistance)
            {
                Debug.Log("Debris Collected !");
                targetDebris.GetComponent<Debris>().Collect();
                targetDebris = null;
                isCollecting = false;
            }
        }
        else
        {
            lr.enabled = false;
        }
    }

    public void Activate(bool activate)
    {
        isActivated = activate;
        SetActivationMaterial();
    }

    public void SetActivationMaterial()
    {
        activationIndicator.GetComponent<Renderer>().material = (isActivated) ? activationOnIndicatorMaterial : activationOffIndicatorMaterial;
    }
}
