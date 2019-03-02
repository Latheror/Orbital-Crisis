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

    public void SetInfo(ResourcesManager.ResourceAmount rCost)
    {
        if(rCost != null)
        {
            associatedResourceCost = rCost;
            resourceImage.sprite = rCost.resourceType.resourceImage;
            resourceCostText.text = rCost.amount.ToString();
        }
        else
        {
            associatedResourceCost = null;
            resourceImage.sprite = null;
            resourceImage.color = StyleManager.instance.transparentColor;
            resourceCostText.text = "";
        }
    }
}
