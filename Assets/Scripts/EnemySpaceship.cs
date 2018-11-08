using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpaceship : Spaceship {

	// Use this for initialization
	void Start () {
        isAllied = false;
        isActivated = true;
        target = null;
        InvokeRepeating("UpdateTarget", 0f, 0.2f);
    }

    // Update is called once per frame
    void Update () {
        if (GameManager.instance.gameState == GameManager.GameState.Default)
        {
            HandleMovements();
            AttackTarget();
        }
    }

    
    protected override void UpdateTarget()
    {
        target = null;
        if (isActivated)
        {
            float minDist = Mathf.Infinity;

            foreach (GameObject alliedSpaceship in SpaceshipManager.instance.alliedSpaceships)
            {
                float dist = Vector3.Distance(transform.position, alliedSpaceship.transform.position);
                if ((dist < minDist) && (alliedSpaceship.GetComponent<Spaceship>().isActivated))
                {
                    target = alliedSpaceship;
                }
            }

            if(target != null)
            {
                //Debug.Log("Ennemy target | Target set");
            }
            else
            {

            }
        }
    }

    protected override void HandleMovements()
    {
        if (isActivated)
        {
            if (target != null)
            {
                if (!IsTargetInRange())
                {
                    // Go closer to target
                    transform.position = Vector3.MoveTowards(transform.position, target.transform.position, Time.deltaTime * movementSpeed);
                }

                RotateTowardsTarget();
            }
        }
    }


    protected override void AttackTarget()
    {
        if (isActivated)
        {
            if (IsTargetInRange() && pulseFinished)
            {
                //Debug.Log("Start firing coroutine");
                StartCoroutine(FireLasersRoutine());
            }
        }
    }

    protected override void DestroySpaceship()
    {
        Debug.Log("Enemy Spaceship has been destroyed !");
        isActivated = false; // temporary
        healthBarPanel.SetActive(false);
        EnemiesManager.instance.enemyWrecks.Add(gameObject);
        EnemiesManager.instance.enemies.Remove(gameObject);
    }

    public void Collect()
    {
        if(!isActivated)
        {
            EnemiesManager.instance.enemyWrecks.Remove(gameObject);
            Destroy(gameObject);
        }
        else
        {
            Debug.Log("Error: Trying to collect a spaceship which is still activated !");
        }
    }
}
