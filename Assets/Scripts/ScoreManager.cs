using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ScoreManager : MonoBehaviour {

    public static ScoreManager instance;
    void Awake(){ 
        if (instance != null){ Debug.LogError("More than one ScoreManager in scene !"); return; } instance = this;
    }

    [Header("Settings")]
    public int experiencePointsPerMeteorUnitOfSize = 10;

    [Header("Operation")]
    public int score;
    public int experiencePoints;

    [Header("UI")]
    public GameObject scoreValueIndicator;
    public GameObject experienceValueText;


    void Start()
    {
        score = 0;
        experiencePoints = 0;
        UpdateScoreDisplay();
        UpdateExperiencePointsDisplay();
    }

    public void SetScore(int score)
    {
        this.score = score;
        UpdateScoreDisplay();
    }

    public void IncreaseScore(int delta)
    {
        this.score += delta;
        UpdateScoreDisplay();
    }

    public void DecreaseScore(int delta)
    {
        this.score -= delta;
        UpdateScoreDisplay();
    }

    public void UpdateScoreDisplay(){
        scoreValueIndicator.GetComponent<TextMeshProUGUI>().text = score.ToString();
    }

    public void SetExperiencePoints(int exp)
    {
        experiencePoints = exp;
        UpdateExperiencePointsDisplay();
    }

    public void IncreaseExperiencePoints(int delta)
    {
        experiencePoints += delta;
        UpdateExperiencePointsDisplay();
    }

    public void DecreaseExperiencePoints(int delta)
    {
        experiencePoints -= delta;
        UpdateExperiencePointsDisplay();
    }

    public void UpdateExperiencePointsDisplay()
    {
        experienceValueText.GetComponent<TextMeshProUGUI>().text = experiencePoints.ToString();
        TechTreeManager.instance.UpdateTechnologyCostIndicatorsDisplay();
        TechTreeManager.instance.SetExperiencePointsDisplay(experiencePoints);
    }

    public void GrantPointsFromDestroyingMeteor(Meteor meteor)
    {
        IncreaseScore((int)(meteor.originalSize * MeteorsManager.instance.valuePerSizeUnit));
    }

    public void GrantExperiencePointsFromDestroyingMeteor(Meteor meteor)
    {
        IncreaseExperiencePoints((int)(meteor.originalSize * experiencePointsPerMeteorUnitOfSize));
    }

}
