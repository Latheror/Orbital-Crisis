using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GatherablesManager : MonoBehaviour {

    public static GatherablesManager instance;
    void Awake()
    {
        if (instance != null) { Debug.LogError("More than one GatherablesManager in scene !"); return; }
        instance = this;
    }

    [Header("Settings")]
    public float earthRadius = 100f;
    public float circleFactor = 50f;
    public float circleFactorRange = 20f;

    [Header("Operation")]
    public List<GameObject> currentGatherablesList;

    [Header("Prefabs")]
    public List<GameObject> gatherablePrefabs;

    public void NewWaveActions(int waveNb)
    {
        SpawnGatherable(Gatherable.GatherableType.Heal);
    }

    public void SpawnGatherables(int nb, Gatherable.GatherableType type)
    {
        while(nb > 0)
        {
            SpawnGatherable(type);
            nb--;
        }
    }

    public void SpawnGatherable(Gatherable.GatherableType type)
    {
        GameObject gatherableOfType = GetGatherableFromType(type);
        if (gatherableOfType != null)
        {
            Vector2 randomCirclePos = Random.insideUnitCircle.normalized;
            float randomCircleFactor = Random.Range(circleFactor - circleFactorRange, circleFactor + circleFactorRange);
            Vector3 pos = new Vector3(randomCirclePos.x * circleFactor, randomCirclePos.y * randomCircleFactor, GameManager.instance.objectsDepthOffset);

            GameObject instantiatedGatherable = Instantiate(gatherableOfType, pos, Quaternion.identity);
            instantiatedGatherable.transform.SetParent(this.gameObject.transform);

            currentGatherablesList.Add(instantiatedGatherable);
        }
        else
        {
            Debug.Log("Error | SpawnGatherable | Type unknown: " + type.ToString());
        }
    }

    public GameObject GetGatherableFromType(Gatherable.GatherableType type)
    {
        GameObject correspondingGatherable = null;
        foreach (GameObject gatherablePrefab in gatherablePrefabs)
        {
            Gatherable g = gatherablePrefab.GetComponent<Gatherable>();
            //Debug.Log("Searched Type: " + type.ToString() + " | Found Type: " + g.type.ToString());
            if (g.type == type)
            {
                correspondingGatherable = gatherablePrefab;
                break;
            }
        }
        return correspondingGatherable;
    }
}
