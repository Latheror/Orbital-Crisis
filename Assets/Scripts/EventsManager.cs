using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EventsManager : MonoBehaviour {

    public static EventsManager instance;
    void Awake()
    {
        if (instance != null) { Debug.LogError("More than one EventsManager in scene !"); return; }
        instance = this;
    }

    public GameObject mainHelpPanel;
    public GameObject freeSpaceshipProvidedPanel;
    public GameObject populationAttributionHelpPanel;

    public void StartFreeSpaceshipPanelAnimation()
    {
        Animator animator = freeSpaceshipProvidedPanel.GetComponent<Animator>();
        animator.SetTrigger("startAnim");
    }

    public void StartPopulationAttributionHelpPanelAnimation()
    {
        Animator animator = populationAttributionHelpPanel.GetComponent<Animator>();
        animator.SetTrigger("startAnim");
    }

    public void StartMainHelpPanelAnimation()
    {
        Animator animator = mainHelpPanel.GetComponent<Animator>();
        animator.SetTrigger("startAnim");
    }

    public void GameSetupFinished()
    {
        StartMainHelpPanelAnimation();
    }

    public void WaveIndexStarted(int waveIndex)
    {
        if(waveIndex == 2)
        {
            StartPopulationAttributionHelpPanelAnimation();
        }
    }

}
