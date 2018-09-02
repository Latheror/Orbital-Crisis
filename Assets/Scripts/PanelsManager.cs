using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanelsManager : MonoBehaviour {

    public GameObject controlsPanel;
    public GameObject shopPanel;
    public GameObject defaultBottomPanel;

    // Use this for initialization
    void Start () {
        SwitchFromShopToControlsPanel();
    }
    
    // Update is called once per frame
    void Update () {
        
    }

    public void SwitchFromControlsToShopPanel()
    {
        defaultBottomPanel.SetActive(false);
        shopPanel.SetActive(true);
    }

    public void SwitchFromShopToControlsPanel()
    {
        shopPanel.SetActive(false);
        defaultBottomPanel.SetActive(true);
    }

    public void BackButtonClicked()
    {
        SwitchFromShopToControlsPanel();
        BuildingManager.instance.buildingState = BuildingManager.BuildingState.Default;
    }
}
