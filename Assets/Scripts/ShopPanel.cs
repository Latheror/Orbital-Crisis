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
    public GameObject[] buildingsLayouts;
    public Color buildingShopItemDefaultBackgroundColor = Color.blue;
    public Color buildingShopItemSelectedCanPayBackgroundColor;
    public Color buildingShopItemSelectedCantPayBackgroundColor;

    [Header("Operation")]
    public GameObject shopItemPanelSelected = null;
    public List<GameObject> buildingShopItemList;
    public int currentPanelDisplayedIndex;

    [Header("UI")]
    public GameObject cancelButton;
    public GameObject buildButton;
    public GameObject buildingsLayout1;
    public GameObject buildingsLayout2;

    [Header("Prefabs")]
    public GameObject buildingShopItemPrefab;



    void Awake()
    {
        if (instance != null) { Debug.LogError("More than one ShopPanel in scene !"); return; }
        instance = this;
        buildingShopItemList = new List<GameObject>();
        buildingsLayouts = new GameObject[] { buildingsLayout1, buildingsLayout2 };
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
        buildingsLayout1ItemNb = 0;
        buildingsLayout2ItemNb = 0;

        foreach (BuildingManager.BuildingType buildingType in BuildingManager.instance.availableBuildings)
        {
            if (buildingType.isUnlocked)
            {
                AddBuildingShopItem(buildingType);
            }
        }
    }

    public void AddBuildingShopItem(BuildingManager.BuildingType buildingType)
    {
        GameObject instantiatedBuildingShopItem = Instantiate(buildingShopItemPrefab, buildingsLayout1.transform.position, Quaternion.identity);
        if (buildingsLayout1ItemNb < nbBuildingShopItemsPerLayout)
        {
            instantiatedBuildingShopItem.transform.SetParent(buildingsLayout1.transform, false);
            buildingsLayout1ItemNb++;
        }
        else
        {
            instantiatedBuildingShopItem.transform.SetParent(buildingsLayout2.transform, false);
            buildingsLayout2ItemNb++;
        }

        buildingShopItemList.Add(instantiatedBuildingShopItem);

        BuildingShopItem item = instantiatedBuildingShopItem.GetComponent<BuildingShopItem>();
        item.buildingType = buildingType;
        item.SetInfos();      
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
        if(currentPanelDisplayedIndex < buildingsLayouts.Length - 1)
        {
            buildingsLayouts[currentPanelDisplayedIndex].transform.parent.gameObject.SetActive(false);
            buildingsLayouts[currentPanelDisplayedIndex + 1 ].transform.parent.gameObject.SetActive(true);
            currentPanelDisplayedIndex++;
        }
    }

    public void PreviousPanelButton()
    {
        if(currentPanelDisplayedIndex > 0)
        {
            buildingsLayouts[currentPanelDisplayedIndex].transform.parent.gameObject.SetActive(false);
            buildingsLayouts[currentPanelDisplayedIndex - 1 ].transform.parent.gameObject.SetActive(true);
            currentPanelDisplayedIndex--;
        }
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

}
