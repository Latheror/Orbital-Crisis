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

    public List<GameObject> saveSlotIndicators;
    public GameObject saveSlotPanelPrefab;
    public GameObject saveSlotPanelsLayout;

    public Color saveButtonEnabledColor;
    public Color saveButtonDisabledColor;

	// Use this for initialization
	void Start () {
        BuildSaveSlotsIndicators();
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void SaveGamePanel_BackButton()
    {
        pausePanel.SetActive(true);
        gameObject.SetActive(false);
    }

    public void SaveGameRequest(int slotIndex)
    {
        Debug.Log("Request saving game in slot [" + slotIndex + "]...");
        SaveManager.instance.SaveGameRequest(slotIndex);
        UpdateSaveSlotsInfo();
    }

    public void UpdateSaveSlotsInfo()
    {
        foreach (SaveManager.SavedGameFilesInfoData.SaveFileInfo saveFileInfo in SaveManager.instance.globalSavedGameInfoData.saveFilesInfo)
        {
            UpdateSaveSlotInfo(saveFileInfo);
        }
    }

    public void BuildSaveSlotsIndicators()
    {
        for(int i=0; i<SaveManager.instance.savedGameFilesNb; i++)
        {
            GameObject instantiatedSaveSlotIndicator = Instantiate(saveSlotPanelPrefab, saveSlotPanelPrefab.transform.position, Quaternion.identity);
            instantiatedSaveSlotIndicator.transform.SetParent(saveSlotPanelsLayout.transform, false);

            saveSlotIndicators.Add(instantiatedSaveSlotIndicator);

            instantiatedSaveSlotIndicator.GetComponent<SaveSlotPanel>().SetInfo(SaveManager.instance.globalSavedGameInfoData.saveFilesInfo[i]);
        }
    }

    public void UpdateSaveSlotInfo(SaveManager.SavedGameFilesInfoData.SaveFileInfo saveFileInfo)
    {
        saveSlotIndicators[saveFileInfo.fileIndex - 1].GetComponent<SaveSlotPanel>().SetInfo(saveFileInfo);
    }
}
