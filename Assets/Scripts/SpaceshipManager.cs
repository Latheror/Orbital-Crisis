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
        GameManager.instance.ChangeSelectionState(GameManager.SelectionState.SpaceshipSelected);
        spaceship.GetComponent<Spaceship>().Highlight();

        SpaceshipInfoPanel.instance.SelectSpaceshipActions();
    }

    public void DeselectSpaceship()
    {
        if (selectedSpaceship != null)
        {
            selectedSpaceship = null;
        }
        SpaceshipInfoPanel.instance.DeselectSpaceshipActions();
    }
}
