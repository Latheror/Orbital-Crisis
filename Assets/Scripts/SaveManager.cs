using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System;

public class SaveManager : MonoBehaviour {

    public static SaveManager instance;
    void Awake()
    {
        if (instance != null) { Debug.LogError("More than one SaveManager in scene !"); return; }
        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public string gameSaveFileName = ("save");  // Add save index after (1-5)
    public string gameSaveFileExtension = ".sav";

    void Start () {

    }
	
	void Update () {
		
	}

    public void LoadButtonClicked()
    {
        //LoadGameState();
    }

    public void SaveGameState(int saveGameSlotIndex)
    {
        Debug.Log("Saving game state...");
        GameManager.GameSavedVariables gameSavedVariables = GatherGameVariables();
        Building.BuildingData[] buildingDatas = GatherBuildingsData();

        GameSaveData gameSaveData = new GameSaveData(gameSavedVariables, buildingDatas);

        // Binary Formatter + Stream
        BinaryFormatter bf = new BinaryFormatter();
        string fileName = (gameSaveFileName + saveGameSlotIndex + gameSaveFileExtension);
        Debug.Log("File Name: " + fileName);
        FileStream stream = new FileStream(Application.persistentDataPath + "/" + fileName, FileMode.Create);
        bf.Serialize(stream, gameSaveData);
        stream.Close();
    }

    public void LoadGameState()
    {
        //InfrastructureManager.instance.ClearBuildings();
        //LoadGameVariables();
        //LoadBuildings();
    }

    public Building.BuildingData[] GatherBuildingsData()
    {
        int buildingNb = BuildingManager.instance.buildingList.Count;
        Debug.Log("Saving [" + buildingNb + "] buildings...");

        // BuildingData List
        Building.BuildingData[] buildingsData = new Building.BuildingData[buildingNb];

        for (int i = 0; i<buildingNb; i++)
        {
            Debug.Log("Saving [" + BuildingManager.instance.buildingList[i].GetComponent<Building>().buildingType.name + "]");

            Building.BuildingData bData = new Building.BuildingData(BuildingManager.instance.buildingList[i].GetComponent<Building>());

            buildingsData[i] = bData;

        }

        return buildingsData;
    }

    public void LoadBuildings()
    {
        Debug.Log("LoadBuildings...");
        /*if (File.Exists(Application.persistentDataPath + "/" + buildingsSaveFile))
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
        }*/
    }

    public GameManager.GameSavedVariables GatherGameVariables()
    {
        int levelReached = LevelManager.instance.currentLevelNumber;
        int unlockedDisksNb = SurroundingAreasManager.instance.unlockedDisksNb;

        Debug.Log("Saving game variables | LevelReached [" + levelReached + "] | UnlockedDisksNb [" + unlockedDisksNb + "]");

        GameManager.GameSavedVariables gameSavedVariables = new GameManager.GameSavedVariables(levelReached, unlockedDisksNb);
        return gameSavedVariables;
    }

    public void LoadGameVariables()
    {
        /*Debug.Log("LoadBuildings...");
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
        }*/
    }

    public void SaveGameRequest(int saveSlotIndex)
    {
        Debug.Log("SaveManager | SaveGameRequest | Slot[" + saveSlotIndex + "]");
        SaveGameState(saveSlotIndex);
    }

    // Data describing a game save
    [Serializable]
    public class GameSaveData
    {
        GameManager.GameSavedVariables gameSavedVariables;
        Building.BuildingData[] buildingsData;

        public GameSaveData(GameManager.GameSavedVariables gameSavedVariables, Building.BuildingData[] buildingsData)
        {
            this.gameSavedVariables = gameSavedVariables;
            this.buildingsData = buildingsData;
        }

    }


}
