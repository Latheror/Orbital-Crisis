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
        if (hasEnoughEnergy)
        {

            LineRenderer lineRenderer = gameObject.GetComponent<LineRenderer>();
            if (meteorTarget != null)
            {
                lineRenderer.enabled = true;
                GameObject target = meteorTarget;
                lineRenderer.SetPosition(0, transform.position);
                lineRenderer.SetPosition(1, target.transform.position);

                DealDamageToMeteorTarget();
            }
            else
            {
                lineRenderer.enabled = false;
            }

        }
    }
}
