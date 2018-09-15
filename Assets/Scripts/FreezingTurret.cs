using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FreezingTurret : Turret {

    public float freezingFactor = 0.8f; // Between 0 and 1
    public Material frozenMeteorMaterial;
    public Material defaultMeteorMaterial;

    void Start () {

        buildingLocationType = BuildingLocationType.Planet;

        // Some InvokeRepeating are in the turret class !
        InvokeRepeating("UpdateTarget", 0f, 0.5f);
        InvokeRepeating("FreezeTarget", 0f, 0.1f); 
    }
	
    public FreezingTurret(string name) :  base(name)
    {
        Debug.Log("FreezingTurret constructor");
    }

    public void FreezeTarget()
    {
        if(hasEnoughEnergy)
        {
            RotateCanonTowardsTarget();

            LineRenderer lineRenderer = gameObject.GetComponent<LineRenderer>();
            if(target != null)
            {
                lineRenderer.enabled = true;
                GameObject chosenTarget = target;
                lineRenderer.SetPosition(0, shootingPoint.transform.position);
                lineRenderer.SetPosition(1, chosenTarget.transform.position);
                SlowDownTarget();
                SetFreezingMaterial();
            }
            else
            {
                lineRenderer.enabled = false;
            }
        }
        else
        {
            Debug.Log("Turret doesn't have enough energy !");
        }

    }

    public void SlowDownTarget()
    {
        target.GetComponent<Meteor>().Freeze(freezingFactor);
    }

    public void SetFreezingMaterial()
    {
        target.GetComponent<Renderer>().material = frozenMeteorMaterial;
    }
}
