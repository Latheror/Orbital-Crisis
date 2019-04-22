using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ResourcesManager : MonoBehaviour {

    public static ResourcesManager instance;
    void Awake()
    {
        if (instance != null) { Debug.LogError("More than one ResourcesManager in scene !"); return; }
        instance = this;

    }

    [Header("Settings")]
    public List<ResourceType> availableResources = new List<ResourceType>();
    public List<ResourceAmount> startResourceAmounts = new List<ResourceAmount>();

    public Sprite steelImage, carbonImage, compositeImage, electronicsImage;
    
    public Color steelColor, carbonColor, compositeColor, electronicsColor;

    public Color maxResourceAmountReachedColor;
    public Color maxResourceAmountNotReachedColor;
    public float maxResourcesMargin = .2f;

    [Header("Operation")]
    public List<ResourceAmount> currentResourceAmounts = new List<ResourceAmount>();
    //public Dictionary<string, ResourceAmount> currentResourceAmountsDictionnary = new Dictionary<string, ResourceAmount>();

    [Header("UI")]
    //public GameObject resourceIndicatorLayout;      // NOT USED ANYMORE //
    public TextMeshProUGUI steelAmountText;
    public TextMeshProUGUI carbonAmountText;
    public TextMeshProUGUI compositeAmountText;
    public TextMeshProUGUI electronicsAmountText;


    [Header("Prefabs")]
    public GameObject resourceIndicatorPrefab;

    [Header("IDs")]
    public const int steelID = 1, carbonID = 2, compositeID = 3, electronicsID = 4;

    // Types of resources and their info
    public void InitializeResources()
    {
        availableResources.Add(new ResourceType(steelID, "steel", steelColor, steelImage, 600, 7500, steelAmountText));
        availableResources.Add(new ResourceType(carbonID, "carbon", carbonColor, carbonImage, 350, 3500, carbonAmountText));
        availableResources.Add(new ResourceType(compositeID, "composite", compositeColor, compositeImage, 150, 2400, compositeAmountText));
        availableResources.Add(new ResourceType(electronicsID, "electronics", electronicsColor, electronicsImage, 100, 2000, electronicsAmountText));
    }

    // Set starting resource amounts
    public void InitializeResourceAmounts()
    {
        foreach (ResourceType rType in availableResources)
        {
            ResourceAmount newResourceAmount = new ResourceAmount(rType, rType.startAmount);
            currentResourceAmounts.Add(newResourceAmount);
        }
    }

    public ResourceAmount GetResourceFromCurrentList(ResourceType rType)
    {
        ResourceAmount toGiveBack = null;
        foreach (var resAmount in currentResourceAmounts)
        {
            //Debug.Log("Comparing Resource \"" + resAmount.resourceType.resourceName + "\" with Resource \"" + rType.resourceName + "\"");
            if(resAmount.resourceType.Equals(rType))
            {
                toGiveBack = resAmount;
                break;
            }
        }
        return toGiveBack;
    }

    public int GetCurrentResourceAmount(ResourceType rType)
    {
        int ra = 0;
        foreach (var resAmount in currentResourceAmounts)
        {
            if (resAmount.resourceType.Equals(rType))
            {
                ra = resAmount.amount;
                break;
            }
        }
        return ra;
    }

    // Build resource indicators                        
    public void InitializeResourceIndicators()
    {
        // NOT USED ANYMORE //
        /*foreach (ResourceType resource in availableResources)
        {
            GameObject instantiatedResourceIndicator = Instantiate(resourceIndicatorPrefab, new Vector3(0f, 0f, 0f), Quaternion.identity);
            instantiatedResourceIndicator.transform.SetParent(resourceIndicatorLayout.transform, false);
            instantiatedResourceIndicator.GetComponent<ResourceIndicator>().SetParameters(resource);

            resource.resourceIndicator = instantiatedResourceIndicator;
            instantiatedResourceIndicator.GetComponent<ResourceIndicator>().resourceAmount = GetResourceFromCurrentList(resource);
        }*/

        foreach (ResourceType resource in availableResources)
        {
            ResourceAmount r = GetResourceFromCurrentList(resource);
            r.resourceType.resourceAmountText.text = r.amount.ToString();
        }
    }

    // Produce an amount of resource
    public void ProduceResource(ResourceType rType, int amount)
    {
        //Debug.Log("Producing resource: " + rType.resourceName + " in quantity: " + amount);
        foreach (ResourceAmount resourceAmount in currentResourceAmounts)
        {
            if(resourceAmount.resourceType.Equals(rType))
            {
                // We found the right resource
                resourceAmount.amount = Mathf.Min(resourceAmount.amount + amount, resourceAmount.resourceType.maxAmount);
                UpdateResourceAmountText(resourceAmount);
                break;
            }
        }

        //ShopPanel.instance.UpdateShopItems();
        BuildingInfoPanel.instance.UpdateInfo();
    }

    public void UpdateResourceAmountText(ResourceAmount rAmount)
    {
        TextMeshProUGUI rText = rAmount.resourceType.resourceAmountText;
        rText.text = rAmount.amount.ToString();
        SetResourceValueColor(rAmount);
    }

    public void SetResourceValueColor(ResourceAmount rAmount)
    {
        if ((rAmount.amount * (1 + maxResourcesMargin) > rAmount.resourceType.maxAmount))
        {
            rAmount.resourceType.resourceAmountText.color = maxResourceAmountReachedColor;
        }
        else
        {
            rAmount.resourceType.resourceAmountText.color = maxResourceAmountNotReachedColor;
        }
    }

    public void UpdateResourceAmountText(ResourceType resourceType)
    {
        UpdateResourceAmountText(GetCurrentResourceAmountByID(resourceType.id));
    }

    public void SetResourceAmount(int resourceId, int amount)
    {
        ResourceAmount rAmount = GetCurrentResourceAmountByID(resourceId);
        if(rAmount != null)
        {
            rAmount.amount = amount;
            UpdateResourceAmountText(rAmount);
        }
    }

    public ResourceAmount GetCurrentResourceAmountByID(int rID)
    {
        ResourceAmount rAmount = null;
        foreach (var resourceAmount in currentResourceAmounts)
        {
            if (resourceAmount.resourceType.id.Equals(rID))
            {
                rAmount = resourceAmount;
            }
        }
        return rAmount;
    }

    public void PayResource(ResourceType resourceType, int amount)
    {
        //Debug.Log("Paying resource: " + resourceType.resourceName + " in quantity: " + amount);
        if(GetResourceFromCurrentList(resourceType).amount < amount)
        {
            Debug.Log("Error : Can't pay resource !");
        }
        else
        {
            DecreaseResource(resourceType, amount);
        }

        //ShopPanel.instance.UpdateShopItems();
        BuildingInfoPanel.instance.UpdateInfo();
    }

    public void PayResourceAmount(ResourceAmount resourceAmount)
    {
        PayResource(resourceAmount.resourceType, resourceAmount.amount);
    }

    public void DecreaseResource(ResourceType resourceType, int amount)
    {
        GetResourceFromCurrentList(resourceType).amount -= amount;
        //resourceType.resourceAmountText.GetComponent<ResourceIndicator>().UpdateIndicator();
        UpdateResourceAmountText(resourceType);
    }

    // Check if we have enough resources to build a building
    public bool CanPayConstruction(BuildingManager.BuildingType bType)
    {
        bool canPay = true;

        foreach (var cost in bType.resourceCosts)
        {
            //Debug.Log("Checking if we can pay the cost in: " + cost.resourceType.resourceName + " (" + cost.amount + ").");
            if(cost.amount > GetResourceFromCurrentList(cost.resourceType).amount)
            {
                canPay = false;
                break;
            }
        }
        return canPay;
    }

    public void PayConstruction(BuildingManager.BuildingType bType)
    {
        foreach (var cost in bType.resourceCosts)
        {
            PayResource(GetResourceFromCurrentList(cost.resourceType).resourceType, cost.amount);
        }

        //ShopPanel.instance.UpdateShopItems();
        BuildingInfoPanel.instance.UpdateInfo();
    }

    public ResourceType GetResourceTypeByName(string rName)
    {
        ResourceType resourceType = null;
        foreach (ResourceType rType in availableResources)
        {
            if(rType.resourceName == rName)
            {
                resourceType = rType;
            }
        }
        return resourceType;
    }

    public bool CanPayUpgradeCosts(Building building)
    {
        bool canPay = true;

        List<ResourceAmount> upgradeCosts =  building.GetComponent<Building>().GetUpgradeCostsForNextTier();

        foreach (ResourceAmount resourceAmount in upgradeCosts)
        {
            if( ! instance.CanPayResourceAmount(resourceAmount))
            {
                canPay = false;
                break;
            }
        }

        return canPay;
    }

    public bool PayUpgradeCosts(Building building)
    {
        bool payWorked = false;

        List<ResourceAmount> upgradeCosts = building.GetComponent<Building>().GetUpgradeCostsForNextTier();
        foreach (ResourceAmount resourceAmount in upgradeCosts)
        {
            PayResourceAmount(resourceAmount);
        }

        payWorked = true;   // Temporary
        return payWorked;
    }

    public bool CanPayResource(ResourceType r, int amount)
    {
        return((GetResourceFromCurrentList(r).amount) >= amount);
    }

    public bool CanPayResourceAmount(ResourceAmount rAmount)
    {
        return ((GetResourceFromCurrentList(rAmount.resourceType).amount) >= rAmount.amount);
    }

    public ResourceData[] BuildResourcesData()
    {
        ResourceData[] resourcesData = new ResourceData[availableResources.Count];
        for(int i=0; i<availableResources.Count; i++)
        {
            resourcesData[i] = new ResourceData(availableResources[i].id, currentResourceAmounts[i].amount);
            Debug.Log("BuildResourcesData | ID [" + availableResources[i].id + "] | Amount [" + currentResourceAmounts[i].amount + "]");
        }
        return resourcesData;
    }

    public ResourceType GetResourceTypeById(int id)
    {
        ResourceType resType = availableResources[0];
        foreach (ResourceType rType in availableResources)
        {
            if(rType.id == id)
            {
                resType = rType;
                break;
            }
        }
        return resType;
    }

    public void SetResourcesAmounts(ResourceData[] resourcesData)
    {
        Debug.Log("SetResourcesAmounts...");
        for(int i=0; i<resourcesData.Length; i++)
        {
            ResourceData resourceDataI = resourcesData[i];
            int resourceId = resourceDataI.resourceId;
            ResourceType resourceType = GetResourceTypeById(resourceId);
            int resourceAmount = resourceDataI.resourceAmount;
            Debug.Log(resourceId + " | " + resourceType.resourceName + " [" + resourceAmount + "]");

            SetResourceAmount(resourceId, resourceAmount);
        }
    }

    public bool CanPayResourceAmounts(List<ResourceAmount> resourceAmounts)
    {
        bool canPay = true;
        foreach (ResourceAmount rAmount in resourceAmounts)
        {
            if(! CanPayResourceAmount(rAmount))
            {
                canPay = false;
                break;
            }
        }
        return canPay;
    }

    public void PayResourceAmounts(List<ResourceAmount> resourceAmounts)
    {
        foreach (ResourceAmount rAmount in resourceAmounts)
        {
            PayResourceAmount(rAmount);
        }
    }


    //[System.Serializable]
    public class Resource
    {
        public ResourceType resourceType;
        public int resourceAmount;
    }

    [System.Serializable]
    public class ResourceType 
    {
        public int id;
        public string resourceName;
        public Color color;
        public int startAmount;
        public int maxAmount;
        public TextMeshProUGUI resourceAmountText;
        public Sprite resourceImage;

        public ResourceType(int id, string name, Color color, Sprite resourceImage, int startAmount, int maxAmount, TextMeshProUGUI resourceAmountText)
        {
            this.id = id;
            this.resourceName = name;
            this.color = color;
            this.resourceImage = resourceImage;
            this.startAmount = startAmount;
            this.maxAmount = maxAmount;
            this.resourceAmountText = resourceAmountText;
        }

        public bool Equals(ResourceType rt)
        {
            return (resourceName.Equals(rt.resourceName));
        }
    }

    [System.Serializable]
    public class ResourceAmount 
    {
        public ResourceType resourceType;
        public int amount;

        public ResourceAmount(ResourceType resourceType, int amount)
        {
            //Debug.Log("Building a ResourceAmount | ResourceType: " + resourceType.resourceName + " | Amount: " + amount);
            this.resourceType = resourceType;
            this.amount = amount;
        }

        public ResourceAmount(string resourceTypeName, int amount)
        {
            ResourceType temp = ResourcesManager.instance.GetResourceTypeByName(resourceTypeName);
            if(temp != null) {
                resourceType = temp;
            }
            else {
                Debug.Log("Can't construct ResourceAmount : ResourceTypeName is unknown !");
            }
            this.amount = amount;
        }
    }

    [System.Serializable]
    public class UpgradeCost
    {
        public int tierIndex;
        public List<ResourceAmount> resourceCosts;

        public UpgradeCost(int tierIndex, List<ResourceAmount> resourceCosts)
        {
            this.tierIndex = tierIndex;
            this.resourceCosts = resourceCosts;
        }
    }

    [System.Serializable]
    public class ResourceData
    {
        public int resourceId;
        public string resourceName;
        public int resourceAmount;

        public ResourceData(int resourceId, int resourceAmount)
        {
            this.resourceId = resourceId;
            this.resourceAmount = resourceAmount;
            this.resourceName = ResourcesManager.instance.GetResourceTypeById(resourceId).resourceName;
        }
    }
}
