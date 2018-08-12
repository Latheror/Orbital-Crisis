using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuMeteor : MonoBehaviour {

    public GameObject menuPlanet;
    public float rotationSpeed = 10f;

	// Use this for initialization
	void Start () {
        menuPlanet = GameObject.Find("Planet");
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
}
