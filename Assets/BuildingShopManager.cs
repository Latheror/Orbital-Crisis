using System.Collections;
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
        Initialize();    
    }

    public const int attackBuildingTypeIndex = 1;
    public const int defenseBuildingTypeIndex = 2;
    public const int productionBuildingTypeIndex = 3;

    // Panels
    public GameObject attackBuildingsShopPanel;
    public GameObject defenseBuildingsShopPanel;
    public GameObject productionBuildingsShopPanel;

    // Panels buildings layout
    /*public GameObject attackBuildingsShopPanelLayout;
    public GameObject defenseBuildingsShopPanelLayout;
    public GameObject productionBuildingsShopPanelLayout;*/


    public List<BuildingShopPanel> buildingShopPanelsList;

    public void Initialize()
    {
        InitializeBuildingShopPanels();
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
    }

    public void ShowPanelsAllBuildings()
    {
        foreach (BuildingShopPanel bsp in buildingShopPanelsList)
        {
            OpenPanel(bsp.buildingTypeIndex, bsp.currentUsedSlots);
        }
    }

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

        public BuildingShopPanel(int buildingTypeIndex, GameObject panel, int maxSlots)
        {
            this.buildingTypeIndex = buildingTypeIndex;
            this.panel = panel;
            this.maxSlots = maxSlots;
            this.currentOpenNb = 0;
        }
    }
}
