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
        BuildingManager.instance.SelectBuilding(buildingType);
    }

    public void SetBuildingNameText(string buildingName)
    {
        buildingNameText.text = buildingName;
    }

    public void SetBuildingImage(Sprite image)
    {
        buildingImage.GetComponent<Image>().sprite = image;
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
        }
        else
        {
            Debug.Log("Can't build Building Shop Item | Building type not assigned.");
        }
    }

}
