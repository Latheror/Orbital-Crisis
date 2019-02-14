using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AchievementsManager : MonoBehaviour {

    public static AchievementsManager instance;
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    public List<Achievement> achievements;

    void Start()
    {
        Initialize();
    }

    public void Initialize()
    {

    }

    public void UnlockAchievementById(int id)
    {
        Achievement a = FindAchievementById(id);
        if(a != null)
            a.Complete(true);
    }

    public Achievement FindAchievementById(int id)
    {
        return achievements.Find(item => item.id == id);
    }

    public AchievementData[] BuildAchievementsData()
    {
        int achievementsNb = achievements.Count;
        Debug.Log("BuildAchievementsData | Nb: " + achievementsNb);
        AchievementData[] achievementsData = new AchievementData[achievementsNb];

        for (int i = 0; i < achievementsNb; i++)
        {
            Achievement achievement = achievements[i];
            achievementsData[i] = new AchievementData(achievement);
        }

        return achievementsData;
    }

    public void LoadAchievements(AchievementData[] achievementsData)
    {
        foreach (AchievementData aData in achievementsData)
        {
            Achievement achievement = FindAchievementById(aData.achievementIndex);
            if (achievement != null)
            {
                achievement.Complete(aData.achievementCompleted);
            }
        }

        // TODO: ?
    }

    public class Achievement
    {
        public int id;
        public string name;
        public bool completed;

        public void Complete(bool complete)
        {
            completed = complete;
        }

        public Achievement(int id, string name, bool completed)
        {
            this.id = id;
            this.name = name;
            this.completed = completed;
        }
    }

    // Class used to store and retrieve player achievements info when saving/loading the game 
    public class AchievementData
    {
        public int achievementIndex;
        public bool achievementCompleted;

        public AchievementData(int achievementIndex, bool achievementCompleted)
        {
            this.achievementIndex = achievementIndex;
            this.achievementCompleted = achievementCompleted;
        }

        public AchievementData(Achievement a)
        {
            this.achievementIndex = a.id;
            this.achievementCompleted = a.completed;
        }
}
}
