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

    [Header("Operation")]
    public int score;

    [Header("UI")]
    public GameObject scoreValueIndicator;


    void Start()
    {
        score = 0;
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


    public void GrantPointsFromDestroyingMeteor(Meteor meteor)
    {
        IncreaseScore((int)meteor.originalSize * MeteorsManager.instance.valuePerSizeUnit);
    }

}
