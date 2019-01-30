using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpaceship : Spaceship {

    public float artifactLootProbability = .5f;
    public bool isBeingCollected = false;
    public bool isBeingTargetedByCollector = false;

    // Use this for initialization
    void Start () {
        isAlly = false;
        isActivated = true;
        target = null;
        InvokeRepeating("UpdateTarget", 0f, 0.2f);
    }

    // Update is called once per frame
    void FixedUpdate () {
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
            GameObject closestTarget = null;

            foreach (GameObject alliedSpaceship in SpaceshipManager.instance.allySpaceships)
            {
                float dist_squared = (transform.position - alliedSpaceship.transform.position).sqrMagnitude;
                if ((dist_squared < minDist) && (alliedSpaceship.GetComponent<Spaceship>().isActivated))
                {
                    minDist = dist_squared;
                    closestTarget = alliedSpaceship;
                }
            }

            if(closestTarget != null)
            {
                target = closestTarget;
                //Debug.Log("Ennemy target | Target set");
            }
            else
            {
                // No target found

            }
        }
    }

    protected override void HandleMovements()
    {
        if (isActivated)
        {
            if (target != null)
            {
                // Set manual destination to target
                SetManualDestination(target.transform.position);

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
            else
            {
                RotateAroundPlanet();
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

    public override void DestroySpaceship()
    {
        //Debug.Log("Enemy Spaceship has been destroyed !");
        isActivated = false; // temporary
        healthBarPanel.SetActive(false);
        EnemiesManager.instance.enemyWrecks.Add(gameObject);
        EnemiesManager.instance.enemies.Remove(gameObject);
    }

    public void Collect()
    {
        if(!isActivated)
        {
            // Add resources
            RewardResources();

            // Add artifacts points
            RewardArtifacts();

            //Debug.Log("EnemySpaceship | Remove wreck from list");
            EnemiesManager.instance.enemyWrecks.Remove(gameObject);
            //Debug.Log("EnemySpaceship | Destroy");
            Destroy(gameObject);
        }
        else
        {
            Debug.Log("Error: Trying to collect a spaceship which is still activated !");
        }
    }

    public bool GetArtifactBasedOnLootProbability()
    {
        float rand = Random.Range(0f, 1f);
        //Debug.Log("GetArtifactBasedOnLootProbability | Rand [" + rand + "] | LootProba [" + artifactLootProbability + "] WillLoot [" + (rand <= artifactLootProbability) + "]");
        return (rand <= artifactLootProbability);
    }

    public void RewardArtifacts()
    {
        if(GetArtifactBasedOnLootProbability())
        {
            //Debug.Log("Enemy Spaceship dropped an artifact !");
            ScoreManager.instance.IncreaseArtifactsNb(1);
        }
    }

    public void RewardResources()
    {
        ResourcesManager.instance.ProduceResource(ResourcesManager.instance.GetResourceTypeByName("composite"),100);
        ResourcesManager.instance.ProduceResource(ResourcesManager.instance.GetResourceTypeByName("electronics"), 100);
    }
}