﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FreezingTurret : Turret {

    public float freezingPower = 5f;
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
            LineRenderer lineRenderer = gameObject.GetComponent<LineRenderer>();
            if(meteorTarget != null)
            {
                lineRenderer.enabled = true;
                GameObject target = meteorTarget;
                lineRenderer.SetPosition(0, transform.position);
                lineRenderer.SetPosition(1, target.transform.position);
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
        meteorTarget.GetComponent<Meteor>().Freeze(freezingPower);
    }

    public void SetFreezingMaterial()
    {
        meteorTarget.GetComponent<Renderer>().material = frozenMeteorMaterial;
    }
}
