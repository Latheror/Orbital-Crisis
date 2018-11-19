using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class StatInfoPanel : MonoBehaviour {

    public GameObject selectedBuilding;
    public Building.BuildingStat buildingStat;
    public TextMeshProUGUI valueText;
    public Image statImage;

    public void SetSelectedBuildingAndStat(GameObject building, Building.BuildingStat buildingStat)
    {
        selectedBuilding = building;
        this.buildingStat = buildingStat;
    }

    public void SetInfo()
    {
        statImage.sprite = buildingStat.statImage;
        valueText.text = selectedBuilding.GetComponent<Building>().GetBuildingStatValue(buildingStat).ToString();
    }

}
