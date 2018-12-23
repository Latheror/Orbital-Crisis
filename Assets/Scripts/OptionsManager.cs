using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OptionsManager : MonoBehaviour {

    public static OptionsManager instance;
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    public bool isTimerEnabled = false;



    public void SetTimerEnabled(bool enabled)
    {
        Debug.Log("SetTimerEnabled [" + enabled + "]");
        isTimerEnabled = enabled;
    }

    public void TimerOptionToggle()
    {
        Debug.Log("TimerOptionToggle [" + !isTimerEnabled + "]");
        isTimerEnabled = !isTimerEnabled;
    }
}
