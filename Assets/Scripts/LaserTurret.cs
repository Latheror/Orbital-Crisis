using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserTurret : Turret {

    [Header("Tier 2")]
    public float power_tier_2 = 20f;
    public float range_tier_2 = 200f;
    public float energyConsumption_tier_2 = 25;

    [Header("Tier 3")]
    public float power_tier_3 = 30f;
    public float range_tier_3 = 300f;
    public float energyConsumption_tier_3 = 40;

    void Start () {

        buildingLocationType = BuildingLocationType.Planet;

        InvokeRepeating("UpdateTarget", 0f, 0.5f);
        InvokeRepeating("LockOnTarget", 0f, 0.1f); 
	}
	
    public LaserTurret()
    {
        //Debug.Log("LaserTurret constructor");
    }

    public void LockOnTarget()
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

                    DealDamageToMeteorTarget();
                }
                else
                {
                    lineRenderer.enabled = false;
                }

                // Direction to shoot towards
                //Vector3 dir = meteorTarget.transform.position - transform.position;
                //Quaternion lookRotation = Quaternion.LookRotation(dir);
                //Vector3 rotation = Quaternion.Lerp(transform.rotation, lookRotation, Time.deltaTime * 10f);
                //transform.rotation = Quaternion.Euler(0f, rotation.y, 0f);
            }
        }
    }


    // Draw a preview of the turret's range
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, range);
    }

    public override void ApplyCurrentTierSettings()
    {
        Debug.Log("ApplyCurrentTierSettings | LASER TURRET | CurrentTier: " + currentTier);
        switch (currentTier)
        {
            case 2:
            {
                power = power_tier_2;
                range = range_tier_2;
                energyConsumption = energyConsumption_tier_2;
                break;
            }
            case 3:
            {
                power = power_tier_3;
                range = range_tier_3;
                energyConsumption = energyConsumption_tier_3;
                break;
            }
        }
    }


}
