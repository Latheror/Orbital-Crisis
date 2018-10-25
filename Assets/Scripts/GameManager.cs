using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class GameManager : MonoBehaviour {

    public static GameManager instance;
    void Awake(){ 
        if (instance != null){ Debug.LogError("More than one GameManager in scene !"); return; } instance = this;
    }

    public enum GameState { Default, Pause } 
    public enum SelectionState { Default, SpaceshipSelected, ShopItemSelected, BuildingSelected }

    [Header("Settings")]
    public float objectsDepthOffset;

    [Header("Operation")]
    public GameState gameState;
    public SelectionState selectionState;

    [Header("World")]
    public GameObject mainPlanet;

    [Header("UI")]
    public GameObject pausePanel;
    public GameObject pauseButton;



	// Use this for initialization
	void Start () {
        Screen.orientation = ScreenOrientation.AutoRotation;
        gameState = GameState.Default;
        selectionState = SelectionState.Default;
        objectsDepthOffset = mainPlanet.transform.position.z;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void PauseUnPause()
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

    public void ResumeButton()
    {
        PauseUnPause();
    }

    public void PauseMenuButtonAction()
    {
        SceneManager.LoadSceneAsync(0);
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
                    break;
                }
                case SelectionState.SpaceshipSelected:
                {
                    BuildingInfoPanel.instance.Deselection();
                    SpaceportInfoPanel.instance.DisplayPanel(false);
                    break;
                }
                case SelectionState.BuildingSelected:
                {
                    SpaceshipManager.instance.DeselectSpaceship();
                    break;
                }
                case SelectionState.ShopItemSelected:
                {
                    SpaceshipManager.instance.DeselectSpaceship();
                    BuildingInfoPanel.instance.Deselection();
                    SpaceportInfoPanel.instance.DisplayPanel(false);
                    break;
                }
            }
        }
        else
        {

        }
    }
}
