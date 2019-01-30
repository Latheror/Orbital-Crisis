using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cruiser : AllySpaceship
{

    [Header("Structure")]
    public List<GameObject> missileShootingPoints;

    [Header("Settings")]
    public float missileCooldownTime = 5f;
    public float missilePower = 20f;
    public float missileSpeed = 50f;

    [Header("Prefabs")]
    public GameObject missilePrefab;

    [Header("Operation")]
    public bool missileCooldownReached = true;

    void FixedUpdate()
    {
        if (GameManager.instance.gameState == GameManager.GameState.Default)
        {
            UpdateTarget();
            HandleMovements();
            AttackTarget();
            AvoidOtherAllies();
            MissileAttack();
        }
    }

    public void MissileAttack()
    {
        if (GameManager.instance.gameState == GameManager.GameState.Default && missileCooldownReached)
        {
            Debug.Log("Cruiser | Missile Attack");
            missileCooldownReached = false;

            if (target != null)
            {
                foreach (GameObject missileShootingPoint in missileShootingPoints)
                {
                    GameObject instantiatedMissile = Instantiate(missilePrefab, missileShootingPoint.transform.position, Quaternion.identity);
                    instantiatedMissile.transform.SetParent(gameObject.transform);

                    Bullet bulletScript = instantiatedMissile.GetComponent<Bullet>();
                    bulletScript.SetTarget(target);
                    bulletScript.speed = missileSpeed;
                    bulletScript.power = missilePower;
                }              
            }

            StartCoroutine(StartMissileCoolDown(missileCooldownTime));
        }
    }

    IEnumerator StartMissileCoolDown(float cooldown)
    {
        while(cooldown > 0f)
        {
            cooldown -= Time.deltaTime;
            yield return null;
        }
        missileCooldownReached = true;
    }
}