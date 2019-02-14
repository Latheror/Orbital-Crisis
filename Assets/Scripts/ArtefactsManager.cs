using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArtefactsManager : MonoBehaviour {

    public static ArtefactsManager instance;
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }
}
