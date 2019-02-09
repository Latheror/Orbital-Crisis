using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShockSatellite : Building {

    [Header("Settings")]
    public float damagePower = 20f;
    public float actionDelay = 5f;

    [Header("Tier 2")]
    public float damagePower_tier_2 = 25f;
    public float range_tier_2 = 100f;
    public float actionDelay_tier_2 = 4f;
    public float energyConsumption_tier_2 = 60;

    [Header("Tier 3")]
    public float damagePower_tier_3 = 30f;
    public float range_tier_3 = 150f;
    public float actionDelay_tier_3 = 3f;
    public float energyConsumption_tier_3 = 100;


    [Header("Operation")]
    public List<GameObject> inRangeMeteors;

    [Header("Parts")]
    public GameObject shockWave;


	// Use this for initialization
	void Start () {
        InvokeRepeating("PlayAnimation", 0f, actionDelay);
	}
	
    public ShockSatellite()
    {
        //Debug.Log("ShockSatellite constructor");
    }

    public void GetInRangeMeteors()
    {
        List<GameObject> meteors = MeteorsManager.instance.meteorsList;
        inRangeMeteors = new List<GameObject>();

        foreach (var meteor in meteors)
        {
            if((transform.position - meteor.transform.position).sqrMagnitude <= range*range)
            {
                inRangeMeteors.Add(meteor);
            }
        }
    }

    public void PlayAnimation()
    {
        if (GameManager.instance.gameState == GameManager.GameState.Default)
        {
            //Debug.Log("ShockSatellite Play");
            if (hasEnoughEnergy)
            {
                Animator animator = GetComponent<Animator>();
                animator.SetBool("StartAnimation", true);
            }
            else
            {
                //Debug.Log("ShockSatellite doesn't have enough energy !");
            }
        }
    }


    public void MiddleAnimationEvent()
    {
        //Debug.Log("MiddleAnimationEvent");
        GetInRangeMeteors();
        DealDamageToInRangeMeteors();
        GetComponent<Animator>().SetBool("StartAnimation", false);
    }

    public void DealDamageToInRangeMeteors()
    {
        foreach (var meteor in inRangeMeteors)
        {
            meteor.GetComponent<Meteor>().TakeDamage(damagePower * (1 + populationBonus));
        }
    }

    public override void ApplyCurrentTierSettings()
    {
        //Debug.Log("ApplyCurrentTierSettings | LASER TURRET | CurrentTier: " + currentTier);
        switch (currentTier)
        {
            case 2:
            {
                damagePower = damagePower_tier_2;
                range = range_tier_2;
                actionDelay = actionDelay_tier_2;
                energyConsumption = energyConsumption_tier_2;
                break;

            }
            case 3:
            {
                damagePower = damagePower_tier_3;
                range = range_tier_3;
                actionDelay = actionDelay_tier_3;
                energyConsumption = energyConsumption_tier_3;
                break;
            }
        }
    }
}
