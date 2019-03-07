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
    public float planetLifeLossPerMeteorSizeUnit = 1f;
    public float planetLifeStart = 1000f;

    [Header("Operation")]
    public int score;
    public int experiencePoints;
    public int artifactsNb;
    public float planetLife;
    public bool isScoreLocked;
    public bool gameOverHappened;

    [Header("UI")]
    public GameObject experienceValueText;
    public GameObject artifactsNbText;
    //public TextMeshProUGUI planetLifeText;    // Text removed from UI


    void Start()
    {
        Initialize();

        // TEMP
        if (OptionsManager.instance.IsTimerOptionEnabled())
        {
            scoreFactor = scoreFactorWithTimerEnabled;
        }
    }

    public void Initialize()
    {
        score = 0;
        experiencePoints = 0;
        artifactsNb = 0;
        planetLife = planetLifeStart;
        isScoreLocked = false;  // Becomes true after gameOver, when "infinite mode" is selected.
        gameOverHappened = false;
        UpdateScoreDisplay();
        UpdateExperiencePointsDisplay();
        UpdatePlanetLifeDisplay();
    }

    public void SetScore(int score)
    {
        this.score = score;
        UpdateScoreDisplay();
    }

    public void SetPlanetLife(int pLife)
    {
        planetLife = pLife;
        UpdatePlanetLifeDisplay();
    }

    public void IncreaseScore(int delta)
    {
        if(! GameManager.instance.IsInfiniteModeEnabled())  // Stop increasing score after a game over
        {
            this.score += delta;
            UpdateScoreDisplay();
        }
    }

    public void DecreaseScore(int delta)
    {
        this.score -= delta;
        UpdateScoreDisplay();
    }

    public void UpdateScoreDisplay(){
        // REMOVED
    }

    public void UpdatePlanetLifeDisplay()
    {
        //planetLifeText.text = Mathf.FloorToInt(planetLife).ToString();        // Text removed from UI
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

    // Obsolete
    /*public void DecreasePlanetLife(float amount)
    {
        planetLife -= amount;
        //Debug.Log("DecreasePlanetLife [" + planetLife + "]");
        if (planetLife <= 0)
        {
            planetLife = 0;
            TriggerGameOver();
        }
        UpdatePlanetLifeDisplay();
    }*/

    public void PlanetHitByMeteor(Meteor meteor)
    {
        //DecreasePlanetLife(meteor.size * planetLifeLossPerMeteorSizeUnit); // Not used anymore
    }

    public void TriggerGameOver()
    {
        if(! gameOverHappened)
        {
            Debug.Log("TriggerGameOver");
            gameOverHappened = true;
            PanelsManager.instance.DisplayGameOverPanel(true);
        }
    }

    public void SetGameOverHappened(bool happened)
    {
        gameOverHappened = happened;
    }

}
