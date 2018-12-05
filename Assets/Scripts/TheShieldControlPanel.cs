using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TheShieldControlPanel : MonoBehaviour {

    public static TheShieldControlPanel instance;
    void Awake()
    {
        if (instance != null) { Debug.LogError("More than one TheShieldControlPanel in scene !"); return; }
        instance = this;
    }

    public GameObject displayPanel;
    public GameObject theShield;

    public float shieldRadius;
    public Slider radiusSlider;

    public TextMeshProUGUI energyConsumptionText;
    public TextMeshProUGUI radiusValueText;


    public void DisplayPanel(bool display)
    {
        displayPanel.SetActive(display);
    }

	public void SubmitSliderSetting()
    {
        shieldRadius = radiusSlider.value;
        SendSettings();
        UpdateRadiusValueDisplay();
    }

    public void CloseButtonClicked()
    {
        DisplayPanel(false);
    }

    public void UpdateEnergyConsumptionDisplay(int energyConsumption)
    {
        energyConsumptionText.text = energyConsumption.ToString();
    }

    public void UpdateRadiusValueDisplay()
    {
        radiusValueText.text = (Mathf.CeilToInt(shieldRadius)).ToString();
    }


    public void SendSettings()
    {
        if(theShield != null && theShield.GetComponent<PlanetaryShield>() != null)
        {
            Debug.Log("TheShieldControlPanel | SendSettings");
            theShield.GetComponent<PlanetaryShield>().ReceiveSettings(shieldRadius);
        }
    }

    public void ReceiveSettingsFromShield(int energyConsumption)
    {
        UpdateEnergyConsumptionDisplay(energyConsumption);
    }

}
