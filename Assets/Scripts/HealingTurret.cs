using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealingTurret : Turret {

	// Use this for initialization
	void Start () {
        InvokeRepeating("UpdateTarget", 0f, 0.5f);
        InvokeRepeating("LockOnTarget", 0f, 0.1f);
    }
	
	// Update is called once per frame
	void Update () {
        UpdateTarget();	
	}

    public HealingTurret(string name) : base(name)
    {
        Debug.Log("Healing turret constructor");
    }

    public void LockOnTarget()
    {
        if (hasEnoughEnergy)
        {
            RotateCanonTowardsTarget();       // No model yet

            LineRenderer lineRenderer = gameObject.GetComponent<LineRenderer>();
            if (target != null)
            {
                if(target.GetComponent<Spaceship>().health < target.GetComponent<Spaceship>().maxHealth)    // Target has not full health
                {
                    lineRenderer.enabled = true;
                    GameObject chosenTarget = target;
                    lineRenderer.SetPosition(0, shootingPoint.transform.position);
                    lineRenderer.SetPosition(1, chosenTarget.transform.position);

                    HealTarget();
                }
                else
                {
                    lineRenderer.enabled = false;
                }

            }
            else
            {
                lineRenderer.enabled = false;
            }
        }
    }





}
