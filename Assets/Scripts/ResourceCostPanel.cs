using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ResourceCostPanel : MonoBehaviour {

    [Header("UI")]
    public TextMeshProUGUI costText;
    public GameObject imagePanelParent;
    public GameObject imagePanel;
    public Sprite resourceImage;

    public Color resourceAmountAvailableColor = Color.green;
    public Color resourceAmountUnavailableColor = Color.red;

    [Header("Operation")]
    public ResourcesManager.ResourceType resourceType;
    public int costAmount;

    public bool resourceAvailable = false;

    void Start () {
        UpdateResourceAvailabilityIndicator();
    }

    public void SetInfo(ResourcesManager.ResourceAmount resourceAmount)
    {
        resourceType = resourceAmount.resourceType;
        costAmount = resourceAmount.amount;
        resourceType = resourceAmount.resourceType;
        resourceImage = resourceAmount.resourceType.resourceImage;

        SetCostAmount();
        SetResourceImage();
        UpdateResourceAvailabilityIndicator();
    }

    public void BuildPanel()
    {
        SetCostAmount();
        SetResourceImage();
    }

    public void SetCostAmount()
    {
        costText.text = costAmount.ToString();
    }

    public void SetResourceImage()
    {
        imagePanel.GetComponent<Image>().sprite = resourceImage;
    }

    public void SetBackgroundColor(Color color)
    {
        GetComponent<Image>().color = color;
    }

    public void SetAvailableResourceColor(bool resourceAvailable)
    {
        SetBackgroundColor((resourceAvailable)? resourceAmountAvailableColor : resourceAmountUnavailableColor);
    }

    public void UpdateResourceAvailabilityIndicator()
    {
        //Debug.Log("UpdateResourceAvailabilityIndicator | Resource: " + resourceType.resourceName);
        if (ResourcesManager.instance.GetResourceFromCurrentList(resourceType).amount >= costAmount)
        {
            resourceAvailable = true;
            SetAvailableResourceColor(true);
        }
        else
        {
            resourceAvailable = false;
            SetAvailableResourceColor(false);
        }

        SetAvailableResourceColor(resourceAvailable);
    }
}
