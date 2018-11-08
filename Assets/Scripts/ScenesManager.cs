using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ScenesManager : MonoBehaviour {

    public static ScenesManager instance;
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        DontDestroyOnLoad(gameObject);
    }

    public void ChangeFromGameToMenuScene()
    {
        SceneManager.LoadSceneAsync(0);
        SaveManager.instance.ReloadGameSavesInMenu();
    }

    public void ChangeFromMenuToGameScene()
    {
        SceneManager.LoadSceneAsync(1);
    }
}
