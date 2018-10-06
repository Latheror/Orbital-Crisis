using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpaceshipManager : MonoBehaviour {

    [Header("Operation")]
    public GameObject selectedSpaceship;
    public GameObject currentSelectedSpaceshipInfoPanel;
    public List<GameObject> alliedSpaceships;

    public static SpaceshipManager instance;
    void Awake()
    {
        if (instance != null) { Debug.LogError("More than one SpaceshipManager in scene !"); return; }
        instance = this;
    }

    public GameObject mainSpaceship;

    // Use this for initialization
    void Start () {
        mainSpaceship.GetComponent<Spaceship>().isActivated = true;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void SelectSpaceship(GameObject spaceship)
    {
        selectedSpaceship = spaceship;
        GameManager.instance.selectionState = GameManager.SelectionState.SpaceShipSelected;
        spaceship.GetComponent<Spaceship>().Highlight();
        spaceship.GetComponent<Spaceship>().infoPanel.SetActive(true);
        currentSelectedSpaceshipInfoPanel = spaceship.GetComponent<Spaceship>().infoPanel;
    }

    public void DeselectSpaceship()
    {
        if (selectedSpaceship != null)
        {
            selectedSpaceship.GetComponent<Spaceship>().infoPanel.SetActive(false);
            currentSelectedSpaceshipInfoPanel = null;
            selectedSpaceship = null;
            GameManager.instance.selectionState = GameManager.SelectionState.Default;
        }
    }

    public void SetSelectionState(GameManager.SelectionState selectionState)
    {
        GameManager.instance.selectionState = selectionState;
        if (selectionState == GameManager.SelectionState.Default)
        {
            DeselectSpaceship();
        }
    }
}
