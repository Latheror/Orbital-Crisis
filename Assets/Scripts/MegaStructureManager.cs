using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MegaStructureManager : MonoBehaviour {

    public static MegaStructureManager instance;
    void Awake()
    {
        if (instance != null) { Debug.LogError("More than one MegaStructureManager in scene !"); return; }
        instance = this;
    }


    public List<MegaStructure> availableMegaStructures;

    public GameObject planetaryShield;

    public GameObject leftPanel;
    public GameObject theShieldControlPanel;


    public void DefineAvailableMegastructures()
    {
        availableMegaStructures.Add(new MegaStructure(1, "Planetary Shield", planetaryShield));
    }

    public void EnableMegaStructure(MegaStructure megaStructure, bool enable)
    {
        megaStructure.go.SetActive(enable);
    }

    public MegaStructure GetMegaStructureFromIndex(int index)
    {
        MegaStructure ms = null;
        foreach (MegaStructure megaStructure in availableMegaStructures)
        {
            if(megaStructure.id == index)
            {
                ms = megaStructure;
                break;
            }
        }
        return ms;
    }



    // Temporary
    public void TheShieldTechnoButtonClicked()
    {
        Debug.Log("TheShieldTechnoButtonClicked");

        bool shieldWasActive = planetaryShield.activeSelf;
        bool activation = !shieldWasActive;

        // Temp
        planetaryShield.SetActive(activation);
        leftPanel.SetActive(activation);
    }




    // Control buttons
    public void TheShieldControlButtonClicked()
    {
        Debug.Log("TheShieldControlButtonClicked");
    }

}
