using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MenuLoadGamePanel : MonoBehaviour {

    public static MenuLoadGamePanel instance;
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        DontDestroyOnLoad(gameObject);
    }

    public TextMeshProUGUI loadGameSlot1dateText;
    public TextMeshProUGUI loadGameSlot2dateText;
    public TextMeshProUGUI loadGameSlot3dateText;
    public TextMeshProUGUI loadGameSlot4dateText;
    public TextMeshProUGUI loadGameSlot5dateText;

    public GameObject loadGameSlot1Button;
    public GameObject loadGameSlot2Button;
    public GameObject loadGameSlot3Button;
    public GameObject loadGameSlot4Button;
    public GameObject loadGameSlot5Button;

    public TextMeshProUGUI loadGameSlot1LevelReachedText;
    public TextMeshProUGUI loadGameSlot2LevelReachedText;
    public TextMeshProUGUI loadGameSlot3LevelReachedText;
    public TextMeshProUGUI loadGameSlot4LevelReachedText;
    public TextMeshProUGUI loadGameSlot5LevelReachedText;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void PlayCanvas_NewGameButton(){
        ScenesManager.instance.LaunchNewGame();
    }

    public void LoadGameSave1ButtonClicked() { LoadGameSaveNumberRequest(1); }
    public void LoadGameSave2ButtonClicked() { LoadGameSaveNumberRequest(2); }
    public void LoadGameSave3ButtonClicked() { LoadGameSaveNumberRequest(3); }
    public void LoadGameSave4ButtonClicked() { LoadGameSaveNumberRequest(4); }
    public void LoadGameSave5ButtonClicked() { LoadGameSaveNumberRequest(5); }


    public void LoadGameSaveNumberRequest(int saveSlotIndex)
    {
        Debug.Log("LoadGameSaveNumberRequest [" + saveSlotIndex + "]");
        if(SaveManager.instance.globalSavedGameInfoData.saveFilesInfo[saveSlotIndex - 1].isUsed)
        {
            Debug.Log("Loading Game Save [" + saveSlotIndex + "], save exist.");
            if (SaveManager.instance.SetGameSaveToLoadIndex(saveSlotIndex))
            {
                ScenesManager.instance.LaunchSavedGame(SaveManager.instance.gameSaveToLoad);
            }
            else
            {
                Debug.LogError("Can't load game save.");
            }
        }
        else
        {
            Debug.LogError("Can't load save [" + saveSlotIndex + "], save doesn't exist.");
        }
    }


    public void UpdateLoadGameSaveElement(SaveManager.SavedGameFilesInfoData.SaveFileInfo saveFileInfo)
    {
        int fileIndex = saveFileInfo.fileIndex;
        string saveTimeText = saveFileInfo.saveTime;
        int levelReached = 0;
        if (saveFileInfo.isUsed)
        {
            levelReached = SaveManager.instance.globalGameSaveData[fileIndex - 1].generalGameData.levelReached;
        }
         
        Debug.Log("UpdateLoadGameSaveElement [" + fileIndex + "] | LevelReached [" + levelReached + "]");
        switch (fileIndex)
        {
            case 1:
            {
                loadGameSlot1dateText.text = saveTimeText;
                if (saveFileInfo.isUsed) { loadGameSlot1LevelReachedText.text = ("Level " + levelReached.ToString()); }
                loadGameSlot1Button.SetActive(saveFileInfo.isUsed);
                break;
            }
            case 2:
            {
                loadGameSlot2dateText.text = saveTimeText;
                if (saveFileInfo.isUsed) { loadGameSlot2LevelReachedText.text = ("Level " + levelReached.ToString()); }
                loadGameSlot2Button.SetActive(saveFileInfo.isUsed);
                break;
            }
            case 3:
            {
                loadGameSlot3dateText.text = saveTimeText;
                if (saveFileInfo.isUsed) { loadGameSlot3LevelReachedText.text = ("Level " + levelReached.ToString()); }
                loadGameSlot3Button.SetActive(saveFileInfo.isUsed);
                break;
            }
            case 4:
            {
                loadGameSlot4dateText.text = saveTimeText;
                if (saveFileInfo.isUsed) { loadGameSlot4LevelReachedText.text = ("Level " + levelReached.ToString()); }
                loadGameSlot4Button.SetActive(saveFileInfo.isUsed);
                break;
            }
            case 5:
            {
                loadGameSlot5dateText.text = saveTimeText;
                if (saveFileInfo.isUsed) { loadGameSlot5LevelReachedText.text = ("Level " + levelReached.ToString()); }
                loadGameSlot5Button.SetActive(saveFileInfo.isUsed);
                break;
            }
        }
    }


}
