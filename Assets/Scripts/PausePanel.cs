﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PausePanel : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void SaveButtonClicked()
    {
        SaveManager.instance.SaveButtonClicked();
    }

    public void LoadButtonClicked()
    {
        SaveManager.instance.LoadButtonClicked();
    }
}
