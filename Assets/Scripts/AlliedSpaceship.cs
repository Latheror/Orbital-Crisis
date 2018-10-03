using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlliedSpaceship : Spaceship {

	// Use this for initialization
	void Start () {
        isAllied = true;
        target = null;
        isActivated = true;
        manualDestination = transform.position;
        manualDestinationReached = true;
        isInAutomaticMode = true;
        infoPanel.SetActive(false);
        SetStartingMode();
    }
	
	// Update is called once per frame
	void Update () {
        UpdateTarget();
        HandleMovements();
        AttackTarget();
    }


    protected override void UpdateTarget()
    {

        // Spaceships only work if they are activated
        if (isActivated)
        {
            //Debug.Log("Laser Turret | Update target");
            List<GameObject> meteors = MeteorsManager.instance.meteorsList;
            List<GameObject> enemies = EnemiesManager.instance.enemies;
            float shortestDistance = Mathf.Infinity;
            int maxPriorityFound = 0;
            GameObject nearestEnemy = null;

            // Search for meteors
            if (meteors.Count > 0)
            {
                int priority = EnemiesManager.instance.meteorPriority;
                if(priority > maxPriorityFound)
                {
                    shortestDistance = Mathf.Infinity;
                    nearestEnemy = null;
                    maxPriorityFound = priority;
                    foreach (GameObject meteor in meteors)      
                    {
                        float distanceToEnemy = Vector3.Distance(transform.position, meteor.transform.position);
                        //Debug.Log("Meteor found - Distance is : " + distanceToEnemy);
                        if (distanceToEnemy < shortestDistance)
                        {
                            shortestDistance = distanceToEnemy;
                            nearestEnemy = meteor;
                        }
                    }
                }
            }

            // Search for enemies (spaceships, ...)
            //Debug.Log("Number of enemy spaceships: " + EnemiesManager.instance.enemies.Count);
            if (EnemiesManager.instance.enemies.Count > 0)
            {
                int priority = EnemiesManager.instance.spaceshipsPriority;
                if (priority > maxPriorityFound)
                {
                    shortestDistance = Mathf.Infinity;
                    nearestEnemy = null;
                    foreach (GameObject enemy in enemies)       
                    {
                        float distanceToEnemy = Vector3.Distance(transform.position, enemy.transform.position);
                        if (distanceToEnemy < shortestDistance /* Temporary */ && enemy.GetComponent<Spaceship>().isActivated)
                        {
                            shortestDistance = distanceToEnemy;
                            nearestEnemy = enemy;
                        }
                    }
                }
            }


            if (nearestEnemy != null)
            {
                target = nearestEnemy;
                if (target != previousTarget)   // This is a new target
                {
                    if (previousTarget != null && previousTarget.CompareTag("meteor"))
                    {
                        previousTarget.GetComponent<Meteor>().ResetMeteorSettings();
                    }
                    previousTarget = target;
                }
                //Debug.Log("New meteor target set: " + target + " - Distance is: " + shortestDistance);
            }
            else
            {
                if (previousTarget != null && previousTarget.CompareTag("meteor"))
                {
                    previousTarget.GetComponent<Meteor>().ResetMeteorSettings();
                }
                target = null;
                previousTarget = target;
            }



        }
        else
        {
            //Debug.Log("Spaceship isn't activated !");
        }

    }

    protected override void HandleMovements()
    {
        if (!isInAutomaticMode)  // Manual Mode
        {
            if (!IsCloseEnoughToDestination())
            {
                // Move towards destination
                transform.position = Vector3.MoveTowards(transform.position, manualDestination, Time.deltaTime * movementSpeed);
            }
            else
            {
                if (target != null && IsTargetInRange())
                {
                    RotateTowardsTarget();
                }
            }
        }
        else  // Automatic mode
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
        if (isActivated && target != null)
        {
            if (isInAutomaticMode) // Automatic Mode
            {
                if (IsTargetInRange() && pulseFinished)
                {
                    //Debug.Log("Start firing coroutine");
                    StartCoroutine(FireLasersRoutine());
                }
            }
            else  // Manual Mode
            {
                if (IsCloseEnoughToDestination() && IsTargetInRange() && pulseFinished)
                {
                    StartCoroutine(FireLasersRoutine());
                }
            }
        }
    }

    protected override void DestroySpaceship()
    {
        Debug.Log("Allied Spaceship has been destroyed !");
        // temporary
        isActivated = false;
    }
}
