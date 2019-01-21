using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class EventsInfoManager : MonoBehaviour {

    public static EventsInfoManager instance;
    void Awake()
    {
        if (instance != null) { Debug.LogError("More than one EventsInfoManager in scene !"); return; }
        instance = this;
    }

    [Header("UI")]
    public GameObject buildingUnlockedPanel;

    [Header("Prefabs")]
    public GameObject newBuildingInfoUIPrefab;

    public void DisplayNewBuildingsInfo(List<BuildingManager.BuildingType> newBuildingTypes)
    {
        buildingUnlockedPanel.SetActive(true);
        buildingUnlockedPanel.GetComponent<BuildingsUnlockedPanel>().DisplayNewBuildingsInfo(newBuildingTypes);
    }
}
