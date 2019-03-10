using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FreezingTurret : Turret {

    [Header("Settings")]
    public float freezingSpeed = 1f;

    [Header("Tier 2")]
    public float freezingSpeed_tier_2 = 2f;
    public float range_tier_2 = 200f;
    public float energyConsumption_tier_2 = 25;

    [Header("Tier 3")]
    public float freezingSpeed_tier_3 = 3f;
    public float range_tier_3 = 300f;
    public float energyConsumption_tier_3 = 40;

    [Header("Prefabs")]
    public Material frozenMeteorMaterial;
    public Material defaultMeteorMaterial;

    void Start () {

        buildingLocationType = BuildingLocationType.Planet;

        // Some InvokeRepeating are in the turret class !
        InvokeRepeating("UpdateTarget", 0f, 0.5f);
        InvokeRepeating("FreezeTarget", 0f, 0.1f); 
    }
	
    public FreezingTurret()
    {
        //Debug.Log("FreezingTurret constructor");
    }

    public void FreezeTarget()
    {
        if (GameManager.instance.gameState == GameManager.GameState.Default)
        {
            LineRenderer lineRenderer = gameObject.GetComponent<LineRenderer>();
            if (hasEnoughEnergy)
            {
                RotateCanonTowardsTarget();
                
                if (target != null && target.GetComponent<Meteor>() != null)
                {
                    Meteor m = target.GetComponent<Meteor>();
                    if(m.freezingFactor < 1)
                    {
                        GameObject chosenTarget = target;

                        lineRenderer.SetPosition(0, shootingPoint.transform.position);
                        lineRenderer.SetPosition(1, chosenTarget.transform.position);
                        lineRenderer.enabled = true;

                        SlowDownTarget();
                        SetFreezingMaterial();
                    }
                }
                else
                {
                    lineRenderer.enabled = false;
                }
            }
            else
            {
                //Debug.Log("Turret doesn't have enough energy !");
                lineRenderer.enabled = false;
            }
        }
    }

    public void SlowDownTarget()
    {
        target.GetComponent<Meteor>().Freeze(freezingSpeed * (1 + populationBonus));
    }

    public void SetFreezingMaterial()
    {
        target.GetComponent<Renderer>().material = frozenMeteorMaterial;
    }

    public override void ApplyCurrentTierSettings()
    {
        Debug.Log("ApplyCurrentTierSettings | FREEZING TURRET | CurrentTier: " + currentTier);
        switch (currentTier)
        {
            case 2:
            {
                freezingSpeed = freezingSpeed_tier_2;
                range = range_tier_2;
                energyConsumption = energyConsumption_tier_2;
                break;
            }
            case 3:
            {
                freezingSpeed = freezingSpeed_tier_3;
                range = range_tier_3;
                energyConsumption = energyConsumption_tier_3;
                break;
            }
        }
    }
}
