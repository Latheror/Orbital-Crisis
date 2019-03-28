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
    public TextMeshProUGUI tierNbText;
    public TextMeshProUGUI upgradeText;
    public GameObject enoughEnergyPanel;
    public GameObject powerOnPanel;
    public Sprite upgradeAvailableSprite;
    public Sprite upgradeNotAvailableSprite;

    // Useful / New
    public GameObject firstTiersButtonsPanel;
    public GameObject lastTiersButtonsPanel;
    public TextMeshProUGUI energyConsumptionProductionText;

    public GameObject baseUpgradeCost_1;
    public GameObject baseUpgradeCost_2;

    public GameObject finalUpgradeLeftCost_1;
    public GameObject finalUpgradeLeftCost_2;
    public GameObject finalUpgradeCenterCost_1;
    public GameObject finalUpgradeCenterCost_2;
    public GameObject finalUpgradeRightCost_1;
    public GameObject finalUpgradeRightCost_2;

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


    public void BuildUpgradesLayout()   // TO REDO
    {
        //Debug.Log("BuildUpgradeCostsLayout");
        /*EmptyUpgradeCostPanelsList();

        List<ResourcesManager.ResourceAmount> resourceAmounts = selectedBuilding.GetComponent<Building>().GetUpgradeCostsForNextTier();
        //Debug.Log("Upgrade costs nb: " + resourceAmounts.Count);

        foreach (ResourcesManager.ResourceAmount resourceAmount in resourceAmounts)
        {
            //Debug.Log("Adding resource indicator to Upgrade panel: " + resourceAmount.resourceType.resourceName);

            //GameObject instantiatedUpgradeCostPanel = Instantiate(upgradeCostPanelPrefab, new Vector3(0f,0f,0f), Quaternion.identity);
        //instantiatedUpgradeCostPanel.transform.SetParent(upgradeCostsLayout.transform, false);

        // Customize CostPanel
        //instantiatedUpgradeCostPanel.GetComponent<ResourceCostPanel>().SetInfo(resourceAmount);

        //upgradeCostPanelsList.Add(instantiatedUpgradeCostPanel);
        /*}

        UpdateInfo();*/

        // --- NEW VERSION --- //
        SetUpgradeCosts();

        DisplayUpgradeButtonsBasedOnCurrentTier(selectedBuilding.GetComponent<Building>().currentTier);

    }

    public void SetUpgradeCosts()
    {
        Building b = selectedBuilding.GetComponent<Building>();
        if (selectedBuilding.GetComponent<Building>().currentTier < 3)   // Display a single upgrade button
        {
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
            firstTiersButtonsPanel.SetActive(true);
            lastTiersButtonsPanel.SetActive(false);
        }
        else
        {
            firstTiersButtonsPanel.SetActive(false);
            lastTiersButtonsPanel.SetActive(true);
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
    }

    public void SetEnergyIndicators()
    {
        if (selectedBuilding.GetComponent<Building>().powerOn)  // Power On
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
        }

        //energyConsumptionProductionText.text = selectedBuilding.GetComponent<Building>().energ
    }

    public void DisplayInfo(bool display)
    {
        displayPanel.SetActive(display);
    }

    public void UpgradeButtonClicked()
    {
        Debug.Log("UpgradeButtonClicked");
        InfrastructureManager.instance.UpgradeBuildingRequest(selectedBuilding.gameObject);
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
