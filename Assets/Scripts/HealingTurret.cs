using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealingTurret : Turret {

    [Header("Tier 2")]
    public float healingPower_tier_2;
    public float range_tier_2;

    [Header("Tier 3")]
    public float healingPower_tier_3;
    public float range_tier_3;

    // Use this for initialization
    void Start () {
        InvokeRepeating("UpdateTarget", 0f, 0.5f);
        InvokeRepeating("LockOnTarget", 0f, 0.1f);
    }
	
	// Update is called once per frame
	void Update () {
        UpdateTarget();	
	}

    public HealingTurret()
    {
        //Debug.Log("Healing turret constructor");
    }

    public void LockOnTarget()
    {
        if (GameManager.instance.gameState == GameManager.GameState.Default)
        {
            if (hasEnoughEnergy)
            {
                LineRenderer lineRenderer = gameObject.GetComponent<LineRenderer>();
                if (target != null)
                {
                    if (target.GetComponent<Spaceship>().healthPoints < target.GetComponent<Spaceship>().maxHealthPoints)    // Target has not full health
                    {
                        RotateCanonTowardsTarget();
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

    public override void ApplyCurrentTierSettings()
    {
        Debug.Log("ApplyCurrentTierSettings | HEALING TURRET | CurrentTier: " + currentTier);
        switch (currentTier)
        {
            case 2:
            {
                healingPower = healingPower_tier_2;
                range = range_tier_2;
                break;

            }
            case 3:
            {
                healingPower = healingPower_tier_3;
                range = range_tier_3;
                break;
            }
        }
    }



}
