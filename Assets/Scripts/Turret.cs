using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : Building {

    [Header("General")]
    public bool offensiveTurret = true;

    [Header("Target")]
    public GameObject previousTarget;
    public GameObject target;
    public string meteorTag = "meteor";
    public float power = 1f;
    public float healingPower = 1f;

    [Header("Parts")]
    public GameObject turretBase;
    public GameObject turretBody;
    public GameObject turretHead;
    public GameObject shootingPoint;

    void Start()
    {
        InvokeRepeating("UpdateTarget", 0f, 0.5f);
    }

    public virtual void UpdateTarget()
    {
        if (GameManager.instance.gameState == GameManager.GameState.Default)
        {
            // Turrets only work if they have the required energy
            target = null;
            if (hasEnoughEnergy)
            {
                if (offensiveTurret) // Attack turret
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
                        target = nearestEnemy;

                        if (previousTarget != null && target != previousTarget)
                        {
                            previousTarget.GetComponent<Meteor>().ResetMeteorSettings();
                            previousTarget = target;
                        }
                        //Debug.Log("New meteor target set: " + target + " - Distance is: " + shortestDistance);
                    }
                }
                else    // Support turret (healing, ...)
                {
                    List<GameObject> allieds = SpaceshipManager.instance.allySpaceships;
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
                        //Debug.Log("New allied target set: " + target + " - Distance is: " + shortestDistance);
                    }
                }
            }
            else
            {
                //Debug.Log("Turret doesn't have enough energy !");
            }
        }
    }

    // Turrets shouldn't be able to shoot through the planet !
    public bool CanReachTarget(GameObject target)
    {
        bool canReachAngle = false;
        bool canReachDistance = false;

        if (GeometryManager.instance.AreObjectsInRange(gameObject, target, range))
        {
            canReachDistance = true;

            if (hasAngleRange == true)
            {
                float targetAngle = GeometryManager.GetRadAngleFromXY(target.transform.position.x, target.transform.position.y);
                //float turretAngle = GeometryManager.GetRadAngleFromXY(transform.position.x, transform.position.y);
                float turretAngle = gameObject.GetComponent<Building>().buildingSpot.GetComponent<BuildingSlot>().angleRad;

                //Debug.Log("CanReachMeteor | meteorAngle: " + targetAngle + " | turretAngle: " + turretAngle);

                float upperLimitAngle = GeometryManager.instance.NormalizeRadianAngle(turretAngle + angleRange / 2);
                float lowerLimitAngle = GeometryManager.instance.NormalizeRadianAngle(turretAngle - angleRange / 2);

                //Debug.Log("CanReachMeteor | upperLimitAngle: " + upperLimitAngle + " | lowerLimitAngle: " + lowerLimitAngle);

                canReachAngle = GeometryManager.instance.IsAngleInRange(turretAngle, angleRange, targetAngle);
            }
            else
            {
                canReachAngle = true;
            }
        }
        else
        {
            canReachDistance = false;
        }

        return (canReachDistance && canReachAngle);
    }

    public void DealDamageToMeteorTarget()
    {
        //Debug.Log("Dealing damage to meteor");
        target.GetComponent<Meteor>().TakeDamage(power);
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
            angle -= buildingSpotAngleDeg;

            //Debug.Log("Angle: " + angle);

            turretHead.transform.localEulerAngles = new Vector3(angle, 0, 0);
        }
    }

    public virtual void ResetPreviousTargetSettings() { }
}
