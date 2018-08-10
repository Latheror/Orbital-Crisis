using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level : MonoBehaviour {


    public int levelNb = 0;
    public string levelName = "level";
    public EnemyWave enemyWave;
    // Temporary
    public int levelMeteorsNb = 0;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}


    public Level(int number, string name, int meteorsNb)
    {
        this.levelNb = number;
        this.levelName = name;
        this.levelMeteorsNb = meteorsNb;
    }




    public class EnemyWave
    {

    }
}
