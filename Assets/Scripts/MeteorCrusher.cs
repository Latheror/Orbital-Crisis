using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeteorCrusher : Turret
{
    [Header("Parts")]
    public GameObject leftCanon;
    public GameObject rightCanon;
    public GameObject leftShootingPoint;
    public GameObject rightShootingPoint;
    public LineRenderer lr_1;
    public LineRenderer lr_2;

    [Header("Settings")]
    public float meteorApproachSpeed = 5f;

    [Header("Tier 2")]
    public float power_tier_2 = 20f;
    public float range_tier_2 = 200f;
    public float energyConsumption_tier_2 = 25;
    public float meteorApproachSpeed_tier_2 = 8f;

    [Header("Tier 3")]
    public float power_tier_3 = 30f;
    public float range_tier_3 = 300f;
    public float energyConsumption_tier_3 = 40;
    public float meteorApproachSpeed_tier_3 = 12f;

    [Header("Operation")]
    public GameObject target_1;
    public GameObject target_2;
    public bool targets_set = false;

    // Use this for initialization
    void Start()
    {
        buildingLocationType = BuildingLocationType.Planet;

        lr_1 = leftShootingPoint.GetComponent<LineRenderer>();
        lr_2 = rightShootingPoint.GetComponent<LineRenderer>();

        InvokeRepeating("UpdateTarget", 0f, 0.25f);
        InvokeRepeating("LockOnTarget", 0f, 0.01f);
    }

    // Update is called once per frame
    void Update()
    {
        Crush();
    }

    override public void UpdateTarget()
    {
        //Debug.Log("UpdateTarget");
        if (GameManager.instance.gameState == GameManager.GameState.Default)
        {
            if (!targets_set)
            {
                target_1 = null;

                //lr_1.enabled = false;
                //lr_2.enabled = false;

                // Turrets only work if they have the required energy
                if (hasEnoughEnergy && powerOn)
                {
                    //Debug.Log("Laser Turret | Update target");
                    List<GameObject> meteors = MeteorsManager.instance.meteorsList;

                    GameObject biggestEnemy_1 = null;
                    GameObject biggestEnemy_2 = null;

                    float biggestMeteorSize_1 = 0f;

                    // First select biggest meteor
                    foreach (GameObject meteor in meteors)
                    {
                        float meteorSize = meteor.GetComponent<Meteor>().size;
                        //Debug.Log("Meteor found - Distance is : " + distanceToEnemy);
                        if (biggestMeteorSize_1 < meteorSize && CanReachTarget(meteor))
                        {
                            biggestMeteorSize_1 = meteorSize;
                            biggestEnemy_1 = meteor;
                        }
                    }

                    if (biggestEnemy_1 != null)
                    {
                        //Debug.Log("MeteorCrusher found 1 target !");
                        target_1 = biggestEnemy_1;

                        // Select second biggest meteor
                        float biggestMeteorSize_2 = 0f;

                        foreach (GameObject meteor in meteors)
                        {
                            float meteorSize = meteor.GetComponent<Meteor>().size;
                            //Debug.Log("Meteor found - Distance is : " + distanceToEnemy);
                            if (biggestMeteorSize_2 < meteorSize && CanReachTarget(meteor) && (meteor != biggestEnemy_1))
                            {
                                biggestMeteorSize_2 = meteorSize;
                                biggestEnemy_2 = meteor;
                            }
                        }


                        if (biggestEnemy_2 != null)
                        {
                            target_2 = biggestEnemy_2;

                            targets_set = true;

                            //Debug.Log("MeteorCrusher found 2 targets !");
                            //Debug.Log("Target_1 size [" + target_1.GetComponent<Meteor>().size + "] | Target_2 size [" + target_2.GetComponent<Meteor>().size + "]");
                        }

                    }
                    else
                    {
                        lr_1.enabled = false;
                        lr_2.enabled = false;
                    }

                }
                else
                {
                    lr_1.enabled = false;
                    lr_2.enabled = false;
                }
            }
            else
            {
                if (target_1 == null || target_2 == null)
                {
                    targets_set = false;
                }
            }
        }
        
    }

    public void LockOnTarget()
    {
        if(powerOn && target_1 != null && target_2 != null)
        {
            RotateCanonsTowardsTargets();
            ReduceTargetsSpeed();
            //Crush();
        }
        else
        {
            lr_1.enabled = false;
            lr_2.enabled = false;
        }
    }

    public void RotateCanonsTowardsTargets()
    {
        // Canons position
        float leftCanon_X = leftCanon.transform.position.x;
        float leftCanon_Y = leftCanon.transform.position.y;
        float rightCanon_X = rightCanon.transform.position.x;
        float rightCanon_Y = rightCanon.transform.position.y;

        // Targets position
        float target_1_X = target_1.transform.position.x;
        float target_1_Y = target_1.transform.position.y;
        float target_2_X = target_2.transform.position.x;
        float target_2_Y = target_2.transform.position.y;

        float delta_left_X = 0;
        float delta_left_Y = 0;
        float delta_right_X = 0;
        float delta_right_Y = 0;

        // Line renderers 
        lr_1.SetPosition(0, leftShootingPoint.transform.position);
        lr_2.SetPosition(0, rightShootingPoint.transform.position);

        if (GeometryManager.IsObjectLeftToOther(target_1, target_2))
        {
            //Debug.Log("First meteor is left, Second is right");
            lr_1.SetPosition(1, target_1.transform.position);
            lr_2.SetPosition(1, target_2.transform.position);

            delta_left_X = target_1_X - leftCanon_X;
            delta_left_Y = target_1_Y - leftCanon_Y;

            delta_right_X = target_2_X - rightCanon_X;
            delta_right_Y = target_2_Y - rightCanon_Y;
        }
        else
        {
            //Debug.Log("First meteor is right, Second is left");
            lr_1.SetPosition(1, target_2.transform.position);
            lr_2.SetPosition(1, target_1.transform.position);

            delta_left_X = target_2_X - leftCanon_X;
            delta_left_Y = target_2_Y - leftCanon_Y;

            delta_right_X = target_1_X - rightCanon_X;
            delta_right_Y = target_1_Y - rightCanon_Y;
        }

        float left_angle = GeometryManager.GetRadAngleFromXY(delta_left_X, delta_left_Y);
        float right_angle = GeometryManager.GetRadAngleFromXY(delta_right_X, delta_right_Y);

        // To degree
        left_angle = left_angle * 180 / Mathf.PI;
        right_angle = right_angle * 180 / Mathf.PI;

        // Take building spot angle into account
        left_angle -= buildingSpotAngleDeg;
        right_angle -= buildingSpotAngleDeg;

        Debug.Log("LeftAngle [" + left_angle + "] | RightAngle [" + right_angle);

        leftCanon.transform.localEulerAngles = new Vector3(0, left_angle, 0);
        rightCanon.transform.localEulerAngles = new Vector3(0, right_angle, 0);

        lr_1.enabled = true;
        lr_2.enabled = true;
    }

    public void ReduceTargetsSpeed()
    {
        //target_1.GetComponent<Meteor>().SetPartialSpeeds(.2f,.2f);
        //target_2.GetComponent<Meteor>().SetPartialSpeeds(.2f, .2f);

        target_1.GetComponent<Meteor>().SetPartialSpeedsWithMax(.1f, meteorApproachSpeed/8, .1f, meteorApproachSpeed/8);
        target_2.GetComponent<Meteor>().SetPartialSpeedsWithMax(.1f, meteorApproachSpeed/8, .1f, meteorApproachSpeed/8);
    }

    public void Crush()
    {
        if (powerOn && target_1 != null && target_2 != null)
        {
            float step = meteorApproachSpeed * Time.deltaTime;
            target_1.transform.position = Vector3.MoveTowards(target_1.transform.position, target_2.transform.position, step);
            target_2.transform.position = Vector3.MoveTowards(target_2.transform.position, target_1.transform.position, step);

            float distance = Vector3.Distance(target_1.transform.position, target_2.transform.position);
            //Debug.Log("Distance: " + distance);
            if (distance < 10f)
            {
                target_1.GetComponent<Meteor>().TakeDamage(200);
                target_2.GetComponent<Meteor>().TakeDamage(200);

                targets_set = false;

                lr_1.enabled = false;
                lr_2.enabled = false;
            }
        }
    }

    public override void ApplyCurrentTierSettings()
    {
        Debug.Log("ApplyCurrentTierSettings | METEOR CRUSHER | CurrentTier: " + currentTier);
        switch (currentTier)
        {
            case 2:
            {
                power = power_tier_2;
                range = range_tier_2;
                energyConsumption = energyConsumption_tier_2;
                meteorApproachSpeed = meteorApproachSpeed_tier_2;
                break;
            }
            case 3:
            {
                power = power_tier_3;
                range = range_tier_3;
                energyConsumption = energyConsumption_tier_3;
                meteorApproachSpeed = meteorApproachSpeed_tier_3;
                break;
            }
        }
    }
}
