using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TechnologyItem : MonoBehaviour {

    public TechTreeManager.Technology associatedTechnology;
    public TextMeshProUGUI experienceCostText;
    public GameObject experienceCostPanel;

    public GameObject button;
    public GameObject background;
    public GameObject image;

    public List<GameObject> outputConnections;

	void Start () {
		
	}

    void Update () {
		
	}

    public void UnlockButtonClicked()
    {
        Debug.Log("UnlockButtonClicked [" + associatedTechnology.name + "]");
        TechTreeManager.instance.UnlockTechnologyRequest(associatedTechnology);
    }

    public void SetExperienceCostText(int expPoints)
    {
        experienceCostText.text = expPoints.ToString();
    }

    public void InitializeUIElements()
    {
        experienceCostText.text = associatedTechnology.experienceCost.ToString();
        UpdateItemDisplay();
    }

    public void UpdateItemDisplay()
    {
        Debug.Log("UpdateItemDisplay | Available [" + associatedTechnology.available + "] | Unlocked [" + associatedTechnology.unlocked + "]");

        bool outputConnectionEnabled = false;

        if (associatedTechnology.available)
        {
            image.GetComponent<Image>().color = TechTreeManager.instance.availableColor;

            if (associatedTechnology.unlocked)
            {
                button.GetComponent<Image>().sprite = TechTreeManager.instance.unlockedTechnoUIBorder;
                outputConnectionEnabled = true;

                // Stop displaying the cost panel
                experienceCostPanel.SetActive(false);
            }
            else
            {
                button.GetComponent<Image>().sprite = TechTreeManager.instance.notUnlockedTechnoUIBorder;
            }
        }
        else
        {
            image.GetComponent<Image>().color = TechTreeManager.instance.unavailableColor;
            button.GetComponent<Image>().sprite = TechTreeManager.instance.unavailableTechnoUIBorder;
        }

        if(outputConnectionEnabled)
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

        UpdateCostDisplay();
    }

    public void UpdateCostDisplay()
    {
        if (TechTreeManager.instance.CanPayTechnology(associatedTechnology))
        {
            experienceCostPanel.GetComponent<Image>().color = TechTreeManager.instance.canPayTechnologyColor;
        }
        else
        {
            experienceCostPanel.GetComponent<Image>().color = TechTreeManager.instance.cantPayTechnologyColor;
        }
    }
}
