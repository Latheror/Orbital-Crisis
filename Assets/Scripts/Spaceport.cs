using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spaceport : Building
{

    [Header("Settings")]
    public float spawnRadius = 20;
    public int maxSpaceships = 1;
    public int fleetPoints = 3;

    [Header("Tier 2")]
    public int maxSpaceshipsNb_tier_2 = 2;
    public float energyConsumption_tier_2 = 25;
    public int fleetPoints_tier_2 = 6;

    [Header("Tier 3")]
    public int maxSpaceshipsNb_tier_3 = 3;
    public float energyConsumption_tier_3 = 40;
    public int fleetPoints_tier_3 = 10;

    [Header("Prefabs")]
    public GameObject spaceshipPrefab;

    [Header("Operation")]
    public List<GameObject> attachedSpaceships;

    // Use this for initialization
    void Start()
    {
        Initialize();
    }

    public void Initialize()
    {
        SpaceshipManager.instance.SetCurrentMaxFleetPoints(fleetPoints);
        attachedSpaceships = new List<GameObject>();
    }

    public void SpawnSpaceship()
    {
        Vector2 randomCirclePos = Random.insideUnitCircle.normalized;
        Vector3 pos = transform.position + new Vector3(randomCirclePos.x * spawnRadius, randomCirclePos.y * spawnRadius, 0f);

        GameObject instantiatedSpaceship = Instantiate(spaceshipPrefab, pos, Quaternion.identity);

        // Set attributes
        instantiatedSpaceship.GetComponent<Spaceship>().homeSpaceport = gameObject;

        attachedSpaceships.Add(instantiatedSpaceship);
        SpaceshipManager.instance.AddAlliedSpaceshipToList(instantiatedSpaceship);

        SpaceportInfoPanel.instance.ImportInfo();
    }

    public override void ApplyCurrentTierSettings()
    {
        Debug.Log("ApplyCurrentTierSettings | SPACEPORT | CurrentTier: " + currentTier);
        switch (currentTier)
        {
            case 2:
            {
                maxSpaceships = maxSpaceshipsNb_tier_2;
                energyConsumption = energyConsumption_tier_2;
                // Fleet points
                fleetPoints = fleetPoints_tier_2;
                SpaceshipManager.instance.SetCurrentMaxFleetPoints(fleetPoints);
                break;
            }
            case 3:
            {
                maxSpaceships = maxSpaceshipsNb_tier_3;
                energyConsumption = energyConsumption_tier_3;
                // Fleet points
                fleetPoints = fleetPoints_tier_3;
                SpaceshipManager.instance.SetCurrentMaxFleetPoints(fleetPoints);
                break;
            }
        }
    }

    public void BuySpaceshipRequest()
    {
        if(attachedSpaceships.Count < maxSpaceships)
        {
            SpawnSpaceship();
        }
        else
        {
            Debug.Log("Maximum spaceships limit reached !");
        }
    }

    public void RemoveSpaceship(GameObject spaceshipToRemove)
    {
        attachedSpaceships.Remove(spaceshipToRemove);

        SpaceshipManager.instance.RemoveSpaceship(spaceshipToRemove);

        SpaceportInfoPanel.instance.ImportInfo();
    }

    // Deprecated
    public void SpawnSpaceshipOfType(SpaceshipManager.SpaceshipType spaceshipType)
    {
        Debug.Log("SpawnSpaceshipOfType [" + spaceshipType.typeName + "]");

        Vector2 randomCirclePos = Random.insideUnitCircle.normalized;
        Vector3 pos = transform.position + new Vector3(randomCirclePos.x * spawnRadius, randomCirclePos.y * spawnRadius, 0f);

        GameObject instantiatedSpaceship = Instantiate(spaceshipType.prefab, pos, Quaternion.Euler(0,90,0));
        instantiatedSpaceship.transform.SetParent(SpaceshipManager.instance.spaceshipsParent);

        // Set attributes
        instantiatedSpaceship.GetComponent<Spaceship>().SetSpaceshipType(spaceshipType);
        instantiatedSpaceship.GetComponent<Spaceship>().homeSpaceport = gameObject;

        attachedSpaceships.Add(instantiatedSpaceship);
        SpaceshipManager.instance.AddAlliedSpaceshipToList(instantiatedSpaceship);

        //SpaceportInfoPanel.instance.ImportInfo();
    }
}
