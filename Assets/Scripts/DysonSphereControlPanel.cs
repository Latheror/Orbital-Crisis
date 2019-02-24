using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DysonSphereControlPanel : MonoBehaviour {

    public static DysonSphereControlPanel instance;
    void Awake()
    {
        if (instance != null) { Debug.LogError("More than one DysonSphereControlPanel in scene !"); return; }
        instance = this;
    }

    public GameObject displayPanel;

    public TextMeshProUGUI currentEnergyProductionText;
    public TextMeshProUGUI currentStructurePointsText;
    public TextMeshProUGUI maxStructurePointsText;

    public GameObject repairCostLayout;
    public GameObject repairCostPrefab;
    public List<GameObject> repairCostIndicators;


    public void ReceiveSettings(DysonSphere.DysonSphereSettings dysonSphereSettings)
    {
        DisplayInfo(dysonSphereSettings);
    }

    public void DisplayInfo(DysonSphere.DysonSphereSettings dysonSphereSettings)
    {
        // Energy / Structure points
        currentEnergyProductionText.text = Mathf.RoundToInt(dysonSphereSettings.energyProduction).ToString();
        currentStructurePointsText.text = Mathf.RoundToInt(dysonSphereSettings.currentStructurePoints).ToString();
        maxStructurePointsText.text = Mathf.RoundToInt(dysonSphereSettings.maxStructurePoint).ToString();

        // Auto Repair State
        SetAutoRepairSwitch(dysonSphereSettings.currentAutoRepairState);

        BuildRepairCostLayout(dysonSphereSettings);
    }

    public void BuildRepairCostLayout(DysonSphere.DysonSphereSettings dysonSphereSettings)
    {
        EmptyRepairCostLayout();

        int structurePointsMissing = Mathf.FloorToInt(dysonSphereSettings.maxStructurePoint - dysonSphereSettings.currentStructurePoints);
        //Debug.Log("BuildRepairCostLayout | Structure Points missing [" + structurePointsMissing +"]");

        if(structurePointsMissing > 0)
        {
            foreach (ResourcesManager.ResourceAmount resourceAmount in DysonSphere.instance.resourceCostPerStructurePointRepair)
            {
                //Debug.Log("BuildRepairCostLayout | Resource [" + resourceAmount.resourceType.resourceName + "] | Amount [" + resourceAmount.amount + "]");

                // UI
                GameObject instantiatedResourceCostPanel = Instantiate(repairCostPrefab, new Vector3(0f,0f,0f), Quaternion.identity);
                instantiatedResourceCostPanel.transform.SetParent(repairCostLayout.transform, false);

                // Multiply resourceAmount amount by StructurePointsMissing
                ResourcesManager.ResourceAmount multipliedResourceAmount = new ResourcesManager.ResourceAmount(resourceAmount.resourceType, resourceAmount.amount * structurePointsMissing);

                instantiatedResourceCostPanel.GetComponent<ResourceCostPanel>().SetInfo(multipliedResourceAmount);

                repairCostIndicators.Add(instantiatedResourceCostPanel);
            }
        }
    }

    public void RepairButtonClicked()
    {
        //Debug.Log("Repair Button Clicked");
        DysonSphere.instance.RepairRequest();
    }

    public void DisplayPanel(bool display)
    {
        displayPanel.SetActive(display);
    }

    public void EmptyRepairCostLayout()
    {
        while(repairCostIndicators.Count > 0)
        {
            GameObject repairCostIndicatorToDelete = repairCostIndicators[0];
            repairCostIndicators.Remove(repairCostIndicatorToDelete);
            Destroy(repairCostIndicatorToDelete);
        }
    }

    public void CloseButtonClicked()
    {
        DisplayPanel(false);
    }

    public void AutoRepairToggleButton()
    {
        DysonSphere.instance.SwitchAutoRepairState();
    }

    public void SetAutoRepairSwitch(bool on)
    {
        //
    }

}
