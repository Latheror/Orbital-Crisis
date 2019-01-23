using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotatingObject : MonoBehaviour {


    public GameObject rotationCenter;
    public float rotationSpeed = 10f;
    public Vector3 axe = Vector3.up;

	// Update is called once per frame
	void Update () {
        transform.RotateAround(rotationCenter.transform.position, axe, Time.deltaTime * rotationSpeed);
	}
}
