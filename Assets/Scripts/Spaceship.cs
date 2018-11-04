using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Spaceship : MonoBehaviour {

    [Header("Main")]
    public bool isActivated;
    public bool isInAutomaticMode;
    public bool selected = false;
    public bool isAllied = false;
    public float maxHealth = 100f;
    public float healthPoints = 100f;
    public bool hasShield;
    public float shieldPoints = 100f;
    public float maxShield = 100f;
    public float shieldRegenerationDelay = 3f;
    public float shieldRegenerationAmount = 5f;
    public GameObject homeSpaceport;

    [Header("Movement")]
    public float movementSpeed = 100f;
    public float rotationSpeed = 50f;
    public Vector3 manualDestination;
    public bool manualDestinationReached = false;
    public float manualDestinationDelta = 20f;
    public Vector3 tempDestination;

    [Header("Parts")]
    public GameObject[] shootingPoints;
    public GameObject trailOrigin1;

    [Header("Attack")]
    public GameObject target;
    public GameObject previousTarget;
    public float attackDistance = 20;
    public bool pulseFinished = true;
    public float pulsePeriod = 1f;
    public float firePeriod = 1f;
    public float laserSpatialLength = 10f;
    public float damagePower = 20;

    [Header("UI")]
    //public GameObject infoPanel;
    //public TextMeshProUGUI infoPanelModeText;
    //public GameObject infoPanelModeButton;
    public GameObject healthBarPanel;
    public GameObject healthPointsBar;
    public GameObject shieldBarPanel;
    public GameObject shieldPointsBar;
    //public Color infoPanelAutoModeColor = Color.green;
    //public Color infoPanelManualModeColor = Color.red;

    // Use this for initialization
    void Start () {
        target = null;
        isActivated = true;
        healthPoints = maxHealth;
        manualDestination = transform.position;
        manualDestinationReached = true;
        isInAutomaticMode = true;
        //infoPanel.SetActive(false);
        SetStartingMode();
	}
	
	// Update is called once per frame
	void Update () {

    }

    protected virtual void UpdateTarget(){ }
    protected virtual void HandleMovements() { }
    protected virtual void AttackTarget() { }

    public bool IsTargetInRange()
    {
        bool isInRange = false;
        if(target != null)
        {
            isInRange = (Vector3.Distance(transform.position, target.transform.position) <= attackDistance);
        }
        return isInRange; 
    }

    public bool IsTargetInRangeWithDelta()
    {
        return (Vector3.Distance(transform.position, target.transform.position) <= attackDistance * 2);
    }

    public void RotateTowardsTarget()
    {
        if(target != null)
        {
            Vector3 targetDir = target.transform.position - transform.position;
            float rotationStep = rotationSpeed * Time.deltaTime;
            Vector3 newDir = Vector3.RotateTowards(transform.forward, targetDir, rotationStep, 0.0f);
            transform.rotation = Quaternion.LookRotation(newDir);
        }
    }

    public void RotateTowardsTempDest()
    {
        Vector3 targetDir = tempDestination - transform.position;
        float rotationStep = rotationSpeed * Time.deltaTime;
        Vector3 newDir = Vector3.RotateTowards(transform.forward, targetDir, rotationStep, 0.0f);
        transform.rotation = Quaternion.LookRotation(newDir);
    }

    protected void RotateTowardsManualDestination()
    {
        Vector3 destDir = manualDestination - transform.position;
        float rotationStep = rotationSpeed * Time.deltaTime;
        Vector3 newDir = Vector3.RotateTowards(transform.forward, destDir, rotationStep, 0.0f);
        transform.rotation = Quaternion.LookRotation(newDir);
    }

    protected IEnumerator FireLasersRoutine()
    {
        //Debug.Log("Firing !");

        bool laserReachedTarget = false;
        pulseFinished = false;
        float elapsedTime = 0f;
        float distanceRatio = 0f;

        while (!laserReachedTarget && target != null)
        {
            // Start firing
            elapsedTime += Time.deltaTime;
            distanceRatio = elapsedTime / pulsePeriod;

            //Debug.Log("Distance ratio: " + distanceRatio);

            foreach (GameObject shootingPoint in shootingPoints)
            {
                Vector3 shootingPos = shootingPoint.transform.position;
                Vector3 posDiff = target.transform.position - shootingPos;
                Vector3 middleLaserPos = (shootingPos + (posDiff * distanceRatio));

                Vector3 leftLaserPos = (shootingPos + (posDiff * distanceRatio * 0.9f));
                Vector3 rightLaserPos = Vector3.zero;

                if ((Vector3.Distance(shootingPos, shootingPos + (posDiff * distanceRatio * 1.1f))) < (Vector3.Distance(shootingPos, target.transform.position)))
                {
                    rightLaserPos = (shootingPos + (posDiff * distanceRatio * 1.1f));
                }
                else
                {
                    rightLaserPos = target.transform.position;
                }

                LineRenderer lr = shootingPoint.GetComponent<LineRenderer>();
                lr.SetPosition(0, leftLaserPos);
                lr.SetPosition(1, rightLaserPos);

            }

            EnableLasers(true);

            // Wait and stop firing
            yield return new WaitForSeconds(pulsePeriod /5);
            EnableLasers(false);

            if (elapsedTime >= pulsePeriod)
            {
                laserReachedTarget = true;
            }
        }

        // Deal damage to target
        DealDamageToTarget();

        yield return new WaitForSeconds(firePeriod);

        pulseFinished = true;
    }

    protected void DealDamageToTarget()
    {
        if (target != null)
        {
            if(target.CompareTag("meteor"))   // Target is a meteor
            {
                target.GetComponent<Meteor>().DealDamage(damagePower);
            }
            else if(target.CompareTag("spaceship") || target.CompareTag("enemy"))
            {
                target.GetComponent<Spaceship>().TakeDamage(damagePower);
            }
        }
    }

    protected void TakeDamage(float damage)
    {
        if(hasShield)
        {
            float damageTakenByShield = AbsorbDamageInShield(damage);
            if (damage > damageTakenByShield)    // Shield down to zero, decrease healthpoints
            {
                float remainingDamage = damage - damageTakenByShield;
                DecreaseHealthPoints(remainingDamage);
            }
        }
        else
        {
            DecreaseHealthPoints(damage);
        }

        if(healthPoints <= 0)
        {
            DestroySpaceship();
        }
    }

    public void Heal(float healingPower)
    {
        healthPoints = Mathf.Min(maxHealth, healthPoints + healingPower);
        UpdateHealthBar();
        SpaceshipInfoPanel.instance.UpdateInfo();
    }

    protected void UpdateHealthBar()
    {
        if(healthBarPanel != null && healthPointsBar != null)
        {
            float healthBarPanelWidth = healthBarPanel.GetComponent<RectTransform>().rect.width;

            float healthRatio = healthPoints / maxHealth;

            RectTransform healthPointsBarRectTransform = healthPointsBar.GetComponent<RectTransform>();
            healthPointsBarRectTransform.sizeDelta = new Vector2(healthBarPanelWidth * healthRatio, healthPointsBarRectTransform.sizeDelta.y);
        }
    }

    protected void UpdateShieldBar()
    {
        if (shieldBarPanel != null && shieldPointsBar != null)
        {
            float shieldBarPanelWidth = shieldBarPanel.GetComponent<RectTransform>().rect.width;

            float shieldRatio = shieldPoints / maxShield;

            RectTransform shieldPointsBarRectTransform = shieldPointsBar.GetComponent<RectTransform>();
            shieldPointsBarRectTransform.sizeDelta = new Vector2(shieldBarPanelWidth * shieldRatio, shieldPointsBarRectTransform.sizeDelta.y);
        }
    }

    protected virtual void DestroySpaceship() { }

    // Not used anymore
    private void SetLasersPositions(Vector3 pos11, Vector3 pos12, Vector3 pos21, Vector3 pos22)
    {
        //foreach (GameObject shootingPoint in shootingPoints)
        //{
        LineRenderer lr1 = shootingPoints[0].GetComponent<LineRenderer>();
        lr1.SetPosition(0, pos11);
        lr1.SetPosition(1, pos12);

        LineRenderer lr2 = shootingPoints[1].GetComponent<LineRenderer>();
        lr2.SetPosition(0, pos21);
        lr2.SetPosition(1, pos22);
        //}
    }


    protected void EnableLasers(bool enable)
    {
        foreach (GameObject shootingPoint in shootingPoints)
        {
            LineRenderer lr = shootingPoint.GetComponent<LineRenderer>();
            if (enable) {
                lr.enabled = true;
            }
            else {
                lr.enabled = false;
            }
        }
    }

    public void Select(bool select)
    {
        this.selected = select;
        if(select)
        {
            SpaceshipManager.instance.SelectSpaceship(this.gameObject);
        }
        else
        {
            SpaceshipManager.instance.DeselectSpaceship();
        }
    }

    public void Highlight()
    {

    }

    public void SwitchMode()
    {
        isInAutomaticMode = ! isInAutomaticMode;
        if(!isInAutomaticMode)
        {
            manualDestination = transform.position;           
        }
        SpaceshipInfoPanel.instance.UpdateModeDisplay();
        Debug.Log("Switching Mode !");
    }

    public void SetManualDestination(Vector3 dest)
    {
        manualDestination = dest;
    }

    public bool IsCloseEnoughToDestination()
    {
        return (Vector3.Distance(transform.position, manualDestination) < manualDestinationDelta);
    }

    public void InfoPanelCloseButtonActions()
    {
        SpaceshipManager.instance.DeselectSpaceship();
    }

    public void SetStartingMode()
    {
        isInAutomaticMode = true;
        SpaceshipInfoPanel.instance.UpdateModeDisplay();
    }

    public void DecreaseHealthPoints(float amount)
    {
        healthPoints = Mathf.Max(0f, healthPoints - amount);
        UpdateHealthBar();
        SpaceshipInfoPanel.instance.UpdateInfo();
    }

    public void IncreaseHealthPoints(float amount)
    {
        healthPoints = Mathf.Min(maxHealth, healthPoints + amount);
        UpdateHealthBar();
        SpaceshipInfoPanel.instance.UpdateInfo();
    }

    public void DecreaseShieldPoints(float amount)
    {
        shieldPoints = Mathf.Max(0f, shieldPoints - amount);
        UpdateShieldBar();
        SpaceshipInfoPanel.instance.UpdateInfo();
    }

    public void IncreaseShieldPoints(float amount)
    {
        shieldPoints = Mathf.Min(maxShield, shieldPoints + amount);
        UpdateShieldBar();
        SpaceshipInfoPanel.instance.UpdateInfo();
    }

    public void RegenerateShield()
    {
        Debug.Log("RegenerateShield");
        if(shieldPoints < maxShield)
        {
            IncreaseShieldPoints(shieldRegenerationAmount);
        }
    }

    public float AbsorbDamageInShield(float amount)
    {
        float shieldPointsBefore = shieldPoints;
        DecreaseShieldPoints(amount);
        float shieldPointsAfter = shieldPoints;
        return (shieldPointsBefore - shieldPointsAfter);
    }
}
