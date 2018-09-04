using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestLineRenderer : MonoBehaviour {

    public GameObject object2;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        LineRenderer lr = GetComponent<LineRenderer>();

        lr.SetPosition(0, transform.position);
        lr.SetPosition(1, object2.transform.position);
	}
}
