using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PopulationManager : MonoBehaviour {

    public static PopulationManager instance;
    void Awake()
    {
        if (instance != null) { Debug.LogError("More than one PopulationManager in scene !"); return; }
        instance = this;
    }

    public enum PopulationAffectationType { Attack, Defense, Production}

    [Header("UI")]
    public TextMeshProUGUI totalPopulationAmountText;
    public GameObject populationPanel;

    [Header("Settings")]
    public float populationLossPerMeteorUnitOfSize = 1f;

    public float attackBonusPerUnitOfPopulation = 0.01f;
    public float defenseBonusPerUnitOfPopulation = 0.01f;
    public float productionBonusPerUnitOfPopulation = 0.01f;
    public float resistanceBonusPerUnitOfPopulation = 0.01f;

    public float startTotalPopulationAmount = 20f;
    public float startMaxPopulationAmount = 20f;

    [Header("Operation")]
    public float totalPopulationAmount = 20f;
    public float maxPopulationAmount = 20f;
    // Population percentages
    public int populationAttackPercentage = 30;
    public int populationDefensePercentage = 30;
    public int populationProductionPercentage = 40;
    // Population numbers
    public int populationAttackAmount = 1;
    public int populationDefenseAmount = 1;
    public int populationProductionAmount = 1;

    public float populationResistanceBonus = 0f;

    private void Start()
    {
        Initialize();
    }

    public void Initialize()
    {
        maxPopulationAmount = startMaxPopulationAmount;
        totalPopulationAmount = startTotalPopulationAmount;
        CalculatePopulationAttribution();
        DisplayInfo();
    }

    public void DisplayInfo()
    {
        PlanetCanvasManager.instance.SetTotalPopulationAmount(Mathf.FloorToInt(totalPopulationAmount), Mathf.FloorToInt(maxPopulationAmount));
        PlanetCanvasManager.instance.SetPopulationPercentages(populationAttackPercentage, populationDefensePercentage, populationProductionPercentage);
    }    

    public void PlanetHitByMeteor(Meteor meteor)
    {
        float populationResistanceFactor = Mathf.Max(0.1f, (1 - populationResistanceBonus));
        Debug.Log("PlanetHitByMeteor | PopulationResistanceBonus [" + populationResistanceFactor + "]");
        totalPopulationAmount = Mathf.Max(0, totalPopulationAmount - (meteor.size * populationLossPerMeteorUnitOfSize) * populationResistanceFactor);
        
        DisplayInfo();

        PlayPopulationHurtAnimation();

        if(totalPopulationAmount <= 0)
        {
            ScoreManager.instance.TriggerGameOver();
        }
    }

    public void PlayPopulationHurtAnimation()
    {
        Debug.Log("PlayPopulationHurtAnimation");
        Animation anim = populationPanel.GetComponent<Animation>();
        anim.Play();
    }

    public void IncreaseDecreaseAssignedPopulation(int populationAffectationTypeIndex, bool increase)
    {
        //Debug.Log("IncreaseDecreaseAssignedPopulation | Index [" + populationAffectationTypeIndex + "] | Increase [" + increase + "]");
        switch (populationAffectationTypeIndex) // Attack: 0, Defense: 1, Production: 2
        {
            case (int)PopulationAffectationType.Attack:
            {
                if(increase)
                {
                    int willIncreaseAttackOf = Mathf.Min(10, 100 - populationAttackPercentage);
                    //Debug.Log("Increasing Attack Percentage of [" + willIncreaseAttackOf + "]");
                    populationAttackPercentage += willIncreaseAttackOf;

                    if(populationDefensePercentage <= populationProductionPercentage)
                    {
                        int willDecreaseDefenseOf = Mathf.Min(populationDefensePercentage, willIncreaseAttackOf / 2);
                        populationDefensePercentage -= willDecreaseDefenseOf;

                        populationProductionPercentage -= (willIncreaseAttackOf - willDecreaseDefenseOf);
                    }
                    else
                    {
                        int willDecreaseProductionOf = Mathf.Min(populationProductionPercentage, willIncreaseAttackOf / 2);
                        populationProductionPercentage -= willDecreaseProductionOf;

                        populationDefensePercentage -= (willIncreaseAttackOf - willDecreaseProductionOf);
                    }
                }
                else
                {
                    int willDecreaseAttackOf = Mathf.Min(10, populationAttackPercentage);
                    //Debug.Log("Decreasing Attack Percentage of [" + willDecreaseAttackOf + "]");
                    populationAttackPercentage -= willDecreaseAttackOf;

                    if (populationDefensePercentage >= populationProductionPercentage)
                    {
                        int willIncreaseDefenseOf = Mathf.Min(100 - populationDefensePercentage, willDecreaseAttackOf / 2);
                        populationDefensePercentage += willIncreaseDefenseOf;

                        populationProductionPercentage += (willDecreaseAttackOf - willIncreaseDefenseOf);
                    }
                    else
                    {
                        int willIncreaseProductionOf = Mathf.Min(100 - populationProductionPercentage, willDecreaseAttackOf / 2);
                        populationProductionPercentage += willIncreaseProductionOf;

                        populationDefensePercentage += (willDecreaseAttackOf - willIncreaseProductionOf);
                    }

                }
                break;
            }
            case (int)PopulationAffectationType.Defense:
            {
                if (increase)
                {
                    int willIncreaseDefenseOf = Mathf.Min(10, 100 - populationDefensePercentage);
                    //Debug.Log("Increasing Defense Percentage of [" + willIncreaseDefenseOf + "]");
                    populationDefensePercentage += willIncreaseDefenseOf;

                    if (populationProductionPercentage <= populationAttackPercentage)
                    {
                        int willDecreaseProductionOf = Mathf.Min(populationProductionPercentage, willIncreaseDefenseOf / 2);
                        populationProductionPercentage -= willDecreaseProductionOf;

                        populationAttackPercentage -= (willIncreaseDefenseOf - willDecreaseProductionOf);
                    }
                    else
                    {
                        int willDecreaseAttackOf = Mathf.Min(populationAttackPercentage, willIncreaseDefenseOf / 2);
                        populationAttackPercentage -= willDecreaseAttackOf;

                        populationProductionPercentage -= (willIncreaseDefenseOf - willDecreaseAttackOf);
                    }
                }
                else
                {
                    int willDecreaseDefenseOf = Mathf.Min(10, populationDefensePercentage);
                    //Debug.Log("Decreasing Defense Percentage of [" + willDecreaseDefenseOf + "]");
                    populationDefensePercentage -= willDecreaseDefenseOf;

                    if (populationProductionPercentage >= populationAttackPercentage)
                    {
                        int willIncreaseProductionOf = Mathf.Min(100 - populationProductionPercentage, willDecreaseDefenseOf / 2);
                        populationProductionPercentage += willIncreaseProductionOf;

                        populationAttackPercentage += (willDecreaseDefenseOf - willIncreaseProductionOf);
                    }
                    else
                    {
                        int willIncreaseAttackOf = Mathf.Min(100 - populationAttackPercentage, willDecreaseDefenseOf / 2);
                        populationAttackPercentage += willIncreaseAttackOf;

                        populationProductionPercentage += (willDecreaseDefenseOf - willIncreaseAttackOf);
                    }

                }
                break;
            }
            case (int)PopulationAffectationType.Production:
            {
                if (increase)
                {
                    int willIncreaseProductionOf = Mathf.Min(10, 100 - populationProductionPercentage);
                    //Debug.Log("Increasing Production Percentage of [" + willIncreaseProductionOf + "]");
                    populationProductionPercentage += willIncreaseProductionOf;

                    if (populationAttackPercentage <= populationDefensePercentage)
                    {
                        int willDecreaseAttackOf = Mathf.Min(populationAttackPercentage, willIncreaseProductionOf / 2);
                        populationAttackPercentage -= willDecreaseAttackOf;

                        populationDefensePercentage -= (willIncreaseProductionOf - willDecreaseAttackOf);
                    }
                    else
                    {
                        int willDecreaseDefenseOf = Mathf.Min(populationDefensePercentage, willIncreaseProductionOf / 2);
                        populationDefensePercentage -= willDecreaseDefenseOf;

                        populationAttackPercentage -= (willIncreaseProductionOf - willDecreaseDefenseOf);
                    }
                }
                else
                {
                    int willDecreaseProductionOf = Mathf.Min(10, populationProductionPercentage);
                    //Debug.Log("Decreasing Production Percentage of [" + willDecreaseProductionOf + "]");
                    populationProductionPercentage -= willDecreaseProductionOf;

                    if (populationAttackPercentage >= populationDefensePercentage)
                    {
                        int willIncreaseAttackOf = Mathf.Min(100 - populationAttackPercentage, willDecreaseProductionOf / 2);
                        populationAttackPercentage += willIncreaseAttackOf;

                        populationDefensePercentage += (willDecreaseProductionOf - willIncreaseAttackOf);
                    }
                    else
                    {
                        int willIncreaseDefenseOf = Mathf.Min(100 - populationDefensePercentage, willDecreaseProductionOf / 2);
                        populationDefensePercentage += willIncreaseDefenseOf;

                        populationAttackPercentage += (willDecreaseProductionOf - willIncreaseDefenseOf);
                    }

                }
                break;
            }
        }

        PlanetCanvasManager.instance.SetPopulationPercentages(populationAttackPercentage, populationDefensePercentage, populationProductionPercentage);

        CalculatePopulationAttribution();
    }

    public void CalculatePopulationAttribution()
    {
        populationAttackAmount = (int)totalPopulationAmount * populationAttackPercentage / 100;
        populationDefenseAmount = (int)totalPopulationAmount * populationDefensePercentage / 100;
        populationProductionAmount = (int)totalPopulationAmount - populationAttackAmount - populationDefenseAmount;

        //Debug.Log("CalculatePopulationAttribution | Attack [" + populationAttackAmount + "] | Defense [" + populationDefenseAmount + "] | Production [" + populationProductionAmount + "]");

        ApplyPopulationEffects();
    }

    public void ApplyPopulationEffects()
    {
        ApplyPopulationEffectsToBuildings();

        populationResistanceBonus = populationDefenseAmount * resistanceBonusPerUnitOfPopulation;
    }

    public void ApplyPopulationEffectsToBuildings()
    {
        float attackBonus = attackBonusPerUnitOfPopulation * populationAttackAmount;
        float defenseBonus = defenseBonusPerUnitOfPopulation * populationDefenseAmount;
        float productionBonus = productionBonusPerUnitOfPopulation * populationProductionAmount;
        Debug.Log("ApplyPopulationEffectsToBuildings | Attack [" + attackBonus + "] | Defense [" + defenseBonus + "] | Production [" + productionBonus + "]");

        foreach (GameObject b in BuildingManager.instance.buildingList)
        {
            Building building = b.GetComponent<Building>();
            switch(building.buildingType.buildingCategory)
            {
                case (BuildingManager.BuildingCategory.Attack):
                {
                    building.populationBonus = attackBonus;
                    break;
                }
                case (BuildingManager.BuildingCategory.Defense):
                {
                    building.populationBonus = defenseBonus;
                    break;
                }
                case (BuildingManager.BuildingCategory.Production):
                {
                    building.populationBonus = productionBonus;
                    if(b.GetComponent<PowerPlant>() != null)
                    {
                        b.GetComponent<PowerPlant>().UpdatePopulationBonusEffectsOnProduction();
                    }
                    break;
                }
            }
            
        }

        EnergyPanel.instance.UpdateEnergyProductionAndConsumption();
    }

    public PopulationData BuildPopulationData()
    {
        return new PopulationData(totalPopulationAmount, populationAttackPercentage, populationDefensePercentage, populationProductionPercentage);
    }

    public void SetupSavedPopulationData(PopulationData populationData)
    {
        Debug.Log("SetupSavedPopulationData");
        SetPopulationParameters(populationData.totalPopulationAmount, populationData.attackPercentage, populationData.defensePercentage, populationData.productionPercentage);
        PlanetCanvasManager.instance.SetPopulationPercentages(populationAttackPercentage, populationDefensePercentage, populationProductionPercentage);
    }

    public void SetPopulationParameters(float totalPop, int attackP, int defenseP, int productionP)
    {
        totalPopulationAmount = totalPop;
        populationAttackPercentage = attackP;
        populationDefensePercentage = defenseP;
        populationProductionPercentage = productionP;

        CalculatePopulationAttribution();
    }

    public void NewWaveStarted(int waveIndex)
    {
        Debug.Log("PopulationManager | NewWaveStarted");
        NewWaveActions(waveIndex);
    }

    public void NewWaveActions(int waveIndex)
    {
        if (waveIndex % 5 == 0)  // Every 5 waves
        {
            IncreaseMaxPopulation(2);
            RetrievePopulation(1);
        }

        RetrievePopulation(1);
    }

    public void RetrievePopulation(int ampunt)
    {
        totalPopulationAmount = Mathf.Min(totalPopulationAmount + 1, maxPopulationAmount);
        DisplayInfo();
    }

    public void IncreaseMaxPopulation(int amount)
    {
        maxPopulationAmount += amount;
        DisplayInfo();
    }

    [System.Serializable]
    public class PopulationData
    {
        public float totalPopulationAmount;

        public int attackPercentage;
        public int defensePercentage;
        public int productionPercentage;

        public PopulationData(float totalPopulationAmount, int attackPercentage, int defensePercentage, int productionPercentage)
        {
            this.totalPopulationAmount = totalPopulationAmount;
            this.attackPercentage = attackPercentage;
            this.defensePercentage = defensePercentage;
            this.productionPercentage = productionPercentage;
        }
    }

}
