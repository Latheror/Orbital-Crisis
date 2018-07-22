﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Meteor : MonoBehaviour {

    public float originalSize;
    public float size;
    public float healthPoints;
    public bool canBeGathered;
    public float minRotationSpeed = 5f;
    public float maxRotationSpeed = 40f;
    public float baseRotationSpeed = 20f;
    public float currentRotationSpeed = 20f;
    public bool isRotating;
    public float baseApproachSpeed = 5f;
    public float currentApproachSpeed = 5f;
    public float minApproachSpeed = 5f;
    public float maxApproachSpeed = 20f;
    public bool isMovingTowardsPlanet;
    public bool willLetDebris = false;

    public Material defaultMeteorMaterial;
    public Material touchedByPlayerMaterial;

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
        ExecuteMovements();
	}

    public Meteor()
    {
        canBeGathered = false;
    }

    public void setRandomSpeeds()
    {
        currentRotationSpeed = baseRotationSpeed = Random.Range(minRotationSpeed, maxRotationSpeed);
        currentApproachSpeed = baseApproachSpeed = Random.Range(minApproachSpeed, maxApproachSpeed);
    }

    public void ExecuteMovements()
    {
        if(isRotating)
        {
            RotateMeteorAroundPlanet();
        }
        if(isMovingTowardsPlanet)
        {
            MoveTowardsPlanet();
        }
    }

    public void RotateMeteorAroundPlanet()
    {
        float step = currentRotationSpeed * Time.deltaTime;   
        this.gameObject.transform.RotateAround(GameManager.instance.mainPlanet.transform.position, Vector3.forward, step);
    }

    public void MoveTowardsPlanet()
    {
        float step = currentApproachSpeed * Time.deltaTime;
        this.gameObject.transform.position = Vector3.MoveTowards(this.gameObject.transform.position, GameManager.instance.mainPlanet.transform.position, step);
    }

    public void DealDamage(float amount)
    {
        //Debug.Log("Dealing " + amount + " damages to meteor.");
        healthPoints -= amount;

        ResizeMeteorFromHealth(healthPoints);

        if(healthPoints <= 0)
        {
            if(willLetDebris)
            {
                SpawnDebris();
                Debug.Log("A meteor has exploded into debris !");
                MeteorsManager.instance.DeleteMeteor(this.gameObject);
            }
            else
            {
                // The meteor have been destroyed
                Debug.Log("A meteor has been destroyed !");
                MeteorsManager.instance.DeleteMeteor(this.gameObject);
                MeteorsManager.instance.MeteorDestroyed(this);
            }
        }
    }

    public void ResizeMeteorFromHealth(float health)
    {
        float newSize = MeteorsManager.instance.GetMeteorSizeFromHealth(health);
        size = newSize;
        transform.localScale = new Vector3(newSize, newSize, newSize);
    }

    public void Freeze(float freezingPower)
    {
        currentApproachSpeed = baseApproachSpeed - freezingPower;
        currentRotationSpeed = baseRotationSpeed - freezingPower;
    }

    public void ResetMeteorSettings()
    {
        currentApproachSpeed = baseApproachSpeed;
        currentRotationSpeed = baseRotationSpeed;
        gameObject.GetComponent<Renderer>().material = defaultMeteorMaterial;
    }

    public void TouchedByPlayer()
    {
        GetComponent<Renderer>().material = touchedByPlayerMaterial;
    }

    public void SpawnDebris()
    {
        DebrisManager.instance.SpawnDebris(transform.position, originalSize);
    }

    // -----
    public void TestMeteorFunction()
    {
        bool willMoveTowardsPlanet = true;
        //bool willMoveTowardsPlanet = LogicFunctions.RandomTrueFalse();
        //Debug.Log("This meteor will move towards the planet : " + willMoveTowardsPlanet);
        isMovingTowardsPlanet = willMoveTowardsPlanet;

        //isRotating = LogicFunctions.RandomTrueFalse();
        isRotating = true;
    }
}
