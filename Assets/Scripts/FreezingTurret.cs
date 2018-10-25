using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FreezingTurret : Turret {

    [Header("Settings")]
    public float freezingFactor = 0.8f; // Between 0 and 1

    [Header("Tier 2")]
    public float freezingFactor_tier_2 = 0.7f;
    public float range_tier_2 = 200f;
    public float energyConsumption_tier_2 = 25;

    [Header("Tier 3")]
    public float freezingFactor_tier_3 = 0.9f;
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
        Debug.Log("FreezingTurret constructor");
    }

    public void FreezeTarget()
    {
        if (GameManager.instance.gameState == GameManager.GameState.Default)
        {
            if (hasEnoughEnergy)
            {
                RotateCanonTowardsTarget();

                LineRenderer lineRenderer = gameObject.GetComponent<LineRenderer>();
                if (target != null)
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
                //Debug.Log("Turret doesn't have enough energy !");
            }
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

    public override void ApplyCurrentTierSettings()
    {
        Debug.Log("ApplyCurrentTierSettings | LASER TURRET | CurrentTier: " + currentTier);
        switch (currentTier)
        {
            case 2:
            {
                freezingFactor = freezingFactor_tier_2;
                range = range_tier_2;
                energyConsumption = energyConsumption_tier_2;
                break;
            }
            case 3:
            {
                freezingFactor = freezingFactor_tier_3;
                range = range_tier_3;
                energyConsumption = energyConsumption_tier_3;
                break;
            }
        }
    }
}
