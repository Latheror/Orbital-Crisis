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
    public float globalPopulationAmount = 20f;

    public void SetInfo()
    {
        globalPopulationAmountText.text = Mathf.RoundToInt(globalPopulationAmount).ToString();
    }



    public void PlanetHitByMeteor(Meteor meteor)
    {
        Debug.Log("PlanetHitByMeteor");
        globalPopulationAmount = Mathf.Max(0, globalPopulationAmount - (meteor.size * populationLossPerMeteorUnitOfSize));
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
