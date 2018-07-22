using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MineBuilding : Building {

    //public List<ResourcesManager.ResourceAmount> production;
    public List<ResourcesManager.ResourceAmount> production = new List<ResourcesManager.ResourceAmount>();
                                                                                                           


    public MineBuilding(string name, List<ResourcesManager.ResourceAmount> production) : base(name)
    {
        this.production = production;
    }

    void Start()
    {
        production.Add(new ResourcesManager.ResourceAmount(ResourcesManager.instance.availableResources.Find(item => item.resourceName == "steel"), 1));

        InvokeRepeating("Produce", 0f, 2f);
    }

    public void Produce()
    {
        if(hasEnoughEnergy)
        {
            foreach (var productionUnit in production)
            {
                ResourcesManager.ResourceType rType = productionUnit.resourceType;
                int amount = productionUnit.amount;

                ResourcesManager.instance.ProduceResource(rType, amount);
            }
        }
        else
        {
            Debug.Log("Mine can't produce, energy requirement isn't met !");
        }
    }
}
