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

    [Header("IDs")]
    public string mainScoreLeaderboardID = "CgkIkrzNsMcbEAIQAQ";

    [Header("UI")]
    public TextMeshProUGUI authenticationResultText;
    public GameObject authenticationButton;
    public GameObject leaderboardButton;
    public GameObject achievementsButton;

    [Header("Operation")]
    public bool player_authenticated = false;

    void Start () {

        PlayGamesClientConfiguration config = new PlayGamesClientConfiguration.Builder().Build();

        PlayGamesPlatform.InitializeInstance(config);
        // recommended for debugging:
        PlayGamesPlatform.DebugLogEnabled = true;
        // Activate the Google Play Games platform
        PlayGamesPlatform.Activate();


        Initialize();
    }

    void Initialize()
    {
        if(! Social.localUser.authenticated)
        {
            DisplayAuthenticationButton(true);
            DisplayLeaderboardButton(false);
            DisplayAchievementsButton(false);
        }
        else
        {
            DisplayAuthenticationButton(false);
            DisplayLeaderboardButton(true);
            DisplayAchievementsButton(true);
        }
    }

    public void DisplayAuthenticationButton(bool display)
    {
        authenticationButton.SetActive(display);
    }

    public void DisplayLeaderboardButton(bool display)
    {
        leaderboardButton.SetActive(display);
    }

    public void DisplayAchievementsButton(bool display)
    {
        achievementsButton.SetActive(display);
    }

    public void OnAuthenticateButton()
    {
        // Start rotating the button
        StartStopSignInButtonRotation(true);

        Authentication();
    }

    public void StartStopSignInButtonRotation(bool start)   // true: start, false: stop
    {
        Animator anim = MenuManager.instance.signInButton.GetComponent<Animator>();
        anim.SetTrigger((start)?"start":"stop");
    }

    public void OnLeaderboardButton()
    {
        DisplayLeaderboard();
    }

    public void OnAchievementsButton()
    {
        DisplayAchievements();
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
        StartStopSignInButtonRotation(false);
        MenuManager.instance.SetSignButtonImage(success);

        authenticationResultText.text = success.ToString();

        if(success)
        {
            DisplayLeaderboardButton(true);
            DisplayAchievementsButton(true);
            DisplayAuthenticationButton(false);
            player_authenticated = true;
        }
    }

    void DisplayLeaderboard()
    {
        Debug.Log("DisplayLeaderboard");
        // show leaderboard UI
        Social.ShowLeaderboardUI();
    }

    void DisplayAchievements()
    {
        Debug.Log("DisplayAchievements");
        // show achievements UI
        Social.ShowAchievementsUI();
    }

    public void WaveCompleted(int waveIndex)
    {
        Debug.Log("PGSManager | WaveCompleted [" + waveIndex + "]");

        if(Social.localUser.authenticated)
        {
            switch (waveIndex)
            {
                case 10:
                {
                    Social.ReportProgress(GPGSIds.achievement_complete_wave_10, 100.0f, (bool success) => {

                    });
                    break;
                }
                case 20:
                {
                    Social.ReportProgress(GPGSIds.achievement_complete_wave_20, 100.0f, (bool success) => {

                    });
                    break;
                }
                case 30:
                {
                    Social.ReportProgress(GPGSIds.achievement_complete_wave_30, 100.0f, (bool success) => {

                    });
                    break;
                }
                case 40:
                {
                    Social.ReportProgress(GPGSIds.achievement_complete_wave_40, 100.0f, (bool success) => {

                    });
                    break;
                }
                case 50:
                {
                    Social.ReportProgress(GPGSIds.achievement_complete_wave_50, 100.0f, (bool success) => {

                    });
                    break;
                }
                default:
                    break;
            }
        }
    }

    public void PostScore()
    {
        float score = ScoreManager.instance.score;
        bool authenticated = Social.localUser.authenticated;
        Debug.Log("PGSManager | Authenticated [" + authenticated + " | Score [" + score + "]");

        if(authenticated)
        {
            Social.ReportScore((long)score, mainScoreLeaderboardID, (bool success) => {
                Debug.Log("PGSManager | Posting score success [" + success + "]");
            });
        }
    }


}
