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

    public void DefineAvailableTutorialIndicators()
    {
        availableTutorialIndicators.Add(new TutorialIndicator(1, "select_building_spot", "", selectBuildingSpotIndicator, true, 0));
        availableTutorialIndicators.Add(new TutorialIndicator(2, "select_building", "", selectBuildingIndicator, false, 0));
        availableTutorialIndicators.Add(new TutorialIndicator(3, "click_on_build", "", clickOnBuildIndicator, false, 0));
        availableTutorialIndicators.Add(new TutorialIndicator(4, "touch_buildig", "", touchBuildingIndicator, false, 0));
        availableTutorialIndicators.Add(new TutorialIndicator(5, "start_wave", "", startWaveIndicator, false, 0));
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

    public void TutorialIndicatorTouched(int id)
    {
        //Debug.Log("Tutorial Indicator Touched [" + id + "]");
        DisplayIndicator(id, false);
    }

    public class TutorialIndicator
    {
        public int id;
        public string name;
        public string text;
        public GameObject panel;
        public bool atStart;
        public bool passed;

        public TutorialIndicator(int id, string name, string text, GameObject panel, bool atStart, int indicatorToDisplayOnTouch)
        {
            this.id = id;
            this.name = name;
            //this.text = text;
            this.panel = panel;
            this.atStart = atStart;
            this.passed = false;

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
