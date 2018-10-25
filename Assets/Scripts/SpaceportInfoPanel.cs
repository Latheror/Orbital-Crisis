using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SpaceportInfoPanel : MonoBehaviour {

    public static SpaceportInfoPanel instance;
    void Awake()
    {
        if (instance != null) { Debug.LogError("More than one SpaceportInfoPanel in scene !"); return; }
        instance = this;
    }

    [Header("UI")]
    public GameObject displayPanel;
    public TextMeshProUGUI spaceshipsNbText;
    public GameObject buySpaceshipButton;

    [Header("Operation")]
    public GameObject selectedSpaceport;
    public int spaceshipsNb = 1;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void SetInfo(GameObject spaceport)
    {
        selectedSpaceport = spaceport;
        ImportInfo();
    }

    public void DisplayInfo()
    {
        SetSpaceshipsNbText();
    }

    public void ImportInfo()
    {
        spaceshipsNb = selectedSpaceport.GetComponent<Spaceport>().attachedSpaceships.Count;
        DisplayInfo();
    }

    public void SetSpaceshipsNbText()
    {
        spaceshipsNbText.text = spaceshipsNb.ToString();
    }

    public void BuySpaceshipButtonClicked()
    {
        selectedSpaceport.GetComponent<Spaceport>().BuySpaceshipRequest();
    }

    public void DisplayPanel(bool display)
    {
        displayPanel.SetActive(display);
    }

    public void SpaceportTouched(GameObject spaceport)
    {
        SetInfo(spaceport);
        DisplayPanel(true);
    }
}
