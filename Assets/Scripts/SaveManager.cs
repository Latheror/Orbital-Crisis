using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public class SaveManager : MonoBehaviour {

    public static SaveManager instance;
    void Awake()
    {
        if (instance != null) { Debug.LogError("More than one SaveManager in scene !"); return; }
        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public string buildingsSaveFile = "buildings.sav";
    public string generalSaveFile = "general.sav";

    void Start () {

    }
	
	void Update () {
		
	}

    public void SaveButtonClicked()
    {
        SaveGameState();
    }

    public void LoadButtonClicked()
    {
        LoadGameState();
    }

    public void SaveGameState()
    {
        Debug.Log("Saving game...");
        SaveGameVariables();
        SaveBuildings();
    }

    public void LoadGameState()
    {
        InfrastructureManager.instance.ClearBuildings();
        LoadGameVariables();
        LoadBuildings();
    }

    public void SaveBuildings()
    {
        int buildingNb = BuildingManager.instance.buildingList.Count;
        Debug.Log("Saving [" + buildingNb + "] buildings...");

        // Binary Formatter + Stream
        BinaryFormatter bf = new BinaryFormatter();
        FileStream stream = new FileStream(Application.persistentDataPath + "/" + buildingsSaveFile, FileMode.Create);

        // BuildingData List
        Building.BuildingData[] buildingDatas = new Building.BuildingData[buildingNb];

        for (int i = 0; i<buildingNb; i++)
        {
            Debug.Log("Saving [" + BuildingManager.instance.buildingList[i].GetComponent<Building>().buildingType.name + "]");

            Building.BuildingData bData = new Building.BuildingData(BuildingManager.instance.buildingList[i].GetComponent<Building>());

            buildingDatas[i] = bData;

        }

        bf.Serialize(stream, buildingDatas);
        stream.Close();
    }

    public void LoadBuildings()
    {
        Debug.Log("LoadBuildings...");
        if (File.Exists(Application.persistentDataPath + "/" + buildingsSaveFile))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream stream = new FileStream(Application.persistentDataPath + "/" + buildingsSaveFile, FileMode.Open);

            Building.BuildingData[] bData = bf.Deserialize(stream) as Building.BuildingData[];

            stream.Close();

            for(int i=0; i<bData.Length; i++)
            {
                // Retrieve info
                int buildingTypeID = bData[i].buildingTypeID;
                int buildingSlotID = bData[i].buildingSlotID;

                // Build building
                Debug.Log("LoadBuildings | Building [" + BuildingManager.instance.GetBuildingTypeByID(buildingTypeID).name + "]");
                BuildingManager.instance.BuildBuildingOnSlot(BuildingManager.instance.GetBuildingTypeByID(buildingTypeID), BuildingSlotManager.instance.GetBuildingSlotByID(buildingSlotID));
            }

            
        }
        else
        {
            Debug.LogError("LoadBuilding | File doesn't exist !");
        }
    }

    public void SaveGameVariables()
    {
        int levelReached = LevelManager.instance.currentLevelNumber;
        int unlockedDisksNb = SurroundingAreasManager.instance.unlockedDisksNb;

        Debug.Log("Saving game variables | LevelReached [" + levelReached + "] | UnlockedDisksNb [" + unlockedDisksNb + "]");

        // Binary Formatter + Stream
        BinaryFormatter bf = new BinaryFormatter();
        FileStream stream = new FileStream(Application.persistentDataPath + "/" + generalSaveFile, FileMode.Create);

        GameManager.GameSavedVariables gameSavecVariables = new GameManager.GameSavedVariables(levelReached, unlockedDisksNb);

        bf.Serialize(stream, gameSavecVariables);
        stream.Close();
    }

    public void LoadGameVariables()
    {
        Debug.Log("LoadBuildings...");
        if (File.Exists(Application.persistentDataPath + "/" + generalSaveFile))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream stream = new FileStream(Application.persistentDataPath + "/" + generalSaveFile, FileMode.Open);

            GameManager.GameSavedVariables gameSavedVariables = bf.Deserialize(stream) as GameManager.GameSavedVariables;

            stream.Close();

            int levelReached = gameSavedVariables.levelReached;
            int unlockedDisksNb = gameSavedVariables.unlockedDisks;

            // Temporary | TODO
            Debug.Log("LevelReached[" + levelReached + "] | UnlockedDisksNb[" + unlockedDisksNb + "]");
        }
        else
        {
            Debug.LogError("GameVariables file doesn't exist !");
        }
    }

}
