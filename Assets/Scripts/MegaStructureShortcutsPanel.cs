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

    public GameObject displayPanel;
    public List<TechnoShortcutItem> shortcutItems = new List<TechnoShortcutItem>();

    public GameObject theShieldShortcut;

    private void Start()
    {
        Init();
        SetupNewGame();
    }

    public void Init()
    {
        shortcutItems = new List<TechnoShortcutItem>();
        shortcutItems.Add(new TechnoShortcutItem(theShieldShortcut, 16));
    }

    public void HideAllShortCuts()
    {
        foreach (TechnoShortcutItem shortcutItem in shortcutItems)
        {
            shortcutItem.go.SetActive(false);
        }
    }

    public void SetupNewGame()
    {
        HideAllShortCuts();
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
