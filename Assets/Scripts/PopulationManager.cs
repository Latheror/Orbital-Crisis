using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PopulationManager : MonoBehaviour {

    public static PopulationManager instance;
    void Awake()
    {
        if (instance != null) { Debug.LogError("More than one PopulationManager in scene !"); return; }
        instance = this;
    }

    public enum PopulationType { Attack, Defense, Production}

    [Header("UI")]
    public TextMeshProUGUI globalPopulationAmountText;
    public GameObject populationPanel;

    [Header("Settings")]
    public float populationLossPerMeteorUnitOfSize = 1f;

    [Header("Operation")]
    public int globalPopulationAmount = 20;

    public void SetInfo()
    {
        globalPopulationAmountText.text = globalPopulationAmount.ToString();
    }



    public void PlanetHitByMeteor(Meteor meteor)
    {
        Debug.Log("PlanetHitByMeteor");
        globalPopulationAmount = Mathf.Max(0, Mathf.FloorToInt(meteor.size * populationLossPerMeteorUnitOfSize));
        SetInfo();

        PlayPopulationHurtAnimation();
    }

    public void PlayPopulationHurtAnimation()
    {
        Debug.Log("PlayPopulationHurtAnimation");
        Animation anim = populationPanel.GetComponent<Animation>();
        anim.Play();
    }
}
