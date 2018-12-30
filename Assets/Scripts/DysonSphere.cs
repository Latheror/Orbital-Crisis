using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DysonSphere : MonoBehaviour {

    public static DysonSphere instance;
    void Awake(){
        if (instance != null) { Debug.LogError("More than one DysonSphere in scene !"); return; } instance = this;
    }

    [Header("Settings")]
    public float baseEnergyProduction = 2000f;

    [Header("Operation")]
    public bool isUnlocked = false;
    public bool isActivated = false;
    public float currentEnergyProduction = 2000f;


    void Start()
    {
        SetSettings();
    }

    public void SetSettings()
    {
        currentEnergyProduction = baseEnergyProduction;
        EnergyPanel.instance.UpdateEnergyProductionAndConsumption();
    }





}
