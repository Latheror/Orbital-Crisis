using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SaveSlotPanel : MonoBehaviour {

    [Header("UI")]
    public TextMeshProUGUI saveSlotTitleText;
    public TextMeshProUGUI saveTimeText;
    public GameObject saveButton;

    [Header("Settings")]
    public int saveSlotIndex;

	public void SetInfo(SaveManager.SavedGameFilesInfoData.SaveFileInfo savedFileInfo)
    {
        saveSlotIndex = savedFileInfo.fileIndex;
        saveSlotTitleText.text = ("Save " + savedFileInfo.fileIndex);
        saveButton.SetActive(true);
        if(savedFileInfo.isUsed) {
            saveTimeText.text = savedFileInfo.saveTime;
        }
        else {
            saveTimeText.text = "Empty";
        }
    }

    public void SaveButtonClicked()
    {
        Debug.Log("SaveButtonClicked [" + saveSlotIndex + "]");
        SaveGamePanel.instance.SaveGameRequest(saveSlotIndex);
    }
}
