using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ResourceCostPanel : MonoBehaviour {

    public ResourcesManager.ResourceType resourceType;

    public TextMeshProUGUI costText;
    public GameObject imagePanelParent;
    public GameObject imagePanel;

    public int costAmount;
    public Sprite resourceImage;

    public Color resourceAmountAvailableColor = Color.green;
    public Color resourceAmountUnavailableColor = Color.red;

    public bool resourceAvailable = false;

    // Use this for initialization
    void Start () {
        UpdateResourceAvailabilityIndicator();
    }
	
	// Update is called once per frame
	void Update () {
		
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
