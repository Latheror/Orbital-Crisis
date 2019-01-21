using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdsManager : MonoBehaviour {

    public static AdsManager instance;
    void Awake()
    {
        if (instance != null) { Debug.LogError("More than one AdsManager in scene !"); return; }
        instance = this;
    }


    // Use this for initialization
    void Start () {
		
	}
}
