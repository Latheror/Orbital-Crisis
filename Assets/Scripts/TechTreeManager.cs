using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TechTreeManager : MonoBehaviour {

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


    void Start () {
        InitializeTechnologies();
	}
	
	void Update () {
		
	}

    public void InitializeTechnologies()
    {
        // Init technos

        // Buildings
        Technology missileTurretTechno = new Technology(1, "Missile Turret", 100, true, new List<Technology>(), "", missileTurretTechnoItem, 2, 0);
        Technology freezingTurretTechno = new Technology(2, "Freezing Turret", 400, false, new List<Technology>(), "", freezingTurretTechnoItem, 3, 0);
        Technology healingTurretTechno = new Technology(3, "Healing Turret", 750, false, new List<Technology>(), "", healingTurretTechnoItem, 9, 0);
        Technology shockSatelliteTechno = new Technology(4, "Shock Satellite", 400, true, new List<Technology>(), "", shockSatelliteTechnoItem, 6, 0);
        Technology stormSatelliteTechno = new Technology(5, "Storm Satellite", 1000, false, new List<Technology>(), "", stormSatelliteTechnoItem, 11, 0);
        Technology meteorCrusherTechno = new Technology(6, "Meteor Crusher", 3000, false, new List<Technology>(), "", meteorCrusherTechnoItem, 12, 0);
        Technology solarStationTechno = new Technology(7, "Solar Station", 1000, false, new List<Technology>(), "", solarStationTechnoItem, 8, 0);
        Technology spaceportTechno = new Technology(8, "Spaceport", 2000, false, new List<Technology>(), "", spaceportTechnoItem, 10, 0);
        Technology recyclingStationTechno = new Technology(9, "Recycling Station", 600, true, new List<Technology>(), "", recyclingStationItem, 7, 0);

        // Disks
        Technology disk1Techno = new Technology(10, "Disk I", 300, true, new List<Technology>(), "", disk1TechnoItem, 0, 1);
        Technology disk2Techno = new Technology(11, "Disk II", 800, false, new List<Technology>(), "", disk2TechnoItem, 0, 2);
        Technology disk3Techno = new Technology(12, "Disk III", 3000, false, new List<Technology>(), "", disk3TechnoItem, 0, 3);


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
            disk1Techno, disk2Techno, disk3Techno
        };

        // Init locked/unlocked techno color
        foreach (Technology techno in technologies)
        {
            techno.UIitem.GetComponent<TechnologyItem>().InitializeUIElements();
        }
    }

    public void DisplayPanel(bool display)
    {
        technologiesPanel.SetActive(display);
    }

    public void BackButtonClicked()
    {
        technologiesPanel.SetActive(false);
        PanelsManager.instance.GoBackToControlsPanel();
    }

    public bool CanPayTechnology(Technology techno)
    {
        return (ScoreManager.instance.experiencePoints >= techno.experienceCost);
    }

    public void PayTechnology(Technology techno)
    {
        ScoreManager.instance.DecreaseExperiencePoints(techno.experienceCost);
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

    public void UnlockTechnology(Technology techno)
    {
        techno.unlocked = true;

        if(techno.unlockedBuildingIndex != 0)
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
            techno.UIitem.GetComponent<TechnologyItem>().UpdateCostDisplay();
        }
    }

    public void SetExperiencePointsDisplay(int experiencePoints)
    {
        experiencePointsText.text = experiencePoints.ToString();
    }

    public class Technology
    {
        public int id;
        public string name;
        public int experienceCost;
        public bool availableAtStart;
        public bool available;
        public List<Technology> unlockingTechnologies;  // Techs unlocked by this one
        public bool unlocked;
        public string imageFilename;
        public GameObject UIitem;
        public int unlockedBuildingIndex;
        public int unlockedDiskIndex;

        public Technology(int id, string name, int experienceCost, bool availableAtStart, List<Technology> unlockingTechnologies, string imageFilename, GameObject UIitem, int unlockedBuildingIndex, int unlockedDiskIndex)
        {
            this.id = id;
            this.name = name;
            this.experienceCost = experienceCost;
            this.availableAtStart = availableAtStart;
            this.available = availableAtStart;
            this.unlockingTechnologies = unlockingTechnologies;
            this.imageFilename = imageFilename;
            this.UIitem = UIitem;
            this.unlocked = false;
            this.UIitem.GetComponent<TechnologyItem>().associatedTechnology = this;
            this.unlockedBuildingIndex = unlockedBuildingIndex;
            this.unlockedDiskIndex = unlockedDiskIndex;
        }
    }
}
