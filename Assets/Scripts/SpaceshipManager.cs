using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpaceshipManager : MonoBehaviour {

    public static SpaceshipManager instance;
    void Awake()
    {
        if (instance != null) { Debug.LogError("More than one SpaceshipManager in scene !"); return; }
        instance = this;
    }

    public GameObject mainSpaceship;

    // Use this for initialization
    void Start () {
        mainSpaceship.GetComponent<Spaceship>().isActivated = true;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
