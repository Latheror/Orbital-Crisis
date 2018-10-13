using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopPanel : MonoBehaviour {

    public static ShopPanel instance;

    [Header("Settings")]
    public int buildingsLayout1ItemNb;
    public int buildingsLayout2ItemNb;
    public int nbBuildingShopItemsPerLayout = 6;
    public Color buildingShopItemDefaultBackgroundColor = Color.blue;
    public Color buildingShopItemSelectedCanPayBackgroundColor;
    public Color buildingShopItemSelectedCantPayBackgroundColor;

    [Header("Operation")]
    public GameObject shopItemPanelSelected = null;
    public List<GameObject> buildingShopItemList;
    public int currentPanelDisplayedIndex;
    public int[] buildingLayoutsItemNbs;

    [Header("UI")]
    public GameObject cancelButton;
    public GameObject buildButton;
    public List<GameObject> buildingsLayouts;
    public GameObject previousLayoutButton;
    public GameObject nextLayoutButton;

    [Header("Prefabs")]
    public GameObject buildingShopItemPrefab;



    void Awake()
    {
        if (instance != null) { Debug.LogError("More than one ShopPanel in scene !"); return; }
        instance = this;
        buildingShopItemList = new List<GameObject>();
        buildingLayoutsItemNbs = new int[] { 0, 0, 0 };    // Change this later
    }

    void Start()
    {
        //BuildStartBuildingShopItems();
        currentPanelDisplayedIndex = 0;

        cancelButton.SetActive(false);
        buildButton.SetActive(false);
    }

    public void BuildStartBuildingShopItems()
    {
        foreach (BuildingManager.BuildingType buildingType in BuildingManager.instance.availableBuildings)
        {
            if (buildingType.isUnlocked)
            {
                AddBuildingShopItem(buildingType);
            }
        }

        UpdateLayoutChangeButtons();
    }

    public void AddBuildingShopItem(BuildingManager.BuildingType buildingType)
    {
        for(int i = 0; i< buildingsLayouts.Count; i++)
        {
            if(buildingLayoutsItemNbs[i] < nbBuildingShopItemsPerLayout) // There is some room in this layout
            {
                GameObject instantiatedBuildingShopItem = Instantiate(buildingShopItemPrefab, buildingsLayouts[i].transform.position, Quaternion.identity);
                instantiatedBuildingShopItem.transform.SetParent(buildingsLayouts[i].transform, false);
                buildingLayoutsItemNbs[i] ++;

                Debug.Log("Nb of building items in layout " + i + " is: " + buildingLayoutsItemNbs[i]);

                buildingShopItemList.Add(instantiatedBuildingShopItem);

                BuildingShopItem item = instantiatedBuildingShopItem.GetComponent<BuildingShopItem>();
                item.buildingType = buildingType;
                item.SetInfos();

                UpdateLayoutChangeButtons();

                break;
            }
        } 
    }

    public void CancelButtonClicked()
    {
        BuildingManager.instance.CancelButton();
    }

    public void BuildButtonClicked()
    {
        BuildingManager.instance.BuildButton();
    }

    public void ShowCancelButton()
    {
        cancelButton.SetActive(true);
    }

    public void HideCancelButton()
    {
        cancelButton.SetActive(false);
    }

    public void ShowBuildButton()
    {
        buildButton.SetActive(true);
    }

    public void HideBuildButton()
    {
        buildButton.SetActive(false);
    } 

    public void NextLayoutButton()
    {
        if(currentPanelDisplayedIndex < buildingsLayouts.Count - 1)
        {
            buildingsLayouts[currentPanelDisplayedIndex].transform.parent.gameObject.SetActive(false);
            buildingsLayouts[currentPanelDisplayedIndex + 1 ].transform.parent.gameObject.SetActive(true);
            currentPanelDisplayedIndex++;
        }

        UpdateLayoutChangeButtons();
    }

    public void PreviousPanelButton()
    {
        if(currentPanelDisplayedIndex > 0)
        {
            buildingsLayouts[currentPanelDisplayedIndex].transform.parent.gameObject.SetActive(false);
            buildingsLayouts[currentPanelDisplayedIndex - 1 ].transform.parent.gameObject.SetActive(true);
            currentPanelDisplayedIndex--;
        }

        UpdateLayoutChangeButtons();
    }

    public void ResetLastShopItemSelected()
    {
        if(shopItemPanelSelected != null)
        {
            shopItemPanelSelected.GetComponent<BuildingShopItem>().SetBackGroundColor(ShopPanel.instance.buildingShopItemDefaultBackgroundColor);
        }
    }

    public void UpdateShopItems()
    {
        Debug.Log("UpdateShopItems");
        foreach (GameObject shopItem in buildingShopItemList)
        {
            shopItem.GetComponent<BuildingShopItem>().UpdateResourcesAvailabilityIndicators();
        }
    }

    public void UpdateLayoutChangeButtons()
    {
        previousLayoutButton.SetActive((currentPanelDisplayedIndex == 0)?false:true);
        nextLayoutButton.SetActive((buildingLayoutsItemNbs[currentPanelDisplayedIndex + 1] > 0) ? true : false);
    }

}
