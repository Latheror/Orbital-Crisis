using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ResourceIndicator : MonoBehaviour {

    [Header("Settings")]
    public ResourcesManager.ResourceAmount resourceAmount;

    [Header("UI")]
    public GameObject resourceImage;
    public TextMeshProUGUI resourceNameText;
    public TextMeshProUGUI resourceValueText;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}


    public void SetParameters(ResourcesManager.ResourceType resource)
    {
        resourceNameText.text = resource.resourceName;
        resourceValueText.text = resource.startAmount.ToString();
        GetComponent<Image>().color = resource.color;
        resourceImage.GetComponent<Image>().sprite = resource.resourceImage;
    }

    public void SetResourceValueText(int value)
    {
        resourceValueText.text = value.ToString();
    }

    public void UpdateIndicator()
    {
        SetResourceValueText(resourceAmount.amount);
    }

}
