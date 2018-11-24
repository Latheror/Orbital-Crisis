using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TechnologyItem : MonoBehaviour {

    public TechTreeManager.Technology associatedTechnology;
    public TextMeshProUGUI experienceCostText;


	void Start () {
		
	}

    void Update () {
		
	}

    public void UnlockButtonClicked()
    {
        Debug.Log("UnlockButtonClicked [" + associatedTechnology.name + "]");
        TechTreeManager.instance.UnlockTechnologyRequest(associatedTechnology);
    }

    public void SetExperienceCostText(int expPoints)
    {
        experienceCostText.text = expPoints.ToString();
    }
}
