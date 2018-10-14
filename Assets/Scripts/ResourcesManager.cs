using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourcesManager : MonoBehaviour {

    public static ResourcesManager instance;

    [Header("Settings")]
    public List<ResourceType> availableResources = new List<ResourceType>();
    public List<ResourceAmount> startResourceAmounts = new List<ResourceAmount>();
    public int productionEnergy;
    public int consumptionEnergy;

    [Header("Operation")]
    public List<ResourceAmount> currentResourceAmounts = new List<ResourceAmount>();
    //public Dictionary<string, ResourceAmount> currentResourceAmountsDictionnary = new Dictionary<string, ResourceAmount>();

    [Header("UI")]
    public GameObject resourceIndicatorLayout;

    [Header("Prefabs")]
    public GameObject resourceIndicatorPrefab;
         

    void Awake()
    {
        if (instance != null){ Debug.LogError("More than one ResourcesManager in scene !"); return; } instance = this;
       
    }

    // Types of resources and their info
    public void InitializeResources()
    {
        availableResources.Add(new ResourceType("steel", Color.grey, null, 500));
        availableResources.Add(new ResourceType("silver", Color.white, null, 500));
        availableResources.Add(new ResourceType("carbon", Color.black, null, 500));
    }

    // Set starting resource amounts
    public void InitializeResourceAmounts()
    {
        foreach (ResourceType rType in availableResources)
        {
            ResourceAmount newResourceAmount = new ResourceAmount(rType, rType.startAmount);
            currentResourceAmounts.Add(newResourceAmount);
            rType.currentResourceAmount = newResourceAmount;
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

    // Build resource indicators
    public void InitializeResourceIndicators()
    {
        foreach (ResourceType resource in availableResources)
        {
            GameObject instantiatedResourceIndicator = Instantiate(resourceIndicatorPrefab, resourceIndicatorLayout.transform.position, Quaternion.identity);
            instantiatedResourceIndicator.transform.SetParent(resourceIndicatorLayout.transform, false);
            instantiatedResourceIndicator.GetComponent<ResourceIndicator>().SetParameters(resource);

            resource.resourceIndicator = instantiatedResourceIndicator;
            instantiatedResourceIndicator.GetComponent<ResourceIndicator>().resourceAmount = resource.currentResourceAmount;
        }
    }

    // Produce an amount of resource
    public void ProduceResource(ResourceType rType, int amount)
    {
        //Debug.Log("Producing resource: " + rType.resourceName + " in quantity: " + amount);
        foreach (var resourceAmount in currentResourceAmounts)
        {
            if(resourceAmount.resourceType.Equals(rType))
            {
                // We found the right resource
                resourceAmount.amount += amount;
                resourceAmount.resourceType.resourceIndicator.GetComponent<ResourceIndicator>().UpdateIndicator();
                break;
            }
        }
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
    }

    public void DecreaseResource(ResourceType resourceType, int amount)
    {
        GetResourceFromCurrentList(resourceType).amount -= amount;
        resourceType.resourceIndicator.GetComponent<ResourceIndicator>().UpdateIndicator();
    }

    // Check if we have enough resources to build a building
    public bool CanPay(BuildingManager.BuildingType bType)
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

    public void Pay(BuildingManager.BuildingType bType)
    {
        foreach (var cost in bType.resourceCosts)
        {
            PayResource(GetResourceFromCurrentList(cost.resourceType).resourceType, cost.amount);
        }

        ShopPanel.instance.UpdateShopItems();
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


    [System.Serializable]
    public class Resource
    {
        public ResourceType resourceType;
        public int resourceAmount;
    }

    [System.Serializable]
    public class ResourceType 
    {
        public string resourceName;
        public Color color;
        public int startAmount;
        public ResourceAmount currentResourceAmount;
        public GameObject resourceIndicator;
        public Sprite resourceImage;

        public ResourceType(string name, Color color, Sprite image, int startAmount)
        {
            this.resourceName = name;
            this.color = color;
            this.resourceImage = image;
            this.startAmount = startAmount;
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
}
