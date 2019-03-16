using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BuildingStatsPanel : MonoBehaviour {

    public static BuildingStatsPanel instance;
    void Awake()
    {
        if (instance != null) { Debug.LogError("More than one BuildingStatsPanel in scene !"); return; }
        instance = this;
    }

    [Header("UI")]
    public GameObject statsLayout;
    public List<GameObject> statInfoPanelsList = new List<GameObject>();
    public GameObject buildingStatPrefab;

    public void BuildStatsInfo(GameObject building) // TO REDO
    {
        //Debug.Log("BuildStatsInfo of [" + building.GetComponent<Building>().buildingType.name + "] | Stats nb [" + building.GetComponent<Building>().buildingType.stats.Count + "]");

        /*CleanStatsLayout();
        foreach (Building.BuildingStat stat in building.GetComponent<Building>().buildingType.stats)
        {
            GameObject instantiatedBuildingStatPanel = Instantiate(buildingStatPrefab, new Vector3(0,0,0), Quaternion.identity);
            instantiatedBuildingStatPanel.transform.SetParent(statsLayout.transform, false);

            statInfoPanelsList.Add(instantiatedBuildingStatPanel);

            instantiatedBuildingStatPanel.GetComponent<StatInfoPanel>().SetSelectedBuildingAndStat(building, stat);
            instantiatedBuildingStatPanel.GetComponent<StatInfoPanel>().SetInfo();
        }   */ 
    }

    public void CleanStatsLayout()
    {
        if(statInfoPanelsList.Count > 0)
        {
            for(int i=0; i<statInfoPanelsList.Count; i++)
            {
                Destroy(statInfoPanelsList[i]);
            }
        }
        statInfoPanelsList = new List<GameObject>();
    }
}
