using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Spaceship : MonoBehaviour {

    [Header("Main")]
    public bool isActivated;
    public bool isInAutomaticMode;
    public float movementSpeed = 100f;
    public float rotationSpeed = 50f;
    public bool selected = false;
    public Vector3 manualDestination = Vector3.zero;
    public bool manualDestinationReached = false;
    public float manualDestinationDelta = 20f;

    [Header("Parts")]
    public GameObject[] shootingPoints;

    [Header("Attack")]
    public GameObject target;
    public GameObject previousTarget;
    public float attackDistance = 20;
    public bool pulseFinished = true;
    public float pulsePeriod = 1f;
    public float laserSpatialLength = 10f;
    public float damagePower = 20;

    [Header("UI")]
    public GameObject infoPanel;
    public TextMeshProUGUI infoPanelModeText;
    public GameObject infoPanelModeButton;
    public Color infoPanelAutoModeColor = Color.green;
    public Color infoPanelManualModeColor = Color.red;

    // Use this for initialization
    void Start () {
        target = null;
        isActivated = true;
        isInAutomaticMode = true;
        infoPanel.SetActive(false);
        SetStartingMode();
	}
	
	// Update is called once per frame
	void Update () {
        UpdateTarget();
        HandleMovements();
        AttackTarget();
    }

    void UpdateTarget() {

        // Turrets only work if they have the required energy
        if (isActivated)
        {
            //Debug.Log("Laser Turret | Update target");
            List<GameObject> meteors = MeteorsManager.instance.meteorsList;
            float shortestDistance = Mathf.Infinity;
            GameObject nearestEnemy = null;

            foreach (GameObject meteor in meteors)
            {
                //Debug.Log("Meteor in meteors.");
                //if (meteor.tag == meteorTag)
                if (meteor.CompareTag("meteor"))
                {
                    //Debug.Log("Meteor has meteor tag");
                    float distanceToEnemy = Vector3.Distance(transform.position, meteor.transform.position);
                    //Debug.Log("Meteor found - Distance is : " + distanceToEnemy);
                    if (distanceToEnemy < shortestDistance)
                    {
                        shortestDistance = distanceToEnemy;
                        nearestEnemy = meteor;
                    }
                }
                else
                {
                    Debug.Log("Error : Meteor doesn't have Meteor tag ! | Meteor has \"" + meteor.tag.ToString() + "\" tag.");
                }
            }

            if (nearestEnemy != null)
            {
                target = nearestEnemy;
                if (target != previousTarget)
                {
                    if (previousTarget != null)
                    {
                        previousTarget.GetComponent<Meteor>().ResetMeteorSettings();
                    }
                    previousTarget = target;
                }
                //Debug.Log("New meteor target set: " + target + " - Distance is: " + shortestDistance);
            }
            

        }
        else
        {
            //Debug.Log("Spaceship isn't activated !");
        }
        
    }

    void HandleMovements()
    {
        if(!isInAutomaticMode)  // Manual Mode
        {
            if (! IsCloseEnoughToDestination())
            {
                // Move towards destination
                transform.position = Vector3.MoveTowards(transform.position, manualDestination, Time.deltaTime * movementSpeed);
            }
            else
            {
                if(target != null && IsTargetInRange())
                {
                    RotateTowardsTarget();
                }
            }
        }
        else  // Automatic mode
        {
            if (target != null)
            {
                if (! IsTargetInRange())
                {
                    // Go closer to target
                    transform.position = Vector3.MoveTowards(transform.position, target.transform.position, Time.deltaTime * movementSpeed);
                }

                RotateTowardsTarget();
            }
        }
    }

    void AttackTarget()
    {
        if (isActivated && target != null)
        {
            if (isInAutomaticMode) // Automatic Mode
            {
                if (IsTargetInRange() && pulseFinished)
                {
                    StartCoroutine(FireLasersRoutine());
                }
            }
            else  // Manual Mode
            {
                if(IsCloseEnoughToDestination() && IsTargetInRange() && pulseFinished)
                {
                    StartCoroutine(FireLasersRoutine());
                }
            }
        }
    }

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

    void RotateTowardsManualDestination()
    {
        Vector3 destDir = manualDestination - transform.position;
        float rotationStep = rotationSpeed * Time.deltaTime;
        Vector3 newDir = Vector3.RotateTowards(transform.forward, destDir, rotationStep, 0.0f);
        transform.rotation = Quaternion.LookRotation(newDir);
    }

    IEnumerator FireLasersRoutine()
    {
        Debug.Log("Firing !");

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
            yield return new WaitForSeconds(pulsePeriod / 5);
            EnableLasers(false);

            if (elapsedTime >= pulsePeriod)
            {
                laserReachedTarget = true;
            }
        }

        // Deal damage to meteor
        if (target != null)
        {
            target.GetComponent<Meteor>().DealDamage(damagePower);
        }

        pulseFinished = true;
    }

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


    private void EnableLasers(bool enable)
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
        isInAutomaticMode = !isInAutomaticMode;
        if(isInAutomaticMode){
            infoPanelModeText.text = "AUTO";
            infoPanelModeButton.GetComponent<Image>().color = infoPanelAutoModeColor;
        }else{
            infoPanelModeText.text = "MANUAL";
            infoPanelModeButton.GetComponent<Image>().color = infoPanelManualModeColor;
        }
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
        infoPanel.SetActive(false);
        SpaceshipManager.instance.DeselectSpaceship();
    }

    public void SetStartingMode()
    {
        isInAutomaticMode = true;
        infoPanelModeText.text = "AUTO";
        infoPanelModeButton.GetComponent<Image>().color = infoPanelAutoModeColor;
    }
}
