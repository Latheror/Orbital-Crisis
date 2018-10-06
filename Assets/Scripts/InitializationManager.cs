using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InitializationManager : MonoBehaviour {

    public static InitializationManager instance;
    void Awake()
    {
        if (instance != null){ Debug.LogError("More than one InitializationManager in scene !"); return; } instance = this;
    }

    public BuildingManager bm;
    public ResourcesManager rm;
    public ShopPanel sp;

	// Use this for initialization
	void Start () {

        rm.InitializeResources();
        rm.InitializeResourceAmounts();
        rm.InitializeResourceIndicators();

        bm.SetAvailableBuildings();
        bm.buildingState = BuildingManager.BuildingState.Default;
        bm.mainPlanet = GameManager.instance.mainPlanet;

        sp.BuildStartBuildingShopItems();

    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
