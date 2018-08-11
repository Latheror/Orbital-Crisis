using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : Building {

    [Header("Target")]
    public GameObject previousTarget;
    public GameObject meteorTarget;
    public float range = 100f;
    public string meteorTag = "meteor";
    public float power = 1f;
    public bool hasAngleRange = true;
    public float angleRange = 3f;

    public GameObject turretBase;
    public GameObject turretBody;
    public GameObject turretHead;
    public GameObject shootingPoint;


    public Turret(string name) :  base(name)
    {
        Debug.Log("LaserTurret constructor");
    }

    void Start()
    {
        // Run a function at a fixed rate
        InvokeRepeating("UpdateTarget", 0f, 0.5f);
        //InvokeRepeating("LockOnTarget", 0f, 0.1f); 
    }


    public void UpdateTarget()
    {
        // Turrets only work if they have the required energy
        if(hasEnoughEnergy)
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
                    if (distanceToEnemy < shortestDistance && CanReachMeteor(meteor))
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
                if(meteorTarget != previousTarget)
                {
                    if(previousTarget != null)
                    {
                        previousTarget.GetComponent<Meteor>().ResetMeteorSettings();
                    }
                    previousTarget = meteorTarget;
                }

                meteorTarget = nearestEnemy;
                Debug.Log("New meteor target set: " + meteorTarget + " - Distance is: " + shortestDistance);
            }
        }
        else
        {
            Debug.Log("Turret doesn't have enough energy !");
        }
    }


    // Turrets shouldn't be able to shoot through the planet !
    public bool CanReachMeteor(GameObject meteor)
    {
        bool canReach = false;

        if(hasAngleRange == true)
        {
            float meteorAngle = GeometryManager.GetRadAngleFromXY(meteor.transform.position.x, meteor.transform.position.y);
            float turretAngle = GeometryManager.GetRadAngleFromXY(transform.position.x, transform.position.y);

            //Debug.Log("CanReachMeteor | meteorAngle: " + meteorAngle + " | turretAngle: " + turretAngle);

            float upperLimitAngle = GeometryManager.instance.NormalizeRadianAngle(turretAngle + angleRange / 2);
            float lowerLimitAngle = GeometryManager.instance.NormalizeRadianAngle(turretAngle - angleRange / 2);

            //Debug.Log("CanReachMeteor | upperLimitAngle: " + upperLimitAngle + " | lowerLimitAngle: " + lowerLimitAngle);

            canReach =  GeometryManager.instance.IsAngleInRange(turretAngle, angleRange, meteorAngle);
        }
        else
        {
            canReach = true;
        }      

        return canReach;
    }

    public void DealDamageToMeteorTarget()
    {
        //Debug.Log("Dealing damage to meteor");
        meteorTarget.GetComponent<Meteor>().DealDamage(power);
    }

    protected void RotateCanonTowardsTarget()
    {
        if(meteorTarget != null)
        {
            float canonX = turretHead.transform.position.x;
            float targetX = meteorTarget.transform.position.x;
            float canonY = turretHead.transform.position.y;
            float targetY = meteorTarget.transform.position.y;

            float deltaX = targetX - canonX;
            float deltaY = targetY - canonY;

            //float angle = Mathf.Atan2(deltaY,deltaX);
            float angle = GeometryManager.GetRadAngleFromXY(deltaX, deltaY);

            Debug.Log("Angle to meteor: " + angle);

            // To degree
            angle = angle * 180 / Mathf.PI - 90;
            // Take building spot angle into account
            angle -= buildingSpotAngle;

            Debug.Log("Angle: " + angle);

            turretHead.transform.localEulerAngles = new Vector3(angle, 0, 0);


        }
    }
}
