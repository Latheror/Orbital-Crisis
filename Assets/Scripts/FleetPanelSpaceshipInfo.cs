using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class FleetPanelSpaceshipInfo : MonoBehaviour {

    [Header("UI")]
    public TextMeshProUGUI healthPointsText;
    public TextMeshProUGUI maxHealthPointsText;
    public TextMeshProUGUI shieldPointsText;
    public TextMeshProUGUI maxShieldPointsText;

    public GameObject healthBarBackground;
    public GameObject healthBar;

    public GameObject shieldBarBackground;
    public GameObject shieldBar;

    [Header("Operation")]
    public GameObject spaceship;

    public void SetSpaceship(GameObject alliedSpaceship)
    {
        spaceship = alliedSpaceship;
    }

    public void UpdateInfo()
    {
        if(spaceship.GetComponent<AllySpaceship>() != null)
        {
            AllySpaceship alliedSpaceship = spaceship.GetComponent<AllySpaceship>();
            healthPointsText.text = alliedSpaceship.healthPoints.ToString();
            maxHealthPointsText.text = alliedSpaceship.maxHealthPoints.ToString();
            shieldPointsText.text = alliedSpaceship.shieldPoints.ToString();
            maxShieldPointsText.text = alliedSpaceship.maxShieldPoints.ToString();

            UpdateShieldBar();
            UpdateHealthBar();
        }
    }

    public void UpdateShieldBar()
    {
        if (shieldBarBackground != null && shieldBar != null)
        {
            float shieldBarBackPanelWidth = shieldBarBackground.GetComponent<RectTransform>().rect.width;
            //Debug.Log("ShieldBarBackPanelWidth [" + shieldBarBackPanelWidth + "]");

            float maxShieldPoints = spaceship.GetComponent<AllySpaceship>().maxShieldPoints;
            float shieldPoints = spaceship.GetComponent<AllySpaceship>().shieldPoints;
            float shieldRatio = shieldPoints / maxShieldPoints;
            //Debug.Log("ShieldRatio [" + shieldRatio + "]");

            RectTransform shieldPointsBarRectTransform = shieldBar.GetComponent<RectTransform>();
            //Debug.Log("shieldPointsBarRectTransform | SizeDeltaX [" + shieldPointsBarRectTransform.sizeDelta.x + "] | SizeDeltaY [" + shieldPointsBarRectTransform.sizeDelta.y + "]");
            shieldPointsBarRectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal,120 * shieldRatio);

        }
    }

    public void UpdateHealthBar()
    {
        if (healthBarBackground != null && healthBar != null)
        {
            float maxHealthPoints = spaceship.GetComponent<AllySpaceship>().maxHealthPoints;
            float healthPoints = spaceship.GetComponent<AllySpaceship>().healthPoints;
            float healthRatio = healthPoints / maxHealthPoints;

            RectTransform healthPointsBarRectTransform = healthBar.GetComponent<RectTransform>();
            healthPointsBarRectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 120 * healthRatio);
        }
    }
}
