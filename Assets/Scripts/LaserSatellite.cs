using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserSatellite : Turret {

	// Use this for initialization
	void Start () {
        InvokeRepeating("UpdateTarget", 0f, 0.5f);
        InvokeRepeating("FireOnTarget", 0f, 0.1f);

        hasAngleRange = false;

        power = 5f;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public LaserSatellite(string name) :  base(name)
    {
        Debug.Log("LaserSatellite constructor");
    }

    public void FireOnTarget()
    {
        if (GameManager.instance.gameState == GameManager.GameState.Default)
        {
            if (hasEnoughEnergy)
            {

                LineRenderer lineRenderer = gameObject.GetComponent<LineRenderer>();
                if (target != null)
                {
                    lineRenderer.enabled = true;
                    GameObject chosenTarget = target;
                    lineRenderer.SetPosition(0, transform.position);
                    lineRenderer.SetPosition(1, chosenTarget.transform.position);

                    DealDamageToMeteorTarget();
                }
                else
                {
                    lineRenderer.enabled = false;
                }

            }
        }
    }
}
