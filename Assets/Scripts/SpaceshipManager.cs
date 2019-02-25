using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class SpaceshipManager : MonoBehaviour {

    public static SpaceshipManager instance;
    void Awake()
    {
        if (instance != null) { Debug.LogError("More than one SpaceshipManager in scene !"); return; }
        instance = this;
    }

    [Header("Prefabs")]
    public GameObject corvettePrefab, cruiserPrefab;

    [Header("Resources")]
    public Sprite oneStarSprite;
    public Sprite twoStarsSprite;
    public Sprite threeStarsSprite;

    [Header("Settings")]
    public GameObject newGameSpaceshipPosition;
    public GameObject alliedSpaceship1_Prefab;
    public GameObject mainSpaceship1_Prefab;
    public float avoidOtherAlliesDistance = 50f;
    public List<SpaceshipType> spaceshipTypes;
    public int startFleetPoints = 1;
    public Transform spaceshipsParent;

    [Header("Operation")]
    public GameObject selectedSpaceship;
    public GameObject currentSelectedSpaceshipInfoPanel;
    public List<GameObject> allySpaceships;
    public SpaceshipData[] spaceshipsData;
    public int currentMaxFleetPoints;
    public int availableFleetPoints;
    public int usedFleetPoints;

    // Use this for initialization
    void Start() {
        Initialize();
    }

    public void Initialize()
    {
        DefineAvailableSpaceships();
        currentMaxFleetPoints = startFleetPoints;
        UpdateFleetPointsInfo();
    }

    public void DefineAvailableSpaceships()
    {
        spaceshipTypes.Add(new SpaceshipType(1, "Corvette", corvettePrefab, "corvette", new List<ResourcesManager.ResourceAmount>(){
                                                                             new ResourcesManager.ResourceAmount("steel", 1400),
                                                                             new ResourcesManager.ResourceAmount("carbon", 1200),
                                                                             new ResourcesManager.ResourceAmount("composite", 1000),
                                                                             new ResourcesManager.ResourceAmount("electronics", 800)
                                                                        }, true, 1, new int[] { 3000, 8000, 19999}, 1.25f));
        spaceshipTypes.Add(new SpaceshipType(2, "Cruiser", cruiserPrefab, "cruiser", new List<ResourcesManager.ResourceAmount>(){
                                                                             new ResourcesManager.ResourceAmount("steel", 2500),
                                                                             new ResourcesManager.ResourceAmount("carbon", 2000),
                                                                             new ResourcesManager.ResourceAmount("composite", 1200),
                                                                             new ResourcesManager.ResourceAmount("electronics", 1200)
                                                                        }, true, 3, new int[] { 6000, 16000, 29999 }, 1.25f));
    }

    public SpaceshipType GetSpaceshipTypeByName(string name)
    {
        SpaceshipType st_found = null;
        foreach (SpaceshipType st in spaceshipTypes)
        {
            if(st.typeName == name)
            {
                st_found = st;
                break;
            }
        }
        return st_found;
    }

    public SpaceshipType GetSpaceshipTypeByIndex(int index)
    {
        SpaceshipType st_found = null;
        foreach (SpaceshipType st in spaceshipTypes)
        {
            if (st.index == index)
            {
                st_found = st;
                break;
            }
        }
        return st_found;
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
        int spaceshipsNb = allySpaceships.Count;
        Debug.Log("BuildSpaceshipsData | Nb: " + spaceshipsNb);
        spaceshipsData = new SpaceshipData[spaceshipsNb];

        for (int i = 0; i < allySpaceships.Count; i++)
        {
            AllySpaceship allyS = allySpaceships[i].GetComponent<AllySpaceship>();
            spaceshipsData[i] = (new SpaceshipData(allyS.spaceshipType.index, new GeometryManager.Position(allySpaceships[i].transform.position), allyS.level, allyS.experiencePoints));
            Debug.Log("Adding spaceship [" + i + "] | TypeIndex [" + allyS.spaceshipType.index + "] | Level [" + allyS.level + "] | Exp [" + allyS.experiencePoints + "]");
        }

        return spaceshipsData;
    }

    public void NewGameSetupActions()
    {
        // Instantiate Main Spaceship
        SpawnSpaceshipOfType(GetSpaceshipTypeByName("Corvette"));
    }

    public GameObject InstantiateSpaceshipAtPosition(GameObject spaceshipPrefab, Vector3 pos)
    {
        GameObject instantiatedSpaceship = Instantiate(spaceshipPrefab, pos, Quaternion.Euler(0, 90, 0));
        // Attribute ID to spaceship
        instantiatedSpaceship.GetComponent<AllySpaceship>().id = GetAvailableSpaceshipId();
        AddAlliedSpaceshipToList(instantiatedSpaceship);

        return instantiatedSpaceship;
    }

    public void AddAlliedSpaceshipToList(GameObject alliedSpaceship)
    {
        allySpaceships.Add(alliedSpaceship);
    }

    public GameObject SpawnSpaceshipTypeAtPos(SpaceshipType sType, GeometryManager.Position pos)
    {
        //Debug.Log("Spawning saved spaceship");
        GameObject instantiatedSpaceship = Instantiate(sType.prefab, new Vector3(pos.x, pos.y, pos.z), Quaternion.identity);

        // Attribute ID to spaceship
        AllySpaceship allyS = instantiatedSpaceship.GetComponent<AllySpaceship>();
        allyS.Initialize();
        allyS.id = GetAvailableSpaceshipId();
        allyS.spaceshipType = sType;

        AddAlliedSpaceshipToList(instantiatedSpaceship);

        instantiatedSpaceship.transform.SetParent(spaceshipsParent.transform);

        UpdateFleetPointsInfo();

        return instantiatedSpaceship;
    }

    public GameObject SpawnSavedSpaceship(SpaceshipData sData)
    {
        //Debug.Log("Spawning saved spaceship");
        GameObject instantiatedSpaceship = Instantiate(GetSpaceshipTypeByIndex(sData.spaceshipTypeIndex).prefab, new Vector3(sData.position.x, sData.position.y, sData.position.z), Quaternion.identity);

        // Attribute ID to spaceship
        AllySpaceship allyS = instantiatedSpaceship.GetComponent<AllySpaceship>();
        allyS.Initialize();
        allyS.id = GetAvailableSpaceshipId();
        allyS.spaceshipType = GetSpaceshipTypeByIndex(sData.spaceshipTypeIndex);
        allyS.level = sData.level;
        allyS.experiencePoints = sData.experiencePoints;

        AddAlliedSpaceshipToList(instantiatedSpaceship);

        instantiatedSpaceship.transform.SetParent(spaceshipsParent.transform);

        UpdateFleetPointsInfo();

        return instantiatedSpaceship;
    }

    public void SetupSavedSpaceships(SpaceshipData[] spaceshipsData)
    {
        for (int i = 0; i < spaceshipsData.Length; i++)
        {
            Debug.Log("SetupSavedSpaceships | " + spaceshipsData[i].ToString());
            SpawnSavedSpaceship(spaceshipsData[i]);
        }
    }

    public int GetAvailableSpaceshipId()
    {
        int idFound = -1;
        int index = 1;
        bool isIdFound = false;
        while (!isIdFound)
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
        foreach (GameObject spaceship in allySpaceships)
        {
            if (spaceship.GetComponent<AllySpaceship>().id == id)
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
        foreach (GameObject otherAlliedSpaceship in allySpaceships)
        {
            if ((otherAlliedSpaceship != referenceAlliedSpaceship) && (referenceAlliedSpaceship.transform.position - otherAlliedSpaceship.transform.position).sqrMagnitude < avoidOtherAlliesDistance*avoidOtherAlliesDistance)
            {
                otherAlly = otherAlliedSpaceship;
                break;
            }
        }
        return otherAlly;
    }

    public bool HasAvailableFleetPointsNb(int nb)
    {
        return (nb <= availableFleetPoints);
    }

    public void UpdateFleetPointsInfo()
    {
        int tmpUsedFleetPoints = 0;
        foreach (GameObject allySpaceship in allySpaceships)
        {
            tmpUsedFleetPoints += allySpaceship.GetComponent<AllySpaceship>().spaceshipType.fleetPointsNeeded;
        }

        usedFleetPoints = tmpUsedFleetPoints;
        availableFleetPoints = currentMaxFleetPoints - usedFleetPoints;
    }

    public void RemoveSpaceship(GameObject spaceshipToRemove)
    {
        allySpaceships.Remove(spaceshipToRemove);
        UpdateFleetPointsInfo();
    }

    public void SpawnSpaceshipOfType(SpaceshipType spaceshipType)
    {
        Debug.Log("SpawnSpaceshipOfType [" + spaceshipType.typeName + "]");

        GameObject instantiatedSpaceship = Instantiate(spaceshipType.prefab, newGameSpaceshipPosition.transform.position, Quaternion.Euler(0, 90, 0));

        // Set attributes
        AllySpaceship spaceship = instantiatedSpaceship.GetComponent<AllySpaceship>();
        spaceship.Initialize();
        spaceship.SetSpaceshipType(spaceshipType);
        spaceship.homeSpaceport = null;

        instantiatedSpaceship.transform.SetParent(spaceshipsParent);

        AddAlliedSpaceshipToList(instantiatedSpaceship);
        UpdateFleetPointsInfo();
    }

    public void SetCurrentMaxFleetPoints(int fPoints)
    {
        currentMaxFleetPoints = fPoints;
        UpdateFleetPointsInfo();
    }

    public void NewWaveActions()
    {
        if(allySpaceships.Count < 1)
        {
            SpawnSpaceshipOfType(GetSpaceshipTypeByIndex(1));
            EventsManager.instance.StartFreeSpaceshipPanelAnimation();
        }
    }

    [Serializable]
    public class SpaceshipData
    {
        public int spaceshipTypeIndex;
        public GeometryManager.Position position;
        public int level;
        public int experiencePoints;

        public SpaceshipData(int spaceshipTypeIndex, GeometryManager.Position position, int level, int experiencePoints)
        {
            this.spaceshipTypeIndex = spaceshipTypeIndex;
            this.position = position;
            this.level = level;
            this.experiencePoints = experiencePoints;
        }

        public override string ToString()
        {
            return ("SpaceshipData | Type [" + spaceshipTypeIndex + "] | Level [" + level + "] | Exp [" + experiencePoints + "]");
        }
    }

    [System.Serializable]
    public class SpaceshipType
    {
        public int index;
        public string typeName;
        public GameObject prefab;
        public List<ResourcesManager.ResourceAmount> resourceCosts;
        public bool isAlly;
        public GameObject associatedSpaceshipShopItem;
        public int fleetPointsNeeded;
        public int[] levelExperiencePointLimits;
        public Sprite sprite;
        public float levelUpStatIncreaseFactor;

        public SpaceshipType(int index, string name, GameObject prefab, string imageName, List<ResourcesManager.ResourceAmount> resourceCosts, bool isAlly, int fleetPointsNeeded, int[] levelExperiencePointLimits, float levelUpStatIncreaseFactor)
        {
            this.index = index;
            this.typeName = name;
            this.prefab = prefab;
            this.resourceCosts = resourceCosts;
            this.isAlly = isAlly;
            this.fleetPointsNeeded = fleetPointsNeeded;
            this.levelExperiencePointLimits = levelExperiencePointLimits;
            this.sprite = Resources.Load<Sprite>("Images/Spaceships/" + imageName);
            this.levelUpStatIncreaseFactor = levelUpStatIncreaseFactor;
        }

        public void SetAssociatedSpaceshipShopItem(GameObject item)
        {
            associatedSpaceshipShopItem = item;
        }
    }

    public class SpaceshipLevel
    {
        public int levelId;
        public int experienceBeforeNextLevel;
        public bool isMaximumLevel;

        public SpaceshipLevel(int levelId, int experienceBeforeNextLevel, bool isMaximumLevel)
        {
            this.levelId = levelId;
            this.experienceBeforeNextLevel = experienceBeforeNextLevel;
            this.isMaximumLevel = isMaximumLevel;
        }
    }
}