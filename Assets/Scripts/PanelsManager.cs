using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanelsManager : MonoBehaviour {

    public static PanelsManager instance;
    void Awake()
    {
        if (instance != null) { Debug.LogError("More than one PanelsManager in scene !"); return; }
        instance = this;
    }

    [Header("UI")]
    public GameObject controlsPanel;
    public GameObject shopPanel;
    public GameObject defaultBottomPanel;

    // Use this for initialization
    void Start () {
        SwitchFromShopToControlsPanel();
    }
    
    // Update is called once per frame
    void Update () {
        
    }

    public void SwitchFromControlsToShopPanel()
    {
        // Tutorial indicator //
        TutorialManager.instance.DisplayIndicator(1, false);
        TutorialManager.instance.DisplayIndicatorIfNotDisplayedYet(2);
        // ------------------ //

        defaultBottomPanel.SetActive(false);
        shopPanel.SetActive(true);
    }

    public void SwitchFromControlsToTechTreePanel()
    {
        GameManager.instance.Pause();
        defaultBottomPanel.SetActive(false);
        TechTreeManager.instance.DisplayPanel(true);
    }

    public void SwitchFromShopToControlsPanel()
    {
        shopPanel.SetActive(false);
        defaultBottomPanel.SetActive(true);
    }

    public void BackButtonClicked()
    {
        SwitchFromShopToControlsPanel();
        BuildingManager.instance.buildingState = BuildingManager.BuildingState.Default;
        GameManager.instance.ChangeSelectionState(GameManager.SelectionState.Default);
    }

    public void GoBackToControlsPanel()
    {
        GameManager.instance.UnPause();
        defaultBottomPanel.SetActive(true);
    }
}
