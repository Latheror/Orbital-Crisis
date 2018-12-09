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

    public GameObject megaStructurePanel1;
    public GameObject megaStructurePanel2;

    public GameObject activateShieldTextGo;

    public Color finalMegaStructureAvailableColor;

    public void BackButtonClicked()
    {
        TechTreeManager.instance.BackFromMegaStructuresPanel();
    }

    public void MegaStructurePanel1ButtonClicked()
    {
        megaStructurePanel1.SetActive(true);
        megaStructurePanel2.SetActive(false);
    }

    public void MegaStructurePanel2ButtonClicked()
    {
        // Nothing for now
    }
}
