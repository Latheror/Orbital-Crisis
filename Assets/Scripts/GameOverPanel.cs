using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOverPanel : MonoBehaviour {

    public static GameOverPanel instance;
    void Awake()
    {
        if (instance != null) { Debug.LogError("More than one GameOverPanel in scene !"); return; }
        instance = this;
    }

    public void OnMainMenuButton()
    {
        ScenesManager.instance.ChangeFromGameToMenuScene();
    }

    public void OnInfiniteModeButton()
    {
        GameManager.instance.SetInfiniteMode(true);
        PanelsManager.instance.DisplayGameOverPanel(false);
    }
}
