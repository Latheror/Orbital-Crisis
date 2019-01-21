using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;


public class GameManager : MonoBehaviour {

    public static GameManager instance;
    void Awake(){ 
        if (instance != null){ Debug.LogError("More than one GameManager in scene !"); return; } instance = this;
    }

    public enum GameState { Default, Pause } 
    public enum SelectionState { Default, SpaceshipSelected, ShopItemSelected, BuildingSelected, EnemySelected, PlanetaryShieldSelected, CollectorSelected, DysonSphereSelected }

    [Header("Settings")]
    public float objectsDepthOffset;

    [Header("Operation")]
    public GameState gameState;
    public SelectionState selectionState;
    public bool infiniteMode;

    [Header("World")]
    public GameObject mainPlanet;

    [Header("UI")]
    public GameObject pausePanel;
    public GameObject pauseButton;



	// Use this for initialization
	void Start () {
        gameState = GameState.Default;
        selectionState = SelectionState.Default;
        objectsDepthOffset = mainPlanet.transform.position.z;
	}
	
    public void PauseUnPauseAndDisplayPausePanel()
    {
        if(gameState == GameState.Default)
        {
            gameState = GameState.Pause;
            pausePanel.SetActive(true);
        }
        else if(gameState == GameState.Pause)
        {
            gameState = GameState.Default;
            pausePanel.SetActive(false);
        }
    }

    public void PauseUnPause()
    {
        if (gameState == GameState.Default)
        {
            gameState = GameState.Pause;
        }
        else if (gameState == GameState.Pause)
        {
            gameState = GameState.Default;
        }
    }

    public void UnPause()
    {
        gameState = GameState.Default;
    }

    public void Pause()
    {
        gameState = GameState.Pause;
    }

    public void ResumeButton()
    {
        PauseUnPauseAndDisplayPausePanel();
    }

    public void PauseMenuButtonAction()
    {
        ScenesManager.instance.ChangeFromGameToMenuScene();
    }

    public void ChangeSelectionState(SelectionState state)
    {
        if(selectionState != state)
        {
            selectionState = state;
            switch (state)
            {
                case SelectionState.Default:
                {
                    SpaceshipManager.instance.DeselectSpaceship();
                    BuildingInfoPanel.instance.Deselection();
                    SpaceportInfoPanel.instance.DisplayPanel(false);
                    EnemiesManager.instance.DeselectEnemy();
                    PlanetaryShieldControlPanel.instance.DisplayPanel(false);
                    CollectorControlPanel.instance.DisplayPanel(false);
                    DysonSphereControlPanel.instance.DisplayPanel(false);
                    break;
                }
                case SelectionState.SpaceshipSelected:
                {
                    BuildingInfoPanel.instance.Deselection();
                    SpaceportInfoPanel.instance.DisplayPanel(false);
                    EnemiesManager.instance.DeselectEnemy();
                    PlanetaryShieldControlPanel.instance.DisplayPanel(false);
                    CollectorControlPanel.instance.DisplayPanel(false);
                    DysonSphereControlPanel.instance.DisplayPanel(false);
                    break;
                }
                case SelectionState.BuildingSelected:
                {
                    SpaceshipManager.instance.DeselectSpaceship();
                    SpaceportInfoPanel.instance.DisplayPanel(false);
                    EnemiesManager.instance.DeselectEnemy();
                    PlanetaryShieldControlPanel.instance.DisplayPanel(false);
                    CollectorControlPanel.instance.DisplayPanel(false);
                    DysonSphereControlPanel.instance.DisplayPanel(false);
                    break;
                }
                case SelectionState.ShopItemSelected:
                {
                    SpaceshipManager.instance.DeselectSpaceship();
                    BuildingInfoPanel.instance.Deselection();
                    SpaceportInfoPanel.instance.DisplayPanel(false);
                    EnemiesManager.instance.DeselectEnemy();
                    PlanetaryShieldControlPanel.instance.DisplayPanel(false);
                    CollectorControlPanel.instance.DisplayPanel(false);
                    DysonSphereControlPanel.instance.DisplayPanel(false);
                    break;
                }
                case SelectionState.EnemySelected:
                {
                    SpaceshipManager.instance.DeselectSpaceship();
                    BuildingInfoPanel.instance.Deselection();
                    SpaceportInfoPanel.instance.DisplayPanel(false);
                    PlanetaryShieldControlPanel.instance.DisplayPanel(false);
                    CollectorControlPanel.instance.DisplayPanel(false);
                    DysonSphereControlPanel.instance.DisplayPanel(false);
                    break;
                }
                case SelectionState.PlanetaryShieldSelected:
                {
                    SpaceshipManager.instance.DeselectSpaceship();
                    BuildingInfoPanel.instance.Deselection();
                    SpaceportInfoPanel.instance.DisplayPanel(false);
                    EnemiesManager.instance.DeselectEnemy();
                    CollectorControlPanel.instance.DisplayPanel(false);
                    DysonSphereControlPanel.instance.DisplayPanel(false);
                    break;
                }
                case SelectionState.CollectorSelected:
                {
                    SpaceshipManager.instance.DeselectSpaceship();
                    BuildingInfoPanel.instance.Deselection();
                    SpaceportInfoPanel.instance.DisplayPanel(false);
                    EnemiesManager.instance.DeselectEnemy();
                    PlanetaryShieldControlPanel.instance.DisplayPanel(false);
                    DysonSphereControlPanel.instance.DisplayPanel(false);
                    break;
                }
                case SelectionState.DysonSphereSelected:
                {
                    SpaceshipManager.instance.DeselectSpaceship();
                    BuildingInfoPanel.instance.Deselection();
                    SpaceportInfoPanel.instance.DisplayPanel(false);
                    EnemiesManager.instance.DeselectEnemy();
                    PlanetaryShieldControlPanel.instance.DisplayPanel(false);
                    CollectorControlPanel.instance.DisplayPanel(false);
                    break;
                }
            }
        }
        else
        {

        }
    }

    public void SetInfiniteMode(bool enable)
    {
        infiniteMode = enable;
    }

    public bool IsInfiniteModeEnabled()
    {
        return infiniteMode;
    }


    [Serializable]
    public class GeneralGameData
    {
        public int levelReached;
        public int unlockedDisks;
        public int score;
        public int hits;
        public int experiencePoints;
        public int artifactsNb;
        public bool timerEnabled;

        public GeneralGameData(int currentLevelNb, int unlockedOrbits, int score, int hits, int experiencePoints, int artifactsNb, bool timerOptionEnabled)
        {
            this.levelReached = currentLevelNb;
            this.unlockedDisks = unlockedOrbits;
            this.score = score;
            this.hits = hits;
            this.experiencePoints = experiencePoints;
            this.artifactsNb = artifactsNb;
            this.timerEnabled = timerOptionEnabled;
        }
    }
}
