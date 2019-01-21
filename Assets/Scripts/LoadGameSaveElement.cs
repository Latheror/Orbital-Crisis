using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LoadGameSaveElement : MonoBehaviour {

    [Header("UI")]
    public GameObject deleteSaveFileButton;
    public TextMeshProUGUI saveTimeText;
    public GameObject loadSaveButton;
    public TextMeshProUGUI saveTitleText;
    public TextMeshProUGUI levelReachedText;

    [Header("Operation")]
    public SaveManager.GameSaveData gameSaveData;
    public SaveManager.SavedGameFilesInfoData.SaveFileInfo saveFileInfo;
    public int saveIndex;

    public void SetInfo(int saveIndex, SaveManager.SavedGameFilesInfoData.SaveFileInfo saveFileInfo, SaveManager.GameSaveData gameSaveData)
    {
        this.gameSaveData = gameSaveData;
        this.saveIndex = saveIndex;

        // Load Button
        loadSaveButton.SetActive(saveFileInfo.isUsed);

        // Delete Button
        deleteSaveFileButton.SetActive(saveFileInfo.isUsed);

        // Title
        saveTitleText.text = ("Save " + saveIndex);
        
        if (saveFileInfo.isUsed)
        {
            // Level Reached
            if ((gameSaveData != null))
            {
                if (gameSaveData.reachedLevelData != null){
                    levelReachedText.text = ("Level " + gameSaveData.reachedLevelData.levelIndex);
                }
                else{
                    levelReachedText.text = ("-");
                }
            }
            else
            {
                Debug.LogError("LoadGame data is null...");
            }

            // Save Time
            saveTimeText.text = (saveFileInfo.saveTime);
        }
        else
        {
            levelReachedText.text = ("Empty");
            saveTimeText.text = "";
        }
    }

    public void LoadButtonClicked()
    {
        Debug.Log("LoadButtonClicked [" + saveIndex + "]");
        MenuLoadGamePanel.instance.LoadGameSaveNumberRequest(saveIndex);
    }

    public void DeleteButtonClicked()
    {
        Debug.Log("DeleteButtonClicked [" + saveIndex + "]");
        MenuLoadGamePanel.instance.DeleteGameSaveButtonClicked(saveIndex);
    }

}
