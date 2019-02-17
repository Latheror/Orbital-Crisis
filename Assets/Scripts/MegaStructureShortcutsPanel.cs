using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MegaStructureShortcutsPanel : MonoBehaviour {

    public static MegaStructureShortcutsPanel instance;
    void Awake()
    {
        if (instance != null) { Debug.LogError("More than one MegaStructureShortcutsPanel in scene !"); return; }
        instance = this;
    }

    [Header("UI")]
    public GameObject displayPanel;
    public GameObject planetaryShieldShortcut;
    public GameObject megaCollectorShortcut;
    public GameObject dysonSphereShortcut;

    [Header("Operation")]
    public List<TechnoShortcutItem> shortcutItems = new List<TechnoShortcutItem>();
    public bool panelOpen = false;

    private void Start()
    {
        Init();
        SetupNewGame();
    }

    public void Init()
    {
        shortcutItems = new List<TechnoShortcutItem>();
        shortcutItems.Add(new TechnoShortcutItem(planetaryShieldShortcut, 1));
        shortcutItems.Add(new TechnoShortcutItem(megaCollectorShortcut, 2));
        shortcutItems.Add(new TechnoShortcutItem(dysonSphereShortcut, 3));

        panelOpen = true;
        OnOpenClosePanelButtonClick();
    }

    public void HideAllShortcuts()
    {
        foreach (TechnoShortcutItem shortcutItem in shortcutItems)
        {
            shortcutItem.go.SetActive(false);
        }
    }

    public void SetupNewGame()
    {
        HideAllShortcuts();
        DisplayPanel(false);
    }


    public void DisplayPanel(bool display)
    {
        displayPanel.SetActive(display);
    }

    public void PlanetaryShieldShortcutClicked()
    {
        MegaStructureManager.instance.SelectPlanetaryShield(true);
    }

    public void MegaCollectorShortcutClicked()
    {
        MegaStructureManager.instance.SelectCollector(true);
    }

    public void DysonSphereShortcutClicked()
    {
        MegaStructureManager.instance.SelectDysonSphere(true);
    }

    public TechnoShortcutItem GetTechnoShortcutItemFromTechnoID(int id)
    {
        TechnoShortcutItem foundShortcurItem = null;
        foreach (TechnoShortcutItem shortcutItem in shortcutItems)
        {
            if(shortcutItem.technoId == id)
            {
                foundShortcurItem = shortcutItem;
                break;
            }
        }
        return foundShortcurItem;
    }

    public void EnableTechnoShortcutItem(int technoId)
    {
        foreach (TechnoShortcutItem shortcutItem in shortcutItems)
        {
            if (shortcutItem.technoId == technoId)
            {
                DisplayPanel(true);
                shortcutItem.go.SetActive(true);
                break;
            }
        }
    }

    public void OnOpenClosePanelButtonClick()
    {
        Debug.Log("OnOpenClosePanelButtonClick");
        Animator animator = GetComponent<Animator>();
        if(panelOpen)
        {
            animator.SetTrigger("CloseTrigger");
            panelOpen = false;
        }
        else
        {
            animator.SetTrigger("OpenTrigger");
            panelOpen = true;
        }
    }

    public class TechnoShortcutItem
    {
        public GameObject go;
        public int technoId;

        public TechnoShortcutItem(GameObject shortcut, int technoId)
        {
            this.go = shortcut;
            this.technoId = technoId;
        }
    }
}
