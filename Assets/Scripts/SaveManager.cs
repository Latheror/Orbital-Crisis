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
        if (instance == null) {
            instance = this;
            DontDestroyOnLoad(gameObject);

            ImportGameSaves();  // Read files decribing saved games
            ImportSavedGeneralData();   // Read file containing info relative to save files

            MenuLoadGamePanel.instance.BuildLoadGameSaveElements();
            MenuLoadGamePanel.instance.DisplayHighScore();
        }
    }

    public int savedGameFilesNb = 2;
    public SavedGameFilesInfoData globalSavedGameInfoData;

    public string gameSaveFileName = ("save");  // Add save index after (1-5)
    public string gameSaveFileExtension = ".sav";

    public string savedGameFilesInfoName = ("saves");
    public string savedGeneralDataFileName = ("general");

    public GameSaveData[] globalGameSaveData = new GameSaveData[5];

    public GameSaveData gameSaveToLoad;

    [Header("General Saved Data")]
    public SavedGeneralData savedGeneralData = null;
    public int highScore = 0;

    public void ImportGameSaves()
    {
        ImportGameSavesInfoFile();
        ImportGameSavesData();
    }

    public void LoadButtonClicked()
    {
        //LoadGameState();
    }

    public void SaveGameStateIntoSlot(int saveGameSlotIndex)
    {
        Debug.Log("Saving game state...");

        string saveTime = DateTime.Now.ToString("yyyy-MM-dd h:mm:ss tt");

        GameManager.GeneralGameData generalData = GatherGeneralGameData();
        Building.BuildingData[] buildingDatas = GatherBuildingsData();
        SpaceshipManager.SpaceshipData[] spaceshipsData = GatherSpaceshipsData();
        Level.LevelData reachedLevelData = GatherReachedLevelData();
        ResourcesManager.ResourceData[] resourcesData = GatherResourcesData();
        BuildingManager.UnlockedBuildingData[] unlockedBuildingsData = GatherUnlockedBuildingsData();
        TechTreeManager.TechnologyData[] technologiesData = GatherTechnologiesData();
        MegaStructureManager.MegaStructuresData megaStructuresData = GatherMegaStructuresData();
        PopulationManager.PopulationData populationData = GatherPopulationData();

        // Build Game Save Data
        GameSaveData gameSaveData = new GameSaveData(generalData, buildingDatas, spaceshipsData, reachedLevelData, resourcesData, unlockedBuildingsData, technologiesData, megaStructuresData, populationData);
        int currentLevelReached = LevelManager.instance.currentLevelNumber;

        // Binary Formatter + Stream
        BinaryFormatter bf = new BinaryFormatter();
        string fileName = (gameSaveFileName + saveGameSlotIndex + gameSaveFileExtension);
        Debug.Log("File Name: " + fileName);
        FileStream stream = new FileStream(Application.persistentDataPath + "/" + fileName, FileMode.Create);

        // Write chosen save file
        bf.Serialize(stream, gameSaveData);
        stream.Close();

        // Update SaveFilesInfo general object
        SavedGameFilesInfoData.SaveFileInfo saveFileInfo = new SavedGameFilesInfoData.SaveFileInfo(saveGameSlotIndex, true, saveTime);
        Debug.Log("Created new SaveFileInfo | SlotIndex [" + saveGameSlotIndex + "] | IsUsed [" + true + "] | SaveTime [" + saveTime +"]");

        globalSavedGameInfoData.UpdateSavedGameFilesInfoForIndex(saveFileInfo);

        // Update indicator in save panel
        SaveGamePanel.instance.UpdateSaveSlotInfo(saveFileInfo);

        // Write game saves info file
        WriteSaveFilesInfo();
    }

    public void SaveGeneralData()
    {
        SavedGeneralData newSavedGeneralData = GatherGeneralData();
        Debug.Log("SaveGeneralData | HighScore [" + newSavedGeneralData.highScore + "]");

        // Binary Formatter + Stream
        BinaryFormatter bf = new BinaryFormatter();
        string fileName = (savedGeneralDataFileName + gameSaveFileExtension);
        Debug.Log("File Name: " + fileName);
        FileStream stream = new FileStream(Application.persistentDataPath + "/" + fileName, FileMode.Create);

        // Write file
        bf.Serialize(stream, newSavedGeneralData);
        stream.Close();
    }

    public void SetGameSaveToLoad(GameSaveData gameSaveData)
    {
        Debug.Log("SetGameSaveToLoad | Level Reached:" + gameSaveData.generalGameData.levelReached + " | Buildings Nb [" + gameSaveData.buildingsData.Length + "]");
        gameSaveToLoad = gameSaveData;
    }

    public bool SetGameSaveToLoadIndex(int gameSaveIndex)
    {
        if (globalSavedGameInfoData.saveFilesInfo[gameSaveIndex - 1].isUsed)
        {
            Debug.Log("SetGameSaveToLoadIndex [" + gameSaveIndex + "]");
            SetGameSaveToLoad(globalGameSaveData[gameSaveIndex - 1]);
            return true;
        }
        else
        {
            Debug.LogError("SetGameSaveToLoadIndex | File marked as not used...");
            return false;
        }
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

    public SpaceshipManager.SpaceshipData[] GatherSpaceshipsData()
    {
        return SpaceshipManager.instance.BuildSpaceshipsData();
    }

    public Level.LevelData GatherReachedLevelData()
    {
        return LevelManager.instance.BuildReachedLevelData();
    }

    public ResourcesManager.ResourceData[] GatherResourcesData()
    {
        return ResourcesManager.instance.BuildResourcesData();
    }

    public BuildingManager.UnlockedBuildingData[] GatherUnlockedBuildingsData()
    {
        return BuildingManager.instance.BuildUnlockedBuildingsData();
    }

    public GameManager.GeneralGameData GatherGeneralGameData()
    {
        int levelReached = LevelManager.instance.currentLevelNumber;
        int unlockedDisksNb = SurroundingAreasManager.instance.unlockedDisksNb;
        int score = ScoreManager.instance.score;
        int hits = InfoManager.instance.nbMeteorCollisions;
        int planetLife = (int)ScoreManager.instance.planetLife;
        int experiencePoints = ScoreManager.instance.experiencePoints;
        int artifactsNb = ScoreManager.instance.artifactsNb;
        bool timerOptionEnabled = TimeManager.instance.isTimerEnabled;
        bool hasGameOverOccured = ScoreManager.instance.gameOverHappened;
        bool infiniteModeEnabled = GameManager.instance.IsInfiniteModeEnabled();

        //Debug.Log("Saving game variables | LevelReached [" + levelReached + "] | UnlockedDisksNb [" + unlockedDisksNb + "]");

        GameManager.GeneralGameData gameSavedVariables = new GameManager.GeneralGameData(levelReached, unlockedDisksNb, score, hits, planetLife, experiencePoints, artifactsNb, timerOptionEnabled, hasGameOverOccured, infiniteModeEnabled);
        return gameSavedVariables;
    }

    public TechTreeManager.TechnologyData[] GatherTechnologiesData()
    {
        return TechTreeManager.instance.BuildTechnologyData();
    }

    public MegaStructureManager.MegaStructuresData GatherMegaStructuresData()
    {
        return MegaStructureManager.instance.BuildMegaStructuresData();
    }

    public PopulationManager.PopulationData GatherPopulationData()
    {
        return PopulationManager.instance.BuildPopulationData();
    }

    public SavedGeneralData GatherGeneralData()
    {
        int highScoreToSave = 0;
        // High score
        if(ScoreManager.instance.score > highScore)
        {
            highScore = ScoreManager.instance.score;
        }
        highScoreToSave = highScore;

        return (new SavedGeneralData(highScoreToSave));
    }

    public void SaveGameRequest(int saveSlotIndex)
    {
        Debug.Log("SaveManager | SaveGameRequest | Slot[" + saveSlotIndex + "]");
        SaveGameStateIntoSlot(saveSlotIndex);
        SaveGeneralData();
    }

    public void ImportGameSavesInfoFile()
    {
        Debug.Log("Importing Game Save Files");
        if (File.Exists(Application.persistentDataPath + "/" + savedGameFilesInfoName + gameSaveFileExtension))
        {
            // saves.sav exist
            Debug.Log("saves.sav exist. Loading save files...");

            // Reading existing files
            BinaryFormatter bf = new BinaryFormatter();
            FileStream stream = new FileStream(Application.persistentDataPath + "/" + savedGameFilesInfoName + gameSaveFileExtension, FileMode.Open);

            globalSavedGameInfoData = bf.Deserialize(stream) as SavedGameFilesInfoData;            

            stream.Close();
        }
        else
        {
            // saves.sav doesn't exist
            Debug.Log("saves.sav file doesn't exist. Creating...");

            // Binary Formatter + Stream
            BinaryFormatter bf = new BinaryFormatter();
            string fileName = (savedGameFilesInfoName + gameSaveFileExtension);  // saves.sav
            Debug.Log("File Name: " + fileName);
            FileStream stream = new FileStream(Application.persistentDataPath + "/" + fileName, FileMode.Create);

            // Initializing globalGameSaveData
            globalSavedGameInfoData = new SavedGameFilesInfoData();

            bf.Serialize(stream, globalSavedGameInfoData);
            stream.Close();
        }
    }

    public void ImportGameSavesData()
    {
        Debug.Log("ImportGameSavesData...");
        foreach (SavedGameFilesInfoData.SaveFileInfo saveFileInfo in globalSavedGameInfoData.saveFilesInfo)
        {
            if(saveFileInfo.isUsed)
            {
                Debug.Log("Save File Nb [" + saveFileInfo.fileIndex + "] is marked as used...");
                LoadGameSaveData(saveFileInfo.fileIndex);
            }
        }
    }

    public void ImportSavedGeneralData()
    {
        Debug.Log("ImportSavedGeneralData...");
        if (File.Exists(Application.persistentDataPath + "/" + savedGeneralDataFileName + gameSaveFileExtension))
        {
            // saves.sav exist
            Debug.Log("general.sav exist. Loading general data...");

            // Reading existing files
            BinaryFormatter bf = new BinaryFormatter();
            FileStream stream = new FileStream(Application.persistentDataPath + "/" + savedGeneralDataFileName + gameSaveFileExtension, FileMode.Open);
            
            savedGeneralData = bf.Deserialize(stream) as SavedGeneralData;
            highScore = savedGeneralData.highScore;
            
            stream.Close();
        }
        else
        {
            // saves.sav doesn't exist
            Debug.Log("general.sav file doesn't exist. Creating...");

            // Binary Formatter + Stream
            BinaryFormatter bf = new BinaryFormatter();
            string fileName = (savedGeneralDataFileName + gameSaveFileExtension);  // general.sav
            Debug.Log("File Name: " + fileName);
            FileStream stream = new FileStream(Application.persistentDataPath + "/" + fileName, FileMode.Create);

            savedGeneralData = new SavedGeneralData(0);

            bf.Serialize(stream, savedGeneralData);
            stream.Close();
        }
    }

    public void LoadGameSaveData(int gameSaveIndex)
    {
        Debug.Log("LoadGameSaveData [" + gameSaveIndex + "]");

        // Binary Formatter + Stream
        BinaryFormatter bf = new BinaryFormatter();
        string fileName = ((gameSaveFileName + gameSaveIndex.ToString() + gameSaveFileExtension));  // saveX.sav
        Debug.Log("Loading File: " + fileName);

        if (File.Exists(Application.persistentDataPath + "/" + fileName)) {
            Debug.Log("File exists");
            FileStream stream = new FileStream(Application.persistentDataPath + "/" + fileName, FileMode.Open);

            // Deserialize
            globalGameSaveData[gameSaveIndex - 1] = (bf.Deserialize(stream) as GameSaveData);

            stream.Close();
        }
        else
        {
            Debug.LogError("File doesn't exist: " + fileName);
        }
    }

    public void WriteSaveFilesInfo()
    {
        Debug.Log("WriteSaveFilesInfo...");

        // Binary Formatter + Stream
        BinaryFormatter bf = new BinaryFormatter();
        string fileName = (savedGameFilesInfoName + gameSaveFileExtension);  // saves.sav
        Debug.Log("File Name: " + fileName);
        FileStream stream = new FileStream(Application.persistentDataPath + "/" + fileName, FileMode.Create);

        bf.Serialize(stream, globalSavedGameInfoData);
        stream.Close();
    }

    public void ReloadSavedDataInMenu()
    {
        Debug.Log("ReloadGameSavesInMenu");
        ImportGameSaves();
        ImportSavedGeneralData();
        MenuLoadGamePanel.instance.BuildLoadGameSaveElements();
        MenuLoadGamePanel.instance.DisplayHighScore();
    }

    public void SetSavedGameDataFileAsNotUsed(int savedGameIndex)
    {
        globalSavedGameInfoData.saveFilesInfo[savedGameIndex - 1].isUsed = false;
        WriteSaveFilesInfo();
    }

    // Data describing a game save
    [Serializable]
    public class GameSaveData
    {
        public GameManager.GeneralGameData generalGameData;
        public Building.BuildingData[] buildingsData;
        public SpaceshipManager.SpaceshipData[] spaceshipsData;
        public Level.LevelData reachedLevelData;
        public ResourcesManager.ResourceData[] resourcesData;
        public BuildingManager.UnlockedBuildingData[] unlockedBuildingsData;
        public TechTreeManager.TechnologyData[] technologiesData;
        public MegaStructureManager.MegaStructuresData megaStructuresData;
        public PopulationManager.PopulationData populationData;

        public GameSaveData(GameManager.GeneralGameData generalGameData, Building.BuildingData[] buildingsData, SpaceshipManager.SpaceshipData[] spaceshipsData, Level.LevelData reachedLevelData, ResourcesManager.ResourceData[] resourcesData, BuildingManager.UnlockedBuildingData[] unlockedBuildingsData, TechTreeManager.TechnologyData[] technologiesData, MegaStructureManager.MegaStructuresData megaStructuresData, PopulationManager.PopulationData populationData)
        {
            this.generalGameData = generalGameData;
            this.buildingsData = buildingsData;
            this.spaceshipsData = spaceshipsData;
            this.reachedLevelData = reachedLevelData;
            this.resourcesData = resourcesData;
            this.unlockedBuildingsData = unlockedBuildingsData;
            this.technologiesData = technologiesData;
            this.megaStructuresData = megaStructuresData;
            this.populationData = populationData;
        }
    }

    [Serializable]
    public class SavedGameFilesInfoData
    {
        public int savedGameFilesNb = 5;
        public SaveFileInfo[] saveFilesInfo;
        public int totalFilesUsed = 0;

        public SavedGameFilesInfoData(SaveFileInfo[] saveFilesInfo)
        {
            int totalUsedFiles = 0;

            this.saveFilesInfo = saveFilesInfo;
            foreach (SaveFileInfo fileNumberUsed in saveFilesInfo)
            {
                if(fileNumberUsed.isUsed)
                {
                    totalUsedFiles++;
                }
            }
            totalFilesUsed = totalUsedFiles;

        }

        public SavedGameFilesInfoData()  // Initialization
        {
            saveFilesInfo = new SaveFileInfo[] { new SaveFileInfo(1,false,"Empty"),
                                                 new SaveFileInfo(2,false,"Empty"),
                                                 new SaveFileInfo(3,false,"Empty"),
                                                 new SaveFileInfo(4,false,"Empty"),
                                                 new SaveFileInfo(5,false,"Empty")
            };
            savedGameFilesNb = 5;
            totalFilesUsed = 0;
        }

        public void UpdateSavedGameFilesInfoForIndex(SaveFileInfo saveFileInfo)
        {
            saveFilesInfo[saveFileInfo.fileIndex - 1] = saveFileInfo;
        }

        public bool IsSaveGameFileIndexUsed(int fileIndex)
        {
            bool isUsed = false;
            if (saveFilesInfo[fileIndex] != null)
            {
                isUsed = (saveFilesInfo[fileIndex].isUsed);
            }
            return isUsed;
        }

        [Serializable]
        public class SaveFileInfo
        {
            public int fileIndex;
            public bool isUsed;
            public string saveTime;

            public SaveFileInfo(int fileIndex, bool isUsed, string saveTime)
            {
                this.fileIndex = fileIndex;
                this.isUsed = isUsed;
                this.saveTime = saveTime;
            }
        }
    }

    [Serializable]
    public class SavedGeneralData
    {
        public int highScore;

        public SavedGeneralData(int highScore)
        {
            this.highScore = highScore;
        }
    }

}
