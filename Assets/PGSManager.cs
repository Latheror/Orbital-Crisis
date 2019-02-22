using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GooglePlayGames;
using GooglePlayGames.BasicApi;
using UnityEngine.SocialPlatforms;
using TMPro;

public class PGSManager : MonoBehaviour {

    public static PGSManager instance;
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    public TextMeshProUGUI authenticationResultText;


    void Start () {

        PlayGamesClientConfiguration config = new PlayGamesClientConfiguration.Builder().Build();

        PlayGamesPlatform.InitializeInstance(config);
        // recommended for debugging:
        PlayGamesPlatform.DebugLogEnabled = true;
        // Activate the Google Play Games platform
        PlayGamesPlatform.Activate();
    }

    public void OnAuthenticateButton()
    {
        Authentication();
    }

    void Authentication()
    {
        // authenticate user:
        Social.localUser.Authenticate((bool success) => {
            // handle success or failure
            AuthenticateCallback(success);
        });
    }

    void AuthenticateCallback(bool success)
    {
        Debug.Log("AuthenticateCallback [" + success + "]");

        authenticationResultText.text = success.ToString();
    }


}
