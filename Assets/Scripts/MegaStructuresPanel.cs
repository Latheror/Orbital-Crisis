using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MegaStructuresPanel : MonoBehaviour {

    public static MegaStructuresPanel instance;
    void Awake()
    {
        if (instance != null) { Debug.LogError("More than one MegaStructuresPanel in scene !"); return; }
        instance = this;
    }

    public enum DisplayMode { ButtonsMenu, Trees};

    [Header("UI")]
    public GameObject buttonsMenuPanel;
    public GameObject megaStructuresTreePanel;
    public GameObject megaStructurePanel1;
    public GameObject megaStructurePanel2;
    public GameObject megaStructurePanel3;
    public GameObject megaStructurePanel4;
    public GameObject activateShieldTextGo;
    public GameObject activateCollectorTextGo;
    public GameObject activateDysonSphereTextGo;
    public Color finalMegaStructureAvailableColor;

    [Header("Operation")]
    public DisplayMode currentDisplayMode = DisplayMode.ButtonsMenu;

    public void InitializePanels()
    {
        currentDisplayMode = DisplayMode.ButtonsMenu;
        DisplayMegaStructuresTreePanel(false);
        DisplayButtonsMenuPanel(true);
    }

    public void BackButtonClicked()
    {
        switch(currentDisplayMode)
        {
            case DisplayMode.ButtonsMenu:
            {
                TechTreeManager.instance.BackFromMegaStructuresPanel();
                break;
            }
            case DisplayMode.Trees:
            {
                DisplayMegaStructuresTreePanel(false);
                DisplayButtonsMenuPanel(true);
                currentDisplayMode = DisplayMode.ButtonsMenu;
                break;
            }
        }
    }

    public void MegaStructurePanel1ButtonClicked()
    {
        MegaStructurePanelButtonOperations();
        EnableMegaStructurePanel(1);
    }

    public void MegaStructurePanel2ButtonClicked()
    {
        MegaStructurePanelButtonOperations();
        EnableMegaStructurePanel(2);
    }

    public void MegaStructurePanel3ButtonClicked()
    {
        // Nothing for now
    }

    public void MegaStructurePanel4ButtonClicked()
    {
        MegaStructurePanelButtonOperations();
        EnableMegaStructurePanel(4);
    }

    public void MegaStructurePanelButtonOperations()
    {
        DisplayButtonsMenuPanel(false);
        DisplayMegaStructuresTreePanel(true);
        DisableAllMegaStructurePanels();

        currentDisplayMode = DisplayMode.Trees;
    }

    public void DisableAllMegaStructurePanels()
    {
        megaStructurePanel1.SetActive(false);
        megaStructurePanel2.SetActive(false);
        megaStructurePanel3.SetActive(false);
        megaStructurePanel4.SetActive(false);
    }

    public void EnableMegaStructurePanel(int panelIndex)
    {
        switch (panelIndex)
        {
            case 1:
            {
                megaStructurePanel1.SetActive(true);
                break;
            }
            case 2:
            {
                megaStructurePanel2.SetActive(true);
                break;
            }
            case 3:
            {
                megaStructurePanel3.SetActive(true);
                break;
            }
            case 4:
            {
                megaStructurePanel4.SetActive(true);
                break;
            }
            default:
                break;
        }
    }

    public void DisplayButtonsMenuPanel(bool display)
    {
        buttonsMenuPanel.SetActive(display);
    }

    public void DisplayMegaStructuresTreePanel(bool display)
    {
        megaStructuresTreePanel.SetActive(display);
    }

}
