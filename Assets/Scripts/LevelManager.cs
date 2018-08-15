using System.Collections;
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
        StartWaitingForStartPhase();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    
    private void InitLevels()
    {
        levelsList = new List<Level>();
        levelsList.Add(new Level(1, "first level", 5));
        levelsList.Add(new Level(2, "second level", 10));
        levelsList.Add(new Level(3, "third level", 20));
        levelsList.Add(new Level(4, "fourth level", 30));
        levelsList.Add(new Level(5, "fifth level", 50));
    }

    public void StartWaitingForStartPhase()
    {
        currentLevelNumber = 0;
        currentLevel = null;
        DisplayPressStartIndication();
        //DisplayWaveNumber();
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


    private void DisplayWaveNumber()
    {
        waveNumberText.text = currentLevelNumber.ToString();
    }

    private void GoToNextLevel()
    {
        if(currentLevelNumber < levelsList.Count)
        {
            Debug.Log("Going to next level.");
            currentLevelNumber++;
            currentLevel = GetLevelFromNumber(currentLevelNumber);
            DisplayWaveNumber();
            UpdateRemainingEnnemiesIndicator();
            ChangeNextLevelButtonColor(nextLevelButtonBaseColor);
            nextLevelButton.SetActive(false);
            remainingEnnemiesPanel.SetActive(true);
            LaunchNewWave();
        }
        else
        {
            Debug.Log("Last level reached !");
        }
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
        }
    }

    public void NextLevelRequest()
    {
        if(currentLevel != null)
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
            // We are supposed to be before level 1 here. Other cases should be investigated
            pressStartPanel.SetActive(false);
            waveInfoPanel.SetActive(true);
            GoToNextLevel();
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
        StartCoroutine(MeteorSpawnCouroutine(currentLevel.levelMeteorsNb, 1f));
    }

    public void StopWave()
    {
        StopCoroutine(MeteorSpawnCouroutine(0, 0));
    }


    public IEnumerator MeteorSpawnCouroutine(int nb, float delay)
    {
        while (nb > 0)
        {
            //Debug.Log("Meteor Spawn Coroutine.");
            MeteorsManager.instance.SpawnNewMeteor();
            IncrementSpawnedMeteorsNb(1);
            nb--;
            yield return new WaitForSeconds(delay); // waits 3 seconds
        }
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
            currentLevel.levelCompleted = true;
            StopCurrentLevel();
            nextLevelButton.SetActive(true);
            remainingEnnemiesPanel.SetActive(false);
            Debug.Log("All meteors have been destroyed !");
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



}
