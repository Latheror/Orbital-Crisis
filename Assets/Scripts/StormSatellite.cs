using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StormSatellite : Building {

    [Header("Settings")]
    public int maxChainTargetsNb = 5;
    public GameObject firstTarget;
    public List<GameObject> targets;
    public float cooldownTime = 5f;
    public float rangeBetweenTargets = 100f;
    public bool finishedDrawing = true;
    public float damagePower = 5f;
    public GameObject head;
    public GameObject shootingPoint;
    public float rotationSpeed = 50f;
    public bool cooldownReached = true;
    public float updateTargetDelay = 0.2f;

    [Header("Tier 2")]
    public float damagePower_tier_2 = 7f;
    public float range_tier_2 = 200f;
    public float rangeBetweenTargets_tier_2 = 200f;
    public float energyConsumption_tier_2 = 20f;
    public int maxChainTargetsNb_tier_2 = 6;
    public float cooldownTime_tier_2 = 4f;

    [Header("Tier 3")]
    public float damagePower_tier_3 = 10f;
    public float range_tier_3 = 300f;
    public float rangeBetweenTargets_tier_3 = 300f;
    public float energyConsumption_tier_3 = 30f;
    public int maxChainTargetsNb_tier_3 = 7;
    public float cooldownTime_tier_3 = 3f;

    // Use this for initialization
    void Start () {
        firstTarget = null;
        cooldownReached = true;
        InvokeRepeating("UpdateTarget", 0f, updateTargetDelay);
    }

    // Update is called once per frame
    void Update () {
		
	}

    public void UpdateTarget()
    {
        GameObject closestTarget = GetClosestMeteorFromObject(gameObject, range);
        if (cooldownReached && closestTarget != null)
        {
            firstTarget = closestTarget;
            //Debug.Log("StormSatellite: New target set");

            BuildTargetList();

            StartAttack();
        }
    }

    public void StartAttack()
    {
        if(hasEnoughEnergy && finishedDrawing )
        {
            cooldownReached = false;
            //Debug.Log("StormSatellite | StartAttack");

            UpdateTarget();
            RotateCanonTowardsTarget();

            if (targets.Count > 0)
            {
                StartCoroutine("DrawLineRenderer");
            }
        }
    }

    public void BuildTargetList()
    {
        //Debug.Log("Building target list");
        targets = new List<GameObject>();
        if(firstTarget != null)
        {
            GameObject currentChainTarget = firstTarget;
            GameObject nextChainTarget = null;
            for(int i=0; i<maxChainTargetsNb; i++)
            {
                nextChainTarget = GetClosestMeteorFromObject(currentChainTarget, rangeBetweenTargets);
                if(nextChainTarget != null)
                {
                    targets.Add(nextChainTarget);
                    currentChainTarget = nextChainTarget;
                }
                else
                {
                    break;
                }
            }
            //Debug.Log("Finished building target list. Nb of targets: " + targets.Count);
        }
    }

    public GameObject GetClosestMeteorFromObject(GameObject reference, float range)
    {
        float minDist = Mathf.Infinity;
        GameObject closestTarget = null;

        foreach (GameObject meteor in MeteorsManager.instance.meteorsList)
        {
            float dist = Vector3.Distance(reference.transform.position, meteor.transform.position);
            if (dist < minDist && dist < range && !IsTargetAlreadyPresentInList(meteor))
            {
                closestTarget = meteor;
                minDist = dist;
            }
        }

        return closestTarget;
    }

    IEnumerator DrawLineRenderer()
    {
        finishedDrawing = false;
        cooldownReached = false;
        LineRenderer lr = GetComponent<LineRenderer>();

        if (targets.Count > 0)
        {
            lr.positionCount = 1;
            lr.SetPosition(0, shootingPoint.transform.position);

            //Debug.Log("StormSatellite | Drawing Line Renderer with [" + (targets.Count + 1) + "] points.");

            lr.enabled = true;
            
            for (int i = 0; i < targets.Count; i++)
            {
                if(targets[i] != null)
                {
                    GameObject chainTarget = targets[i];
                    lr.positionCount++;
                    lr.SetPosition(i + 1, chainTarget.transform.position);
                    chainTarget.GetComponent<Meteor>().DealDamage(damagePower);
                    yield return new WaitForSeconds(.2f);
                }
                else
                {
                    break;
                }
            }
        }

        lr.enabled = false;
        yield return new WaitForSeconds(cooldownTime);
        finishedDrawing = true;
        cooldownReached = true;
    }

    public bool IsTargetAlreadyPresentInList(GameObject t)
    {
        return (targets.Contains(t));
    }

    protected void RotateCanonTowardsTarget()
    {
        if (targets.Count > 0 && targets[0] != null)
        {
            float deltaX = (targets[0].transform.position.x - transform.position.x);
            float deltaY = (targets[0].transform.position.y - transform.position.y);

            float angle = GeometryManager.GetRadAngleFromXY(deltaX, deltaY);

            //Debug.Log("StormSatellite | Angle to meteor: " + angle);

            // To degree
            angle = GeometryManager.RadiansToDegrees(angle);
            angle = 360 - angle + buildingSpotAngleDeg;

            Debug.Log("Angle: " + angle);
            head.transform.localEulerAngles = new Vector3(0, 0, angle);
        }
    }

    public override void ApplyCurrentTierSettings()
    {
        //Debug.Log("ApplyCurrentTierSettings | STORM SATELLITE | CurrentTier: " + currentTier);
        switch (currentTier)
        {
            case 2:
            {
                damagePower = damagePower_tier_2;
                range = range_tier_2;
                rangeBetweenTargets = rangeBetweenTargets_tier_2;
                energyConsumption = energyConsumption_tier_2;
                maxChainTargetsNb = maxChainTargetsNb_tier_2;
                cooldownTime = cooldownTime_tier_2;
                break;
            }
            case 3:
            {
                damagePower = damagePower_tier_3;
                range = range_tier_3;
                rangeBetweenTargets = rangeBetweenTargets_tier_3;
                energyConsumption = energyConsumption_tier_3;
                maxChainTargetsNb = maxChainTargetsNb_tier_3;
                cooldownTime = cooldownTime_tier_3;
                break;
            }
        }
    }
}
