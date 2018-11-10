using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class InfoManager : MonoBehaviour {

    public static InfoManager instance;
    void Awake(){ 
        if (instance != null){ Debug.LogError("More than one InfoManager in scene !"); return; } instance = this;
    }

    [Header("Operation")]
    public int nbMeteorCollisions;

    [Header("UI")]
    public TextMeshProUGUI meteorCollisionsValue;

    void Start()
    {
        nbMeteorCollisions = 0;
    }

    public void SetMeteorCollisionsValue(int nb)
    {
        Debug.Log("Changing Meteor Collisions Value to: " + nb);
        nbMeteorCollisions = nb;
        UpdateMeteorCollisionsValueDisplay();
    }

    public void UpdateMeteorCollisionsValueDisplay()
    {
        meteorCollisionsValue.text = nbMeteorCollisions.ToString();
    }

    public void IncrementMeteorCollisionsValue()
    {
        //Debug.Log("Incrementing Meteor Collisions Value.");
        nbMeteorCollisions++;
        UpdateMeteorCollisionsValueDisplay();
    }

}
