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
    public BuildingShopManager bsm;

	// Use this for initialization              // TODO: Put this into GameSetupManager
	void Start () {

        rm.InitializeResources();
        rm.InitializeResourceAmounts();
        rm.InitializeResourceIndicators();

        bm.SetAvailableBuildings();
        bm.Initialize();

        bsm.Initialize();
        bsm.BuildStartBuildingShopItems();      // NEW
    }

}
