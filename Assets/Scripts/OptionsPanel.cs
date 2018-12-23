using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OptionsPanel : MonoBehaviour {

    public static OptionsPanel instance;
    void Awake()
    {
        if (instance != null) { Debug.LogError("More than one OptionsPanel in scene !"); return; }
        instance = this;
    }

    public void TimerOptionButtonClicked()
    {
        OptionsManager.instance.TimerOptionToggle();
    }
}
