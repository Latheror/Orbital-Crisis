using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TechTreeManager : MonoBehaviour
{

    public static TechTreeManager instance;
    void Awake()
    {
        if (instance != null) { Debug.LogError("More than one TechTreeManager in scene !"); return; }
        instance = this;
    }

    [Header("Data")]
    public List<Technology> technologies = new List<Technology>();

    [Header("UI")]
    public GameObject technologiesPanel;
    public GameObject technoListPanel;

    public Sprite availableTechnoUIBorder;
    public Sprite unavailableTechnoUIBorder;
    public Sprite tooExpensiveTechnoUIBorder;
    public Sprite unlockedTechnoUIBorder;
    public Sprite notUnlockedTechnoUIBorder;

    public Color availableColor;
    public Color unavailableColor;

    public Sprite disabledConnectionSprite;
    public Sprite enabledConnectionSprite;

    public Color canPayTechnologyColor;
    public Color cantPayTechnologyColor;

    public TextMeshProUGUI experiencePointsText;
    public TextMeshProUGUI artifactsNbText;

    public GameObject techTab1;
    public GameObject techTab2;

    public GameObject mainTabletPanel;
    public GameObject megaStructuresTabletPanel;

    public GameObject technologyInfoPanel;

    [Header("Techno Items")]
    public GameObject missileTurretTechnoItem;
    public GameObject freezingTurretTechnoItem;
    public GameObject healingTurretTechnoItem;
    public GameObject shockSatelliteTechnoItem;
    public GameObject stormSatelliteTechnoItem;
    public GameObject meteorCrusherTechnoItem;
    public GameObject solarStationTechnoItem;
    public GameObject spaceportTechnoItem;
    public GameObject recyclingStationItem;

    public GameObject disk1TechnoItem;
    public GameObject disk2TechnoItem;
    public GameObject disk3TechnoItem;

    // The Shield
    public GameObject shield_generatorTechnoItem;
    public GameObject shield_stabiliserTechnoItem;
    public GameObject shield_deflectorTechnoItem;
    public GameObject theShield_TechnoItem;

    [Header("Operation")]
    public Technology selectedTechno;
    public TechnologyData[] technologiesData;

    public void InitializeTechnologies()
    {
        // Init technos

        // Buildings
        Technology missileTurretTechno = new Technology(1, "Missile Turret", 100, true, false, new List<Technology>(), "", missileTurretTechnoItem, 2, 0, 0, false, 0);
        Technology freezingTurretTechno = new Technology(2, "Freezing Turret", 400, false, false, new List<Technology>(), "", freezingTurretTechnoItem, 3, 0, 0, false, 0);
        Technology healingTurretTechno = new Technology(3, "Healing Turret", 750, false, false, new List<Technology>(), "", healingTurretTechnoItem, 9, 0, 0, false, 0);
        Technology shockSatelliteTechno = new Technology(4, "Shock Satellite", 400, true, false, new List<Technology>(), "", shockSatelliteTechnoItem, 6, 0, 0, false, 0);
        Technology stormSatelliteTechno = new Technology(5, "Storm Satellite", 1000, false, false, new List<Technology>(), "", stormSatelliteTechnoItem, 11, 0, 0, false, 0);
        Technology meteorCrusherTechno = new Technology(6, "Meteor Crusher", 3000, false, false, new List<Technology>(), "", meteorCrusherTechnoItem, 12, 0, 0, false, 0);
        Technology solarStationTechno = new Technology(7, "Solar Station", 1000, false, false, new List<Technology>(), "", solarStationTechnoItem, 8, 0, 0, false, 0);
        Technology spaceportTechno = new Technology(8, "Spaceport", 2000, false, false, new List<Technology>(), "", spaceportTechnoItem, 10, 0, 0, false, 0);
        Technology recyclingStationTechno = new Technology(9, "Recycling Station", 600, true, false, new List<Technology>(), "", recyclingStationItem, 7, 0, 0, false, 0);

        // Disks
        Technology disk1Techno = new Technology(10, "Disk I", 300, true, true, new List<Technology>(), "", disk1TechnoItem, 0, 1, 0, false, 0);
        Technology disk2Techno = new Technology(11, "Disk II", 800, false, false, new List<Technology>(), "", disk2TechnoItem, 0, 2, 0, false, 0);
        Technology disk3Techno = new Technology(12, "Disk III", 3000, false, false, new List<Technology>(), "", disk3TechnoItem, 0, 3, 0, false, 0);

        // The Shield
        Technology shield_generatorTechno = new Technology(13, "Generator", 1500, false, false, new List<Technology>(), "", shield_generatorTechnoItem, 0, 0, 0, true, 5);
        Technology shield_stabiliserTechno = new Technology(14, "Stabiliser", 1500, false, false, new List<Technology>(), "", shield_stabiliserTechnoItem, 0, 0, 0, true, 7);
        Technology shield_deflectorTechno = new Technology(15, "Deflector", 1500, false, false, new List<Technology>(), "", shield_deflectorTechnoItem, 0, 0, 0, true, 10);
        // Final step
        Technology theShield_Techno = new Technology(16, "Deflector", 3000, false, false, new List<Technology>(), "", theShield_TechnoItem, 0, 0, 1, true, 0);

        // Link technos
        missileTurretTechno.unlockingTechnologies.Add(freezingTurretTechno);
        freezingTurretTechno.unlockingTechnologies.Add(healingTurretTechno);
        shockSatelliteTechno.unlockingTechnologies.Add(stormSatelliteTechno);
        stormSatelliteTechno.unlockingTechnologies.Add(meteorCrusherTechno);
        recyclingStationTechno.unlockingTechnologies.Add(solarStationTechno);
        solarStationTechno.unlockingTechnologies.Add(spaceportTechno);

        disk1Techno.unlockingTechnologies.Add(disk2Techno);
        disk2Techno.unlockingTechnologies.Add(disk3Techno);



        technologies = new List<Technology>()
        {
            // Buildings
            missileTurretTechno, freezingTurretTechno, healingTurretTechno, shockSatelliteTechno, stormSatelliteTechno,
            meteorCrusherTechno, solarStationTechno, spaceportTechno, recyclingStationTechno,
            // Disks
            disk1Techno, disk2Techno, disk3Techno,
            // Megastructures
                // The Shield
                shield_generatorTechno, shield_stabiliserTechno, shield_deflectorTechno
        };

        // Init locked/unlocked techno color
        foreach (Technology techno in technologies)
        {
            if(!techno.isMegaStructureTechnology)
            {
                techno.UIitem.GetComponent<TechnologyItem>().InitializeUIElements();
            }
            else
            {
                techno.UIitem.GetComponent<MegaStructureTechnologyItem>().InitializeUIElements();
            }
        }

        // Unlock Technos
        foreach (Technology techno in technologies)
        {
            if(techno.unlockedAtStart)
            {
                UnlockTechnology(techno);
            }
        }
    }

    public void DisplayPanel(bool display)
    {
        technologiesPanel.SetActive(display);
    }

    public void BackButtonClicked()
    {
        technologiesPanel.SetActive(false);
        HideTechnoInfoPanel();
        PanelsManager.instance.GoBackToControlsPanel();        
    }

    public void HideTechnoInfoPanel()
    {
        technologyInfoPanel.SetActive(false);
    }

    public bool CanPayTechnology(Technology techno)
    {
        bool canPay = false;
        if(techno.isMegaStructureTechnology)
        {
            canPay = ((ScoreManager.instance.experiencePoints >= techno.experienceCost) && (ScoreManager.instance.artifactsNb >= techno.artifactCost));
        }
        else
        {
            canPay = (ScoreManager.instance.experiencePoints >= techno.experienceCost);
        }
        return canPay;
    }

    public void PayTechnology(Technology techno)
    {
        ScoreManager.instance.DecreaseExperiencePoints(techno.experienceCost);
        if (techno.isMegaStructureTechnology)
        {
            ScoreManager.instance.DecreaseArtifactsNb(techno.artifactCost);
        }
    }

    public void UnlockTechnologyRequest(Technology techno)
    {
        Debug.Log("UnlockTechnologyRequest | CanPay [" + CanPayTechnology(techno) + "]");
        if (techno.available && CanPayTechnology(techno))
        {
            PayTechnology(techno);

            UnlockTechnology(techno);
        }
    }

    public void UnlockSelectedTechnologyRequest()
    {
        Debug.Log("UnlockTechnologyRequest | CanPay [" + CanPayTechnology(selectedTechno) + "]");
        if (selectedTechno.available && CanPayTechnology(selectedTechno))
        {
            PayTechnology(selectedTechno);

            UnlockTechnology(selectedTechno);
        }
    }

    public void UnlockTechnology(Technology techno)
    {
        techno.unlocked = true;

        if (techno.unlockedBuildingIndex != 0)
        {
            UnlockBuilding(techno.unlockedBuildingIndex);
        }

        if (techno.unlockedDiskIndex != 0)
        {
            UnlockDisk(techno.unlockedDiskIndex);
        }

        foreach (Technology unlockedByTechno in techno.unlockingTechnologies)
        {
            Debug.Log("A new technology is available [" + unlockedByTechno.name + "]");
            unlockedByTechno.available = true;
            UpdateTechnologyItemDisplay(unlockedByTechno);
        }

        foreach (Technology tech in technologies)
        {
            techno.UIitem.GetComponent<TechnologyItem>().UpdateCostDisplay();
        }

        UpdateTechnologyItemDisplay(techno);
    }

    public void UpdateTechnologyItemDisplay(Technology tech)
    {
        tech.UIitem.GetComponent<TechnologyItem>().UpdateItemDisplay();
    }

    public void UnlockBuilding(int buildingIndex)
    {
        BuildingManager.instance.UnlockBuildingType(BuildingManager.instance.GetBuildingTypeByID(buildingIndex));
    }

    public void UnlockDisk(int diskIndex)
    {
        SurroundingAreasManager.instance.UnlockDisk(diskIndex);
    }

    public void UpdateTechnologyCostIndicatorsDisplay()
    {
        foreach (Technology techno in technologies)
        {
            if(techno.isMegaStructureTechnology)
            {
                // Todo
            }
            else if (techno.UIitem.GetComponent<TechnologyItem>() != null)
            {
                techno.UIitem.GetComponent<TechnologyItem>().UpdateCostDisplay();
            }
        }
    }

    public void SetExperiencePointsDisplay(int experiencePoints)
    {
        experiencePointsText.text = experiencePoints.ToString();
    }

    public void SetArtifactsNbDisplay(int artifactsNb)
    {
        artifactsNbText.text = artifactsNb.ToString();
    }

    public void TabButton1Clicked()
    {
        Debug.Log("Switch to Techno Tab 1");
        techTab1.SetActive(true);
        techTab2.SetActive(false);
        DisplayTechnologyInfoPanel(false);
    }

    public void TabButton2Clicked()
    {
        Debug.Log("Switch to Techno Tab 2");
        techTab1.SetActive(false);
        techTab2.SetActive(true);
        DisplayTechnologyInfoPanel(false);
    }

    public void MegaStructurePanelButtonClicked()
    {
        Debug.Log("Switch to Mega Structures Panel");

        mainTabletPanel.SetActive(false);
        DisplayMegaStructuresPanel(true);
    }

    public void SetSelectedTechno(Technology technology)
    {
        selectedTechno = technology;
    }

    public void DisplayTechnologyInfoPanel(bool display)
    {
        technologyInfoPanel.SetActive(display);
    }

    public void DisplayMegaStructuresPanel(bool display)
    {
        megaStructuresTabletPanel.SetActive(display);
    }

    public TechnologyData[] BuildTechnologyData()
    {
        int technologyNb = technologies.Count;
        Debug.Log("BuildTechnologyData | Nb: " + technologyNb);
        technologiesData = new TechnologyData[technologyNb];

        for (int i = 0; i < technologies.Count; i++)
        {
            technologiesData[i] = (new TechnologyData(technologies[i].id, technologies[i].unlocked));
            Debug.Log("Adding technology [" + i + "]");
        }

        return technologiesData;
    }

    public void SetupSavedTechnologies(TechnologyData[] technologiesData)
    {
        Debug.Log("SetupSavedTechnologies | Nb [" + technologiesData.Length + "]");
        for (int i=0; i<technologiesData.Length; i++)
        {
            if(technologiesData[i].unlocked)    // Technology was previously unlocked
            {
                Debug.Log("Unlocking saved technology [" + GetTechnologyByID(technologiesData[i].technologyID).name + "]");
                UnlockTechnology(GetTechnologyByID(technologiesData[i].technologyID));
            }
        }

        ShopPanel.instance.UpdateBuildingItemsAvailability();
    }

    public Technology GetTechnologyByID(int id)
    {
        Technology techno = null;
        foreach (Technology technology in technologies)
        {
            if (technology.id == id)
            {
                techno = technology;
                break;
            }
        }
        return techno;
    }

    public void BackFromMegaStructuresPanel()
    {
        megaStructuresTabletPanel.SetActive(false);
        mainTabletPanel.SetActive(true);
    }

    [System.Serializable]
    public class Technology
    {
        public int id;
        public string name;
        public int experienceCost;
        public int artifactCost; // For MegaStructure technos
        public bool availableAtStart;
        public bool available;
        public List<Technology> unlockingTechnologies;  // Techs unlocked by this one
        public bool unlockedAtStart;
        public bool unlocked;
        public string imageFilename;
        public GameObject UIitem;
        public int unlockedBuildingIndex;
        public int unlockedDiskIndex;
        public int unlockedMegaStructureIndex;
        public bool isMegaStructureTechnology;

        public Technology(int id, string name, int experienceCost, bool availableAtStart, bool unlockedAtStart, List<Technology> unlockingTechnologies, string imageFilename, GameObject UIitem, int unlockedBuildingIndex, int unlockedDiskIndex, int unlockedMegaStructureIndex, bool isMegaStructureTechnology, int artifactCost)
        {
            this.id = id;
            this.name = name;
            this.experienceCost = experienceCost;
            this.availableAtStart = availableAtStart;
            this.available = availableAtStart;
            this.unlockingTechnologies = unlockingTechnologies;
            this.imageFilename = imageFilename;
            this.UIitem = UIitem;
            this.unlockedAtStart = unlockedAtStart;
            this.unlocked = unlockedAtStart;
            this.unlockedBuildingIndex = unlockedBuildingIndex;
            this.unlockedDiskIndex = unlockedDiskIndex;
            this.unlockedMegaStructureIndex = unlockedMegaStructureIndex;
            this.isMegaStructureTechnology = isMegaStructureTechnology;
            this.artifactCost = artifactCost;

            if (this.isMegaStructureTechnology)
            {
                if(this.UIitem.GetComponent<MegaStructureTechnologyItem>() != null)
                {
                    this.UIitem.GetComponent<MegaStructureTechnologyItem>().associatedTechnology = this;
                }
            }
            else
            {
                this.UIitem.GetComponent<TechnologyItem>().associatedTechnology = this;
            }
        }
    }


    [System.Serializable]
    public class TechnologyData
    {
        public int technologyID;
        public bool unlocked;

        public TechnologyData(int technologyID, bool unlocked)
        {
            this.technologyID = technologyID;
            this.unlocked = unlocked;
        }

    }


}
