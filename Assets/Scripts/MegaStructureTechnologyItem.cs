using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MegaStructureTechnologyItem : MonoBehaviour {

    public TextMeshProUGUI nameText;
    public TextMeshProUGUI experienceCostText;
    public TextMeshProUGUI artifactCostText;

    public List<GameObject> outputConnections;

    public TechTreeManager.Technology associatedTechnology;


    public void InitializeUIElements()
    {
        if(associatedTechnology != null)
        {
            SetNameText(associatedTechnology.name);
            SetExperienceCostText(associatedTechnology.experienceCost);
            SetArtifactCostText(associatedTechnology.artifactCost);
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
        if(TechTreeManager.instance.CanPayTechnology(associatedTechnology))
        {
            TechTreeManager.instance.PayTechnology(associatedTechnology);
        }
    }

}
