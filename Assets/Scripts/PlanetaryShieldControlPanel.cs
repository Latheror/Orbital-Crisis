using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlanetaryShieldControlPanel : MonoBehaviour {

    public static PlanetaryShieldControlPanel instance;
    void Awake()
    {
        if (instance != null) { Debug.LogError("More than one TheShieldControlPanel in scene !"); return; }
        instance = this;
    }

    [Header("UI")]
    public GameObject displayPanel;
    public Slider radiusSlider;
    public Slider powerSlider;
    public TextMeshProUGUI energyConsumptionText;
    public TextMeshProUGUI radiusValueText;
    public TextMeshProUGUI powerValueText;
    public Image viewModeDisplayImage;
    public Sprite ViewModeFullImage;
    public Sprite ViewModePartialImage;
    public Sprite ViewModeHiddenImage;

    [Header("Operation")]
    public GameObject theShield;
    public float shieldRadius;
    public float shieldPower;
    public PlanetaryShield.ViewMode currentViewMode = PlanetaryShield.ViewMode.Full;

    public bool disableSlidersTrigger = false;

    public void DisplayPanel(bool display)
    {
        displayPanel.SetActive(display);
    }

    public void DisableSlidersTrigger(bool disable)
    {
        disableSlidersTrigger = disable;
    }

    public void SubmitRadiusSliderSetting()
    {
        if(! disableSlidersTrigger)
        {
            Debug.Log("SubmitRadiusSliderSetting");
            shieldRadius = radiusSlider.value;
            SendSettings();
            UpdateRadiusValueDisplay();
        }
    }

    public void SubmitPowerSliderSetting()
    {
        if (! disableSlidersTrigger)
        {
            shieldPower = powerSlider.value;
            SendSettings();
            UpdatePowerValueDisplay();
        }
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
        radiusSlider.value = shieldRadius;
    }

    public void UpdatePowerValueDisplay()
    {
        powerValueText.text = (Mathf.CeilToInt(shieldPower)).ToString();
        powerSlider.value = shieldPower;
    }

    public void UpdateCurrentViewModeDisplay()
    {
        switch (currentViewMode)
        {
            case PlanetaryShield.ViewMode.Full:
            {
                viewModeDisplayImage.sprite = ViewModeFullImage;
                break;
            }
            case PlanetaryShield.ViewMode.Partial:
            {
                viewModeDisplayImage.sprite = ViewModePartialImage;
                break;
            }
            case PlanetaryShield.ViewMode.Hidden:
            {
                viewModeDisplayImage.sprite = ViewModeHiddenImage;
                break;
            }
        }
    }


    public void SendSettings()
    {
        if(theShield != null && theShield.GetComponent<PlanetaryShield>() != null)
        {
            Debug.Log("TheShieldControlPanel | SendSettings");
            theShield.GetComponent<PlanetaryShield>().ReceiveSettings(shieldRadius, shieldPower, currentViewMode);
        }
    }

    public void ReceiveSettingsFromShield(float radius, float damagePower, int energyConsumption, PlanetaryShield.ViewMode viewMode)
    {
        UpdateEnergyConsumptionDisplay(energyConsumption);

        shieldRadius = radius;
        shieldPower = damagePower;
        currentViewMode = viewMode;

        DisableSlidersTrigger(true);
        UpdateRadiusValueDisplay();
        UpdatePowerValueDisplay();
        DisableSlidersTrigger(false);

        UpdateCurrentViewModeDisplay();
    }

    public void ViewModeButtonClicked()
    {
        currentViewMode = GetNextViewMode(currentViewMode);
        SendSettings();
        UpdateCurrentViewModeDisplay();
    }

    public PlanetaryShield.ViewMode GetNextViewMode(PlanetaryShield.ViewMode currentViewMode)
    {
        PlanetaryShield.ViewMode nextViewMode = PlanetaryShield.ViewMode.Full;
        switch (currentViewMode)
        {
            case PlanetaryShield.ViewMode.Full:
            {
                nextViewMode = PlanetaryShield.ViewMode.Partial;
                break;
            }
            case PlanetaryShield.ViewMode.Partial:
            {
                nextViewMode = PlanetaryShield.ViewMode.Hidden;
                break;
            }
            case PlanetaryShield.ViewMode.Hidden:
            {
                nextViewMode = PlanetaryShield.ViewMode.Full;
                break;
            }
        }
        return nextViewMode;
    }

}
