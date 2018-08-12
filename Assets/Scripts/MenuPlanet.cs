using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuPlanet : MonoBehaviour {

    public float rotationSpeed = 10f;

    public GameObject shootingPoint1;
    public GameObject shootingPoint2;
    public GameObject meteor1;
    public GameObject meteor2;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        RotatePlanet();
        UpdateLineRenderers();
	}

    public void RotatePlanet()
    {
        transform.Rotate(0, rotationSpeed*Time.deltaTime, 0, Space.World);
    }

    public void UpdateLineRenderers()
    {
        LineRenderer lr_1 = shootingPoint1.GetComponent<LineRenderer>();
        lr_1.SetPositions(new Vector3[]{shootingPoint1.transform.position, meteor1.transform.position});

        LineRenderer lr_2 = shootingPoint2.GetComponent<LineRenderer>();
        lr_2.SetPositions(new Vector3[]{shootingPoint2.transform.position, meteor2.transform.position});
    }

}
