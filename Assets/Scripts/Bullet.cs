using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour {

    [Header("General")]
    public GameObject target;
    public float speed = 50f;
    public float power = 10f;
    public float rotationSpeed = 10f;

    void Update()
    {
        ChaseTarget();
    }

    public void SetTarget(GameObject newTarget)
    {
        target = newTarget;
    }

    public void ChaseTarget()
    {
        if(GameManager.instance.gameState == GameManager.GameState.Default)
        {
            if (target != null)
            {
                transform.position = Vector3.MoveTowards(transform.position, target.transform.position, speed * Time.deltaTime);
                RotateTowardsTarget();

                // Prevent missiles from being stuck
                if (Vector3.Distance(transform.position, target.transform.position) < .1f)
                {
                    DestroyBullet();
                }
            }
            else
            {
                // The target has already been destroyed
                DestroyBullet();
            }
        }

    }

    public void DestroyBullet()
    {
        Destroy(gameObject);
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "meteor")
        {
            //Debug.Log("Bullet hit a meteor !");
            other.gameObject.GetComponent<Meteor>().TakeDamage(power);
            DestroyBullet();
        }
        else if (other.gameObject.tag == "enemy")
        {
            if(other.gameObject.GetComponent<EnemySpaceship>() != null)
            {
                other.gameObject.GetComponent<EnemySpaceship>().TakeDamage(power);
                DestroyBullet();
            }
        }
    }

    private void RotateTowardsTarget()
    {
        if (target != null)
        {
            Vector3 targetDir = target.transform.position - transform.position;
            float rotationStep = rotationSpeed * Time.deltaTime;
            Vector3 newDir = Vector3.RotateTowards(transform.forward, targetDir, rotationStep, 0.0f);
            transform.rotation = Quaternion.LookRotation(newDir);
        }
    }



}
