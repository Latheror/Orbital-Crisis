using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanetCanvasManager : MonoBehaviour {

    public static PlanetCanvasManager instance;
    void Awake()
    {
        if (instance != null) { Debug.LogError("More than one PlanetCanvasManager in scene !"); return; }
        instance = this;
    }

    [Header("UI")]
    public GameObject dezoomInfoPanel;
    public GameObject zoomInfoPanel;

    public void DisplayZoomInfoPanel(bool display)
    {
        zoomInfoPanel.SetActive(display);
        dezoomInfoPanel.SetActive(!display);
    }

}
