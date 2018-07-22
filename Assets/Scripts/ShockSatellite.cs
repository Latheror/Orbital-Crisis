using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShockSatellite : Building {

    public GameObject shockWave;
    public float range = 50f;
    public List<GameObject> inRangeMeteors;
    public float damagePower = 20f;
    public float actionDelay = 5f;

	// Use this for initialization
	void Start () {
        InvokeRepeating("PlayAnimation", 0f, actionDelay);
	}
	
    public ShockSatellite(string name) :  base(name)
    {
        Debug.Log("ShockSatellite constructor");
    }

    public void GetInRangeMeteors()
    {
        List<GameObject> meteors = MeteorsManager.instance.meteorsList;
        inRangeMeteors = new List<GameObject>();

        foreach (var meteor in meteors)
        {
            if(Vector3.Distance(transform.position, meteor.transform.position) <= range)
            {
                inRangeMeteors.Add(meteor);
            }
        }
    }

    public void PlayAnimation()
    {
        Debug.Log("ShockSatellite Play");
        if(hasEnoughEnergy)
        {
            Animator animator = GetComponent<Animator>();
            animator.SetBool("StartAnimation", true);
        }
        else
        {
            Debug.Log("ShockSatellite doesn't have enough energy !");
        }
    }


    public void MiddleAnimationEvent()
    {
        Debug.Log("MiddleAnimationEvent");
        GetInRangeMeteors();
        DealDamageToInRangeMeteors();
        GetComponent<Animator>().SetBool("StartAnimation", false);
    }

    public void DealDamageToInRangeMeteors()
    {
        foreach (var meteor in inRangeMeteors)
        {
            meteor.GetComponent<Meteor>().DealDamage(damagePower);
        }
    }
}
