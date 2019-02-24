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
    public float hardMeteorsProportion = 0; // Between 0 and 1

    [Header("Operation")]
    public int spawnedMeteorsNb = 0;
    public int destroyedMeteorsNb = 0;
    public bool levelCompleted = false;
    public bool allLevelMeteorsDestroyed = false;

	// Use this for initialization
	void Start () {
        destroyedMeteorsNb = 0;
        levelCompleted = false;
        allLevelMeteorsDestroyed = false;
    }
	
    public Level(int number, string name, int meteorsNb, int meteorSerieNb, float meteorSpawnSizeFactor, float timeBetweenSpawns, List<GameObject> enemies, float countdownTime, float hardMeteorsProportion)
    {
        this.levelNb = number;
        this.levelName = name;
        this.levelMeteorsNb = meteorsNb;
        this.meteorSerieNb = meteorSerieNb;
        this.meteorSpawnSizeFactor = meteorSpawnSizeFactor;
        this.timeBetweenSpawns = timeBetweenSpawns;
        this.enemies = enemies;
        this.countdownTime = countdownTime;
        this.hardMeteorsProportion = hardMeteorsProportion;

        this.spawnedMeteorsNb = 0;
        this.destroyedMeteorsNb = 0;
        this.allLevelMeteorsDestroyed = false;
    }

    // Returns true if level is completed
    public void IncrementDestroyedMeteorsNb(int nb)
    {
        destroyedMeteorsNb += nb;
        //Debug.Log("IncrementDestroyedMeteorsNb | LevelIndex [" + levelNb + "] | Destroyed [" + destroyedMeteorsNb + "] | Total [" + levelMeteorsNb + "]");

        if(LevelManager.instance.currentLevelNumber == levelNb)
        {
            if (destroyedMeteorsNb >= (levelMeteorsNb / 2))
            {
                LevelManager.instance.HalfCurrentLevelMeteorsDestroyed();
            }
            if (destroyedMeteorsNb >= levelMeteorsNb)
            {
                allLevelMeteorsDestroyed = true;
                LevelManager.instance.AllLevelMeteorsDestroyed(levelNb);
            }
        }
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
