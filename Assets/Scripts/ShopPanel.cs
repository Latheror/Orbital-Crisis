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
    public Color buildingShopItemUniqueDefaultBackgroundColor;
    public Color buildingShopItemSelectedCanPayBackgroundColor;
    public Color buildingShopItemSelectedCantPayBackgroundColor;

    [Header("Operation")]
    public GameObject shopItemPanelSelected = null;
    public List<GameObject> buildingShopItemList;
    public int currentPanelDisplayedIndex;
    public int[] buildingLayoutsItemNbs;

    [Header("UI")]
    public List<GameObject> buildingsLayouts;
    public GameObject previousLayoutButton;
    public GameObject nextLayoutButton;

    [Header("Prefabs")]
    public GameObject buildingShopItemPrefab;
    public Sprite goldenBorder;

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
    }

    /*
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
    }*/

    /*public void AddBuildingShopItem(BuildingManager.BuildingType buildingType)
    {
        for(int i = 0; i< buildingsLayouts.Count; i++)
        {
            if(buildingLayoutsItemNbs[i] < nbBuildingShopItemsPerLayout) // There is some room in this layout
            {
                GameObject instantiatedBuildingShopItem = Instantiate(buildingShopItemPrefab, new Vector3(0f, 0f, 0f), Quaternion.identity);
                instantiatedBuildingShopItem.transform.SetParent(buildingsLayouts[i].transform, false);
                //instantiatedBuildingShopItem.transform.position = new Vector3(0f, 0f, 0f);
                buildingLayoutsItemNbs[i] ++;

                //Debug.Log("Nb of building items in layout " + i + " is: " + buildingLayoutsItemNbs[i]);

                buildingShopItemList.Add(instantiatedBuildingShopItem);

                BuildingShopItem item = instantiatedBuildingShopItem.GetComponent<BuildingShopItem>();
                item.buildingType = buildingType;
                item.SetInfo();

                // Gold border for Unique Buildings
                if(buildingType.isUnique)
                {
                    item.borderPanel.GetComponent<Image>().sprite = goldenBorder;
                }

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
        // Tutorial indicator //
        TutorialManager.instance.DisplayIndicator(4, false);
        TutorialManager.instance.DisplayIndicatorIfNotDisplayedYet(5);
        // ------------------ //

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

    public void ResetLastShopItemSelected()     // OLD : TO REDO
    {
        /*if(shopItemPanelSelected != null)
        {
            if (shopItemPanelSelected.GetComponent<BuildingShopItem>().buildingType.isUnique)
            {
                shopItemPanelSelected.GetComponent<BuildingShopItem>().SetBackGroundColor(ShopPanel.instance.buildingShopItemUniqueDefaultBackgroundColor);
            }
            else
            {
                shopItemPanelSelected.GetComponent<BuildingShopItem>().SetBackGroundColor(ShopPanel.instance.buildingShopItemDefaultBackgroundColor);
            }
        }
    }*/

    /*public void UpdateShopItems()
    {
        //Debug.Log("UpdateShopItems");
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

    public GameObject GetShopItemAssociatedWithBuildingType(BuildingManager.BuildingType buildingType)
    {
        GameObject shopItem = null;
        foreach (GameObject buildingShopItem in buildingShopItemList)
        {
            if(buildingShopItem.GetComponent<BuildingShopItem>().buildingType == buildingType)
            {
                shopItem = buildingShopItem;
                break;
            }
        }
        return shopItem;
    }

    public void UpdateBuildingItemsAvailability()
    {
        foreach (GameObject buildingShopItem in buildingShopItemList)
        {
            // Remove item when building is Unique but already placed
            if(buildingShopItem.GetComponent<BuildingShopItem>().buildingType.isUnique)
            {
                if(BuildingManager.instance.IsBuildingTypeAtLeastPlacedOnce(buildingShopItem.GetComponent<BuildingShopItem>().buildingType))
                {
                    buildingShopItem.SetActive(false);
                }
            }
        }
    }

    public void DisplayBuildingShopItemBack(BuildingManager.BuildingType buildingType)
    {
        GameObject shopItem = GetShopItemAssociatedWithBuildingType(buildingType);
        if (shopItem != null)
        {
            shopItem.SetActive(true);
        }
    }*/

}
