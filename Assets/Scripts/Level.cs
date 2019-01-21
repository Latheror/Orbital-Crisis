using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[System.Serializable]
public class Level {

    [Header("Settings")]
    public int levelNb = 0;
    public string levelName = "level";
    public int levelMeteorsNb = 0;
    public int meteorSerieNb = 1;
    public float timeBetweenSpawns = 1f;
    public EnemyWave enemyWave;
    public List<GameObject> enemies;
    public float meteorSpawnSizeFactor;
    public float countdownTime;

    [Header("Operation")]
    public int destroyedMeteorsNb = 0;
    public bool levelCompleted = false;
    public bool allLevelMeteorsDestroyed = false;

	// Use this for initialization
	void Start () {
        destroyedMeteorsNb = 0;
        levelCompleted = false;
        allLevelMeteorsDestroyed = false;
    }
	
    public Level(int number, string name, int meteorsNb, int meteorSerieNb, float meteorSpawnSizeFactor, float timeBetweenSpawns, List<GameObject> enemies, float countdownTime)
    {
        this.levelNb = number;
        this.levelName = name;
        this.levelMeteorsNb = meteorsNb;
        this.meteorSerieNb = meteorSerieNb;
        this.meteorSpawnSizeFactor = meteorSpawnSizeFactor;
        this.timeBetweenSpawns = timeBetweenSpawns;
        this.enemies = enemies;
        this.countdownTime = countdownTime;
    }

    // Returns true if level is completed
    public bool IncrementDestroyedMeteorsNb(int nb)
    {
        //Debug.Log("IncrementDestroyedMeteorsNb | Destroyed: " + destroyedMeteorsNb + " | Total: " + levelMeteorsNb);

        destroyedMeteorsNb += nb;

        if(destroyedMeteorsNb >= (levelMeteorsNb / 2))
        {
            LevelManager.instance.HalfLevelMeteorsDestroyed();
        }

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

    [Serializable]
    public class LevelData
    {
        public int levelIndex;

        public LevelData(int levelIndex)
        {
            this.levelIndex = levelIndex;
        }
    }
}
