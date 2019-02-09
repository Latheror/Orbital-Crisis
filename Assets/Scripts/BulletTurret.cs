using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletTurret : Turret {

    [Header("Settings")]
    public float bulletSpeed = 100f;
    public float bulletPower = 10f;

    [Header("Tier 2")]
    public float range_tier_2 = 150;
    public float bulletSpeed_tier_2 = 120f;
    public float bulletPower_tier_2 = 15f;
    public float energyConsumption_tier_2 = 25;

    [Header("Tier 3")]
    public float range_tier_3 = 200;
    public float bulletSpeed_tier_3 = 140f;
    public float bulletPower_tier_3 = 20f;
    public float energyConsumption_tier_3 = 40;

    [Header("Prefabs")]
    public GameObject bulletPrefab;

	// Use this for initialization
	void Start () {

        buildingLocationType = BuildingLocationType.Planet;

        InvokeRepeating("UpdateTarget", 0f, 0.05f);
        InvokeRepeating("ShootOnTarget", 0f, 1.5f); 
	}
	
    public void ShootOnTarget()
    {
        if (GameManager.instance.gameState == GameManager.GameState.Default)
        {
            if (hasEnoughEnergy)
            {
                RotateCanonTowardsTarget();

                //Debug.Log("BulletTurret | ShootOnTarget");
                if (target != null)
                {
                    //Debug.Log("BulletTurret | Turret has a meteor target.");

                    // Instantiate a bullet
                    GameObject instantiatedBullet = Instantiate(bulletPrefab, shootingPoint.transform.position, Quaternion.identity);
                    instantiatedBullet.transform.SetParent(gameObject.transform);

                    Bullet bulletScript = instantiatedBullet.GetComponent<Bullet>();
                    bulletScript.SetTarget(target);
                    bulletScript.speed = bulletSpeed;
                    bulletScript.power = bulletPower * (1 + populationBonus);
                    bulletScript.activated = true;

                    // bulletPrefab.GetComponent<Renderer>().material.color = Color.red;

                }
            }
            else
            {
                //Debug.Log("Turret doesn't have enough energy !");
            }
        }
    }

    public override void ApplyCurrentTierSettings()
    {
        Debug.Log("ApplyCurrentTierSettings | LASER TURRET | CurrentTier: " + currentTier);
        switch (currentTier)
        {
            case 2:
            {
                range = range_tier_2;
                bulletSpeed = bulletSpeed_tier_2;
                bulletPower = bulletPower_tier_2;
                energyConsumption = energyConsumption_tier_2;
                break;

            }
            case 3:
            {
                range = range_tier_3;
                bulletSpeed = bulletSpeed_tier_3;
                bulletPower = bulletPower_tier_3;
                energyConsumption = energyConsumption_tier_3;
                break;
            }
        }
    }
}
