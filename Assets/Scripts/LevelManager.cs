using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LevelManager : MonoBehaviour {

    [Header("Levels")]
    public List<Level> levelsList = new List<Level>();
    public int currentLevelNumber = 0;
    public Level currentLevel = null;

    [Header("UI")]
    public TextMeshProUGUI waveNumberText;


	// Use this for initialization
	void Start () {
        InitLevels();
        StartAtFirstLevel();
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

    public void StartAtFirstLevel()
    {
        currentLevelNumber = 1;
        currentLevel = levelsList[0]; 
        DisplayWaveNumber(); 
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
            currentLevelNumber++;
            currentLevel = GetLevelFromNumber(currentLevelNumber);
            DisplayWaveNumber();
        }
        else
        {
            Debug.Log("Last level reached !");
        }
    }






}
