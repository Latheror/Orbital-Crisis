using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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

}
