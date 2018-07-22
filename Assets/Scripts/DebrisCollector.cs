using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebrisCollector : MonoBehaviour {

    public int resourcesPerUnitOfSize = 10;
    public GameObject debrisTarget = null;
    public float operationDistance = 50f;
    public float movementSpeed = 20f;
    public float rotationSpeed = 20f;
    public bool debrisIsBeingCollected = false;

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
        if(DebrisManager.instance.debrisList.Count > 0 /*&& (! debrisIsBeingCollected)*/)
        {
            // DEBUG
            if(debrisIsBeingCollected && (debrisTarget == null))
            {
                debrisIsBeingCollected = false;
            }

            float minDistance = Mathf.Infinity;
            GameObject closestDesbris = null;

            foreach (var debris in DebrisManager.instance.debrisList)
            {
                float distance = Vector3.Distance(transform.position, debris.transform.position);
                if(distance < minDistance)
                {
                    minDistance = distance;
                    closestDesbris = debris;
                }
            }

            debrisTarget = closestDesbris;
        }
        else
        {
            debrisTarget = null;
        }
    }

    public void FollowTargetOrRotateAroundStation()
    {
        LineRenderer lineRenderer = gameObject.GetComponent<LineRenderer>();
        if(debrisTarget != null)
        {   
            if(! debrisIsBeingCollected)
            {
                if(DistanceToTarget() > operationDistance)
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
                debrisTarget.GetComponent<Debris>().Collect();
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
}
