using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : Building {

    [Header("General")]
    public bool offensiveTurret = true;

    [Header("Target")]
    public GameObject previousTarget;
    public GameObject target;
    public float range = 100f;
    public string meteorTag = "meteor";
    public float power = 1f;
    public float healingPower = 1f;
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
        target = null;
        if(hasEnoughEnergy)
        {
            if(offensiveTurret) // Attack turret
            {
                //Debug.Log("Laser Turret | Update target");
                List<GameObject> meteors = MeteorsManager.instance.meteorsList;
                float shortestDistance = Mathf.Infinity;
                GameObject nearestEnemy = null;

                foreach (GameObject meteor in meteors)
                {
                    float distanceToEnemy = Vector3.Distance(transform.position, meteor.transform.position);
                    //Debug.Log("Meteor found - Distance is : " + distanceToEnemy);
                    if (distanceToEnemy < shortestDistance && CanReachTarget(meteor))
                    {
                        shortestDistance = distanceToEnemy;
                        nearestEnemy = meteor;
                    }
                }

                if (nearestEnemy != null)
                {
                    if (target != previousTarget)
                    {
                        if (previousTarget != null)
                        {
                            previousTarget.GetComponent<Meteor>().ResetMeteorSettings();
                        }
                        previousTarget = target;
                    }

                    target = nearestEnemy;
                    Debug.Log("New meteor target set: " + target + " - Distance is: " + shortestDistance);
                }
            }
            else    // Support turret (healing, ...)
            {
                List<GameObject> allieds = SpaceshipManager.instance.alliedSpaceships;
                float shortestDistance = Mathf.Infinity;
                GameObject nearestAlly = null;

                foreach (GameObject ally in allieds)
                {
                    float distanceToAlly = Vector3.Distance(transform.position, ally.transform.position);
                    if (distanceToAlly < shortestDistance && CanReachTarget(ally))
                    {
                        shortestDistance = distanceToAlly;
                        nearestAlly = ally;
                    }
                }

                if (nearestAlly != null)
                {
                    if (target != previousTarget)
                    {
                        previousTarget = target;
                    }
                    target = nearestAlly;
                    Debug.Log("New allied target set: " + target + " - Distance is: " + shortestDistance);
                }
            }
        }
        else
        {
            //Debug.Log("Turret doesn't have enough energy !");
        }
    }


    // Turrets shouldn't be able to shoot through the planet !
    public bool CanReachTarget(GameObject target)
    {
        bool canReach = false;

        if(hasAngleRange == true)
        {
            float targetAngle = GeometryManager.GetRadAngleFromXY(target.transform.position.x, target.transform.position.y);
            float turretAngle = GeometryManager.GetRadAngleFromXY(transform.position.x, transform.position.y);

            Debug.Log("CanReachMeteor | meteorAngle: " + targetAngle + " | turretAngle: " + turretAngle);

            float upperLimitAngle = GeometryManager.instance.NormalizeRadianAngle(turretAngle + angleRange / 2);
            float lowerLimitAngle = GeometryManager.instance.NormalizeRadianAngle(turretAngle - angleRange / 2);

            //Debug.Log("CanReachMeteor | upperLimitAngle: " + upperLimitAngle + " | lowerLimitAngle: " + lowerLimitAngle);

            canReach =  GeometryManager.instance.IsAngleInRange(turretAngle, angleRange, targetAngle);
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
        target.GetComponent<Meteor>().DealDamage(power);
    }

    public void HealTarget()
    {
        Debug.Log("Healing ally.");
        if(!offensiveTurret)
        {
            target.GetComponent<Spaceship>().Heal(healingPower);
        }
        else
        {
            Debug.Log("Can't heal ally. Turret is in offensive mode.");
        }
    }

    protected void RotateCanonTowardsTarget()
    {
        if(target != null)
        {
            float canonX = turretHead.transform.position.x;
            float targetX = target.transform.position.x;
            float canonY = turretHead.transform.position.y;
            float targetY = target.transform.position.y;

            float deltaX = targetX - canonX;
            float deltaY = targetY - canonY;

            //float angle = Mathf.Atan2(deltaY,deltaX);
            float angle = GeometryManager.GetRadAngleFromXY(deltaX, deltaY);

            //Debug.Log("Angle to meteor: " + angle);

            // To degree
            angle = angle * 180 / Mathf.PI - 90;
            // Take building spot angle into account
            angle -= buildingSpotAngle;

            //Debug.Log("Angle: " + angle);

            turretHead.transform.localEulerAngles = new Vector3(angle, 0, 0);


        }
    }
}
