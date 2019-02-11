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

    [Header("Operation")]
    public List<MegaStructure> availableMegaStructures;

    [Header("Planetary Shield")]
    public GameObject planetaryShield;
    public GameObject planetaryShieldActivationAnimationGo;
    public GameObject planetaryShieldActivationAnimationGoParticleSystem;
    public GameObject planetaryShieldTechnoItem;

    [Header("MegaCollector")]
    public GameObject megaCollector;
    //public GameObject planetaryShieldActivationAnimationGo;
    //public GameObject planetaryShieldActivationAnimationGoParticleSystem;
    public GameObject collectorTechnoItem;

    [Header("Dyson Sphere")]
    public GameObject dysonSphere;
    public GameObject dysonSphereTechnoItem;

    [Header("UI")]
    public GameObject leftPanel;
    public GameObject planetaryShieldControlPanel;

    public GameObject planetaryShield_logo;
    public GameObject megaCollector_logo;
    public GameObject dysonSphere_logo;

    public Color shieldUnlockedColor;
    public Color megaCollectorUnlockedColor;
    public Color dysonSphereUnlockedColor;

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
        availableMegaStructures.Add(new MegaStructure(3, "Dyson Sphere", dysonSphere));
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

    public void DysonSphereTechnoButtonClicked()
    {
        Debug.Log("DysonSphereTechnoButtonClicked");

        if (TechTreeManager.instance.GetTechnologyByID(24).available && !TechTreeManager.instance.GetTechnologyByID(24).unlocked)
        {
            TechTreeManager.instance.UnlockTechnology(TechTreeManager.instance.GetTechnologyByID(24));
        }
    }


    public void DeselectAllControlPanels()
    {
        PlanetaryShieldControlPanel.instance.DisplayPanel(false);
        CollectorControlPanel.instance.DisplayPanel(false);
        DysonSphereControlPanel.instance.DisplayPanel(false);
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

    public void SelectDysonSphere(bool select)
    {
        if (select)
        {
            DeselectAllControlPanels();
            GameManager.instance.ChangeSelectionState(GameManager.SelectionState.DysonSphereSelected);
            DysonSphereControlPanel.instance.DisplayPanel(true);
        }
        else
        {
            DysonSphereControlPanel.instance.DisplayPanel(false);
        }
    }

    public void UnlockMegaStructureActions(int megaStructureIndex)
    {
        //Debug.Log("UnlockMegaStructureActions | Index [" + megaStructureIndex + "]");
        if (megaStructureIndex == 16)
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
        else
        if (megaStructureIndex == 24)
        {
            // Enable Dyson Sphere itself
            dysonSphere.SetActive(true);
            DysonSphere.instance.isUnlocked = true;

            // Initialize settings
            DysonSphere.instance.Initialize();

            // Disable "Activate!" text
            if (MegaStructuresPanel.instance != null)
            {
                MegaStructuresPanel.instance.activateDysonSphereTextGo.SetActive(false);
            }

            // Change collector color
            dysonSphere_logo.GetComponent<Image>().color = dysonSphereUnlockedColor;
        }
    }

    public MegaStructuresData BuildMegaStructuresData()
    {
        PlanetaryShield ps = planetaryShield.GetComponent<PlanetaryShield>();
        MegaCollector mc = megaCollector.GetComponent<MegaCollector>();
        DysonSphere ds = dysonSphere.GetComponent<DysonSphere>();
        return new MegaStructuresData(ps.enabled, ps.radius, ps.damagePower, mc.isUnlocked, mc.currentCollectionPointNb, mc.currentCollectionSpeed, ds.isUnlocked, ds.currentStructurePoints, ds.currentAutoRepairState);
    }

    public void SetupSavedMegaStructuresData(MegaStructuresData megaStructuresData)
    {
        PlanetaryShield ps = planetaryShield.GetComponent<PlanetaryShield>();
        MegaCollector mc = megaCollector.GetComponent<MegaCollector>();
        DysonSphere ds = dysonSphere.GetComponent<DysonSphere>();

        Debug.Log("SetupSavedMegaStructuresData | PlanetaryShield unlocked [" + ps.isUnlocked + "] | MegaCollector unlocked [" + mc.isUnlocked + "] | DysonSphere unlocked [" + ds.isUnlocked + "]");

        ps.isUnlocked = megaStructuresData.shieldUnlocked;
        if (ps.isUnlocked)
        {
            //ps.Initialize();
            ps.ReceiveSettings(megaStructuresData.shieldRadius, megaStructuresData.shieldPower, PlanetaryShield.ViewMode.Full);
        }

        mc.isUnlocked = megaStructuresData.collectorUnlocked;
        if(mc.isUnlocked)
        {
            //mc.Initialize();
            mc.BuildCollectionPoints();
            mc.Configure(megaStructuresData.collectionSpeed, megaStructuresData.collectionPointsNb);
        }
            

        ds.isUnlocked = megaStructuresData.dysonSphereUnlocked;
        if(ds.isUnlocked)
        {
            //ds.Initialize();
            ds.Configure(megaStructuresData.dysonSphereStructurePoints, megaStructuresData.dysonSphereAutoRepair);
        }
    }

    [System.Serializable]
    public class MegaStructuresData
    {
        // Shield
        public bool shieldUnlocked;
        public float shieldRadius;
        public float shieldPower;

        // Collector
        public bool collectorUnlocked;
        public int collectionPointsNb;
        public float collectionSpeed;

        // Dyson Sphere
        public bool dysonSphereUnlocked;
        public float dysonSphereStructurePoints;
        public bool dysonSphereAutoRepair;

        public MegaStructuresData(bool shieldUnlocked, float shieldRadius, float shieldPower, bool collectorUnlocked, int collectionPointsNb, float collectionSpeed, bool dysonSphereUnlocked, float dysonSphereStructurePoints, bool dysonSphereAutoRepair)
        {
            this.shieldUnlocked = shieldUnlocked;
            this.shieldRadius = shieldRadius;
            this.shieldPower = shieldPower;
            this.collectorUnlocked = collectorUnlocked;
            this.collectionPointsNb = collectionPointsNb;
            this.collectionSpeed = collectionSpeed;
            this.dysonSphereUnlocked = dysonSphereUnlocked;
            this.dysonSphereStructurePoints = dysonSphereStructurePoints;
            this.dysonSphereAutoRepair = dysonSphereAutoRepair;
        }
    }

}
