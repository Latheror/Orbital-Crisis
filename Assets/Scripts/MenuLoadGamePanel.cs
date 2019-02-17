using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MenuLoadGamePanel : MonoBehaviour {

    public static MenuLoadGamePanel instance;
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        //DontDestroyOnLoad(gameObject);
    }

    public enum PanelDisplayMode { Default, Options };

    [Header("UI")]
    public GameObject newGameButtonsLayout;
    public GameObject loadGamePanel;
    public GameObject optionsPanel;

    [Header("High Score UI")]
    public GameObject highScorePanel;
    public TextMeshProUGUI highScoreText;

    [Header("Prefabs")]
    public GameObject newGameButtonPrefab;

    [Header("Operation")]
    public List<GameObject> newGameButtonsList;
    public PanelDisplayMode leftPanelDisplayMode;
    public bool newGameButtonsAvailable = true;

    // Use this for initialization
    void Start () {
        Initialize();
    }

    public void Initialize()
    {
        loadGamePanel.SetActive(true);
        optionsPanel.SetActive(false);
        newGameButtonsAvailable = true;
    }

    public void PlayCanvas_NewGameButton(){
        ScenesManager.instance.LaunchNewGame();
    }

    // Instantiate and display information on "NewGame/LoadGame" buttons
    public void BuildLoadGameSaveElements()
    {
        Debug.Log("BuildLoadGameSaveElements");
        for (int i=0; i<SaveManager.instance.savedGameFilesNb; i++)
        {
            // Instantiate NewGame button
            GameObject instantiatedNewGameButton = Instantiate(newGameButtonPrefab, Vector3.zero, Quaternion.identity);
            // Set its parent to the layout
            instantiatedNewGameButton.transform.SetParent(newGameButtonsLayout.transform, false);

            // Set parameters within
            NewGameButton ngb = instantiatedNewGameButton.GetComponent<NewGameButton>();
            ngb.SetInfo(i + 1, SaveManager.instance.globalSavedGameInfoData.saveFilesInfo[i], SaveManager.instance.globalGameSaveData[i]);

            // Add it to the list
            newGameButtonsList.Add(instantiatedNewGameButton);
        }
    }

    public void UpdateLoadGameSavePanel()
    {
        Debug.Log("UpdateLoadGameSavePanel");
        foreach (GameObject newGameButton in newGameButtonsList)
        {
            UpdateNewGameButton(newGameButton);
        }
    }

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


    public void UpdateNewGameButton(GameObject loadGameSaveElement)
    {
        int saveIndex = loadGameSaveElement.GetComponent<NewGameButton>().saveIndex;
        Debug.Log("UpdateLoadGameSaveElement [" + saveIndex + "]");

        SaveManager.GameSaveData gameSaveData = SaveManager.instance.globalGameSaveData[saveIndex - 1];
        SaveManager.SavedGameFilesInfoData.SaveFileInfo saveFileInfo = SaveManager.instance.globalSavedGameInfoData.saveFilesInfo[saveIndex - 1];

        loadGameSaveElement.GetComponent<NewGameButton>().SetInfo(saveIndex, saveFileInfo, gameSaveData);
    }

    public void DeleteGameSaveButtonClicked(int gameSaveIndex)
    {
        SaveManager.instance.SetSavedGameDataFileAsNotUsed(gameSaveIndex);
        SaveManager.instance.ImportGameSavesInfoFile();
        UpdateLoadGameSavePanel();
    }

    public void DisplayHighScore()
    {
        if (SaveManager.instance.savedGeneralData != null)
        {
            highScorePanel.SetActive((SaveManager.instance.savedGeneralData.highScore > 0));
            highScoreText.text = SaveManager.instance.savedGeneralData.highScore.ToString();
        }
    }

    public void ShowOptionsPanelButtonClicked()
    {
        loadGamePanel.SetActive(false);
        optionsPanel.SetActive(true);
    }

    public void ShowLoadGamePanelButtonClicked()
    {
        optionsPanel.SetActive(false);
        loadGamePanel.SetActive(true);
    }

    public void NewGameRequest(int saveSlotIndex = 1)
    {
        Debug.Log("NewGameRequest | Slot [" + saveSlotIndex + "]");
        ScenesManager.instance.LaunchNewGame();

        // Nothing is done with saveSlotIndex for now
    }

    public void NewGameButtonClicked()
    {
        newGameButtonsAvailable = false;
    }

}
