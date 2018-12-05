using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spaceport : Building
{

    [Header("Settings")]
    public float spawnRadius = 20;
    public int maxSpaceships = 1;

    [Header("Tier 2")]
    public int maxSpaceshipsNb_tier_2 = 2;
    //public float range_tier_2 = 200f;
    //public float energyConsumption_tier_2 = 25;

    [Header("Tier 3")]
    public int maxSpaceshipsNb_tier_3 = 3;
    //public float power_tier_3 = 30f;
    //public float range_tier_3 = 300f;
    //public float energyConsumption_tier_3 = 40;*/

    [Header("Prefabs")]
    public GameObject spaceshipPrefab;

    [Header("Operation")]
    public List<GameObject> attachedSpaceships;

    // Use this for initialization
    void Start()
    {
        attachedSpaceships = new List<GameObject>();
    }

    // Update is called once per frame
    void Update()
    {

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
                break;
            }
            case 3:
            {
                maxSpaceships = maxSpaceshipsNb_tier_3;
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

        SpaceportInfoPanel.instance.ImportInfo();
    }
}
