using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MathManager : MonoBehaviour {

    public static MathManager instance;
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }


    public float GetLevelMeteorNbFactor(int levelId)
    {
        return Mathf.Exp(-LevelManager.instance.alpha * levelId);
    }

    public float GetLevelHardMeteorsProportion(int levelId)
    {
        return 1-Mathf.Exp(-LevelManager.instance.hardMeteorsProportionAlpha* levelId);
    }
}
