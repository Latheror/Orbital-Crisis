using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanetaryShield : MonoBehaviour {

    public static PlanetaryShield instance;
    void Awake()
    {
        if (instance != null) { Debug.LogError("More than one PlanetaryShield in scene !"); return; }
        instance = this;
    }


    public int energyConsumption;
    public float radius;
    public ParticleSystem shieldParticleSystem;

    public void ReceiveSettings(float shieldRadiusSetting)
    {
        Debug.Log("Planetary Shield | ReceiveSettings | ShieldRadius: [" + shieldRadiusSetting + "]");
        SetRadius(shieldRadiusSetting);

        ApplySettings();

        SendSettingsBackToControlPanel();
    }

    public void SendSettingsBackToControlPanel()
    {
        TheShieldControlPanel.instance.ReceiveSettingsFromShield(energyConsumption);
    }

    public void SetRadius(float radiusSetting)
    {
        radius = radiusSetting;
        CalculateEnergyConsumption();
    }

    public void CalculateEnergyConsumption()
    {
        energyConsumption = Mathf.CeilToInt(radius * 10);
        ApplySettings();
    }

    public void ApplySettings()
    {
        // Apply settings to Particle System
        if(shieldParticleSystem != null)
        {
            Debug.Log("Planetary Shield | ApplySettings | Setting radius to [" + radius + "]");
            shieldParticleSystem.startSize = radius;
        }
    }




}
