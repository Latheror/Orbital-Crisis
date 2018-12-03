using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialManager : MonoBehaviour {

    public static TutorialManager instance;
    void Awake()
    {
        if (instance != null) { Debug.LogError("More than one TutorialManager in scene !"); return; }
        instance = this;
    }

    public List<TutorialIndicator> availableTutorialIndicators = new List<TutorialIndicator>();
    public List<GameObject> instantiatedTutorialIndicators = new List<GameObject>();

    [Header("Indicators")]
    public GameObject clickOnShopIndicator;
    public GameObject selectBuildingIndicator;
    public GameObject clickOnBuildIndicator;
    public GameObject selectBuildingLocationIndicator;
    public GameObject startWhenReadyIndicator;


    // Use this for initialization
    void Start () {

	}

    public void DefineAvailableTutorialIndicators()
    {
        availableTutorialIndicators.Add(new TutorialIndicator(1, "click_on_shop", "Buy buildings from the shop", clickOnShopIndicator, true));
        availableTutorialIndicators.Add(new TutorialIndicator(2, "select_building", "Select a building", selectBuildingIndicator, false));
        availableTutorialIndicators.Add(new TutorialIndicator(3, "select_building_location", "Tap to choose a building spot", selectBuildingLocationIndicator, false));
        availableTutorialIndicators.Add(new TutorialIndicator(4, "click_on_build", "Click to build !", clickOnBuildIndicator, false));
        availableTutorialIndicators.Add(new TutorialIndicator(5, "start_when_ready", "Start when ready !", startWhenReadyIndicator, true));
    }

    public void DisplayStartIndicators()
    {
        foreach (TutorialIndicator tutorialIndicator in availableTutorialIndicators)
        {
            tutorialIndicator.panel.SetActive(tutorialIndicator.atStart);
        }
    }

    public void HideIndicators()
    {
        foreach (TutorialIndicator tutorialIndicator in availableTutorialIndicators)
        {
            tutorialIndicator.panel.SetActive(false);
        }
    }

    public TutorialIndicator GetTutorialIndicatorById(int id)
    {
        TutorialIndicator t = null;
        foreach (TutorialIndicator tutoIndi in availableTutorialIndicators)
        {
            if(tutoIndi.id == id)
            {
                t = tutoIndi;
                break;
            }
        }
        return t;
    }

    public void DisplayIndicator(int id, bool enable)
    {
        TutorialIndicator t = GetTutorialIndicatorById(id);
        if(t != null)
        {
            t.panel.SetActive(enable);
            if (enable)
            {
                t.passed = true;
            }
        }
    }

    public bool hasIndicatorBeenDisplayed(int id)
    {
        TutorialIndicator t = GetTutorialIndicatorById(id);
        bool hasBeenDisplayed = false;
        if (t != null)
        {
            hasBeenDisplayed = t.passed;
        }
        return hasBeenDisplayed;
    }

    public void DisplayIndicatorIfNotDisplayedYet(int id)
    {
        if(!hasIndicatorBeenDisplayed(id))
        {
            DisplayIndicator(id, true);
        }
    }

    public class TutorialIndicator
    {
        public int id;
        public string name;
        public string text;
        public GameObject panel;
        public bool atStart;
        public bool passed;

        public TutorialIndicator(int id, string name, string text, GameObject panel, bool atStart)
        {
            this.id = id;
            this.name = name;
            this.text = text;
            this.panel = panel;
            this.atStart = atStart;
            this.passed = false;
        }
    }

}
