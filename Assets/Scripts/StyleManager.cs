using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StyleManager : MonoBehaviour {

    public static StyleManager instance;

    public Color[] colors;
    public GameObject mainPlanet;

    public Color transparentColor;

    void Awake(){ 
        if (instance != null){ Debug.LogError("More than one StyleManager in scene !"); return; } instance = this;
    }

    public void TouchCountToPlanetColor(int touchCount)
    {
        mainPlanet.GetComponent<Renderer>().material.color = colors[touchCount];
    }
}
