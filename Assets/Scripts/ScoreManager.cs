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
    public float scoreFactor = 1f;
    public float scoreFactorWithTimerEnabled = 1.25f;

    [Header("Operation")]
    public int score;
    public int experiencePoints;
    public int artifactsNb;

    [Header("UI")]
    public GameObject scoreValueIndicator;
    public GameObject experienceValueText;
    public GameObject artifactsNbText;


    void Start()
    {
        score = 0;
        experiencePoints = 0;
        artifactsNb = 0;
        UpdateScoreDisplay();
        UpdateExperiencePointsDisplay();

        // TEMP
        if (OptionsManager.instance.IsTimerOptionEnabled())
        {
            scoreFactor = scoreFactorWithTimerEnabled;
        }

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

    public void SetExperiencePointsAndArtifactsNb(int exp, int artifactsNb)
    {
        experiencePoints = exp;
        this.artifactsNb = artifactsNb;
        UpdateExperiencePointsDisplay();
        UpdateArtifactsNbDisplay();
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

    public void DecreaseArtifactsNb(int delta)
    {
        artifactsNb = Mathf.Max(0, artifactsNb - delta);
        UpdateArtifactsNbDisplay();
    }

    public void IncreaseArtifactsNb(int delta)
    {
        artifactsNb += delta;
        UpdateArtifactsNbDisplay();
    }

    public void UpdateArtifactsNbDisplay()
    {
        artifactsNbText.GetComponent<TextMeshProUGUI>().text = artifactsNb.ToString();
        TechTreeManager.instance.UpdateTechnologyCostIndicatorsDisplay();       // To complete
        TechTreeManager.instance.SetArtifactsNbDisplay(artifactsNb);
    }

    public void GrantPointsFromDestroyingMeteor(Meteor meteor)
    {
        IncreaseScore((int)(meteor.originalSize * MeteorsManager.instance.valuePerSizeUnit * scoreFactor));
    }

    public void GrantExperiencePointsFromDestroyingMeteor(Meteor meteor)
    {
        IncreaseExperiencePoints((int)(meteor.originalSize * experiencePointsPerMeteorUnitOfSize));
    }

}
