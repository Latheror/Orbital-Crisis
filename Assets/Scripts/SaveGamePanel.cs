using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SaveGamePanel : MonoBehaviour {

    public static SaveGamePanel instance;
    void Awake()
    {
        if (instance != null) { Debug.LogError("More than one SaveGamePanel in scene !"); return; }
        instance = this;
    }

    public GameObject pausePanel;

    public GameObject saveSlot1_title_panel;
    public TextMeshProUGUI saveSlot1_time_panel;
    public GameObject saveSlot1_button_panel;

    public GameObject saveSlot2_title_panel;
    public TextMeshProUGUI saveSlot2_time_panel;
    public GameObject saveSlot2_button_panel;

    public GameObject saveSlot3_title_panel;
    public TextMeshProUGUI saveSlot3_time_panel;
    public GameObject saveSlot3_button_panel;

    public GameObject saveSlot4_title_panel;
    public TextMeshProUGUI saveSlot4_time_panel;
    public GameObject saveSlot4_button_panel;

    public GameObject saveSlot5_title_panel;
    public TextMeshProUGUI saveSlot5_time_panel;
    public GameObject saveSlot5_button_panel;

    public Color saveButtonEnabledColor;
    public Color saveButtonDisabledColor;

	// Use this for initialization
	void Start () {
        UpdateSaveSlotsInfo();
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void SaveGamePanel_BackButton()
    {
        pausePanel.SetActive(true);
        gameObject.SetActive(false);
    }

    public void SaveGamePanel_Slot1Button() { SaveGameRequest(1); }
    public void SaveGamePanel_Slot2Button() { SaveGameRequest(2); }
    public void SaveGamePanel_Slot3Button() { SaveGameRequest(3); }
    public void SaveGamePanel_Slot4Button() { SaveGameRequest(4); }
    public void SaveGamePanel_Slot5Button() { SaveGameRequest(5); }

    public void SaveGameRequest(int slotIndex)
    {
        Debug.Log("Request saving game in slot [" + slotIndex + "]...");
        SaveManager.instance.SaveGameRequest(slotIndex);
    }

    public void UpdateSaveSlotsInfo()
    {
        foreach (SaveManager.SavedGameFilesInfoData.SaveFileInfo saveFileInfo in SaveManager.instance.globalSavedGameInfoData.saveFilesInfo)
        {
            UpdateSaveSlotInfo(saveFileInfo);
        }
    }

    public void UpdateSaveSlotInfo(SaveManager.SavedGameFilesInfoData.SaveFileInfo saveFileInfo)
    {
        Debug.Log("UpdateSaveSlotInfo [" + saveFileInfo.fileIndex + "]");
        switch(saveFileInfo.fileIndex)
        {
            case 1:
            {
                saveSlot1_time_panel.text = saveFileInfo.saveTime;
                break;
            }
            case 2:
            {
                saveSlot2_time_panel.text = saveFileInfo.saveTime;
                break;
            }
            case 3:
            {
                saveSlot3_time_panel.text = saveFileInfo.saveTime;
                break;
            }
            case 4:
            {
                saveSlot4_time_panel.text = saveFileInfo.saveTime;
                break;
            }
            case 5:
            {
                saveSlot5_time_panel.text = saveFileInfo.saveTime;
                break;
            }
        }
    }
}
