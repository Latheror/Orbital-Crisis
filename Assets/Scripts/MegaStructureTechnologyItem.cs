using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MegaStructureTechnologyItem : MonoBehaviour {

    public GameObject experienceCostPanel;
    public GameObject artifactCostPanel;

    public TextMeshProUGUI nameText;
    public TextMeshProUGUI experienceCostText;
    public TextMeshProUGUI artifactCostText;

    public GameObject button;

    public List<GameObject> outputConnections;

    public TechTreeManager.Technology associatedTechnology;


    public void InitializeUIElements()
    {
        if(associatedTechnology != null)
        {
            if (!associatedTechnology.isFinalMegaStructureTechnology)
            {
                SetNameText(associatedTechnology.name);
                SetExperienceCostText(associatedTechnology.experienceCost);
                SetArtifactCostText(associatedTechnology.artifactCost);
            }
            UpdatePanelDisplay();
        }
        else
        {
            Debug.LogError("InitializeUIElements | AssociatedTechnology is null !");
        }
    }

    public void SetNameText(string name) { nameText.text = name; }
    public void SetExperienceCostText(int experiencePoints) { experienceCostText.text = experiencePoints.ToString(); }
    public void SetArtifactCostText(int artifactsNb) { artifactCostText.text = artifactsNb.ToString(); }

    public void ButtonClicked()
    {
        Debug.Log("MegaStructureTechnologyItem | ButtonClicked [" + associatedTechnology.name + "]");
        if(TechTreeManager.instance.CanPayTechnology(associatedTechnology) && !associatedTechnology.unlocked)
        {
            TechTreeManager.instance.PayTechnology(associatedTechnology);

            TechTreeManager.instance.UnlockTechnology(associatedTechnology);
        }
    }

    public void UpdateItemDisplay()
    {
        UpdatePanelDisplay();
    }

    public void UpdatePanelDisplay()
    {
        bool outputConnectionEnabled = false;

        if (!associatedTechnology.isFinalMegaStructureTechnology)
        {
            if (associatedTechnology.unlocked)
            {
                button.GetComponent<Image>().sprite = TechTreeManager.instance.unlockedTechnoUIBorder;
                outputConnectionEnabled = true;
                DisplayCostPanels(false);
            }
            else
            {
                if (TechTreeManager.instance.CanPayTechnology(associatedTechnology))
                {
                    button.GetComponent<Image>().sprite = TechTreeManager.instance.notUnlockedTechnoUIBorder;
                }
                else
                {
                    button.GetComponent<Image>().sprite = TechTreeManager.instance.tooExpensiveTechnoUIBorder;
                }
            }

            // Experience cost text
            experienceCostText.color = (TechTreeManager.instance.CanPayExperienceCost(associatedTechnology)) ? MegaStructureManager.instance.canPayColor : MegaStructureManager.instance.cantPayColor;

            // Artifact cost text
            artifactCostText.color = (TechTreeManager.instance.CanPayArtifactCost(associatedTechnology)) ? MegaStructureManager.instance.canPayColor : MegaStructureManager.instance.cantPayColor;
        }
        else
        {
            if (associatedTechnology.unlocked)
            {

            }
            else
            {
                if (associatedTechnology.available)
                {
                    GetComponent<Image>().color = MegaStructuresPanel.instance.finalMegaStructureAvailableColor;
                    switch (associatedTechnology.id)
                    {
                        case 16:
                        {
                            Debug.Log("Planetary shield available !");
                            MegaStructuresPanel.instance.activateShieldTextGo.SetActive(true);
                            break;
                        }
                        case 20:
                        {
                            MegaStructuresPanel.instance.activateCollectorTextGo.SetActive(true);
                            break;
                        }
                        default:
                            break;
                    }
                }
                else
                {

                }
            }
        }

        if (outputConnectionEnabled)
        {
            foreach (GameObject outputConnection in outputConnections)
            {
                outputConnection.GetComponent<Image>().sprite = TechTreeManager.instance.enabledConnectionSprite;
            }
        }
        else
        {
            foreach (GameObject outputConnection in outputConnections)
            {
                outputConnection.GetComponent<Image>().sprite = TechTreeManager.instance.disabledConnectionSprite;
            }
        }
    }

    public void DisplayCostPanels(bool display)
    {
        experienceCostPanel.SetActive(display);
        artifactCostPanel.SetActive(display);
    }

}
