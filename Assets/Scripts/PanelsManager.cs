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
    public GameObject fleetPanel;
    public GameObject gameOverPanel;

    public void DisplayFleetPanel(bool display)
    {
        GameManager.instance.Pause();
        if (display)
        {
            fleetPanel.GetComponent<FleetPanel>().BuildInfo();
        }
        TechTreeManager.instance.DisplayPanel(display);
    }

    public void DisplayTechTreePanel(bool display)
    {
        GameManager.instance.Pause();
        TechTreeManager.instance.DisplayPanel(display);
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

    public void DisplayDefaultView()
    {
        GameManager.instance.UnPause();
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

    public void OnFleetButtonTouch()
    {
        DisplayFleetPanel(true);
    }

    public void OnTechTreeButtonTouch()
    {
        DisplayTechTreePanel(true);
    }
}
