﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LevelManager : MonoBehaviour {

    public static LevelManager instance;
    void Awake(){ 
        if (instance != null){ Debug.LogError("More than one LevelManager in scene !"); return; } instance = this;
    }

    [Header("Settings")]
    public float defaultCountdownSeconds = 20f;

    [Header("Levels")]
    public List<Level> levelsList = new List<Level>();
    public int currentLevelNumber = 0;
    public Level currentLevel = null;
    public int currentLevelSpawnedMeteorsNb = 0;
    public bool currentLevelAllMeteorsSpawned = false;
    public bool autoGeneratedLevels = true;
    public int[] diskUnlockingLevelNbs;
    public bool currentLevelFinished = false;

    [Header("UI")]
    public TextMeshProUGUI waveNumberText;
    public GameObject nextLevelButton;
    public Color nextLevelButtonBaseColor;
    public Color nextLevelButtonSecondaryColor;
    //public GameObject remainingEnemiesPanel; // RECENTLY REMOVED
    public TextMeshProUGUI remainingEnemiesText;
    public GameObject pressStartPanel;
    public GameObject waveInfoPanel;

    [Header("Maths")]
    public float alpha = 0.05f;
    public float hardMeteorsProportionAlpha = 0.02f;

    // REMOVE
    public int currentWaveNumber;


    // Use this for initialization
    void Start () {
        InitLevels();
        diskUnlockingLevelNbs = new int[]{3,6};
    }
	    
    private void InitLevels()
    {
        if(autoGeneratedLevels)
        {
            levelsList = new List<Level>();
            levelsList.Add(new Level(1, "first level", 10, 1, 1, 1f, new List<GameObject> { EnemiesManager.instance.enemySpaceship_1 }, 15, 0));
        }
    }

    public void StartWaitingForStartPhase()
    {
        currentLevelNumber = 0;
        currentLevel = null;
        currentLevelFinished = true;
        DisplayPressStartIndication();
        //DisplayWaveNumber();
        UpdateRemainingEnnemiesIndicator(); // Useless because hidden
        ChangeNextLevelButtonColor(nextLevelButtonBaseColor);
        nextLevelButton.SetActive(true);

        //remainingEnemiesPanel.SetActive(false); // RECENTLY REMOVED
    }

    public void NewGameSetup()
    {
        StartWaitingForStartPhase();
    }

    public void SetupGameAtLevelIndex(int levelIndex)
    {
        Debug.Log("SetupGameAtLevelIndex [" + levelIndex +"]");
        currentLevelNumber = levelIndex;

        if(levelsList.Count < levelIndex)
        {
            levelsList.Add(CreateNewLevelAtIndex(currentWaveNumber));
        }

        currentLevel = GetLevelByIndex(levelIndex);

        currentLevelAllMeteorsSpawned = true;
        currentLevelFinished = true;
        DisplayPressStartIndication();
        waveInfoPanel.SetActive(true);
        DisplayWaveNumber();
        UpdateRemainingEnnemiesIndicator(); // Useless because hidden
        ChangeNextLevelButtonColor(nextLevelButtonBaseColor);
        nextLevelButton.SetActive(true);
        //remainingEnemiesPanel.SetActive(false); // RECENTLY REMOVED
    }

    public void TriggerLevelStart()
    {
     // TODO
    }

    private Level GetLevelFromNumber(int levelNb)
    {
        return levelsList[levelNb - 1];
    }

    private Level GetLevelByIndex(int index)
    {
        foreach (Level level in levelsList)
        {
            if(level.levelNb == index)
            {
                return level;
            }
        }
        return null;
    }


    private void DisplayWaveNumber()
    {
        waveNumberText.text = currentLevelNumber.ToString();
    }

    public void AllLevelMeteorsDestroyed(int levelId)
    {
        Debug.Log("AllLevelMeteorsDestroyed");
        //currentLevelFinished = true;

        // Tell it to PGSManager to handle achievements
        PGSManager.instance.WaveCompleted(levelId);
    }

    private void GoToNextLevel()
    {
        if(currentLevelNumber < levelsList.Count)
        {
            //Debug.Log("Going to next level | Nb: " + currentLevelNumber);
            currentLevelNumber++;
            currentLevel = GetLevelFromNumber(currentLevelNumber);
            DisplayWaveNumber();
            UpdateRemainingEnnemiesIndicator();
            ChangeNextLevelButtonColor(nextLevelButtonBaseColor);
            nextLevelButton.SetActive(false);
            //remainingEnemiesPanel.SetActive(true); // RECENTLY REMOVED

            LaunchNewWave();
            GatherablesManager.instance.NewWaveActions(currentLevelNumber);
            SpaceshipManager.instance.NewWaveActions(); // Spawns a free spaceship if we got none

            if (OptionsManager.instance.IsTimerOptionEnabled())
            {
                TimeManager.instance.DisplayTimeLeftPanel(false);
            }

            currentLevelFinished = false;

            PopulationManager.instance.NewWaveStarted(currentLevelNumber);
            EventsManager.instance.WaveIndexStarted(currentLevelNumber);
        }
        else
        {
            //Debug.Log("Last level reached !");
            //levelsList.Add(new Level(currentLevelNumber + 1, "Level Nb " + (currentLevelNumber + 1), (currentLevelNumber + 1) * 10, (currentLevelNumber + 1), 1f, new List<GameObject> {EnemiesManager.instance.ennemySpaceship_1 }));
            levelsList.Add(CreateNewLevelAtIndex(currentLevelNumber + 1));
            GoToNextLevel();
        }
    }

    public Level CreateNewLevelAtIndex(int index)
    {
        // Enemies
        List<GameObject> levelEnemies = new List<GameObject> {};
        for (int i = 0; i < index; i++)
        {
            if(i%9 == 0)
            {
                levelEnemies.Add(EnemiesManager.instance.enemySpaceship_1);
            }            
        }

        Level newLevel = new Level(index, "Level Nb " + (index), Mathf.FloorToInt((index) * 10f * MathManager.instance.GetLevelMeteorNbFactor(index)), (index), (1 + .05f * index), (1 + 0.2f * index), levelEnemies, 20+(index * 2), MathManager.instance.GetLevelHardMeteorsProportion(index));

        return newLevel;
    }

    public Level GetLevelFromIndex(int levelIndex)
    {
        Level levelFound = levelsList[0];
        foreach (Level l in levelsList)
        {
            if(l.levelNb == levelIndex)
            {
                levelFound = l;
                break;
            }
        }
        return levelFound;

    }

    // To remove
    public void ResumeNewLevelActionsAfterNewBuildingInfoDisplay()
    {
        LaunchNewWave();
        GatherablesManager.instance.NewWaveActions(currentLevelNumber);
    }

    /*public void IncrementCurrentLevelDestroyedMeteorsNb(int nb)
    {
        // All meteors destroyed
        if(currentLevel.IncrementDestroyedMeteorsNb(nb))
        {
            //StopCurrentLevel();
            ChangeNextLevelButtonColor(nextLevelButtonSecondaryColor);
            //nextLevelButton.SetActive(true);
            //remainingEnnemiesPanel.SetActive(false);
            //Debug.Log("All meteors have been destroyed !");
            //GoToNextLevel();

            AllLevelMeteorsDestroyed();
        }
    }*/

    public void NextLevelRequest()
    {
        if(GameManager.instance.gameState == GameManager.GameState.Default)
        {
            if (currentLevel != null)
            {
                if (currentLevelAllMeteorsSpawned)
                {
                    StopCurrentLevel();
                    GoToNextLevel();
                }
                else
                {
                    Debug.Log("Can't go to next level, current one isn't completed !");
                }
            }
            else
            {
                // Before level one
                pressStartPanel.SetActive(false);
                waveInfoPanel.SetActive(true);
                GoToNextLevel();
            }
        }
        TutorialManager.instance.DisplayIndicator(5, false);
    }

    public void StopCurrentLevel()
    {
        //MeteorsManager.instance.DeleteAllMeteors();
        LevelManager.instance.StopWave();
    }


    public void NextWaveButton()
    {
        LaunchNewWave();
    }

    public void LaunchNewWave()
    {
        currentLevelSpawnedMeteorsNb = 0;
        currentLevelAllMeteorsSpawned = false;
        StartCoroutine(MeteorSpawnCouroutine(currentLevel));
    }

    public void StopWave()
    {
        StopCoroutine(MeteorSpawnCouroutine(null));
    }


    public IEnumerator MeteorSpawnCouroutine(Level level)
    {
        float levelTotalMeteorNb = level.levelMeteorsNb;
        int serieNb = level.meteorSerieNb;
        float hardMeteorsProportion = level.hardMeteorsProportion;
        MeteorsManager.instance.currentSpawnSizeFactor = level.meteorSpawnSizeFactor;
        //Debug.Log("MeteorSpawnCouroutine | TotalNb [" + levelTotalMeteorNb + "] | SerieNb [" + serieNb + "]");

        while (level.spawnedMeteorsNb < levelTotalMeteorNb) // While there is still meteors to spawn
        {
            //Debug.Log("Meteor Spawn Coroutine | SerieNb [" + serieNb + "] | HardMeteorsProportion [" + hardMeteorsProportion + "] | MeteorsLeftToSpawn [" + (levelTotalMeteorNb - level.spawnedMeteorsNb) + "]");
            int nbToSpawn = (int)Mathf.Min(levelTotalMeteorNb - level.spawnedMeteorsNb, serieNb);
            MeteorsManager.instance.SpawnNewMeteors(level.levelNb, nbToSpawn, hardMeteorsProportion);
            IncrementSpawnedMeteorsNb(serieNb);
            level.spawnedMeteorsNb += serieNb;
            yield return new WaitForSeconds(level.timeBetweenSpawns);
        }

        SpawnEnemies();
    }

    public void UpdateRemainingEnnemiesIndicator()
    {
        if(currentLevel != null)
        {
            remainingEnemiesText.text = (currentLevel.levelMeteorsNb - currentLevelSpawnedMeteorsNb).ToString();
        }
        else
        {
            remainingEnemiesText.text = (" - ");
        }
    }

    public void IncrementSpawnedMeteorsNb(int nb)
    {
        currentLevelSpawnedMeteorsNb += nb;
        UpdateRemainingEnnemiesIndicator();

        // Have all meteors been spawned ?
        if (currentLevelSpawnedMeteorsNb >= currentLevel.levelMeteorsNb)
        {
            currentLevelAllMeteorsSpawned = true;
            //currentLevel.levelCompleted = true;
            StopCurrentLevel();
            //remainingEnemiesPanel.SetActive(false); // RECENTLY REMOVED
            Debug.Log("All meteors have been spawned !");

            if (OptionsManager.instance.IsTimerOptionEnabled())
            {
                TimeManager.instance.StartCountdown(currentLevel.countdownTime);
            }
        }
    }

    public void DisplayPressStartIndication()
    {
        pressStartPanel.SetActive(true);
        waveInfoPanel.SetActive(false);
    }

    public void ChangeNextLevelButtonColor(Color color)
    {
        nextLevelButton.GetComponent<Image>().color = color;
    }

    public void SpawnEnemies()
    {
        foreach (GameObject enemy in currentLevel.enemies)
        {
            GameObject instantiatedEnemy = Instantiate(enemy, GeometryManager.instance.RandomSpawnPositionAtRadius(EnemiesManager.instance.enemySpawnRadius), Quaternion.identity);
            instantiatedEnemy.transform.SetParent(EnemiesManager.instance.gameObject.transform);
            EnemiesManager.instance.enemies.Add(instantiatedEnemy);
        }
    }

    public List<BuildingManager.BuildingType> UnlockedBuildingsAtLevelNb(int nb)
    {
        List<BuildingManager.BuildingType> bTypes = new List<BuildingManager.BuildingType>();
        foreach (BuildingManager.BuildingType buildingType in BuildingManager.instance.availableBuildings)
        {
            if(buildingType.unlockedAtLevelNb == nb && !buildingType.isUnlocked)
            {
                bTypes.Add(buildingType);
            }
        }
        return bTypes;
    }

    public Level.LevelData BuildReachedLevelData()
    {
        Level.LevelData reachedLevelData = new Level.LevelData(currentLevelNumber);

        return reachedLevelData;
    }

    public void HalfCurrentLevelMeteorsDestroyed()
    {
        //Debug.Log("HalfLevelMeteorsDestroyed");
        nextLevelButton.SetActive(true);

        ChangeNextLevelButtonColor(nextLevelButtonSecondaryColor);
    }



}
