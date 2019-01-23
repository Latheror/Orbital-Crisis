using System.Collections;
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

    [Header("UI")]
    public GameObject displayPanel;
    public Image spaceshipImage;
    public GameObject modeButton;
    public GameObject modePanel;
    public TextMeshProUGUI spaceshipTypeText;
    public TextMeshProUGUI healthNumbersText;
    public TextMeshProUGUI shieldNumbersText;
    public TextMeshProUGUI modeButtonText;
    public Color autoModeColor = Color.green;
    public Color manualModeColor = Color.red;

    void SetSpaceshipTypeText(AllySpaceship allyS)
    {
        spaceshipTypeText.text = allyS.spaceshipType.typeName;
    }

    void SetHealthText(int healthPoints, int maxHealthPoints)
    {
        //Debug.Log("SetHealthText: " + healthPoints + " / " + maxHealthPoints);
        healthNumbersText.text = (healthPoints + "/" + maxHealthPoints);
    }

    void SetShieldText(int shieldPoints, int maxShieldPoints)
    {
        shieldNumbersText.text = (shieldPoints + "/" + maxShieldPoints);
    }

    void SetSpaceshipImage(AllySpaceship allyS)
    {
        spaceshipImage.sprite = allyS.spaceshipType.sprite;
    }

    void UpdateHealthInfo(AllySpaceship allyS)
    {
        SetHealthText((int)allyS.healthPoints, (int)allyS.maxHealthPoints);
    }

    void UpdateShieldInfo(AllySpaceship allyS)
    {
        SetShieldText((int)allyS.shieldPoints, (int)allyS.maxShieldPoints);
    }

    public void UpdateModeDisplay()
    {
        GameObject selectedS = SpaceshipManager.instance.selectedSpaceship;
        if(selectedS != null)
        {
            AllySpaceship allyS = selectedS.GetComponent<AllySpaceship>();
            if (allyS != null)
            {
                if (allyS.isInAutomaticMode)
                {
                    modeButtonText.text = "AUTO";
                    modePanel.GetComponent<Image>().color = autoModeColor;
                }
                else
                {
                    modeButtonText.text = "MANUAL";
                    modePanel.GetComponent<Image>().color = manualModeColor;
                }
            }
        }

    }

    public void SelectSpaceshipActions()
    {
        //Debug.Log("SelectSpaceshipActions");

        // Set all information
        UpdateInfo();

        // Display all information
        displayPanel.SetActive(true);
    }

    public void DeselectSpaceshipActions()
    {
        //Debug.Log("DeselectSpaceshipActions");
        displayPanel.SetActive(false);
    }

    public void UpdateInfo()
    {
        GameObject selectedSpaceship = SpaceshipManager.instance.selectedSpaceship;
        if (selectedSpaceship != null)
        {
            AllySpaceship allyS = selectedSpaceship.GetComponent<AllySpaceship>();
            if(allyS != null)
            {
                //Debug.Log("Update Spaceship panel info");
                UpdateHealthInfo(allyS);
                UpdateShieldInfo(allyS);
                UpdateModeDisplay();
                SetSpaceshipImage(allyS);
                SetSpaceshipTypeText(allyS);
            }
        }
        else
        {
            //Debug.Log("SetInfo: No selected Spaceship !");
        }
    }

    public void ModeButtonClicked()
    {
        AllySpaceship allyS = SpaceshipManager.instance.selectedSpaceship.GetComponent<AllySpaceship>();
        allyS.SwitchMode();
        UpdateModeDisplay();
    }

    public void CloseButtonClicked()
    {
        GameManager.instance.ChangeSelectionState(GameManager.SelectionState.Default);
    }
}
