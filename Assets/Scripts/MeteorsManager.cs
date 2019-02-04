using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeteorsManager : MonoBehaviour {

    public static MeteorsManager instance;

    [Header("World")]
    public GameObject mainPlanet;

    [Header("Prefabs")]
    public GameObject meteorPrefab;
    public GameObject meteorTest;
    public List<GameObject> meteorPrefabsList;
    public List<GameObject> hardMeteorPrefabsList;

    [Header("Settings")]
    public float rotationSpeed = 20f;
    public float spawnOffset = 30f;
    public float circleFactor = 50f;
    public float meteorSpawnMinSize = 5f;
    public float meteorSpawnMaxSize = 15f;
    public float meteorSpawnSizeFactor = 1f;
    public float healthPointsAtMinSize = 10f;
    public float healthPointsAtMaxSize = 30f;
    public float healthSizeFactor = 1f;
    public int valuePerSizeUnit = 10;
    public float currentSpawnSizeFactor = 1f;

    [Header("Operation")]
    public List<GameObject> meteorsList;

    void Awake(){ 
        if (instance != null){ Debug.LogError("More than one MeteorsManager in scene !"); return; } instance = this;
    }

	void Start () {
        meteorsList = new List<GameObject>();
        CalculateHealthSizeFactor();
	}
	
    public void CalculateHealthSizeFactor()
    {
        healthSizeFactor = (healthPointsAtMaxSize - healthPointsAtMinSize) / (meteorSpawnMaxSize*currentSpawnSizeFactor - meteorSpawnMinSize*currentSpawnSizeFactor);
        //Debug.Log("healthSizeFactor: " + healthSizeFactor);
    }

    public void SpawnNewMeteor(GameObject prefabToSpawn, float hardnessFactor)
    {
        Vector2 randomCirclePos = Random.insideUnitCircle.normalized;
        Vector3 pos = new Vector3(randomCirclePos.x * circleFactor, randomCirclePos.y * circleFactor, GameManager.instance.objectsDepthOffset);

        CalculateHealthSizeFactor();

        float meteorSize = Random.Range(meteorSpawnMinSize, meteorSpawnMaxSize) * currentSpawnSizeFactor;
        float meteorHealth = GetMeteorHealthFromSize(meteorSize);

        //Debug.Log("SpawnNewMeteor | SizeFactor [" + currentSpawnSizeFactor + "] | Size [" + meteorSize + "] | Health [" + meteorHealth + "] | Hardness [" + hardnessFactor + "]");

        // Instantiate Meteor Prefab
        GameObject instantiatedMeteor = Instantiate(prefabToSpawn, pos, Quaternion.Euler(Random.Range(0f,360f),Random.Range(0f,360f),Random.Range(0f,360f)));
        instantiatedMeteor.transform.SetParent(transform);
        instantiatedMeteor.transform.localScale = new Vector3(meteorSize, meteorSize, meteorSize);

        //Debug.Log("Spawning a meteor with size: " + meteorSize + " and health: " + meteorHealth);

        Meteor meteor = instantiatedMeteor.GetComponent<Meteor>();
        meteor.SetRandomSpeeds();
        meteor.originalSize = meteorSize;
        meteor.size = meteorSize;
        meteor.healthPoints = meteorHealth;
        meteor.willLetDebris = LogicFunctions.RandomTrueFalse();
        meteor.hardnessFactor = hardnessFactor;

        // Add meteor to list
        meteorsList.Add(instantiatedMeteor);

        // TODO : Remove
        instantiatedMeteor.GetComponent<Meteor>().TestMeteorFunction();
    }

    public void SpawnNewMeteors(int nb, float hardMeteorsProportion = 0f){

        //Debug.Log("SpawnNewMeteors | Nb [" + nb + "] | hardMeteorsProportion [" + hardMeteorsProportion + "]");
        if (hardMeteorsProportion >= 0f && hardMeteorsProportion <= 1)
        {
            int hardMeteorsNb = Mathf.FloorToInt(nb * hardMeteorsProportion);
            int regularMeteorsNb = nb - hardMeteorsNb;
            GameObject meteorModel = null;
            bool spawnHardMeteor = false;
            float hardnessFactor = 1f;

            while ((regularMeteorsNb > 0 || hardMeteorsNb > 0) && (regularMeteorsNb >= 0 && hardMeteorsNb >= 0)) // While we still have meteors to spawn
            {
                if(regularMeteorsNb > 0)
                {
                    if(hardMeteorsNb > 0)
                    {
                        float r = Random.Range(0f, 1f);
                        Debug.Log("Random Number [" + r + "]");
                        if (r <= hardMeteorsProportion)
                        {
                            spawnHardMeteor = true;
                        }
                    }
                }
                else
                {
                    spawnHardMeteor = true;
                }

                if(spawnHardMeteor)
                {
                    hardnessFactor = 2f;
                    meteorModel = hardMeteorPrefabsList[(Random.Range(0, hardMeteorPrefabsList.Count))];
                    hardMeteorsNb--;
                }
                else
                {
                    hardnessFactor = 1f;
                    meteorModel = meteorPrefabsList[(Random.Range(0, meteorPrefabsList.Count))];
                    regularMeteorsNb--;
                }

                SpawnNewMeteor(meteorModel, hardnessFactor);
                Debug.Log("Spawned a meteor [" + ((spawnHardMeteor) ? "Hard" : "Regular") + "] | RegularMeteorNumberLeft [" + regularMeteorsNb + "] | HardMeteorsNbLeft [" + hardMeteorsNb + "]");
            }
        }
        else
        {
            Debug.LogError("SpawnNewMeteors | MeteorProportion invalid [" + hardMeteorsProportion + "]");
        }      
    }

    public void DeleteMeteor(GameObject meteorToDelete)
    {
        if(meteorsList.Contains(meteorToDelete))
        {
            meteorsList.Remove(meteorToDelete);
            Destroy(meteorToDelete);
            LevelManager.instance.IncrementCurrentLevelDestroyedMeteorsNb(1); 
        }
        else
        {
            Debug.Log("Trying to remove a meteor which isn't in the list !");
        }
    }

    public void DeleteAllMeteors()
    {
        //Debug.Log("Deleting all meteors");
        foreach (GameObject meteor in meteorsList.ToArray())
        {
            meteorsList.Remove(meteor);
            Destroy(meteor);
        }
    }

    public float GetMeteorHealthFromSize(float size)
    {
        if (size >= meteorSpawnMinSize * currentSpawnSizeFactor && size <= meteorSpawnMaxSize * currentSpawnSizeFactor)
        {
            //return (size - meteorSpawnMinSize) * (healthPointsAtMaxSize - healthPointsAtMinSize) / (meteorSpawnMaxSize - meteorSpawnMinSize) + healthPointsAtMinSize;
            return (healthSizeFactor * (size - meteorSpawnMinSize * currentSpawnSizeFactor) + healthPointsAtMinSize * currentSpawnSizeFactor);
            // MAP : y = (x - in_min) * (out_max - out_min) / (in_max - in_min) + out_min;
            // IN: Size, OUT: Health
        }
        else
        {
            Debug.LogError("GetMeteorHealthFromSize | Size out of range: " + size);
            return healthPointsAtMinSize * currentSpawnSizeFactor;
        }
    }

    public float GetMeteorSizeFromHealth(float health)
    {
        if(health >= healthPointsAtMinSize * currentSpawnSizeFactor  && health <= healthPointsAtMaxSize * currentSpawnSizeFactor)
        {
            //return ((health)*(meteorSpawnMaxSize - meteorSpawnMinSize) - healthPointsAtMinSize)/(healthPointsAtMaxSize - healthPointsAtMinSize) + meteorSpawnMinSize;
            //return ((health - healthPointsAtMaxSize) * (meteorSpawnMaxSize + meteorSpawnMinSize) / (healthPointsAtMaxSize - healthPointsAtMinSize) + meteorSpawnMinSize);
            return (((health - healthPointsAtMinSize * currentSpawnSizeFactor) / (healthSizeFactor)) + meteorSpawnMinSize * currentSpawnSizeFactor);
            // MAP : x = (y - out_min) * (in_max - in_min) / (out_max - out_min) + in_min
        }
        else
        {
            return meteorSpawnMinSize * currentSpawnSizeFactor;
        }
    }

    public void MeteorDestroyed(Meteor meteor)
    {
        ScoreManager.instance.GrantPointsFromDestroyingMeteor(meteor);
        ScoreManager.instance.GrantExperiencePointsFromDestroyingMeteor(meteor);
    }
}
