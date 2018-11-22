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
    public GameObject remainingEnnemiesPanel;
    public TextMeshProUGUI remainingEnnemiesText;
    public GameObject pressStartPanel;
    public GameObject waveInfoPanel;

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
            levelsList.Add(new Level(1, "first level", 10, 1, 1f, new List<GameObject>{EnemiesManager.instance.enemySpaceship_1}));
            levelsList.Add(new Level(2, "second level", 20, 2, 1.1f, new List<GameObject> { EnemiesManager.instance.enemySpaceship_1 }));
            levelsList.Add(new Level(3, "third level", 30, 3, 1.2f, new List<GameObject> { EnemiesManager.instance.enemySpaceship_1 }));
            levelsList.Add(new Level(4, "fourth level", 40, 4, 1.3f, new List<GameObject> { EnemiesManager.instance.enemySpaceship_1, EnemiesManager.instance.enemySpaceship_1 }));
            levelsList.Add(new Level(5, "fifth level", 50, 5, 1.4f, new List<GameObject> { EnemiesManager.instance.enemySpaceship_1, EnemiesManager.instance.enemySpaceship_1 }));
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
        remainingEnnemiesPanel.SetActive(false);
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
        remainingEnnemiesPanel.SetActive(false);
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

    private void AllLevelMeteorsDestroyed()
    {
        Debug.Log("AllLevelMeteorsDestroyed");

        // Get unlocked building(s)
        List<BuildingManager.BuildingType> unlockedBuildings = UnlockedBuildingsAtLevelNb(currentLevelNumber);
        if (unlockedBuildings.Count > 0)
        {
            Debug.Log("Buildings are unlocked at the end of this level.");

            // Unlock building(s)
            BuildingManager bManager = BuildingManager.instance;
            foreach (BuildingManager.BuildingType bType in unlockedBuildings)
            {
                if (!bType.isUnlocked && bType.unlockedAtLevelNb == currentLevelNumber)
                {
                    bManager.UnlockBuildingType(bType);
                }
            }

            EventsInfoManager.instance.DisplayNewBuildingsInfo(unlockedBuildings);
        }

        nextLevelButton.SetActive(true);

        // Unlock satellite disks (Temporary solution)
        if ((diskUnlockingLevelNbs.Length >= SurroundingAreasManager.instance.unlockedDisksNb) && (diskUnlockingLevelNbs[SurroundingAreasManager.instance.unlockedDisksNb - 1] == currentLevelNumber))
        {
            SurroundingAreasManager.instance.UnlockNextDisk();
        }

        currentLevelFinished = true;
    }

    private void GoToNextLevel()
    {
        if(currentLevelNumber < levelsList.Count)
        {
            Debug.Log("Going to next level | Nb: " + currentLevelNumber);
            currentLevelNumber++;
            currentLevel = GetLevelFromNumber(currentLevelNumber);
            DisplayWaveNumber();
            UpdateRemainingEnnemiesIndicator();
            ChangeNextLevelButtonColor(nextLevelButtonBaseColor);
            nextLevelButton.SetActive(false);
            remainingEnnemiesPanel.SetActive(true);

            LaunchNewWave();
            GatherablesManager.instance.NewWaveActions(currentLevelNumber);

            currentLevelFinished = false;
        }
        else
        {
            Debug.Log("Last level reached !");
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
            if(i%4 == 0)
            {
                levelEnemies.Add(EnemiesManager.instance.enemySpaceship_1);
            }            
        }

        Level newLevel = new Level(currentLevelNumber + 1, "Level Nb " + (index), (index) * 10, (index), 1f, levelEnemies);

        return newLevel;
    }

    // To remove
    public void ResumeNewLevelActionsAfterNewBuildingInfoDisplay()
    {
        LaunchNewWave();
        GatherablesManager.instance.NewWaveActions(currentLevelNumber);
    }

    public void IncrementCurrentLevelDestroyedMeteorsNb(int nb)
    {
        if(currentLevel.IncrementDestroyedMeteorsNb(nb))
        {
            //StopCurrentLevel();
            ChangeNextLevelButtonColor(nextLevelButtonSecondaryColor);
            //nextLevelButton.SetActive(true);
            //remainingEnnemiesPanel.SetActive(false);
            Debug.Log("All meteors have been destroyed !");
            //GoToNextLevel();

            AllLevelMeteorsDestroyed();
        }
    }

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
        StartCoroutine(MeteorSpawnCouroutine(currentLevel.levelMeteorsNb, currentLevel.meteorSerieNb, currentLevel.timeBetweenSpawns));
    }

    public void StopWave()
    {
        StopCoroutine(MeteorSpawnCouroutine(0, 0, 0));
    }


    public IEnumerator MeteorSpawnCouroutine(int totalNb, int serieNb, float delay)
    {
        while (totalNb > 0)
        {
            //Debug.Log("Meteor Spawn Coroutine.");
            MeteorsManager.instance.SpawnNewMeteors(serieNb);
            IncrementSpawnedMeteorsNb(serieNb);
            totalNb -= serieNb;
            yield return new WaitForSeconds(delay);
        }

        SpawnEnemies();
    }

    public void UpdateRemainingEnnemiesIndicator()
    {
        if(currentLevel != null)
        {
            remainingEnnemiesText.text = (currentLevel.levelMeteorsNb - currentLevelSpawnedMeteorsNb).ToString();
        }
        else
        {
            remainingEnnemiesText.text = (" - ");
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
            remainingEnnemiesPanel.SetActive(false);
            Debug.Log("All meteors have been spawned !");
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
            GameObject instantiatedEnemy = Instantiate(enemy, GeometryManager.instance.RandomSpawnPosition(), Quaternion.identity);
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



}
