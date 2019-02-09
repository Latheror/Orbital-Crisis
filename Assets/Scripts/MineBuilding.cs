using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MineBuilding : Building {

    //public List<ResourcesManager.ResourceAmount> production;
    public List<ResourcesManager.ResourceAmount> production = new List<ResourcesManager.ResourceAmount>();

    public float productionDelay = 2f;
    public float productionFactor = 1f;

    [Header("Tier 2")]
    public float productionDelay_tier_2 = 1.6f;
    public float energyConsumption_tier_2 = 25;

    [Header("Tier 3")]
    public float productionDelay_tier_3 = 1.2f;
    public float energyConsumption_tier_3 = 50;

    [Header("Operation")]
    public bool productionCooldownElapsed = true;


    public MineBuilding(List<ResourcesManager.ResourceAmount> production) : base()
    {
        this.production = production;
    }

    void Start()
    {
        production.Add(new ResourcesManager.ResourceAmount(ResourcesManager.instance.GetResourceTypeByName("steel"), 2));
        production.Add(new ResourcesManager.ResourceAmount(ResourcesManager.instance.GetResourceTypeByName("copper"), 2));
        production.Add(new ResourcesManager.ResourceAmount(ResourcesManager.instance.GetResourceTypeByName("carbon"), 2));
        //production.Add(new ResourcesManager.ResourceAmount(ResourcesManager.instance.GetResourceTypeByName("composite"), 1));     // Gathered from ennemy spaceships
        //production.Add(new ResourcesManager.ResourceAmount(ResourcesManager.instance.GetResourceTypeByName("electronics"), 1));

        productionCooldownElapsed = true;
    }

    void Update()
    {
        Produce();
    }

    public void Produce()
    {
        if (GameManager.instance.gameState == GameManager.GameState.Default)
        {
            if (hasEnoughEnergy)
            {
                if(!LevelManager.instance.currentLevelFinished && productionCooldownElapsed)
                {
                    foreach (var productionUnit in production)
                    {
                        ResourcesManager.ResourceType rType = productionUnit.resourceType;
                        int amount = productionUnit.amount;
                        // int amount = Mathf.FloorToInt(productionUnit.amount * productionFactor);


                        ResourcesManager.instance.ProduceResource(rType, amount);
                    }

                    StartCoroutine("StartProductionCoolDown", this);

                    ShopPanel.instance.UpdateShopItems();
                }
                else
                {
                    //Debug.Log("Mine | Production CoolDown not elapsed...");
                }
            }
            else
            {
                //Debug.Log("Mine can't produce, energy requirement isn't met !");
            }
        }
    }

    IEnumerator StartProductionCoolDown(MineBuilding mineBuilding)
    {
        mineBuilding.SetCoolDownElapsed(false);
        yield return new WaitForSeconds(mineBuilding.productionDelay * (1 - populationBonus));
        mineBuilding.SetCoolDownElapsed(true);
    }

    public void SetCoolDownElapsed(bool elapsed)
    {
        productionCooldownElapsed = elapsed;
    }

    public override void SetHasEnoughEnergy(bool enough)
    {
        //Debug.Log("MiningFacility | SetHasEnoughEnergy [" + enough + "]");
        hasEnoughEnergy = enough;
        if (powerMissingCanvas != null)
        {
            powerMissingCanvas.SetActive(!enough);
        }
        StartStopMiningAnimation();
    }

    public void StartStopMiningAnimation()  // TODO: Include level state
    {
        //Debug.Log("StartStopMiningAnimation | HasEnoughEnery [" + hasEnoughEnergy + "] | PowerOn [" + powerOn + "]");
        Animator animator = GetComponent<Animator>();
        if (hasEnoughEnergy && powerOn)
        {
            animator.SetTrigger("StartProduction");
        }
        else
        {
            animator.SetTrigger("StopProduction");
        }
    }

    public override void ApplyCurrentTierSettings()
    {
        //Debug.Log("ApplyCurrentTierSettings | MINE | CurrentTier: " + currentTier);
        switch (currentTier)
        {
            case 2:
            {
                productionDelay = productionDelay_tier_2;
                energyConsumption = energyConsumption_tier_2;
                break;
            }
            case 3:
            {
                productionDelay = productionDelay_tier_3;
                energyConsumption = energyConsumption_tier_3;
                break;
            }
        }
    }
}
