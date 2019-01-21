using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class InfoPanel : MonoBehaviour {

    public static InfoPanel instance;
    void Awake()
    {
        if (instance != null) { Debug.LogError("More than one InfoPanel in scene !"); return; }
        instance = this;
    }

    public GameObject spaceshipInfoPanel;
    public GameObject buildingInfoPanel;
    public GameObject enemyInfoPanel;

    public void spaceshipSelectedActions()
    {
        Debug.Log("Spaceship selected");
    }
}
