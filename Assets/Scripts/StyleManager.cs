using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StyleManager : MonoBehaviour {

    public static StyleManager instance;

    public Color[] colors;
    public GameObject mainPlanet;


    void Awake(){ 
        if (instance != null){ Debug.LogError("More than one StyleManager in scene !"); return; } instance = this;
    }


	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}


    public void TouchCountToPlanetColor(int touchCount)
    {
        mainPlanet.GetComponent<Renderer>().material.color = colors[touchCount];
    }


}
