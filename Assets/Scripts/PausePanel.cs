using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PausePanel : MonoBehaviour {

    public GameObject saveGamePanel;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void SaveButtonClicked()
    {
        saveGamePanel.SetActive(true);
        gameObject.SetActive(false);
        //SaveManager.instance.SaveButtonClicked();
    }

    public void LoadButtonClicked()
    {
        SaveManager.instance.LoadButtonClicked();
    }
}
