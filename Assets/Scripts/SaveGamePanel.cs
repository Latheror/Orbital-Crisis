using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveGamePanel : MonoBehaviour {

    public GameObject pausePanel;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void SaveGamePanel_BackButton()
    {
        pausePanel.SetActive(true);
        gameObject.SetActive(false);
    }

    public void SaveGamePanel_Slot1Button()
    {
        SaveGameRequest(1);
    }

    public void SaveGamePanel_Slot2Button()
    {
        SaveGameRequest(2);
    }

    public void SaveGamePanel_Slot3Button()
    {
        SaveGameRequest(3);
    }

    public void SaveGamePanel_Slot4Button()
    {
        SaveGameRequest(4);
    }

    public void SaveGamePanel_Slot5Button()
    {
        SaveGameRequest(5);
    }

    public void SaveGameRequest(int slotIndex)
    {
        Debug.Log("Request saving game in slot [" + slotIndex + "]...");
        SaveManager.instance.SaveGameRequest(slotIndex);
    }

}
