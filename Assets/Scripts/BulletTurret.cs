using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletTurret : Turret {

    [Header("Settings")]
    public float bulletSpeed = 100f;

    [Header("Prefabs")]
    public GameObject bulletPrefab;

	// Use this for initialization
	void Start () {

        buildingLocationType = BuildingLocationType.Planet;

        InvokeRepeating("UpdateTarget", 0f, 0.05f);
        InvokeRepeating("ShootOnTarget", 0f, 1.5f); 
	}
	
    public BulletTurret(string name) :  base(name)
    {
        Debug.Log("BulletTurret constructor");
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

                    // bulletPrefab.GetComponent<Renderer>().material.color = Color.red;

                }
                else
                {


                }
            }
            else
            {
                //Debug.Log("Turret doesn't have enough energy !");
            }
        }
    }
}
