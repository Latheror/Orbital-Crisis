using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour {

	public GameObject target;
    public float speed = 50f;
    public float damage = 10;

    void Update()
    {
        ChaseTarget();
    }

    public void SetTarget(GameObject newTarget)
    {
        this.target = newTarget;
    }

    public void ChaseTarget()
    {
        if(target != null)
        {
            transform.position = Vector3.MoveTowards(transform.position, target.transform.position, speed * Time.deltaTime);
        }
        else
        {
            // The target has already been destroyed
            DestroyBullet();
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



}
