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

    [Header("Settings")]
    public float rotationSpeed = 20f;
    public float spawnOffset = 30f;
    public float circleFactor = 50f;
    public float meteorSpawnMinSize = 5;
    public float meteorSpawnMaxSize = 15;
    public float healthPointsAtMinSize = 10;
    public float healthPointsAtMaxSize = 30;
    public int valuePerSizeUnit = 10;

    [Header("Operation")]
    public List<GameObject> meteorsList;


    void Awake(){ 
        if (instance != null){ Debug.LogError("More than one MeteorsManager in scene !"); return; } instance = this;
    }

	// Use this for initialization
	void Start () {
        meteorsList = new List<GameObject>();
	}
	
	// Update is called once per frame
	void Update () {

	}

    public void SpawnNewMeteor()
    {
        GameObject meteorModel = meteorPrefabsList[(Random.Range(0, meteorPrefabsList.Count))];

        Vector2 randomCirclePos = Random.insideUnitCircle.normalized;
        Vector3 pos = new Vector3(randomCirclePos.x * circleFactor, randomCirclePos.y * circleFactor, GameManager.instance.objectsDepthOffset);

        float meteorSize = Random.Range(meteorSpawnMinSize, meteorSpawnMaxSize);
        float meteorHealth = GetMeteorHealthFromSize(meteorSize);

        // Instantiate Meteor Prefab
        GameObject instantiatedMeteor = Instantiate(meteorModel, pos, Quaternion.Euler(Random.Range(0f,360f),Random.Range(0f,360f),Random.Range(0f,360f)));
        instantiatedMeteor.transform.localScale = new Vector3(meteorSize, meteorSize, meteorSize);
        instantiatedMeteor.transform.SetParent(transform);

        Meteor meteor = instantiatedMeteor.GetComponent<Meteor>();
        meteor.SetRandomSpeeds();
        meteor.originalSize = meteorSize;
        meteor.size = meteorSize;
        meteor.healthPoints = meteorHealth;
        meteor.willLetDebris = LogicFunctions.RandomTrueFalse();

        // Add meteor to list
        meteorsList.Add(instantiatedMeteor);

        // TODO : Remove
        instantiatedMeteor.GetComponent<Meteor>().TestMeteorFunction();
    }

    public void SpawnNewMeteors(int nb){
        while(nb > 0){
            SpawnNewMeteor();
            nb--;
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
        Debug.Log("Deleting all meteors");
        foreach (GameObject meteor in meteorsList.ToArray())
        {
            meteorsList.Remove(meteor);
            Destroy(meteor);
        }
    }

    public float GetMeteorHealthFromSize(float size)
    {
        return (size - meteorSpawnMinSize) * (healthPointsAtMaxSize - healthPointsAtMinSize) / (meteorSpawnMaxSize - meteorSpawnMinSize) + healthPointsAtMinSize;
        // MAP : (x - in_min) * (out_max - out_min) / (in_max - in_min) + out_min;
    }

    public float GetMeteorSizeFromHealth(float health)
    {
        return ((health)*(meteorSpawnMaxSize - meteorSpawnMinSize) - healthPointsAtMinSize)/(healthPointsAtMaxSize - healthPointsAtMinSize) + meteorSpawnMinSize;
        // MAP : (x - in_min) * (out_max - out_min) / (in_max - in_min) + out_min;
    }

    public void MeteorDestroyed(Meteor meteor)
    {
        ScoreManager.instance.GrantPointsFromDestroyingMeteor(meteor);
    }
}
