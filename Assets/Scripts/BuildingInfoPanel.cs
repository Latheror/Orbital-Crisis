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

    public GameObject displayPanel;

    public GameObject buildingImagePanel;
    public GameObject buildingNamePanel;
    public GameObject upgradeButton;
    public GameObject upgradeCostsLayout;
    public GameObject upgradeCostPanelPrefab;
    public List<GameObject> upgradeCostPanelsList;
    public GameObject destroyButton;
    public TextMeshProUGUI tierNbText;

    public GameObject selectedBuilding;

    public Color upgradePossibleColor = Color.green;
    public Color upgradeImpossibleColor = Color.red;

    // Use this for initialization
    void Start () {

    }
	
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


    public void BuildUpgradeCostsLayout()
    {
        //Debug.Log("BuildUpgradeCostsLayout");
        EmptyUpgradeCostPanelsList();

        List<ResourcesManager.ResourceAmount> resourceAmounts = selectedBuilding.GetComponent<Building>().GetUpgradeCostsForNextTier();
        //Debug.Log("Upgrade costs nb: " + resourceAmounts.Count);

        foreach (ResourcesManager.ResourceAmount resourceAmount in resourceAmounts)
        {
            //Debug.Log("Adding resource indicator to Upgrade panel: " + resourceAmount.resourceType.resourceName);

            GameObject instantiatedUpgradeCostPanel = Instantiate(upgradeCostPanelPrefab, upgradeCostsLayout.transform.position, Quaternion.identity);
            instantiatedUpgradeCostPanel.transform.SetParent(upgradeCostsLayout.transform, false);

            // Customize CostPanel
            instantiatedUpgradeCostPanel.GetComponent<ResourceCostPanel>().SetInfos(resourceAmount);

            upgradeCostPanelsList.Add(instantiatedUpgradeCostPanel);
        }

        UpdateInfo();
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
        BuildUpgradeCostsLayout();
    }

    public void SetSelectedBuilding(GameObject building)
    {
        selectedBuilding = building;
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
    }

    public void Deselection()
    {
        DisplayInfo(false);
        if(selectedBuilding != null)
        {
            selectedBuilding.GetComponent<Building>().BuildingDeselected();
        }
    }

    public void UpdateResourceAvailabilityIndicators()
    {
        bool isUpgradeAvailable = true;
        foreach (GameObject upgradeCostPanel in upgradeCostPanelsList)
        {
            upgradeCostPanel.GetComponent<ResourceCostPanel>().UpdateResourceAvailabilityIndicator();
            if (! upgradeCostPanel.GetComponent<ResourceCostPanel>().resourceAvailable)
            {
                isUpgradeAvailable = false;
            }
        }
        UpgradeUpdateButtonColor(isUpgradeAvailable);
    }

    public void UpdateInfo()
    {
        UpdateResourceAvailabilityIndicators();
    }

    public void UpgradeUpdateButtonColor(bool upgradeAvailable)
    {
        upgradeButton.GetComponent<Image>().color = (upgradeAvailable) ? upgradePossibleColor : upgradeImpossibleColor;
    }

    public void DestroyButtonClicked()
    {
        InfrastructureManager.instance.DestroyBuilding(selectedBuilding);
        DisplayInfo(false);
    }

    public void CloseButtonClicked()
    {
        GameManager.instance.ChangeSelectionState(GameManager.SelectionState.Default);
    }
}
