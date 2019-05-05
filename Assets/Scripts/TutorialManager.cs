using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialManager : MonoBehaviour {

    public static TutorialManager instance;
    void Awake()
    {
        if (instance != null) { Debug.LogError("More than one TutorialManager in scene !"); return; }
        instance = this;
    }

    [Header("Operation")]
    public List<TutorialIndicator> availableTutorialIndicators = new List<TutorialIndicator>();
    public List<GameObject> instantiatedTutorialIndicators = new List<GameObject>();

    [Header("Indicators")]
    public GameObject selectBuildingSpotIndicator;
    public GameObject selectBuildingIndicator;
    public GameObject clickOnBuildIndicator;
    public GameObject touchBuildingIndicator;
    public GameObject startWaveIndicator;
    public GameObject protectPeopleIndicator;
    public GameObject basicResourcesIndicator;
    public GameObject selectSpaceshipIndicator;
    public GameObject recycleEnemySpaceshipIndicator;
    public GameObject enterLabIndicator;
    public GameObject advancedResourcesIndicator;

    public enum IndicatorID
    {
        protect_people,
        select_building_spot,
        select_building,
        click_on_build,
        touch_building,
        start_wave,
        basic_resources,
        advances_resources,
        select_spaceship,
        recycle_enemy_spaceship,
        enter_lab
    }

    public void DefineAvailableTutorialIndicators()
    {
        availableTutorialIndicators.Add(new TutorialIndicator(IndicatorID.protect_people, "protect_peoplet", "", protectPeopleIndicator, true, IndicatorID.basic_resources));
        availableTutorialIndicators.Add(new TutorialIndicator(IndicatorID.basic_resources, "basic_resources", "", basicResourcesIndicator, false, IndicatorID.advances_resources));
        availableTutorialIndicators.Add(new TutorialIndicator(IndicatorID.select_building_spot, "select_building_spot", "", selectBuildingSpotIndicator, true, 0));
        availableTutorialIndicators.Add(new TutorialIndicator(IndicatorID.select_building, "select_building", "", selectBuildingIndicator, false, 0));
        availableTutorialIndicators.Add(new TutorialIndicator(IndicatorID.click_on_build, "click_on_build", "", clickOnBuildIndicator, false, 0));
        availableTutorialIndicators.Add(new TutorialIndicator(IndicatorID.touch_building, "touch_buildig", "", touchBuildingIndicator, false, 0));
        availableTutorialIndicators.Add(new TutorialIndicator(IndicatorID.start_wave, "start_wave", "", startWaveIndicator, false, 0));
        availableTutorialIndicators.Add(new TutorialIndicator(IndicatorID.select_spaceship, "select_spaceship", "", selectSpaceshipIndicator, false, 0));
        availableTutorialIndicators.Add(new TutorialIndicator(IndicatorID.recycle_enemy_spaceship, "recycle_enemy_spaceship", "", recycleEnemySpaceshipIndicator, false, 0));
        availableTutorialIndicators.Add(new TutorialIndicator(IndicatorID.enter_lab, "enter_lab", "", enterLabIndicator, false, 0));
        availableTutorialIndicators.Add(new TutorialIndicator(IndicatorID.advances_resources, "advances_resources", "", advancedResourcesIndicator, false, 0));
    }

    public void DisplayStartIndicators()
    {
        foreach (TutorialIndicator tutorialIndicator in availableTutorialIndicators)
        {
            //Debug.Log("DisplayStartIndicator [" + tutorialIndicator.id + "] | [" + tutorialIndicator.atStart + "]");
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

    public TutorialIndicator GetTutorialIndicatorById(IndicatorID id)
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

    public void DisplayIndicator(IndicatorID id, bool enable)
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

    public bool hasIndicatorBeenDisplayed(IndicatorID id)
    {
        TutorialIndicator t = GetTutorialIndicatorById(id);
        bool hasBeenDisplayed = false;
        if (t != null)
        {
            hasBeenDisplayed = t.passed;
        }
        return hasBeenDisplayed;
    }

    public void DisplayIndicatorIfNotDisplayedYet(IndicatorID id)
    {
        if(!hasIndicatorBeenDisplayed(id))
        {
            DisplayIndicator(id, true);
        }
    }

    public void TutorialIndicatorTouched(IndicatorID id)
    {
        //Debug.Log("Tutorial Indicator Touched [" + id + "]");
        DisplayIndicator(id, false);

        IndicatorID nextElementID = GetTutorialIndicatorById(id).indicatorToDisplayOnTouch;
        if (nextElementID != 0)
        {
            DisplayIndicatorIfNotDisplayedYet(nextElementID);
        }
    }

    public class TutorialIndicator
    {
        public IndicatorID id;
        public string name;
        public string text;
        public GameObject panel;
        public bool atStart;
        public bool passed;
        public IndicatorID indicatorToDisplayOnTouch;

        public TutorialIndicator(IndicatorID id, string name, string text, GameObject panel, bool atStart, IndicatorID indicatorToDisplayOnTouch)
        {
            this.id = id;
            this.name = name;
            //this.text = text;
            this.panel = panel;
            this.atStart = atStart;
            this.passed = false;
            this.indicatorToDisplayOnTouch = indicatorToDisplayOnTouch;

            Initialize();
        }

        public void Initialize()
        {
            // Add a Button
            panel.AddComponent<Button>();
            panel.GetComponent<Button>().onClick.AddListener(() => instance.TutorialIndicatorTouched(id));

            TutorialElement te = panel.GetComponent<TutorialElement>();
            if (te != null)
            {
                te.id = id;
            }
        }
    }

}
