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
    public float health = 100f;
    public float shield = 100f;
    public float maxShield = 100f;

    [Header("Movement")]
    public float movementSpeed = 100f;
    public float rotationSpeed = 50f;
    public Vector3 manualDestination;
    public bool manualDestinationReached = false;
    public float manualDestinationDelta = 20f;

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
    //public Color infoPanelAutoModeColor = Color.green;
    //public Color infoPanelManualModeColor = Color.red;

    // Use this for initialization
    void Start () {
        target = null;
        isActivated = true;
        health = maxHealth;
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
        return (Vector3.Distance(transform.position, target.transform.position) <= attackDistance);
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
        health = Mathf.Max(0, health - damage);
        UpdateHealthBar();
        if(health <= 0)
        {
            DestroySpaceship();
        }
        else
        {
            SpaceshipInfoPanel.instance.UpdateInfo();
        }
    }

    public void Heal(float healingPower)
    {
        health = Mathf.Min(maxHealth, health + healingPower);
        UpdateHealthBar();
        SpaceshipInfoPanel.instance.UpdateInfo();
    }

    protected void UpdateHealthBar()
    {
        if(healthBarPanel != null && healthPointsBar != null)
        {
            float healthBarPanelWidth = healthBarPanel.GetComponent<RectTransform>().rect.width;

            float healthRatio = health / maxHealth;

            RectTransform healthPointsBarRectTransform = healthPointsBar.GetComponent<RectTransform>();
            healthPointsBarRectTransform.sizeDelta = new Vector2(healthBarPanelWidth * healthRatio, healthPointsBarRectTransform.sizeDelta.y);

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
        this.isInAutomaticMode = ! this.isInAutomaticMode;
        SpaceshipInfoPanel.instance.UpdateModeDisplay();
        Debug.Log("Switching Mode !");
    }

    public void SetManualDestination(Vector3 dest)
    {
        manualDestination = dest;
        if(! isInAutomaticMode)
        {
            RotateTowardsManualDestination();
        }
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
}
