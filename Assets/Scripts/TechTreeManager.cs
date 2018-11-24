using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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


    void Start () {
        InitializeTechnologies();
	}
	
	void Update () {
		
	}

    public void InitializeTechnologies()
    {
        technologies.Add(new Technology(1, "Missile Turret", 100, new List<Technology>(), new List<Technology>(), "", null));
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
        if (CanPayTechnology(techno))
        {
            PayTechnology(techno);

        }
    }

    public class Technology
    {
        public int id;
        public string name;
        public int experienceCost;
        public List<Technology> unlockedByTechnologies; // Techs leading to this one
        public List<Technology> unlockingTechnologies;  // Techs unlocked by this one
        public bool unlocked;
        public string imageFilename;
        public GameObject UIitem;

        public Technology(int id, string name, int experienceCost, List<Technology> unlockedByTechnologies, List<Technology> unlockingTechnologies, string imageFilename, GameObject UIitem)
        {
            this.id = id;
            this.name = name;
            this.experienceCost = experienceCost;
            this.unlockedByTechnologies = unlockedByTechnologies;
            this.unlockingTechnologies = unlockingTechnologies;
            this.imageFilename = imageFilename;
            this.UIitem = UIitem;
            this.unlocked = false;
        }
    }
}
