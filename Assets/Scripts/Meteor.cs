using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Meteor : MonoBehaviour {

    [Header("Settings")]
    public float originalSize;
    public float size;
    public bool canBeGathered;
    public float minRotationSpeed = 5f;
    public float maxRotationSpeed = 40f;
    public float baseRotationSpeed = 20f;
    public float currentRotationSpeed = 20f;
    public bool isRotating;
    public float baseApproachSpeed = 5f;
    public float minApproachSpeed = 5f;
    public float maxApproachSpeed = 20f;
    public bool isMovingTowardsPlanet;
    public bool willLetDebris = false;

    [Header("Prefabs")]
    public GameObject brownImpactEffect;
    public GameObject yellowImpactEffect;
    public Material defaultMeteorMaterial;
    public Material touchedByPlayerMaterial;

    [Header("Operation")]
    public float currentApproachSpeed = 5f;
    public float healthPoints;
    public bool hasAlreadyBeenHitByPlanetaryShield = false;

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void FixedUpdate () {
        if (GameManager.instance != null && GameManager.instance.gameState == GameManager.GameState.Default)
        {
            ExecuteMovements();
        }
	}

    public Meteor()
    {
        canBeGathered = false;
    }

    public void SetRandomSpeeds()
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

    public bool DealDamage(float amount)
    {
        bool isFatal = false;
        //Debug.Log("Dealing " + amount + " damages to meteor.");
        healthPoints -= amount;
        InstantiateImpactEffect(1);

        if (healthPoints <= 0)
        {
            isFatal = true;
            DestroyMeteor();
        }
        else
        {
            ResizeMeteorFromHealth(healthPoints);
        }
        return isFatal;
    }

    public void InstantiateImpactEffect(int impactEffectId)
    {
        GameObject impactEffect = null;
        switch (impactEffectId)
        {
            case 1:
            {
                impactEffect = brownImpactEffect;
                break;
            }
            case 2:
            {
                impactEffect = yellowImpactEffect;
                break;
            }
        }
        GameObject impactEff = Instantiate(impactEffect, transform.position, transform.rotation);
        Destroy(impactEff, 2f);
    }

    public void DestroyMeteor()
    {
        // LevelManager.instance.IncrementCurrentLevelDestroyedMeteorsNb(1); Done in meteors manager to handle meteors crashing into the planet

        if (willLetDebris)
        {
            SpawnDebris();
        }

        MeteorsManager.instance.DeleteMeteor(gameObject);
        MeteorsManager.instance.MeteorDestroyed(this);
    }

    public void ResizeMeteorFromHealth(float health)
    {
        float newSize = MeteorsManager.instance.GetMeteorSizeFromHealth(health);
        size = newSize;
        //Debug.Log("Downsizing meteor from [" + transform.localScale.x + "] to [" + newSize + "]");
        transform.localScale = new Vector3(newSize, newSize, newSize);
    }

    public void Freeze(float freezingFactor)
    {
        if (freezingFactor >= 0f && freezingFactor <= 1f)
        {
            currentApproachSpeed = baseApproachSpeed * (1 - freezingFactor);
            currentRotationSpeed = baseRotationSpeed * (1 - freezingFactor);
        }
        else
        {
            Debug.Log("ERROR : Freezing Factor must be in the 0 - 1 range.");
        }
    }

    public void ResetMeteorSettings()
    {
        //Debug.Log("ResetMeteorSettings");
        currentApproachSpeed = baseApproachSpeed;
        currentRotationSpeed = baseRotationSpeed;
        gameObject.GetComponent<Renderer>().material = defaultMeteorMaterial;
    }

    public void TouchedByPlayer()
    {
        //GetComponent<Renderer>().material = touchedByPlayerMaterial;

        // TODO
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

    public void SetPartialSpeeds(float approachSpeedFactor, float rotationSpeedFactor)
    {
        float appSpeedFactor = Mathf.Clamp(approachSpeedFactor, 0, 1);
        float rotSpeedFactor = Mathf.Clamp(rotationSpeedFactor, 0, 1);

        currentApproachSpeed = baseApproachSpeed * appSpeedFactor;
        currentRotationSpeed = baseRotationSpeed * rotSpeedFactor;
    }

    public void SetPartialSpeedsWithMax(float approachSpeedFactor, float approachSpeedMax, float rotationSpeedFactor, float rotationSpeedMax)
    {
        float appSpeedFactor = Mathf.Clamp(approachSpeedFactor, 0, 1);
        float rotSpeedFactor = Mathf.Clamp(rotationSpeedFactor, 0, 1);

        currentApproachSpeed = Mathf.Min(baseApproachSpeed * appSpeedFactor, approachSpeedMax);
        currentRotationSpeed = Mathf.Min(baseRotationSpeed * rotSpeedFactor, rotationSpeedMax);
    }
}
