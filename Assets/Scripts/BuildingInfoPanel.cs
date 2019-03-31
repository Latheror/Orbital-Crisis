using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BuildingInfoPanel : MonoBehaviour {

    public static BuildingInfoPanel instance;
    void Awake()
    {
        if (instance != null) { Debug.LogError("More than one BuildingInfoPanel in scene !"); return; }
        instance = this;
    }


    [Header("UI Elements")]
    public GameObject displayPanel;
    public GameObject buildingImagePanel;
    public GameObject buildingNamePanel;

    public GameObject upgradeCostsLayout;
    public GameObject upgradeCostPanelPrefab;
    public List<GameObject> upgradeCostPanelsList;
    public GameObject destroyButton;

    public GameObject enoughEnergyPanel;
    public GameObject powerOnPanel;
    public Sprite upgradeAvailableSprite;
    public Sprite upgradeNotAvailableSprite;

    // Useful / New
    public GameObject firstTiersUpgradeButtonsPanel;
    public GameObject advancedUpgradeButtonsPanel;
    public TextMeshProUGUI tierNbText;
    public TextMeshProUGUI upgradeText;
    public TextMeshProUGUI energyConsumptionProductionText;

    public GameObject baseUpgradeCost_1;
    public GameObject baseUpgradeCost_2;

    public GameObject finalUpgradeLeftCost_1;
    public GameObject finalUpgradeLeftCost_2;
    public GameObject finalUpgradeCenterCost_1;
    public GameObject finalUpgradeCenterCost_2;
    public GameObject finalUpgradeRightCost_1;
    public GameObject finalUpgradeRightCost_2;

    // Basic Stats
    public TextMeshProUGUI healthPointsText;
    public TextMeshProUGUI shieldPointsText;
    public TextMeshProUGUI rangePointsText;

    public GameObject healthPointsIndicator;
    public GameObject shieldPointsIndicator;
    public GameObject rangePointsIndicator;

    // Specific Stats
    public GameObject specificStat_1;
    public GameObject specificStat_2;
    public Image specificStat_1_image;
    public TextMeshProUGUI specificStat_1_value;
    public Image specificStat_2_image;
    public TextMeshProUGUI specificStat_2_value;

    [Header("Colors")]
    public Color upgradePossibleColor = Color.green;
    public Color upgradeImpossibleColor = Color.red;
    public Color powerOnColor = Color.yellow;
    public Color powerOffColor = Color.grey;
    public Color enoughEnergyColor = Color.green;
    public Color notEnoughEnergyColor = Color.red;
    public Color transparentColor;
    public Color fullWhiteColor;

    [Header("Operation")]
    public GameObject selectedBuilding;
    public bool upgradeAvailable;
    public bool isPanelOpen = false;
	
	public void SetImage()
    {
        buildingImagePanel.GetComponent<Image>().sprite = selectedBuilding.GetComponent<Building>().buildingType.buildingImage;
    }

    public void SetName()
    {
        buildingNamePanel.GetComponent<TextMeshProUGUI>().text = selectedBuilding.GetComponent<Building>().buildingType.name;
    }

    void SetTierText()
    {
        tierNbText.text = (selectedBuilding.GetComponent<Building>().currentTier.ToString());
    }


    public void BuildUpgradesLayout()
    {
        //Debug.Log("BuildUpgradesLayout CurrentTier [" + selectedBuilding.GetComponent<Building>().currentTier + "]");
        if (selectedBuilding.GetComponent<Building>().currentTier <= /*3*/ 2)   // Set this back to 3 when Specialized Upgrades are finished
        {
            DisplayBaseUpgradePanel(true);
            DisplayAdvancedUpgradePanel(false);
            SetUpgradeCosts();
        }
        else    // Building can't be upgraded anymore
        {
            HideAllUpgradeElements();
        }
    }

    public void UpdateUpgradeText()
    {
        upgradeText.text = (ResourcesManager.instance.CanPayResourceAmounts(selectedBuilding.GetComponent<Building>().GetUpgradeCostsForNextTier())) ? "Upgrade" : "No Resources !";
    }

    public void SetUpgradeCosts()
    {
        Building b = selectedBuilding.GetComponent<Building>();
        if (selectedBuilding.GetComponent<Building>().currentTier < 3)   // Display a single upgrade button
        {
            baseUpgradeCost_1.SetActive(true);
            baseUpgradeCost_2.SetActive(true);

            List<ResourcesManager.ResourceAmount> nextTierUpgradeCosts = b.GetUpgradeCostsForNextTier();
            //Debug.Log("SetUpgradeCosts | nextTierUpgradeCosts size [" + nextTierUpgradeCosts.Count + "]");
            if (nextTierUpgradeCosts.Count == 2)
            {
                baseUpgradeCost_1.GetComponent<ResourceCostPanelV2>().SetInfo(nextTierUpgradeCosts[0]);
                baseUpgradeCost_2.GetComponent<ResourceCostPanelV2>().SetInfo(nextTierUpgradeCosts[1]);
            }
            else
            {
                Debug.LogError("nextTierUpgradeCosts count not equal to 2");
            }
        }
        else   // Display 3 upgrade buttons
        {
            // Remove upgrade costs from main Button
            baseUpgradeCost_1.SetActive(false);
            baseUpgradeCost_2.SetActive(false);

            List<BuildingManager.SpecializedUpgrade> specializedUpgrades = b.buildingType.specializedUpgrades;
            if(specializedUpgrades.Count == 3)
            {
                finalUpgradeLeftCost_1.GetComponent<ResourceCostPanelV2>().SetInfo(specializedUpgrades[0].upgradeCosts[0]);
                finalUpgradeLeftCost_2.GetComponent<ResourceCostPanelV2>().SetInfo(specializedUpgrades[0].upgradeCosts[1]);

                finalUpgradeCenterCost_1.GetComponent<ResourceCostPanelV2>().SetInfo(specializedUpgrades[1].upgradeCosts[0]);
                finalUpgradeCenterCost_2.GetComponent<ResourceCostPanelV2>().SetInfo(specializedUpgrades[1].upgradeCosts[1]);

                finalUpgradeRightCost_1.GetComponent<ResourceCostPanelV2>().SetInfo(specializedUpgrades[2].upgradeCosts[0]);
                finalUpgradeRightCost_2.GetComponent<ResourceCostPanelV2>().SetInfo(specializedUpgrades[2].upgradeCosts[1]);
            }
            else
            {
                Debug.LogError("specializedUpgrades count not equal to 3");
            }
        }
    }

    public void DisplayUpgradeButtonsBasedOnCurrentTier(int currentTier)
    {
        Debug.Log("DisplayUpgradeButtonsBasedOnCurrentTier | Tier [" + currentTier + "]");
        if(currentTier < 3)
        {
            firstTiersUpgradeButtonsPanel.SetActive(true);
            advancedUpgradeButtonsPanel.SetActive(false);
        }
        else
        {
            firstTiersUpgradeButtonsPanel.SetActive(false);
            advancedUpgradeButtonsPanel.SetActive(true);
        }
    }

    public void EmptyUpgradeCostPanelsList()
    {
        foreach (GameObject upgradeCostPanel in upgradeCostPanelsList)
        {
            Destroy(upgradeCostPanel);
        }
        upgradeCostPanelsList = new List<GameObject>();
    }

    public void SetInfo()
    {
        SetImage();
        SetName();
        SetTierText();
        BuildUpgradesLayout();      // TO REDO
        UpdateUpgradeText();
        SetBuildingStats(); // TO REDO
        SetEnergyIndicators();
    }

    public void SetSelectedBuilding(GameObject building)
    {
        selectedBuilding = building;
    }

    public void SetBuildingStats()  // TO REDO
    {
        //BuildingStatsPanel.instance.BuildStatsInfo(selectedBuilding);
        SetBaseBuildingStats();
        SetSpecificBuildingStats();
    }

    public void SetBaseBuildingStats()
    {
        Building b = selectedBuilding.GetComponent<Building>();
        // Health
        healthPointsText.text = b.healthPoints.ToString();
        // Shield
        shieldPointsText.text = b.shieldPoints.ToString();
        // Range
        rangePointsIndicator.SetActive(b.buildingType.hasRange);
        rangePointsText.text = b.range.ToString();
    }

    public void SetSpecificBuildingStats()
    {
        specificStat_1.SetActive(false);
        specificStat_2.SetActive(false);
        Building b = selectedBuilding.GetComponent<Building>();
        if (b.buildingType.specificStats.Count >= 1)
        {
            List<Building.BuildingStat> specStats = b.buildingType.specificStats;
            specificStat_1.SetActive(true);
            specificStat_1_value.text = b.GetBuildingStatValue(specStats[0]).ToString();
            specificStat_1_image.sprite = specStats[0].statImage;
            if (b.buildingType.specificStats.Count >= 2)
            {
                specificStat_2.SetActive(true);
                specificStat_2_value.text = b.GetBuildingStatValue(specStats[1]).ToString();
                specificStat_2_image.sprite = specStats[1].statImage;
            }
        }
    }

    public void SetEnergyIndicators()
    {
        /*if (selectedBuilding.GetComponent<Building>().powerOn)  // Power On                   // TO REDO //
        {
            powerOnPanel.GetComponent<Image>().color = powerOnColor;
        }
        else  // Power Off
        {
            powerOnPanel.GetComponent<Image>().color = powerOffColor;
        }

        if (selectedBuilding.GetComponent<Building>().hasEnoughEnergy)  // Enough energy
        {
            enoughEnergyPanel.GetComponent<Image>().color = enoughEnergyColor;
        }
        else // Not enough energy
        {
            enoughEnergyPanel.GetComponent<Image>().color = notEnoughEnergyColor;
        }*/

        energyConsumptionProductionText.text = (selectedBuilding.GetComponent<PowerPlant>() != null) ? ("+" + selectedBuilding.GetComponent<PowerPlant>().effectiveEnergyProduction) : ("-" + selectedBuilding.GetComponent<Building>().energyConsumption);
    }

    public void DisplayInfo(bool display)
    {
        displayPanel.SetActive(display);
    }

    public void UpgradeButtonClicked()
    {
        Debug.Log("UpgradeButtonClicked");
        if(selectedBuilding.GetComponent<Building>().currentTier <= 2)
        {
            InfrastructureManager.instance.UpgradeBuildingRequest(selectedBuilding.gameObject);
        }
        else
        {
            DisplayAdvancedUpgradePanel(true);
        }
    }

    public void OnAdvancedUpgradeLeftButtonClick()
    {
        Debug.Log("OnAdvancedUpgradeLeftButtonClick");
        if (selectedBuilding.GetComponent<Building>().currentTier == 3)
        {
            InfrastructureManager.instance.UpgradeBuildingRequest(selectedBuilding.gameObject, 1);
        }
        else
        {
            Debug.LogError("OnAdvancedUpgradeLeftButtonClick | Wrong currentTier [" + selectedBuilding.GetComponent<Building>().currentTier + "]");
        }
    }

    public void OnAdvancedUpgradeCenterButtonClick()
    {
        Debug.Log("OnAdvancedUpgradeCenterButtonClick");
        if (selectedBuilding.GetComponent<Building>().currentTier == 3)
        {
            InfrastructureManager.instance.UpgradeBuildingRequest(selectedBuilding.gameObject, 2);
        }
        else
        {
            Debug.LogError("OnAdvancedUpgradeLeftButtonClick | Wrong currentTier [" + selectedBuilding.GetComponent<Building>().currentTier + "]");
        }
    }

    public void OnAdvancedUpgradeRightButtonClick()
    {
        Debug.Log("OnAdvancedUpgradeRightButtonClick");
        if (selectedBuilding.GetComponent<Building>().currentTier == 3)
        {
            InfrastructureManager.instance.UpgradeBuildingRequest(selectedBuilding.gameObject, 3);
        }
        else
        {
            Debug.LogError("OnAdvancedUpgradeLeftButtonClick | Wrong currentTier [" + selectedBuilding.GetComponent<Building>().currentTier + "]");
        }
    }

    public void HideAllUpgradeElements()    // Used when building can't be upgraded anymore
    {
        firstTiersUpgradeButtonsPanel.SetActive(false);
        advancedUpgradeButtonsPanel.SetActive(false);
    }

    public void DisplayBaseUpgradePanel(bool display)
    {
        firstTiersUpgradeButtonsPanel.SetActive(display);
    }

    public void DisplayAdvancedUpgradePanel(bool display)
    {
        advancedUpgradeButtonsPanel.SetActive(display);
    }

    public void BuildingTouched(GameObject building)
    {
        DisplayInfo(true);
        SetSelectedBuilding(building);
        SetInfo();
        OpenCloseBuildingInfoPanel(true);
    }

    // Not used anymore
    /*public void Deselection()
    {
        DisplayInfo(false);
        if(selectedBuilding != null)
        {
            selectedBuilding.GetComponent<Building>().BuildingDeselected();
        }
    }*/

    public void UpdateResourceAvailabilityIndicators()
    {
        bool upgradeResourcesAvailable = true;
        foreach (GameObject upgradeCostPanel in upgradeCostPanelsList)
        {
            upgradeCostPanel.GetComponent<ResourceCostPanel>().UpdateResourceAvailabilityIndicator();
            if (! upgradeCostPanel.GetComponent<ResourceCostPanel>().resourceAvailable)
            {
                upgradeResourcesAvailable = false;
            }
        }

        CalculateUpgradeAvailablility(upgradeResourcesAvailable);
    }

    public void CalculateUpgradeAvailablility(bool upgradeResourcesAvailable)
    {
        bool upgradeAvailable = (upgradeResourcesAvailable && !GetMaxUpgradeLevelReached());
        UpgradeUpdateButtonColorAndText(upgradeAvailable);
    }

    public bool GetMaxUpgradeLevelReached()
    {
        if (selectedBuilding != null)
        {
            return selectedBuilding.GetComponent<Building>().maxUpgradeLevelReached;
        }
        else
        {
            return false;
        }
    }

    public void UpdateInfo()
    {
        UpdateResourceAvailabilityIndicators();
    }

    public void UpgradeUpdateButtonColorAndText(bool upgradeAvailable)  // TO REDO
    {
        //upgradeButton.GetComponent<Image>().color = (upgradeAvailable) ? upgradePossibleColor : upgradeImpossibleColor;
        //upgradeButtonBorder.GetComponent<Image>().sprite = (GetMaxUpgradeLevelReached()) ? null : upgradeAvailableSprite;
        //upgradeButtonBorder.GetComponent<Image>().color = (GetMaxUpgradeLevelReached()) ? transparentColor : fullWhiteColor;
        //upgradeText.text = (GetMaxUpgradeLevelReached()) ? "Max level" : "Upgrade";
    }

    public void DestroyButtonClicked()
    {
        InfrastructureManager.instance.DestroyBuilding(selectedBuilding);
        DisplayInfo(false);
    }

    // Not used anymore
    /*public void CloseButtonClicked()
    {
        GameManager.instance.ChangeSelectionState(GameManager.SelectionState.Default);
        OpenCloseBuildingInfoPanel(false);
    }*/

    public void PowerSwitchButtonClicked()
    {
        Debug.Log("PowerSwitchButtonClicked");
        selectedBuilding.GetComponent<Building>().PowerSwitch();
    }

    public void OpenCloseBuildingInfoPanel(bool open)
    {
        Debug.Log("OpenCloseBuildingInfoPanel | Open [" + open + "]");
        Animator anim = GetComponent<Animator>();
        if(open && !isPanelOpen)
        {
            anim.SetTrigger("open");
            isPanelOpen = true;
        }
        else if(!open && isPanelOpen)
        {
            anim.SetTrigger("close");
            isPanelOpen = false;
        }
    }
}
