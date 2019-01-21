using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebrisManager : MonoBehaviour {

    public static DebrisManager instance;
    void Awake(){ 
        if (instance != null){ Debug.LogError("More than one DebrisManager in scene !"); return; } instance = this;
    }

    [Header("Settings")]
    public float spawnRadius = 10f;

    [Header("Prefabs")]
    public GameObject debrisPrefab;

    [Header("Operation")]
    public List<GameObject> debrisList = new List<GameObject>();

    public void SpawnDebris(Vector3 pos, float originalMeteorSize)
    {
        int debrisNb = 3;
        float stepAngle = Mathf.PI * 2 / debrisNb;
        float radius = originalMeteorSize * 1.5f;
        int debrisIndex = debrisNb;
        float debrisSizeRandomFactor = Random.Range(-0.4f, 0.4f);

        while(debrisIndex > 0)
        {
            float angle = stepAngle * debrisIndex;

            Vector3 debrisPos = new Vector3(pos.x + radius * Mathf.Cos(angle), pos.y + radius * Mathf.Sin(angle), pos.z);
            GameObject instantiatedDebris = Instantiate(debrisPrefab, debrisPos, Quaternion.identity);
            instantiatedDebris.transform.localScale = new Vector3((originalMeteorSize / debrisNb)*(1 + debrisSizeRandomFactor), originalMeteorSize / debrisNb, originalMeteorSize / debrisNb);
            debrisList.Add(instantiatedDebris);
            instantiatedDebris.transform.SetParent(transform);
            instantiatedDebris.GetComponent<Debris>().SetOriginalSize(originalMeteorSize / debrisNb);
            debrisIndex--;
        }
    }
}
