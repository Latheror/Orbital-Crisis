using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemiesManager : MonoBehaviour {

    public static EnemiesManager instance;
    void Awake()
    {
        if (instance != null) { Debug.LogError("More than one EnemiesManager in scene !"); return; }
        instance = this;
    }

    public GameObject[] enemies;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
