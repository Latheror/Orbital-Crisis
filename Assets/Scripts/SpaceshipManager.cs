using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpaceshipManager : MonoBehaviour {

    public GameObject selectedSpaceship;
    public GameObject currentSelectedSpaceshipInfoPanel;

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
        selectedSpaceship = null;
        GameManager.instance.selectionState = GameManager.SelectionState.Default;
    }
}
