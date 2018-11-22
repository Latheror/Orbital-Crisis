using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class EnemyInfoPanel : MonoBehaviour {

    public static EnemyInfoPanel instance;
    void Awake()
    {
        if (instance != null) { Debug.LogError("More than one EnemyInfoPanel in scene !"); return; }
        instance = this;
    }

    public GameObject displayPanel;
    public GameObject enemyImageUI;
    public TextMeshProUGUI healthNumbersText;
    public TextMeshProUGUI shieldNumbersText;
    public GameObject modeButton;
    public TextMeshProUGUI modeButtonText;
    public GameObject modePanel;

    void SetHealthText(int healthPoints, int maxHealthPoints)
    {
        //Debug.Log("SetHealthText: " + healthPoints + " / " + maxHealthPoints);
        healthNumbersText.text = (healthPoints + "/" + maxHealthPoints);
    }

    void UpdateHealthInfo(int health)
    {
        SetHealthText(health, 100);
    }

    public void SelectEnemyActions()
    {
        //Debug.Log("SelectSpaceshipActions");

        // Set all information
        UpdateInfo();

        // Display all information
        DisplayPanel(true);
    }

    public void DeselectEnemyActions()
    {
        //Debug.Log("DeselectSpaceshipActions");
        displayPanel.SetActive(false);
    }

    public void UpdateInfo()
    {
        if (EnemiesManager.instance.selectedEnemy != null)    
        {
            // Enemy Spaceship
            if(EnemiesManager.instance.selectedEnemy.GetComponent<EnemySpaceship>() != null)
            {
                float health = EnemiesManager.instance.selectedEnemy.GetComponent<EnemySpaceship>().healthPoints;
                //Debug.Log("Update Spaceship panel info");
                UpdateHealthInfo((int)health);
                //UpdateShieldInfo();
                //UpdateModeDisplay();
            }
        }
        else
        {
            //Debug.Log("SetInfo: No selected Spaceship !");
        }
    }

    public void CloseButtonClicked()
    {
        GameManager.instance.ChangeSelectionState(GameManager.SelectionState.Default);

        DisplayPanel(false);
    }

    public void DisplayPanel(bool display)
    {
        displayPanel.SetActive(display);
    }
}
