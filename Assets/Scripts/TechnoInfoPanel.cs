using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TechnoInfoPanel : MonoBehaviour {

    public static TechnoInfoPanel instance;
    void Awake()
    {
        if (instance != null) { Debug.LogError("More than one TechnoInfoPanel in scene !"); return; }
        instance = this;
    }

    public TextMeshProUGUI technoNameText;
    public TextMeshProUGUI technoDescriptionText;

    public GameObject unlockTechnoButtonBackground;
    public GameObject unlockTechnoButton;
    public TextMeshProUGUI unlockTechnoButtonText;

    public Color unlockableTechnoColor;
    public Color notUnlockableTechnoColor;

    public Color unlockedTechnoColor;
    public Color notUnlockedTechnoColor;
    public Sprite notUnlockedTechnoSprite;


    public void SetInfo(TechTreeManager.Technology technology)
    {
        Debug.Log("TechoInfoPanel | SetInfo");
        gameObject.SetActive(true);
        technoNameText.text = technology.name.ToString();

        if(technology.unlockedBuildingIndex != 0)   // This technology unlocks a building
        {
            technoDescriptionText.text = BuildingManager.instance.GetBuildingTypeByID(technology.unlockedBuildingIndex).description.ToString();
        }
        else if(technology.unlockedDiskIndex != 0) // This technology unlocks a disk
        {
            string descriptionText = ("Unlocks orbital disk ");
            switch(technology.unlockedDiskIndex)
            {
                case 1:
                {
                    descriptionText += "I";
                    descriptionText += ". Enables building satellites.";
                    break;
                }
                case 2:
                {
                    descriptionText += "II";
                    break;
                }
                case 3:
                {
                    descriptionText += "III";
                    break;
                }
            }
            technoDescriptionText.text = descriptionText;
        }
        else
        {
            technoDescriptionText.text = "";
        }

        SetUnlockButtonParameters(technology);
    }


    public void SetUnlockButtonParameters(TechTreeManager.Technology technology)
    {
        if(technology.available)
        {
            if (technology.unlocked)
            {
                unlockTechnoButtonText.text = "Unlocked !";
                unlockTechnoButtonBackground.GetComponent<Image>().color = unlockableTechnoColor;
                unlockTechnoButton.GetComponent<Image>().sprite = null;
                unlockTechnoButton.GetComponent<Image>().color = unlockedTechnoColor;
            }
            else
            {
                unlockTechnoButtonText.text = "Unlock";
                unlockTechnoButton.GetComponent<Image>().sprite = notUnlockedTechnoSprite;
                unlockTechnoButton.GetComponent<Image>().color = notUnlockedTechnoColor;

                if (TechTreeManager.instance.CanPayTechnology(technology))
                {
                    unlockTechnoButtonBackground.GetComponent<Image>().color = unlockableTechnoColor;
                }
                else
                {
                    unlockTechnoButtonBackground.GetComponent<Image>().color = notUnlockableTechnoColor;
                }
            }
        }
        else
        {
            unlockTechnoButtonText.text = "Not available";
            unlockTechnoButton.GetComponent<Image>().sprite = notUnlockedTechnoSprite;
            unlockTechnoButton.GetComponent<Image>().color = notUnlockedTechnoColor;
            unlockTechnoButtonBackground.GetComponent<Image>().color = notUnlockableTechnoColor;
        }
        
    }

    public void UnlockTechnoButtonClicked()
    {
        Debug.Log("UnlockTechnoButtonClicked");
        TechTreeManager.instance.UnlockSelectedTechnologyRequest();

        SetInfo(TechTreeManager.instance.selectedTechno);
    }

    public void HidePanel()
    {
        gameObject.SetActive(false);
    }


}
