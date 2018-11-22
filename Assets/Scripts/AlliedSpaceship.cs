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
        //infoPanel.SetActive(false);
        SetStartingMode();
        InvokeRepeating("RegenerateShield", 0f, shieldRegenerationDelay);
    }

    // Update is called once per frame
    void FixedUpdate () {
        if (GameManager.instance.gameState == GameManager.GameState.Default)
        {
            UpdateTarget();
            HandleMovements();
            AttackTarget();
        }
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
        if(isActivated)
        {
            // Set manual destination to target
            if(isInAutomaticMode && target != null)
            {
                SetManualDestination(target.transform.position);
            }

            if (manualDestination != null)
            {
                // Check if path towards destination intersects with planet
                if (!IsCloseEnoughToDestination() && (GeometryManager.instance.SegmentIntersectWithPlanet(gameObject.transform.position, manualDestination)))
                {
                    //Debug.Log("HandleMovements | Setting a temp dest");
                    float spaceshipPosAngle = GeometryManager.GetRadAngleFromXY(transform.position.x, transform.position.y);
                    float spaceshipPosDistance = GeometryManager.instance.GetDistanceFromPlanetCenter(transform.position);
                    //Debug.Log("spaceshipPosAngle: " + spaceshipPosAngle + " | spaceshipPosDistance: " + spaceshipPosDistance);
                    float manualDestPosAngle = GeometryManager.GetRadAngleFromXY(manualDestination.x, manualDestination.y);
                    float manualDestPosDistance = GeometryManager.instance.GetDistanceFromPlanetCenter(manualDestination);
                    //Debug.Log("manualDestPosAngle: " + manualDestPosAngle + " | manualDestPosDistance: " + manualDestPosDistance);

                    // Mean angle
                    float meanAngle = GeometryManager.GetMeanAngle(spaceshipPosAngle, manualDestPosAngle);
                    float meanDistance = Mathf.Max((manualDestPosAngle + manualDestPosDistance) / 2, 10);
                    //Debug.Log("meanAngle: " + meanAngle + " | meanDistance: " + meanDistance);

                    tempDestination = new Vector3(meanDistance * Mathf.Cos(meanAngle), meanDistance * Mathf.Sin(meanAngle), manualDestination.z);
                }
                else
                {
                    // Set temp dest to manual dest
                    tempDestination = manualDestination;
                }
            }

            if (!isInAutomaticMode)  // Manual Mode
            {
                if (!IsCloseEnoughToDestination())
                {
                    // Move towards destination
                    transform.position = Vector3.MoveTowards(transform.position, tempDestination, Time.deltaTime * movementSpeed);
                    RotateTowardsTempDest();
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
                        transform.position = Vector3.MoveTowards(transform.position, tempDestination, Time.deltaTime * movementSpeed);
                    }
                    RotateTowardsTempDest();
                }
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
                //Debug.Log("AttackTarget, manual mode | IsCloseEnoughToDestination [" + IsCloseEnoughToDestination() + "] | IsTargetInRange [" + IsTargetInRange() + "]");
                if (IsCloseEnoughToDestination() && IsTargetInRange() && pulseFinished)
                {
                    StartCoroutine(FireLasersRoutine());
                }
            }
        }
    }

    protected override void DestroySpaceship()
    {
        //Debug.Log("Allied Spaceship has been destroyed !");
        // temporary
        isActivated = false;
        SpaceshipManager.instance.alliedSpaceships.Remove(gameObject);
        Destroy(gameObject);
    }

    void OnCollisionEnter(Collision col)
    {
        //Debug.Log("Spaceship collided with : " + col.gameObject.name);
        if (col.gameObject.CompareTag("gatherable"))
        {
            //Debug.Log("Touched gatherable object !");
            Gatherable g = col.gameObject.GetComponent<Gatherable>();
            g.ActOnSpaceship(this);       
        }
    }
}
