using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BuildingShopItem : MonoBehaviour {

    [Header("UI")]
    public GameObject buildingImage;
    public TextMeshProUGUI buildingNameText;
    public GameObject buildingCostsPanel;
    public List<GameObject> buildingCostPanelList = new List<GameObject>();

    [Header("Prefabs")]
    public GameObject resourceCostPanelPrefab;

    public BuildingManager.BuildingType buildingType;
    // public TextMeshProUGUI building

    public void Start()
    {

    }

    public void SetInfos()
    {
        ApplyBuildingNameText();
        ApplyBuildingImage();
        SetBackGroundColor(ShopPanel.instance.buildingShopItemDefaultBackgroundColor);
        BuildCostsList();
    }

    public void BuildingShopItemClicked()
    {
        Debug.Log("Building Shop Item Clicked !");
        ShopPanel.instance.ResetLastShopItemSelected();
        BuildingManager.instance.SelectBuilding(buildingType);
        ShopPanel.instance.shopItemPanelSelected = this.gameObject;
        if(ResourcesManager.instance.CanPay(buildingType))
        {
            SetBackGroundColor(ShopPanel.instance.buildingShopItemSelectedCanPayBackgroundColor);
        }
        else
        {
            SetBackGroundColor(ShopPanel.instance.buildingShopItemSelectedCantPayBackgroundColor);
        }
    }

    public void ApplyBuildingNameText()
    {
        buildingNameText.text = buildingType.name;
    }

    public void ApplyBuildingImage()
    {
        if (buildingType.buildingImage != null)
        {
            buildingImage.GetComponent<Image>().sprite = buildingType.buildingImage;
        }
    }

    public void SetBackGroundColor(Color color)
    {
        this.gameObject.GetComponent<Image>().color = color;
    }

    public void BuildCostsList()
    {
        foreach (ResourcesManager.ResourceAmount resourceAmount in buildingType.resourceCosts)
        {
            GameObject instantiatedResourceCostPanel = Instantiate(resourceCostPanelPrefab, buildingCostsPanel.transform.position, Quaternion.identity);
            instantiatedResourceCostPanel.transform.SetParent(buildingCostsPanel.transform, false);

            ResourceCostPanel rcPanel = instantiatedResourceCostPanel.GetComponent<ResourceCostPanel>();
            rcPanel.SetInfos(resourceAmount);
            rcPanel.BuildPanel();

            Debug.Log("BuildCostsList | Building: " + buildingType.name + " | Resource: " + resourceAmount.resourceType.resourceName + " | Adding to list");
            buildingCostPanelList.Add(instantiatedResourceCostPanel);

            Debug.Log("Nb CostPanels: " + buildingCostPanelList.Count);
        }
    }

    public void UpdateResourcesAvailabilityIndicators()
    {
        Debug.Log("UpdateResourcesAvailabilityIndicators | " + buildingType.name + " | ResourcesNb: " + buildingCostPanelList.Count);
        foreach (GameObject costPanel in buildingCostPanelList)
        {
            costPanel.GetComponent<ResourceCostPanel>().UpdateResourceAvailabilityIndicator();
        }
    }
}
