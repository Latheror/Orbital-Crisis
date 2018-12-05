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

    public GameObject theShieldShortcut;




    public void PlanetaryShieldShortcutClicked()
    {
        TheShieldControlPanel.instance.DisplayPanel(true);
    }


    public void DisplayTheShieldShortcut(bool display)
    {
        theShieldShortcut.SetActive(display);
    }
}
