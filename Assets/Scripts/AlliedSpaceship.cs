using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlliedSpaceship : Spaceship {

	// Use this for initialization
	void Start () {
        isAllied = true;
        target = null;
        isActivated = true;
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

        // Turrets only work if they have the required energy
        if (isActivated)
        {
            //Debug.Log("Laser Turret | Update target");
            List<GameObject> meteors = MeteorsManager.instance.meteorsList;
            float shortestDistance = Mathf.Infinity;
            GameObject nearestEnemy = null;

            foreach (GameObject meteor in meteors)      // Search for meteors
            {
                float distanceToEnemy = Vector3.Distance(transform.position, meteor.transform.position);
                //Debug.Log("Meteor found - Distance is : " + distanceToEnemy);
                if (distanceToEnemy < shortestDistance)
                {
                    shortestDistance = distanceToEnemy;
                    nearestEnemy = meteor;
                }
            }

            foreach (GameObject enemy in EnemiesManager.instance.enemies)       // Search for enemies (spaceships, ...)
            {
                float distanceToEnemy = Vector3.Distance(transform.position, enemy.transform.position);
                if (distanceToEnemy < shortestDistance /* Temporary */ && enemy.GetComponent<Spaceship>().isActivated)
                {
                    shortestDistance = distanceToEnemy;
                    nearestEnemy = enemy;
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
}
