using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class FleetPanel : MonoBehaviour {

    public static FleetPanel instance;
    void Awake()
    {
        if (instance != null) { Debug.LogError("More than one FleetPanel in scene !"); return; }
        instance = this;
    }

    [Header("Prefabs")]
    public GameObject spaceshipInfoPanelPrefab;
    public GameObject addSpaceshipPanelPrefab;

    [Header("UI")]
    public GameObject allySpaceshipsLayout;
    public GameObject allySpaceshipsDisplayPanel;
    public GameObject allySpaceshipsHiddingPanel;
    public GameObject spaceshipShopPanel;

    public GameObject availableSpaceshipModelsLayout;
    public GameObject availableSpaceshipModelPanelPrefab;

    public GameObject buySpaceshipButton;

    public TextMeshProUGUI allySpaceshipUsedFleetPointsText;
    public TextMeshProUGUI allySpaceshipTotalFleetPointsText;

    [Header("Settings")]
    public float refreshInfoRate = 1f;

    [Header("Operation")]
    public GameObject mainSpaceship;
    public bool spaceportPresent = false;
    public SpaceshipManager.SpaceshipType selectedSpaceshipType = null;
    public List<GameObject> spaceshipShopPanelsList;

    void Start()
    {
        BuildInfo();
        InvokeRepeating("RefreshInfo", 0f, refreshInfoRate);
    }

    public void BuildInfo()
    {
        BuildAlliedSpaceshipsInfo();

        BuildSpaceshipsShopInfo();
    }

    public void RefreshInfo()
    {
        BuildAlliedSpaceshipsInfo();
    }
    
    public void BuildAlliedSpaceshipsInfo()
    {
        BuildGeneralAllySpaceshipsInfo();
        EmptyAllySpaceshipsLayout();

        if(InfrastructureManager.instance.spaceport != null)
        {
            allySpaceshipsHiddingPanel.SetActive(false);

            foreach (GameObject alliedSpaceship in SpaceshipManager.instance.allySpaceships)
            {
                GameObject instantiatedSpaceshipInfoPanel = Instantiate(spaceshipInfoPanelPrefab, Vector3.zero, Quaternion.identity);
                instantiatedSpaceshipInfoPanel.transform.SetParent(allySpaceshipsLayout.transform, false);

                instantiatedSpaceshipInfoPanel.GetComponent<FleetPanelSpaceshipInfo>().SetSpaceship(alliedSpaceship);
                instantiatedSpaceshipInfoPanel.GetComponent<FleetPanelSpaceshipInfo>().UpdateInfo();
            }

            if(true)    // TODO: Check is max spaceship nb isn't reached
            {
                GameObject instantiatedAddSpaceshipPanel = Instantiate(addSpaceshipPanelPrefab, Vector3.zero, Quaternion.identity);
                instantiatedAddSpaceshipPanel.transform.SetParent(allySpaceshipsLayout.transform, false);

                instantiatedAddSpaceshipPanel.AddComponent<Button>();
                instantiatedAddSpaceshipPanel.GetComponent<Button>().onClick.AddListener(() => OnAddSpaceshipButton());
            }
        }
        else
        {
            allySpaceshipsHiddingPanel.SetActive(true);
        }
    }

    public void BuildGeneralAllySpaceshipsInfo()
    {
        allySpaceshipUsedFleetPointsText.text = SpaceshipManager.instance.usedFleetPoints.ToString();
        allySpaceshipTotalFleetPointsText.text = SpaceshipManager.instance.currentMaxFleetPoints.ToString();
    }

    public void BuildSpaceshipsShopInfo()
    {
        EmptySpaceshipTypesLayout();
        spaceshipShopPanelsList = new List<GameObject>();
        foreach (SpaceshipManager.SpaceshipType spaceshipType in SpaceshipManager.instance.spaceshipTypes)
        {
            if(spaceshipType.isAlly)
            {
                GameObject instantiatedSpaceshipTypePanel = Instantiate(availableSpaceshipModelPanelPrefab, Vector3.zero, Quaternion.identity);
                instantiatedSpaceshipTypePanel.transform.SetParent(availableSpaceshipModelsLayout.transform, false);

                instantiatedSpaceshipTypePanel.GetComponent<SpaceshipTypeShopPanel>().SetAssociatedSpaceshipType(spaceshipType);
                instantiatedSpaceshipTypePanel.GetComponent<SpaceshipTypeShopPanel>().SetInfo();

                spaceshipType.SetAssociatedSpaceshipShopItem(instantiatedSpaceshipTypePanel);

                spaceshipShopPanelsList.Add(instantiatedSpaceshipTypePanel);
            }
        }
    }

    public void EmptyAllySpaceshipsLayout()
    {
        foreach (Transform child in allySpaceshipsLayout.transform)
        {
            Destroy(child.gameObject);
        }
    }

    public void EmptySpaceshipTypesLayout()
    {
        foreach (Transform child in availableSpaceshipModelsLayout.transform)
        {
            Destroy(child.gameObject);
        }
    }

    public void OnAddSpaceshipButton()
    {
        Debug.Log("OnAddSpaceshipButtonClick");
        spaceshipShopPanel.SetActive(true);
        buySpaceshipButton.SetActive(false);
        allySpaceshipsDisplayPanel.SetActive(false);
    }

    public void OnSpaceshipShopPanelCloseButton()
    {
        spaceshipShopPanel.SetActive(false);
        allySpaceshipsDisplayPanel.SetActive(true);
    }

    public void OnBuySpaceshipButton()
    {
        Debug.Log("OnBuySpaceshipButton | SelectedSpaceshipType [" + selectedSpaceshipType + "]");
        if(selectedSpaceshipType != null && InfrastructureManager.instance.spaceport != null)
        {
            // Pay spaceship costs
            ResourcesManager.instance.PayResourceAmounts(selectedSpaceshipType.resourceCosts);

            InfrastructureManager.instance.spaceport.GetComponent<Spaceport>().SpawnSpaceshipOfType(selectedSpaceshipType);

            SpaceshipManager.instance.UpdateFleetPointsInfo();

            buySpaceshipButton.SetActive(false);

            BuildInfo();

            spaceshipShopPanel.SetActive(false);
            allySpaceshipsDisplayPanel.SetActive(true);
        }
    }
    
    public void SelectSpaceshipOnShop(SpaceshipManager.SpaceshipType spaceshipType)
    {
        if(spaceshipType != null)
        {
            Debug.Log("SelectSpaceshipOnShop [" + spaceshipType.name + "]");
            selectedSpaceshipType = spaceshipType;
            if(ResourcesManager.instance.CanPayResourceAmounts(spaceshipType.resourceCosts) && SpaceshipManager.instance.HasAvailableFleetPointsNb(selectedSpaceshipType.fleetPointsNeeded))
            {
                buySpaceshipButton.SetActive(true);
                EnableOnlyThisSpaceshipShopItemBorder(spaceshipType);
            }
            else
            {
                buySpaceshipButton.SetActive(false);
                DisableAllSpaceshipShopItemsBorder();
            }
        }
    }

    public void DisableAllSpaceshipShopItemsBorder()
    {
        foreach (GameObject spaceshipShopPanel in spaceshipShopPanelsList)
        {
            spaceshipShopPanel.GetComponent<SpaceshipTypeShopPanel>().DisplayBorderPanel(false);
        }
    }

    public void EnableOnlyThisSpaceshipShopItemBorder(SpaceshipManager.SpaceshipType spaceshipType)
    {
        DisableAllSpaceshipShopItemsBorder();
        spaceshipType.associatedSpaceshipShopItem.GetComponent<SpaceshipTypeShopPanel>().DisplayBorderPanel(true);
    }


}
