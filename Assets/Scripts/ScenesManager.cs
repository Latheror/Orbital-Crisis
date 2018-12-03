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
        StartCoroutine("ChangeFromGameToMenuSceneCoroutine");
    }

    IEnumerator ChangeFromGameToMenuSceneCoroutine()
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(0);

        // Wait until the asynchronous scene fully loads
        while (!asyncLoad.isDone)
        {
            yield return null;
        }

        SaveManager.instance.ReloadSavedDataInMenu();
    }

    IEnumerator ChangeFromMenuToGameScene()
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(1);

        // Wait until the asynchronous scene fully loads
        while (!asyncLoad.isDone)
        {
            yield return null;
        }

        GameSetupManager.instance.SetupGame();
    }

    public void LaunchNewGame()
    {
        GameSetupManager.instance.SetGameSetupParameters(new GameSetupManager.GameSetupParameters(true, null));
        StartCoroutine("ChangeFromMenuToGameScene");
    }

    public void LaunchSavedGame(SaveManager.GameSaveData gameSaveData)
    {
        GameSetupManager.instance.SetGameSetupParameters(new GameSetupManager.GameSetupParameters(false, gameSaveData));
        StartCoroutine("ChangeFromMenuToGameScene");
    }
}
