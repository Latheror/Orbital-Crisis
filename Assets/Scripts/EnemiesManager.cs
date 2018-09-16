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

    public List<GameObject> enemies;
    public List<GameObject> enemyWrecks;

    public int meteorPriority = 1;
    public int spaceshipsPriority = 2;

    [Header("Enemies")]
    public GameObject ennemySpaceship_1;



    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
