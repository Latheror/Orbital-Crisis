using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainPlanet : MonoBehaviour {

    public GameObject mainPlanetEmpty;

    [Header("General")]
    public string planetName = "MainPlanet";
    public float size;

    [Header("Buildings")]
    public List<GameObject> surroundingLevels;
    public int currentSurroundingLevelsShown;

    void Start()
    {
        size = transform.localScale.x;
        currentSurroundingLevelsShown = 0;
        //InvokeRepeating("AnimateSurroundingLevels", 0f, 0.5f);
    }


    void OnTriggerEnter(Collider other)
    {
        //Debug.Log("Planet collided with : " + other.gameObject.GetComponent<Meteor>());

        if(other.gameObject.tag == "meteor")
        {
            //Debug.Log("Planet hit by a meteor.");
            InfoManager.instance.IncrementMeteorCollisionsValue();

            other.GetComponent<Meteor>().InstantiateImpactEffect(2);
            MeteorsManager.instance.DeleteMeteor(other.gameObject);
        }
    }

    void OnTriggerStay(Collider other)
    {
        //Debug.Log("Planet collided with : " + other.gameObject.GetComponent<Meteor>());
        //InfoManager.instance.IncrementMeteorCollisionsValue();
    }

    public void IncreasePlanetSize()
    {
        Debug.Log("Increasing Planet Size.");
        mainPlanetEmpty.transform.localScale *= 1.1f;
        Camera.main.fieldOfView *= 1.1f;
    }

    public void DecreasePlanetSize()
    {
        Debug.Log("Decreasing Planet Size.");
        mainPlanetEmpty.transform.localScale *= 0.9f;
        Camera.main.fieldOfView *= 0.9f;
    }

    public void ShowUpToSurroundingLevel(int nb)
    {
        if(nb >= 0)
        {
            if(nb == 0)
            {
                foreach (var level in surroundingLevels)
                {
                    level.SetActive(false);
                }
            }else
            {
                for (int i = 0; i < nb; i++) // < or <= ?
                {
                    surroundingLevels[i].SetActive(true);
                }
                for (int j = nb + 1; j < surroundingLevels.Count; j++)
                {
                    surroundingLevels[j].SetActive(false);
                }
            }
        }
    }

    public void AnimateSurroundingLevels()
    {
        int nbLevels = surroundingLevels.Count;
        //Debug.Log("Animate Surrounding Levels | Nb levels: " + nbLevels + " | Current level: " + currentSurroundingLevelsShown);

        ShowUpToSurroundingLevel(currentSurroundingLevelsShown);

        currentSurroundingLevelsShown = (currentSurroundingLevelsShown + 1) % (nbLevels + 1);
    }

}
