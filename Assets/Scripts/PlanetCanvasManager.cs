using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlanetCanvasManager : MonoBehaviour {

    public static PlanetCanvasManager instance;
    void Awake()
    {
        if (instance != null) { Debug.LogError("More than one PlanetCanvasManager in scene !"); return; }
        instance = this;
    }

    [Header("UI")]
    public GameObject dezoomInfoPanel;
    public GameObject zoomInfoPanel;

    public GameObject attackUpButton;
    public GameObject attackDownButton;
    public GameObject defenseUpButton;
    public GameObject defenseDownButton;
    public GameObject productionUpButton;
    public GameObject productionDownButton;

    public Color defaultPopulationAmountColor;
    public Color lowPopulationAmountColor;

    // Dezoom Canvas
    public TextMeshProUGUI populationAmountText;
    public TextMeshProUGUI maxPopulationAmountText;
    public TextMeshProUGUI energyAmountText;

    // Zoom Canvas
    public TextMeshProUGUI productionAssignedPopulationAmountText;
    public TextMeshProUGUI attackAssignedPopulationAmountText;
    public TextMeshProUGUI defenseAssignedPopulationAmountText;

    [Header("Operation")]
    public int currentPopulationAmount;
    public int maxPopulationAmount;
    public int currentEnergyDifferentialAmount;

    public int populationAttackPercentage;
    public int populationDefensePercentage;
    public int populationProductionPercentage;


    public void DisplayZoomInfoPanel(bool display)
    {
        zoomInfoPanel.SetActive(display);
        dezoomInfoPanel.SetActive(!display);
    }

    // Dezoom Panel ------- //
    public void SetTotalPopulationAmount(int totalPopulationAmount, int maxPop)
    {
        currentPopulationAmount = totalPopulationAmount;
        maxPopulationAmount = maxPop;
        DisplayDezoomPanelInfo();
    }
    //
    public void SetEnergyDifferentialAmount(int energyDifferentialAmount)
    {
        currentEnergyDifferentialAmount = energyDifferentialAmount;
        DisplayDezoomPanelInfo();
    }
    // ------------------ //

    // Zoom Panel ------- //
    public void SetPopulationPercentages(int attackPercentage, int defensePercentage, int productionPercentage)
    {
        populationAttackPercentage = attackPercentage;
        populationDefensePercentage = defensePercentage;
        populationProductionPercentage = productionPercentage;

        DisplayPopulationPercentages();
    }
    // ------------------ //

    public void DisplayPopulationPercentages()
    {
        productionAssignedPopulationAmountText.text = populationProductionPercentage.ToString() + " %";
        attackAssignedPopulationAmountText.text = populationAttackPercentage.ToString() + " %";
        defenseAssignedPopulationAmountText.text = populationDefensePercentage.ToString() + " %";

        attackUpButton.SetActive(populationAttackPercentage != 100);
        attackDownButton.SetActive(populationAttackPercentage != 0);
        defenseUpButton.SetActive(populationDefensePercentage != 100);
        defenseDownButton.SetActive(populationDefensePercentage != 0);
        productionUpButton.SetActive(populationProductionPercentage != 100);
        productionDownButton.SetActive(populationProductionPercentage != 0);
    }

    public void DisplayDezoomPanelInfo()
    {
        populationAmountText.text = currentPopulationAmount.ToString();
        populationAmountText.color = (currentPopulationAmount > PopulationManager.instance.maxPopulationAmount * 0.25) ? defaultPopulationAmountColor : lowPopulationAmountColor;
        maxPopulationAmountText.text = maxPopulationAmount.ToString();
        energyAmountText.text = currentEnergyDifferentialAmount.ToString();
    }

    public void OnPopulationUpButton(int affectationTypeIndex)
    {
        PopulationManager.instance.IncreaseDecreaseAssignedPopulation(affectationTypeIndex, true);
    }

    public void OnPopulationDownButton(int affectationTypeIndex)
    {
        PopulationManager.instance.IncreaseDecreaseAssignedPopulation(affectationTypeIndex, false);
    }
}
