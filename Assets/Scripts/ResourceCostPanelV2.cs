using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ResourceCostPanelV2 : MonoBehaviour
{
    public ResourcesManager.ResourceAmount associatedResourceCost;

    public Image resourceImage;
    public TextMeshProUGUI resourceCostText;

    public Color canPayColor;
    public Color cantPayColor;

    public void SetInfo(ResourcesManager.ResourceAmount rCost)
    {
        if(rCost != null && rCost.resourceType != null)
        {
            associatedResourceCost = rCost;
            resourceImage.sprite = rCost.resourceType.resourceImage;
            resourceCostText.text = rCost.amount.ToString();
            resourceCostText.color = (ResourcesManager.instance.CanPayResourceAmount(rCost)) ? canPayColor : cantPayColor;
        }
        else
        {
            associatedResourceCost = null;
            resourceImage.sprite = null;
            resourceImage.color = StyleManager.instance.transparentColor;
            resourceCostText.text = "";
        }
    }

    public bool CanPayUpgradeCost()
    {
        if(associatedResourceCost != null)
        {
            return ResourcesManager.instance.CanPayResourceAmount(associatedResourceCost);
        }
        else
        {
            Debug.LogError("ResourceCostPanelV2 | CanPayUpgradeCost | No associated resource cost");
            return false;
        }
    }
}
