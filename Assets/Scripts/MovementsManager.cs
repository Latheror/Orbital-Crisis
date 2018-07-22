using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementsManager : MonoBehaviour {

    public static MovementsManager instance;

    public GameObject mainPlanet;
    public float rotate_speed = 1f;
    public float skyBoxRotationSpeed = 1f;

    void Awake(){ 
        if (instance != null){ Debug.LogError("More than one MovementsManager in scene !"); return; } instance = this;
    }

	// Use this for initialization
	void Start()
    {

    }
	
	// Update is called once per frame
	void Update () {

        RotatePlanet();

        RotateSkyBox();

	}



    public void RotatePlanet()
    {
        mainPlanet.transform.Rotate(0, rotate_speed*Time.deltaTime, 0, Space.World);
    }

    public void RotateSkyBox()
    {
        RenderSettings.skybox.SetFloat("_Rotation", Time.time * skyBoxRotationSpeed);
    }


}
