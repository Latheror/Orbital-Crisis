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
    public GameObject defaultBottomPanel;
    public GameObject fleetPanel;
    public GameObject gameOverPanel;

    public void SwitchFromControlsToTechTreePanel()
    {
        GameManager.instance.Pause();
        defaultBottomPanel.SetActive(false);
        TechTreeManager.instance.DisplayPanel(true);
    }

    public void SwitchFromControlToFleetPanel()
    {
        GameManager.instance.Pause();
        defaultBottomPanel.SetActive(false);
        fleetPanel.SetActive(true);
        fleetPanel.GetComponent<FleetPanel>().BuildInfo();
    }

    public void SwitchFromFleetToControlPanel()
    {
        GameManager.instance.UnPause();
        defaultBottomPanel.SetActive(true);
        fleetPanel.SetActive(false);
    }

    /*public void SwitchFromPanelToPanel(int panelFromId, int panelToId)
    {
        // FROM
        switch(panelFromId)
        {
            case 0: // Control panel
            {
                defaultBottomPanel.SetActive(false);
                break;
            }
            case 1: // Shop panel
            {
                shopPanel.SetActive(false);
                break;
            }
            case 2: // Lab panel
            {
                GameManager.instance.Pause();
                defaultBottomPanel.SetActive(false);
                TechTreeManager.instance.DisplayPanel(true);
                break;
            }
            case 3: // Fleet panel
            {
                break;
            }
        }

        // TO
        switch (panelToId)
        {
            case 0: // Control panel
            {
                break;
            }
            case 1: // Shop panel
            {
                shopPanel.SetActive(false);
                defaultBottomPanel.SetActive(true);
                break;
            }
            case 2: // Lab panel
            {
                GameManager.instance.Pause();
                defaultBottomPanel.SetActive(false);
                TechTreeManager.instance.DisplayPanel(true);
                break;
            }
            case 3: // Fleet panel
            {
                break;
            }
        }
    }*/

    public void BackButtonClicked()
    {
        BuildingManager.instance.buildingState = BuildingManager.BuildingState.Default;
        GameManager.instance.ChangeSelectionState(GameManager.SelectionState.Default);
    }

    public void GoBackToControlsPanel()
    {
        GameManager.instance.UnPause();
        defaultBottomPanel.SetActive(true);
    }

    public void DisplayGameOverPanel(bool display)
    {
        if(display)
        {
            GameManager.instance.Pause();
            gameOverPanel.SetActive(true);
        }
        else
        {
            GameManager.instance.UnPause();
            gameOverPanel.SetActive(false);
        }
    }
}
