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

    public enum SelectionState { Default, SpaceShipSelected }

    public GameState gameState;
    public SelectionState selectionState;
    public GameObject mainPlanet;
    public float objectsDepthOffset;
    public GameObject pausePanel;
    public GameObject pauseButton;



	// Use this for initialization
	void Start () {
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
}
