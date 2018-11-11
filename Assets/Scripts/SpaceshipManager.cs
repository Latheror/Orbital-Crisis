using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class SpaceshipManager : MonoBehaviour {

    [Header("Settings")]
    public GameObject newGameSpaceshipPosition;
    public GameObject alliedSpaceship1_Prefab;

    [Header("Operation")]
    public GameObject selectedSpaceship;
    public GameObject currentSelectedSpaceshipInfoPanel;
    public List<GameObject> alliedSpaceships;
    public SpaceshipData[] spaceshipsData;

    public static SpaceshipManager instance;
    void Awake()
    {
        if (instance != null) { Debug.LogError("More than one SpaceshipManager in scene !"); return; }
        instance = this;
    }

    // Use this for initialization
    void Start () {
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

    public SpaceshipData[] BuildSpaceshipsData()
    {
        int spaceshipsNb = alliedSpaceships.Count;
        Debug.Log("BuildSpaceshipsData | Nb: " + spaceshipsNb);
        spaceshipsData = new SpaceshipData[spaceshipsNb];

        for(int i=0; i<alliedSpaceships.Count; i++)
        {
            spaceshipsData[i] = (new SpaceshipData(1, new GeometryManager.Position(alliedSpaceships[i].transform.position)));
            Debug.Log("Adding spaceship [" + i + "]");
        }

        return spaceshipsData;
    }

    public void NewGameSetupActions()
    {
        InstantiatedSpaceshipAtPosition(alliedSpaceship1_Prefab, newGameSpaceshipPosition.transform.position);
    }

    public void InstantiatedSpaceshipAtPosition(GameObject spaceshipPrefab, Vector3 pos)
    {
        GameObject instantiatedSpaceship = Instantiate(spaceshipPrefab, pos, Quaternion.identity);
        AddAlliedSpaceshipToList(instantiatedSpaceship);
    }

    public void AddAlliedSpaceshipToList(GameObject alliedSpaceship)
    {
        alliedSpaceships.Add(alliedSpaceship);
    }

    public void SpawnSpaceshipAtPos(GeometryManager.Position pos)
    {
        Debug.Log("Spawning saved spaceship");
        GameObject instantiatedSpaceship = Instantiate(alliedSpaceship1_Prefab, new Vector3(pos.x, pos.y, pos.z), Quaternion.identity);
        AddAlliedSpaceshipToList(instantiatedSpaceship);
    }

    public void SetupSavedSpaceships(SpaceshipData[] spaceshipsData)
    {
        for(int i=0; i<spaceshipsData.Length; i++)
        {
            SpawnSpaceshipAtPos(spaceshipsData[i].position);
        }
    }

    [Serializable]
    public class SpaceshipData
    {
        public int spaceshipTypeIndex;
        public GeometryManager.Position position;

        public SpaceshipData(int spaceshipTypeIndex, GeometryManager.Position position)
        {
            this.spaceshipTypeIndex = spaceshipTypeIndex;
            this.position = position;
        }
    }

}
