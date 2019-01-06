﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SpaceshipTypeShopPanel : MonoBehaviour {

    [Header("UI")]
    public TextMeshProUGUI spaceshipTypeNameText;
    public GameObject spaceshipCostLayout;
    public GameObject borderPanel;

    [Header("Prefabs")]
    public GameObject spaceshipCostPrefab;

    [Header("Operation")]
    public SpaceshipManager.SpaceshipType associatedSpaceshipType;
    public List<GameObject> costPanels;



    public void SetAssociatedSpaceshipType(SpaceshipManager.SpaceshipType spaceshipType)
    {
        this.associatedSpaceshipType = spaceshipType;
    }

    public void SetInfo()
    {
        if(associatedSpaceshipType != null)
        {
            spaceshipTypeNameText.text = associatedSpaceshipType.name.ToString();

            // Costs
            EmptyResourceCostsLayout();
            foreach (ResourcesManager.ResourceAmount rAmount in associatedSpaceshipType.resourceCosts)
            {
                GameObject instantiatedResourceCostPanel = Instantiate(spaceshipCostPrefab, Vector3.zero, Quaternion.identity);
                instantiatedResourceCostPanel.transform.SetParent(spaceshipCostLayout.transform, false);

                instantiatedResourceCostPanel.GetComponent<ResourceCostPanel>().SetInfo(rAmount);
            }
        }
    }

    public void OnButton()
    {
        Debug.Log("Button Clicked [" + associatedSpaceshipType.name + "]");
        FleetPanel.instance.SelectSpaceshipOnShop(associatedSpaceshipType);
    }

    public void EmptyResourceCostsLayout()
    {
        foreach (Transform child in spaceshipCostLayout.transform)
        {
            Destroy(child.gameObject);
        }
    }

    public void DisplayBorderPanel(bool display)
    {
        borderPanel.SetActive(display);
    }

}