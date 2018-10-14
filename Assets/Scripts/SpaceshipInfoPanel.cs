﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class SpaceshipInfoPanel : MonoBehaviour {

    public static SpaceshipInfoPanel instance;
    void Awake()
    {
        if (instance != null) { Debug.LogError("More than one SpaceshipInfoPanel in scene !"); return; }
        instance = this;
    }

    public GameObject displayPanel;
    public GameObject spaceshipImageUI;
    public TextMeshProUGUI healthNumbersText;
    public TextMeshProUGUI shieldNumbersText;
    public GameObject modeButton;
    public TextMeshProUGUI modeButtonText;

    public Color autoModeColor = Color.green;
    public Color manualModeColor = Color.red;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void SetHealthText(int healthPoints, int maxHealthPoints)
    {
        Debug.Log("SetHealthText: " + healthPoints + " / " + maxHealthPoints);
        healthNumbersText.text = (healthPoints + "/" + maxHealthPoints);
    }

    void SetShieldText(int shieldPoints, int maxShieldPoints)
    {
        shieldNumbersText.text = (shieldPoints + "/" + maxShieldPoints);
    }

    void UpdateHealthInfo()
    {
        SetHealthText((int)SpaceshipManager.instance.selectedSpaceship.GetComponent<Spaceship>().health, (int)SpaceshipManager.instance.selectedSpaceship.GetComponent<Spaceship>().maxHealth);
    }

    void UpdateShieldInfo()
    {
        SetShieldText((int)SpaceshipManager.instance.selectedSpaceship.GetComponent<Spaceship>().shield, (int)SpaceshipManager.instance.selectedSpaceship.GetComponent<Spaceship>().maxShield);
    }

    public void UpdateModeDisplay()
    {
        if(SpaceshipManager.instance.selectedSpaceship != null)
        {
            if (SpaceshipManager.instance.selectedSpaceship.GetComponent<Spaceship>().isInAutomaticMode)
            {
                modeButtonText.text = "AUTO";
                modeButton.GetComponent<Image>().color = autoModeColor;
            }
            else
            {
                modeButtonText.text = "MANUAL";
                modeButton.GetComponent<Image>().color = manualModeColor;
            }
        }
    }

    public void SelectSpaceshipActions()
    {
        Debug.Log("SelectSpaceshipActions");

        // Set all information
        UpdateInfo();

        // Display all information
        displayPanel.SetActive(true);
    }

    public void DeselectSpaceshipActions()
    {
        Debug.Log("DeselectSpaceshipActions");
        displayPanel.SetActive(false);
    }

    public void UpdateInfo()
    {
        if(SpaceshipManager.instance.selectedSpaceship != null)
        {
            Debug.Log("Update Spaceship panel info");
            UpdateHealthInfo();
            UpdateShieldInfo();
            UpdateModeDisplay();
        }
        else
        {
            Debug.LogError("SetInfo: No selected Spaceship !");
        }
    }

    public void ModeButtonClicked()
    {
        SpaceshipManager.instance.selectedSpaceship.GetComponent<Spaceship>().SwitchMode();
        UpdateModeDisplay();
    }
}