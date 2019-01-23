using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CollectorControlPanel : MonoBehaviour {

    public static CollectorControlPanel instance;
    void Awake()
    {
        if (instance != null) { Debug.LogError("More than one CollectorControlPanel in scene !"); return; }
        instance = this;
    }

    [Header("UI")]
    public GameObject displayPanel;
    public Slider collectionPointNbSlider;
    public Slider collectionSpeedSlider;
    public TextMeshProUGUI collectionSpeedText;
    public TextMeshProUGUI collectionPointNbText;
    public TextMeshProUGUI energyConsumptionText;

    [Header("Operation")]
    public int collectionPointNb = 1;
    public float collectionSpeed = 10f;
    public float energyConsumption = 0f;

    public bool disableSlidersTrigger = false;


    public void DisplayPanel(bool display)
    {
        displayPanel.SetActive(display);
    }

    public void SubmitCollectionPointNbSliderSetting()
    {
        if(!disableSlidersTrigger)
        {
            Debug.Log("SubmitCollectionPointNbSliderSetting");
            collectionPointNb = (int)collectionPointNbSlider.value;
            SendSettings();
            UpdateCollectionPointNbDisplay();
        }
    }

    public void SubmitPowerSliderSetting()
    {
        if (!disableSlidersTrigger)
        {
            Debug.Log("SubmitPowerSliderSetting");
            collectionSpeed = collectionSpeedSlider.value;
            SendSettings();
            UpdateCollectionSpeedValueDisplay();
        }
    }

    public void DisableSlidersTrigger(bool disable)
    {
        disableSlidersTrigger = disable;
    }

    public void SendSettings()
    {
        MegaCollector.instance.Configure(collectionSpeed, collectionPointNb);
    }

    public void UpdateCollectionPointNbDisplay()
    {
        collectionPointNbText.text = collectionPointNb.ToString();
        collectionPointNbSlider.value = collectionPointNb;
    }

    public void UpdateCollectionSpeedValueDisplay()
    {
        collectionSpeedText.text = collectionSpeed.ToString();
        collectionSpeedSlider.value = collectionSpeed;
    }

    public void UpdateEnergyConsumptionValueDisplay()
    {
        energyConsumptionText.text = energyConsumption.ToString();
    }

    public void ReceiveSettings(float collectionSpeedSetting, int collectionPointNbSetting, float energyConsumptionSetting)
    {
        Debug.Log("Collector Control Panel | ReceiveSettings | CollectionSpeed [" + collectionSpeedSetting + "] | CollectionPointsNb [" + collectionPointNbSetting + "] | EnergyConsumption [" + energyConsumptionSetting + "]");
        collectionSpeed = collectionSpeedSetting;
        collectionPointNb = collectionPointNbSetting;
        energyConsumption = energyConsumptionSetting;
        UpdatePanel();
    }

    public void UpdatePanel()
    {
        DisableSlidersTrigger(true);
        UpdateCollectionPointNbDisplay();
        UpdateCollectionSpeedValueDisplay();
        DisableSlidersTrigger(false);

        UpdateEnergyConsumptionValueDisplay();
    }

    public void CloseButtonClicked()
    {
        DisplayPanel(false);
    }

}
