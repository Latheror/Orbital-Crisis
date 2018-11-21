using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BuildingsUnlockedPanel : MonoBehaviour {

    public static BuildingsUnlockedPanel instance;
    void Awake()
    {
        if (instance != null) { Debug.LogError("More than one BuildingsUnlockedPanel in scene !"); return; }
        instance = this;
    }

    [Header("UI")]
    public GameObject buildingImageUI;
    public TextMeshProUGUI buildingNameTextUI;
    public TextMeshProUGUI buildingDescriptionTextUI;

    [Header("Settings")]
    public BuildingManager.BuildingType buildingType;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void SetInfo()
    {
        SetBuildingImage();
        SetBuildingName();
        SetBuildingDescription();
    }

    private void SetBuildingImage()
    {
        buildingImageUI.GetComponent<Image>().sprite = buildingType.buildingImage;
    }

    private void SetBuildingName()
    {
        buildingNameTextUI.text = buildingType.name;
    }

    private void SetBuildingDescription()
    {
        buildingDescriptionTextUI.text = buildingType.description;
    }

    public void DisplayNewBuildingsInfo(List<BuildingManager.BuildingType> newBuildingTypes)
    {
        buildingType = newBuildingTypes[0]; // Temp
        SetInfo();

        GameManager.instance.gameState = GameManager.GameState.Pause;

        //DisplayPanel(true);   Already done in EventsInfoManager
    }

    public void DisplayPanel(bool display)
    {
        gameObject.SetActive(display);
    }

    public void OkButtonClicked()
    {
        Debug.Log("Ok Button Clicked");
        ClosePanelAndResume();
    }

    public void ClosePanelAndResume()
    {
        DisplayPanel(false);
        GameManager.instance.gameState = GameManager.GameState.Default;
        //LevelManager.instance.ResumeNewLevelActionsAfterNewBuildingInfoDisplay();
    }
}
