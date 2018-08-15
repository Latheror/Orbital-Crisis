using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level {


    public int levelNb = 0;
    public string levelName = "level";
    public EnemyWave enemyWave;
    // Temporary
    public int levelMeteorsNb = 0;
    public int destroyedMeteorsNb = 0;
    public bool levelCompleted = false;
    public bool allLevelMeteorsDestroyed = false;

	// Use this for initialization
	void Start () {
        destroyedMeteorsNb = 0;
        levelCompleted = false;
        allLevelMeteorsDestroyed = false;
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

    // Returns true if level is completed
    public bool IncrementDestroyedMeteorsNb(int nb)
    {
        Debug.Log("IncrementDestroyedMeteorsNb | Destroyed: " + destroyedMeteorsNb + " | Total: " + levelMeteorsNb);

        destroyedMeteorsNb += nb;
        if(destroyedMeteorsNb >= levelMeteorsNb)
        {
            allLevelMeteorsDestroyed = true;
        }
        else
        {
            // System Changed - Now displayed remaining ennemies to spawn.
            //LevelManager.instance.UpdateRemainingEnnemiesIndicator();
        }
        return allLevelMeteorsDestroyed;
    }

    public class EnemyWave
    {

    }
}
