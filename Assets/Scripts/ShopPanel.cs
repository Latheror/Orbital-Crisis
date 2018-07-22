using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopPanel : MonoBehaviour {

    public static ShopPanel instance;
    void Awake(){ 
        if (instance != null){ Debug.LogError("More than one ShopPanel in scene !"); return; } instance = this;
    }


    public GameObject cancelButton;
    public GameObject buildButton;

    public GameObject buildingShopItemPrefab;
    public GameObject buildingsLayout1;
    public int buildingsLayout1ItemNb;
    public GameObject buildingsLayout2;
    public int buildingsLayout2ItemNb;
    public int nbBuildingShopItemsPerLayout = 5;
    public GameObject[] buildingsLayouts;

    public int currentPanelDisplayedIndex;

    void Start()
    {

        buildingsLayouts = new GameObject[]{buildingsLayout1, buildingsLayout2};

        BuildBuildingShopItems();
        currentPanelDisplayedIndex = 0;
    }




    public void BuildBuildingShopItems()
    {
        buildingsLayout1ItemNb = 0;
        buildingsLayout2ItemNb = 0;

        foreach (BuildingManager.BuildingType buildingType in BuildingManager.instance.availableBuildings)
        {
            GameObject instantiatedBuildingShopItem = Instantiate(buildingShopItemPrefab, buildingsLayout1.transform.position, Quaternion.identity);

            if(buildingsLayout1ItemNb < nbBuildingShopItemsPerLayout)
            {
                instantiatedBuildingShopItem.transform.SetParent(buildingsLayout1.transform, false);
                buildingsLayout1ItemNb++;
            }
            else
            {
                instantiatedBuildingShopItem.transform.SetParent(buildingsLayout2.transform, false);
                buildingsLayout2ItemNb++;
            }


            instantiatedBuildingShopItem.GetComponent<BuildingShopItem>().buildingType = buildingType;
            instantiatedBuildingShopItem.GetComponent<BuildingShopItem>().SetBuildingNameText(buildingType.name);
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

}
