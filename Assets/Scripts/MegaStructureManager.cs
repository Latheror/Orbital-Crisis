using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MegaStructureManager : MonoBehaviour {

    public static MegaStructureManager instance;
    void Awake()
    {
        if (instance != null) { Debug.LogError("More than one MegaStructureManager in scene !"); return; }
        instance = this;
    }


    public List<MegaStructure> availableMegaStructures;

    // Planetary Shield
    public GameObject planetaryShield;
    public GameObject planetaryShieldActivationAnimationGo;
    public GameObject planetaryShieldActivationAnimationGoParticleSystem;
    public GameObject planetaryShieldTechnoItem;

    // MegaCollector
    public GameObject megaCollector;
    //public GameObject planetaryShieldActivationAnimationGo;
    //public GameObject planetaryShieldActivationAnimationGoParticleSystem;
    public GameObject collectorTechnoItem;

    public GameObject leftPanel;
    public GameObject planetaryShieldControlPanel;

    public GameObject planetaryShield_logo;
    public GameObject megaCollector_logo;

    public Color shieldUnlockedColor;
    public Color megaCollectorUnlockedColor;

    public Color canPayColor;
    public Color cantPayColor;

    private void Start()
    {
        DefineAvailableMegastructures();
    }


    public void DefineAvailableMegastructures()
    {
        availableMegaStructures.Add(new MegaStructure(1, "Planetary Shield", planetaryShield));
        availableMegaStructures.Add(new MegaStructure(2, "Mega Collector", megaCollector));
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
    public void PlanetaryShieldTechnoButtonClicked()
    {
        Debug.Log("TheShieldTechnoButtonClicked");

        if(TechTreeManager.instance.GetTechnologyByID(16).available && !TechTreeManager.instance.GetTechnologyByID(16).unlocked)
        {
            TechTreeManager.instance.UnlockTechnology(TechTreeManager.instance.GetTechnologyByID(16));
        }
    }

    public void MegaCollectorTechnoButtonClicked()
    {
        Debug.Log("MegaCollectorTechnoButtonClicked");

        if (TechTreeManager.instance.GetTechnologyByID(20).available && !TechTreeManager.instance.GetTechnologyByID(20).unlocked)
        {
            TechTreeManager.instance.UnlockTechnology(TechTreeManager.instance.GetTechnologyByID(20));
        }
    }

    public void DeselectAllControlPanels()
    {
        PlanetaryShieldControlPanel.instance.DisplayPanel(false);
        CollectorControlPanel.instance.DisplayPanel(false);
    }


    public void SelectPlanetaryShield(bool select)
    {
        if(select)
        {
            DeselectAllControlPanels();
            GameManager.instance.ChangeSelectionState(GameManager.SelectionState.PlanetaryShieldSelected);
            PlanetaryShieldControlPanel.instance.DisplayPanel(true);
        }
        else
        {
            PlanetaryShieldControlPanel.instance.DisplayPanel(false);
        }
    }

    public void SelectCollector(bool select)
    {
        if (select)
        {
            DeselectAllControlPanels();
            GameManager.instance.ChangeSelectionState(GameManager.SelectionState.CollectorSelected);
            CollectorControlPanel.instance.DisplayPanel(true);
        }
        else
        {
            CollectorControlPanel.instance.DisplayPanel(false);
        }
    }

    public void UnlockMegaStructureActions(int megaStructureIndex)
    {
        if(megaStructureIndex == 16)
        {
            // Enable Shield itself
            planetaryShield.SetActive(true);
            PlanetaryShield.instance.isUnlocked = true;

            // Initialize Planetary Shield settings
            PlanetaryShield.instance.Initialize();

            // Disable "Activate!" text
            if(MegaStructuresPanel.instance != null)
            {
                MegaStructuresPanel.instance.activateShieldTextGo.SetActive(false);
            }

            // Change shield color
            planetaryShield_logo.GetComponent<Image>().color = shieldUnlockedColor;
            //planetaryShieldActivationAnimationGo.SetActive(true);
            //planetaryShieldActivationAnimationGoParticleSystem.GetComponent<ParticleSystem>().startSize = planetaryShieldTechnoItem.GetComponent<RectTransform>().rect.width / 3;
        }
        else
        if (megaStructureIndex == 20)
        {
            // Enable MegaCollector itself
            megaCollector.SetActive(true);
            MegaCollector.instance.isUnlocked = true;

            // Initialize settings
            MegaCollector.instance.Initialize();

            // Disable "Activate!" text
            if (MegaStructuresPanel.instance != null)
            {
                MegaStructuresPanel.instance.activateCollectorTextGo.SetActive(false);
            }

            // Change collector color
            megaCollector_logo.GetComponent<Image>().color = megaCollectorUnlockedColor;
        }
    }

}
