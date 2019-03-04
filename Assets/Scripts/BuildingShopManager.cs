﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingShopManager : MonoBehaviour
{
    public static BuildingShopManager instance;
    void Awake()
    {
        if (instance != null) { Debug.LogError("More than one BuildingShopManager in scene !"); return; }
        instance = this;
    }

    void Start()
    {
        //Initialize();    // Done in initializationManager
    }

    public const int attackBuildingTypeIndex = 1;
    public const int defenseBuildingTypeIndex = 2;
    public const int productionBuildingTypeIndex = 3;

    [Header("Panels")]
    public GameObject attackBuildingsShopPanel;
    public GameObject defenseBuildingsShopPanel;
    public GameObject productionBuildingsShopPanel;

    [Header("Prefabs")]
    public GameObject buildingShopItemPrefab;

    [Header("UI")]
    public Sprite attackCircle;
    public Sprite defenseCircle;
    public Sprite productionCircle;

    [Header("Operation")]
    public List<BuildingShopPanel> buildingShopPanelsList;
    public BuildingManager.BuildingType.BuildingLocationType currentBuildingLocationType = BuildingManager.BuildingType.BuildingLocationType.Planet;
    public bool panelsCurrentlyOpen = false;

    public void Initialize()
    {
        InitializeBuildingShopPanels();
        panelsCurrentlyOpen = false;
    }

    public void InitializeBuildingShopPanels()
    {
        buildingShopPanelsList.Add(new BuildingShopPanel(1, attackBuildingsShopPanel, 3));
        buildingShopPanelsList.Add(new BuildingShopPanel(2, defenseBuildingsShopPanel, 3));
        buildingShopPanelsList.Add(new BuildingShopPanel(3, productionBuildingsShopPanel, 3));
    }

    public BuildingShopPanel FindBuildingShopPanelByIndex(int id)
    {
        BuildingShopPanel bsp = null;
        foreach (BuildingShopPanel buildingShopPanel in buildingShopPanelsList)
        {
            if(buildingShopPanel.buildingTypeIndex == id)
            {
                bsp = buildingShopPanel;
                break;
            }
        }
        return bsp;
    }

    // Open buildings layout, displaying N buildings
    public void OpenClosePanel(int panelIndex, bool open, int buildingsNb = 0)
    {
        Debug.Log("OpenClosePanel | PanelIndex [" + panelIndex + "] | Open [" + open + "] | BuildingsNb [" + buildingsNb + "]");
        BuildingShopPanel bsp = FindBuildingShopPanelByIndex(panelIndex);

        if(bsp != null && (buildingsNb <= bsp.maxSlots) && (!open || buildingsNb > 0))
        {
            Animator anim = bsp.panel.GetComponent<Animator>();
            if(anim != null)
            {
                if(open)
                {
                    anim.SetTrigger("Open" + buildingsNb);
                    bsp.currentOpenNb = buildingsNb;
                }
                else
                {
                    anim.SetTrigger("Close");
                    bsp.currentOpenNb = 0;
                }
            }
        }
    }

    public void ClosePanel(int buildingTypeIndex)
    {
        OpenClosePanel(buildingTypeIndex, false);
    }

    public void OpenPanel(int buildingTypeIndex, int buildingsNb)
    {
        OpenClosePanel(buildingTypeIndex, true, buildingsNb);
    }

    public void CloseAllPanels()
    {
        for(int i = 1; i <= 3; i++)
        {
            ClosePanel(i);
        }
        panelsCurrentlyOpen = false;
    }

    public void ShowPanelsAllBuildings()
    {
        if(!panelsCurrentlyOpen)
        {
            bool atLeastOnePanelOpen = false;
            foreach (BuildingShopPanel bsp in buildingShopPanelsList)
            {
                OpenPanel(bsp.buildingTypeIndex, bsp.currentUsedSlots);
                if (bsp.currentUsedSlots > 0)
                    atLeastOnePanelOpen = true;
            }

            if(atLeastOnePanelOpen)
                panelsCurrentlyOpen = true;
        }
    }

    public void OnShopButtonClick()
    {
        Debug.Log("OnShopButtonClick | panelsCurrentlyOpen [" + panelsCurrentlyOpen + "]");
        if (panelsCurrentlyOpen)
        {
            CloseAllPanels();
        }
        else
        {
            BuildShopPanels();
            ShowPanelsAllBuildings();
        }
        
    }

    public void BuildShopPanels()
    {
        CleanShopPanels();

        foreach (BuildingManager.BuildingType buildingType in BuildingManager.instance.availableBuildings)
        {
            if(buildingType.buildingLocationType == currentBuildingLocationType && buildingType.isUnlocked)
                AddBuildingShopItem(buildingType);
        }
    }

    public void CleanShopPanels()
    {
        foreach (BuildingShopPanel bsp in buildingShopPanelsList)
        {
            List<GameObject> bsiList = bsp.buildingShopItemsList;
            while (bsiList.Count > 0)
            {
                GameObject bsi = bsiList[0];
                bsiList.Remove(bsi);
                Destroy(bsi.gameObject);
            }
            bsp.currentUsedSlots = 0;
        }
    }

    public void AddBuildingShopItem(BuildingManager.BuildingType buildingType)
    {
        BuildingShopPanel buildingShopPanel = null;
        switch (buildingType.buildingCategory)   // Attack, Defense, Production
        {
            case BuildingManager.BuildingCategory.Attack:
            {
                buildingShopPanel = FindBuildingShopPanelByIndex(attackBuildingTypeIndex);
                break;
            }
            case BuildingManager.BuildingCategory.Defense:
            {
                buildingShopPanel = FindBuildingShopPanelByIndex(defenseBuildingTypeIndex);
                break;
            }
            case BuildingManager.BuildingCategory.Production:
            {
                buildingShopPanel = FindBuildingShopPanelByIndex(productionBuildingTypeIndex);
                break;
            }
            default:
            {
                Debug.LogError("AddBuildingShopItem | Wrong Building Type");
                break;
            }
        }

        if(buildingShopPanel != null)
        {
            if(buildingShopPanel.currentUsedSlots < buildingShopPanel.maxSlots)
            {
                // Instantiation
                GameObject instantiatedBuildingShopItem = Instantiate(buildingShopItemPrefab, Vector3.zero, Quaternion.identity);
                instantiatedBuildingShopItem.transform.SetParent(buildingShopPanel.panel.transform, false);

                // Configuration
                instantiatedBuildingShopItem.GetComponent<BuildingShopItemV2>().SetInfo(buildingType);

                // Panel update
                buildingShopPanel.buildingShopItemsList.Add(instantiatedBuildingShopItem);
                buildingShopPanel.currentUsedSlots++;
            }
            else
            {
                Debug.LogError("AddBuildingShopItem | Max nb of BuildingItem in panel reached");
            }
        }
        else
        {
            Debug.LogError("AddBuildingShopItem | No BuildingShopPanel selected");
        }
    }

    // Add buildings that are available at the start of the game to the shop panels
    public void BuildStartBuildingShopItems()
    {
        foreach (BuildingManager.BuildingType buildingType in BuildingManager.instance.availableBuildings)
        {
            if (buildingType.isUnlocked)
            {
                AddBuildingShopItem(buildingType);
            }
        }
    }

    public void BuildAndShowPanelsBasedOnBuildingLocationType(BuildingManager.BuildingType.BuildingLocationType buildingLocationType)
    {
        Debug.Log("BuildAndShowPanelsBasedOnBuildingLocationType | LocationType [" + buildingLocationType + "]");
        currentBuildingLocationType = buildingLocationType;

        BuildShopPanels();
        ShowPanelsAllBuildings();
    }

    // Used to build new unlocked building shop item
    public void OnNewBuildingUnlocked(BuildingManager.BuildingType buildingType)
    {
        // Removed
    }

    // ------------------------------ TEST ------------------------------- //
    public void OnTestButton1()
    {
        /*int buildingTypeIndex = Mathf.RoundToInt(Random.Range(1, 4));
        int buildingNb = Mathf.RoundToInt(Random.Range(1, 4));

        OpenClosePanel(buildingTypeIndex, true, buildingNb);*/
        ShowPanelsAllBuildings();
    }

    public void OnTestButton2()
    {
        CloseAllPanels();
    }

    [System.Serializable]
    public class BuildingShopPanel
    {
        public int buildingTypeIndex;
        public GameObject panel;
        public int maxSlots = 3;
        public int currentUsedSlots = 0;
        public int currentOpenNb = 0;
        public List<GameObject> buildingShopItemsList;

        public BuildingShopPanel(int buildingTypeIndex, GameObject panel, int maxSlots)
        {
            this.buildingTypeIndex = buildingTypeIndex;
            this.panel = panel;
            this.maxSlots = maxSlots;
            this.currentOpenNb = 0;
            buildingShopItemsList = new List<GameObject>();
        }
    }
}