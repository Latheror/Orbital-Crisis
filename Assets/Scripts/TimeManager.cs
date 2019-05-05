using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TimeManager : MonoBehaviour {

    public static TimeManager instance;
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    [Header("UI")]
    public GameObject timeLeftPanel;
    public TextMeshProUGUI timeLeftValueText;

    [Header("Settings")]
    public bool isTimerEnabled = false;

    [Header("Operation")]
    public float currCountdownValue = 0f;

    public IEnumerator StartCountdownCoroutine(float countdownValue = 10)
    {
        currCountdownValue = countdownValue;
        while (currCountdownValue > 0)
        {
            Debug.Log("Countdown: " + currCountdownValue);
            DisplayTimeLeft(currCountdownValue);
            yield return new WaitForSeconds(1.0f);
            if(GameManager.instance.gameState == GameManager.GameState.Default)
            {
                currCountdownValue--;
            }
        }

        Debug.Log("Countdown finished !");
        CountdownElapsed();
    }

    public void DisplayTimeLeft(float timeLeftInSeconds)
    {
        timeLeftValueText.text = (Mathf.RoundToInt(timeLeftInSeconds).ToString()/* + " sec"*/);
    }

    public void DisplayTimeLeftPanel(bool display)
    {
        timeLeftPanel.SetActive(display);
    }

    public void StartCountdown(float seconds)
    {
        //DisplayTimeLeftPanel(true);

        StartCoroutine(StartCountdownCoroutine(seconds));
    }

    public void CountdownElapsed()
    {
        timeLeftValueText.text = "";
        LevelManager.instance.NextLevelRequest();
    }

    public void SetTimerEnabled(bool enabled)
    {
        isTimerEnabled = enabled;
    }


}
