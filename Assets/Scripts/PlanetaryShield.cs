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

    [Header("Parts")]
    public ParticleSystem shieldParticleSystem;

    [Header("UI")]
    public Color ViewModeFullColor;
    public Color ViewModePartialColor;
    public Color ViewModeHiddenColor;

    [Header("Operation")]
    public float damagePower = 0f;
    public int energyConsumption;
    public float radius;
    public bool isUnlocked = false;
    public bool hasEnoughEnergy = false;
    public float damageMultiplicationFactor = 2f;

    public enum ViewMode { Full, Hidden, Partial };
    public ViewMode viewMode;

    public void Initialize()
    {
        viewMode = ViewMode.Full;
        CalculateEnergyConsumption();
        ApplySettings();
        SendSettingsBackToControlPanel();
    }

    public void ReceiveSettings(float shieldRadiusSetting, float shieldPowerSetting, ViewMode viewMode)
    {
        Debug.Log("Planetary Shield | ReceiveSettings | ShieldRadius: [" + shieldRadiusSetting + "]");
        SetRadius(shieldRadiusSetting);
        SetPower(shieldPowerSetting);
        SetViewMode(viewMode);

        ApplySettings();

        SendSettingsBackToControlPanel();
    }

    public void SendSettingsBackToControlPanel()
    {
        PlanetaryShieldControlPanel.instance.ReceiveSettingsFromShield(radius, damagePower, energyConsumption, viewMode);
    }

    public void SetRadius(float radiusSetting)
    {
        radius = radiusSetting;
        CalculateEnergyConsumption();
    }

    public void SetPower(float powerSetting)
    {
        damagePower = powerSetting;
        CalculateEnergyConsumption();
    }

    public void SetViewMode(ViewMode viewMode)
    {
        this.viewMode = viewMode;
    }

    public void CalculateEnergyConsumption()
    {
        energyConsumption = Mathf.CeilToInt(radius * 0.5f * damagePower);
        EnergyPanel.instance.UpdateEnergyProductionAndConsumption();
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

        // Set Sphere Collider radius
        GetComponent<SphereCollider>().radius = radius /2;      // Need to divide the radius by 2

        // Apply View Mode
        UpdateShieldApparenceBasedOnViewMode();
    }

    void OnTriggerEnter(Collider other)
    {
        Debug.Log("Shield collided with : " + other.gameObject.tag);

        if(hasEnoughEnergy)
        {
            if (other.gameObject.tag == "meteor")
            {
                if(! other.gameObject.GetComponent<Meteor>().hasAlreadyBeenHitByPlanetaryShield)
                {
                    other.gameObject.GetComponent<Meteor>().hasAlreadyBeenHitByPlanetaryShield = true;
                    other.gameObject.GetComponent<Meteor>().TakeDamage(damagePower * damageMultiplicationFactor);
                }
            }
        }
    }

    public void UpdateShieldApparenceBasedOnViewMode()
    {
        if (shieldParticleSystem != null)
        {
            switch (viewMode)
            {
                case ViewMode.Full:
                {
                    shieldParticleSystem.startColor = ViewModeFullColor;
                    break;
                }
                case ViewMode.Partial:
                {
                    shieldParticleSystem.startColor = ViewModePartialColor;
                    break;
                }
                case ViewMode.Hidden:
                {
                    shieldParticleSystem.startColor = ViewModeHiddenColor;
                    break;
                }
            }
        }
    }

}
