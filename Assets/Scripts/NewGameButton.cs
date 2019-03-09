using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class NewGameButton : MonoBehaviour {

    [Header("UI")]
    public GameObject mainButton;
    public GameObject deleteSaveFileButton;
    public TextMeshProUGUI mainText;
    public TextMeshProUGUI saveDateText;
    public TextMeshProUGUI saveTitleText;
    public TextMeshProUGUI levelReachedText;

    [Header("Operation")]
    public SaveManager.GameSaveData gameSaveData;
    public SaveManager.SavedGameFilesInfoData.SaveFileInfo saveFileInfo;
    public int saveIndex;
    public bool isUsed;

    public void SetInfo(int saveIndex, SaveManager.SavedGameFilesInfoData.SaveFileInfo saveFileInfo, SaveManager.GameSaveData gameSaveData)
    {
        Debug.Log("NewGameButton | SetInfo");
        this.gameSaveData = gameSaveData;
        this.saveIndex = saveIndex;
        isUsed = saveFileInfo.isUsed;

        // Set main button info
        SetMainButtonInfo();

        // Display delete Button if file is used
        deleteSaveFileButton.SetActive(isUsed);      
    }

    public void SetMainButtonInfo()
    {
        // Title
        saveTitleText.text = ("Save " + saveIndex);         // To comment if not needed

        if (isUsed)
        {
            // Main Text
            mainText.text = ("LOAD GAME");

            // Level Reached
            if ((gameSaveData != null))
            {
                if (gameSaveData.reachedLevelData != null)
                {
                    levelReachedText.text = ("Level " + gameSaveData.reachedLevelData.levelIndex);
                }
                else
                {
                    levelReachedText.text = ("-");
                }
            }
            else
            {
                Debug.LogError("LoadGame data is null...");
            }

            // Save Time
            Debug.Log("Save Date [" + saveFileInfo.saveTime + "]");
            saveDateText.text = (saveFileInfo.saveTime);
        }
        else
        {
            mainText.text = ("NEW GAME");
            levelReachedText.text = ("");
            saveDateText.text = ("");
        }
    }

    public void OnNewGameButtonClick()
    {
        Debug.Log("OnNewGameButtonClick [" + saveIndex + "] | IsUsed [" + isUsed + "]");
        if(MenuLoadGamePanel.instance.newGameButtonsAvailable)
        {
            MenuLoadGamePanel.instance.NewGameButtonClicked(saveIndex, isUsed);
        }
    }

    public void OnDeleteButtonClick()
    {
        Debug.Log("OnDeleteButtonClick [" + saveIndex + "]");
        MenuLoadGamePanel.instance.DeleteGameSaveButtonClicked(saveIndex);
    }

}
