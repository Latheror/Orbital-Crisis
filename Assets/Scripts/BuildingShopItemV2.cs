using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BuildingShopItemV2 : MonoBehaviour
{
    public BuildingManager.BuildingType associatedBuildingType;

    public Image buildingImage;
    public GameObject resourceCost_1;
    public GameObject resourceCost_2;


    public void SetInfo(BuildingManager.BuildingType bType = null)
    {
        Debug.Log("BuildingShopItem | SetInfo");
        if (bType != null)
        {
            associatedBuildingType = bType;
        }
        if(associatedBuildingType != null)
        {
            buildingImage.sprite = associatedBuildingType.buildingImage;

            List<ResourcesManager.ResourceAmount> resourceCosts = bType.resourceCosts;

            ResourcesManager.ResourceAmount rCost_1 = null;
            ResourcesManager.ResourceAmount rCost_2 = null;

            if (resourceCosts.Count > 0)
            {
                rCost_1 = bType.resourceCosts[0];
            }
            resourceCost_1.GetComponent<ResourceCostPanelV2>().SetInfo(rCost_1);

            if (resourceCosts.Count > 1)
            {
                rCost_2 = bType.resourceCosts[1];
            }
            resourceCost_2.GetComponent<ResourceCostPanelV2>().SetInfo(rCost_2);

            Sprite backgroundImage = BuildingShopManager.instance.attackCircle;
            switch(bType.buildingCategory)
            {
                case BuildingManager.BuildingCategory.Attack:
                {
                    backgroundImage = BuildingShopManager.instance.attackCircle;
                    break;
                }
                case BuildingManager.BuildingCategory.Defense:
                {
                    backgroundImage = BuildingShopManager.instance.defenseCircle;
                    break;
                }
                case BuildingManager.BuildingCategory.Production:
                {
                    backgroundImage = BuildingShopManager.instance.productionCircle;
                    break;
                }
                default:
                {
                    Debug.LogError("BuildingShopItem | SetInfo | Building category invalid");
                    break;
                }
            }

            gameObject.GetComponent<Image>().sprite = backgroundImage;

        }
    }

    public void OnButtonTouch()
    {
        BuildingManager.instance.BuildingShopItemSelected(associatedBuildingType);
    }
}
