using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemiesManager : MonoBehaviour {

    public static EnemiesManager instance;
    void Awake()
    {
        if (instance != null) { Debug.LogError("More than one EnemiesManager in scene !"); return; }
        instance = this;
    }

    [Header("Settings")]
    public int meteorPriority = 1;
    public int spaceshipsPriority = 2;

    [Header("Operation")]
    public List<GameObject> enemies;
    public List<GameObject> enemyWrecks;
    public GameObject selectedEnemy;
    public GameObject previouslySelectedEnemy;

    [Header("Enemies")]
    public GameObject enemySpaceship_1;



    public void EnemyTouched(GameObject enemyGO)
    {
        if(selectedEnemy != null && selectedEnemy != enemyGO)
        {
            previouslySelectedEnemy = selectedEnemy;
            // Old selected enemy was an Enemy Spaceship
            if (previouslySelectedEnemy.GetComponent<EnemySpaceship>() != null)
            {
                previouslySelectedEnemy.GetComponent<EnemySpaceship>().selected = false;
            }
        }

        selectedEnemy = enemyGO;

        // Enemy Spaceship
        if(enemyGO.GetComponent<EnemySpaceship>() != null)
        {
            enemyGO.GetComponent<EnemySpaceship>().selected = true;
        }

        GameManager.instance.ChangeSelectionState(GameManager.SelectionState.EnemySelected);

        EnemyInfoPanel.instance.DisplayPanel(true);
        EnemyInfoPanel.instance.UpdateInfo();

        if(previouslySelectedEnemy != null)
        {

        }
    }

    public void DeselectEnemy()
    {
        selectedEnemy = null;
        EnemyInfoPanel.instance.DisplayPanel(false);
    }
}
