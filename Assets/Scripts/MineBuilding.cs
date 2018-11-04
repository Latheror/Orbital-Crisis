using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MineBuilding : Building {

    //public List<ResourcesManager.ResourceAmount> production;
    public List<ResourcesManager.ResourceAmount> production = new List<ResourcesManager.ResourceAmount>();

    public float productionDelay = 2f;
                                                                                                           


    public MineBuilding(List<ResourcesManager.ResourceAmount> production) : base()
    {
        this.production = production;
    }

    void Start()
    {
        production.Add(new ResourcesManager.ResourceAmount(ResourcesManager.instance.GetResourceTypeByName("steel"), 1));
        production.Add(new ResourcesManager.ResourceAmount(ResourcesManager.instance.GetResourceTypeByName("silver"), 1));
        production.Add(new ResourcesManager.ResourceAmount(ResourcesManager.instance.GetResourceTypeByName("carbon"), 1));

        InvokeRepeating("Produce", 0f, productionDelay);
    }

    public void Produce()
    {
        if (GameManager.instance.gameState == GameManager.GameState.Default)
        {
            if (hasEnoughEnergy)
            {
                foreach (var productionUnit in production)
                {
                    ResourcesManager.ResourceType rType = productionUnit.resourceType;
                    int amount = productionUnit.amount;

                    ResourcesManager.instance.ProduceResource(rType, amount);
                }

                ShopPanel.instance.UpdateShopItems();
            }
            else
            {
                Debug.Log("Mine can't produce, energy requirement isn't met !");
            }
        }
    }
}
