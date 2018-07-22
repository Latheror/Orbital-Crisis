using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanelsManager : MonoBehaviour {

    public GameObject controlsPanel;
    public GameObject shopPanel;

    // Use this for initialization
    void Start () {
        
    }
    
    // Update is called once per frame
    void Update () {
        
    }

    public void SwitchFromControlsToShopPanel()
    {
        controlsPanel.SetActive(false);
        shopPanel.SetActive(true);
    }

    public void SwitchFromShopToControlsPanel()
    {
        shopPanel.SetActive(false);
        controlsPanel.SetActive(true);
    }

    public void BackButtonClicked()
    {
        SwitchFromShopToControlsPanel();
        BuildingManager.instance.buildingState = BuildingManager.BuildingState.Default;
    }
}
