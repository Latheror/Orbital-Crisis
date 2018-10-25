using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuOrbitingObject : MonoBehaviour {

    public GameObject menuPlanet;
    public float rotationSpeed = 10f;
    public bool isMeteor;
    public bool isBeingTargeted;
    public GameObject meteorImpactEffect;


    // Use this for initialization
    void Start () {
        menuPlanet = GameObject.Find("Planet");
        isBeingTargeted = false;
        InvokeRepeating("InstantiateMeteorEffect", 0f, 0.2f);
    }
	
	// Update is called once per frame
	void Update()
    {
        RotateAroundPlanet();
    }

    void RotateAroundPlanet()
    {
        transform.RotateAround(menuPlanet.transform.position, Vector3.forward, rotationSpeed * Time.deltaTime);
    }

    public void InstantiateMeteorEffect()
    {
        if (isMeteor && isBeingTargeted)
        {
            GameObject impactEff = Instantiate(meteorImpactEffect, transform.position, transform.rotation);
            Destroy(impactEff, 1.5f);
        }
    }
}
