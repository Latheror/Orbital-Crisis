using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SpaceportInfoPanel : MonoBehaviour {

    public static SpaceportInfoPanel instance;
    void Awake()
    {
        if (instance != null) { Debug.LogError("More than one SpaceportInfoPanel in scene !"); return; }
        instance = this;
    }

    [Header("UI")]
    public GameObject displayPanel;
    public TextMeshProUGUI usedFleetPointsText;
    public TextMeshProUGUI maxFleetPointsText;
    public GameObject openFleetPanelButton;

    [Header("Operation")]
    public int usedFleetPoints = 0;
    public int maxFleetPoints = 0;

    public void SetInfo()
    {
        ImportInfo();
    }

    public void DisplayInfo()
    {
        SetUsedFleetPointsText();
        SetMaxFleetPointsText();
    }

    public void ImportInfo()
    {
        maxFleetPoints = SpaceshipManager.instance.currentMaxFleetPoints;
        usedFleetPoints = SpaceshipManager.instance.usedFleetPoints;
        DisplayInfo();
    }

    public void SetUsedFleetPointsText()
    {
        usedFleetPointsText.text = usedFleetPoints.ToString();
    }

    public void SetMaxFleetPointsText()
    {
        maxFleetPointsText.text = maxFleetPoints.ToString();
    }

    public void OnOpenFleetPanelButton()
    {
        //Debug.Log("OnOpenFleetPanelButton");
        PanelsManager.instance.DisplayFleetPanel(true);
    }

    public void DisplayPanel(bool display)
    {
        displayPanel.SetActive(display);
    }

    public void SpaceportTouched(GameObject spaceport)
    {
        SetInfo();
        DisplayPanel(true);
    }
}
