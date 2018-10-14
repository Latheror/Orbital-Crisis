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

	// Use this for initialization
	void Start () {
        UpdateResourceAvailabilityIndicator();
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void SetInfos(ResourcesManager.ResourceAmount resourceAmount)
    {
        this.resourceType = resourceAmount.resourceType;
        this.costAmount = resourceAmount.amount;
        this.resourceType = resourceAmount.resourceType;
        this.resourceImage = resourceAmount.resourceType.resourceImage;
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
        Debug.Log("UpdateResourceAvailabilityIndicator | Resource: " + resourceType.resourceName);
        if (ResourcesManager.instance.GetResourceFromCurrentList(resourceType).amount >= costAmount)
        {
            SetAvailableResourceColor(true);
        }
        else
        {
            SetAvailableResourceColor(false);
        }
    }
}
