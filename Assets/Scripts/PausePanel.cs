using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PausePanel : MonoBehaviour {

    public GameObject saveGamePanel;

    public void SaveButtonClicked()
    {
        saveGamePanel.SetActive(true);
        gameObject.transform.parent.gameObject.SetActive(false);
        //SaveManager.instance.SaveButtonClicked();
    }

    public void LoadButtonClicked()
    {
        SaveManager.instance.LoadButtonClicked();
    }

    public void OnPostScoreButtonClicked()
    {
        PGSManager.instance.PostScore();
    }
}
