using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DebugManager : MonoBehaviour {

    public static DebugManager instance;
    void Awake(){ 
        if (instance != null){ Debug.LogError("More than one DebugManager in scene !"); return; } instance = this;
    }

    [Header("Operation")]
    public bool debugEnabled;

    void Start()
    {
        debugEnabled = false;
    }

    public void SetDebugEnabled(bool enabled)
    {
        debugEnabled = enabled;
    }
}
