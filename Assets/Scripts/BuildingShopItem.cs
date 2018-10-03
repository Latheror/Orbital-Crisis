using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BuildingShopItem : MonoBehaviour {

    public GameObject buildingImage;
    public TextMeshProUGUI buildingNameText;

    public BuildingManager.BuildingType buildingType;
    // public TextMeshProUGUI building

    // TODO
    //public void SetInfos()
    //{
    //    SetBuildingNameText
    //}

    public void BuildingShopItemClicked()
    {
        Debug.Log("Building Shop Item Clicked !");
        ShopPanel.instance.ResetLastShopItemSelected();
        BuildingManager.instance.SelectBuilding(buildingType);
        ShopPanel.instance.shopItemPanelSelected = this.gameObject;
        SetBackGroundColor(ShopPanel.instance.buildingShopItemSelectedBackgroundColor);

        SpaceshipManager.instance.SetSelectionState(GameManager.SelectionState.Default);
    }

    public void SetBuildingNameText(string buildingName)
    {
        buildingNameText.text = buildingName;
    }

    public void SetBuildingImage(Sprite image)
    {
        buildingImage.GetComponent<Image>().sprite = image;
    }

    public void SetBackGroundColor(Color color)
    {
        this.gameObject.GetComponent<Image>().color = color;
    }

    public void ApplySettings()
    {
        if(buildingType != null)
        {
            SetBuildingNameText(buildingType.name);
            if(buildingType.buildingImage != null)
            {
                SetBuildingImage(buildingType.buildingImage);
            }
            SetBackGroundColor(ShopPanel.instance.buildingShopItemDefaultBackgroundColor);
        }
        else
        {
            Debug.Log("Can't build Building Shop Item | Building type not assigned.");
        }
    }

}
