using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class SpaceshipManager : MonoBehaviour {

    [Header("Settings")]
    public GameObject newGameSpaceshipPosition;
    public GameObject alliedSpaceship1_Prefab;

    public float avoidOtherAlliesDistance = 50f;

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
        GameObject instantiatedSpaceship = Instantiate(spaceshipPrefab, pos, Quaternion.Euler(0,90,0));
        // Attribute ID to spaceship
        instantiatedSpaceship.GetComponent<AlliedSpaceship>().id = GetAvailableSpaceshipId();
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

        // Attribute ID to spaceship
        instantiatedSpaceship.GetComponent<AlliedSpaceship>().id = GetAvailableSpaceshipId();

        AddAlliedSpaceshipToList(instantiatedSpaceship);
    }

    public void SetupSavedSpaceships(SpaceshipData[] spaceshipsData)
    {
        for(int i=0; i<spaceshipsData.Length; i++)
        {
            SpawnSpaceshipAtPos(spaceshipsData[i].position);
        }
    }

    public int GetAvailableSpaceshipId()
    {
        int idFound = -1;
        int index = 1;
        bool isIdFound = false;
        while(!isIdFound)
        {
            if (IsSpaceshipIdAvailable(index))
            {
                idFound = index;
                isIdFound = true;
                break;
            }
            else
            {
                index++;
            }
        }
        return idFound;
    }

    public bool IsSpaceshipIdAvailable(int id)
    {
        bool available = true;
        foreach (GameObject spaceship in alliedSpaceships)
        {
            if(spaceship.GetComponent<AlliedSpaceship>().id == id)
            {
                available = false;
                break;
            }
        }
        return available;
    }

    public GameObject IsOtherAllyInRange(GameObject referenceAlliedSpaceship)
    {
        GameObject otherAlly = null;
        foreach (GameObject otherAlliedSpaceship in alliedSpaceships)
        {
            if((otherAlliedSpaceship != referenceAlliedSpaceship) && Vector3.Distance(referenceAlliedSpaceship.transform.position, otherAlliedSpaceship.transform.position) < avoidOtherAlliesDistance)
            {
                otherAlly = otherAlliedSpaceship;
                break;
            }
        }
        return otherAlly;
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
