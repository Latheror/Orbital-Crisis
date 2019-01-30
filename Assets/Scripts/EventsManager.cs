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

    public GameObject freeSpaceshipProvidedPanel;

    public void StartFreeSpaceshipPanelAnimation()
    {
        Animator animator = freeSpaceshipProvidedPanel.GetComponent<Animator>();
        animator.SetTrigger("startAnim");
    }

}
