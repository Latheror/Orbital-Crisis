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

    public GameObject selectedBuilding;

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

    public void BuildUpgradeCostsLayout()
    {
        Debug.Log("BuildUpgradeCostsLayout");
        EmptyUpgradeCostPanelsList();

        List<ResourcesManager.ResourceAmount> resourceAmounts = selectedBuilding.GetComponent<Building>().GetUpgradeCostsForNextTier();
        Debug.Log("Upgrade costs nb: " + resourceAmounts.Count);

        foreach (ResourcesManager.ResourceAmount resourceAmount in resourceAmounts)
        {
            Debug.Log("Adding resource indicator to Upgrade panel: " + resourceAmount.resourceType.resourceName);

            GameObject instantiatedUpgradeCostPanel = Instantiate(upgradeCostPanelPrefab, upgradeCostsLayout.transform.position, Quaternion.identity);
            instantiatedUpgradeCostPanel.transform.SetParent(upgradeCostsLayout.transform, false);

            // Customize CostPanel
            instantiatedUpgradeCostPanel.GetComponent<ResourceCostPanel>().SetInfos(resourceAmount);

            upgradeCostPanelsList.Add(instantiatedUpgradeCostPanel);
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
    }

    public void BuildingTouched(GameObject building)
    {
        BuildingInfoPanel.instance.DisplayInfo(true);
        BuildingInfoPanel.instance.SetSelectedBuilding(building);
        BuildingInfoPanel.instance.SetInfo();
    }

    public void Deselection()
    {
        DisplayInfo(false);
        if(selectedBuilding != null)
        {
            selectedBuilding.GetComponent<Building>().BuildingDeselected();
        }
    }
}
