using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour {

    [Header("General")]
    public GameObject target;
    public float speed = 50f;
    public float damage = 10f;
    public float rotationSpeed = 10f;

    void Start()
    {
        //ChaseTarget();
        InvokeRepeating("ChaseTarget", 0f, 0.5f);
    }

    public void SetTarget(GameObject newTarget)
    {
        this.target = newTarget;
    }

    public void ChaseTarget()
    {
        if(GameManager.instance.gameState == GameManager.GameState.Default)
        {
            if (target != null)
            {
                transform.position = Vector3.MoveTowards(transform.position, target.transform.position, speed * Time.deltaTime);
                RotateTowardsTarget();
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
            Debug.Log("Bullet hit a meteor !");
            other.gameObject.GetComponent<Meteor>().DealDamage(damage);
            DestroyBullet();
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
