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


    public void DisplayPanel(bool display)
    {
        displayPanel.SetActive(display);
    }

    public void SubmitCollectionPointNbSliderSetting()
    {
        collectionPointNb = (int)collectionPointNbSlider.value;
        SendSettings();
        UpdateCollectionPointNbDisplay();
    }

    public void SubmitPowerSliderSetting()
    {
        collectionSpeed = collectionSpeedSlider.value;
        SendSettings();
        UpdateCollectionSpeedValueDisplay();
    }

    public void SendSettings()
    {
        MegaCollector.instance.Configure(collectionSpeed, collectionPointNb);
    }

    public void UpdateCollectionPointNbDisplay()
    {
        collectionPointNbText.text = collectionPointNb.ToString();
    }

    public void UpdateCollectionSpeedValueDisplay()
    {
        collectionSpeedText.text = collectionSpeed.ToString();
    }

    public void UpdateEnergyConsumptionValueDisplay()
    {
        energyConsumptionText.text = energyConsumption.ToString();
    }

    public void ReceiveSettings(float collectionSpeedSetting, int collectionPointNbSetting, float energyConsumptionSetting)
    {
        collectionSpeed = collectionSpeedSetting;
        collectionPointNb = collectionPointNbSetting;
        energyConsumption = energyConsumptionSetting;
        UpdatePanel();
    }

    public void UpdatePanel()
    {
        UpdateCollectionPointNbDisplay();
        UpdateCollectionSpeedValueDisplay();
        UpdateEnergyConsumptionValueDisplay();
    }

    public void CloseButtonClicked()
    {
        DisplayPanel(false);
    }

}
