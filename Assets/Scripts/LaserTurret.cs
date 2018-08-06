using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserTurret : Turret {

    public GameObject turretBase;
    public GameObject turretBody;
    public GameObject turretHead;
    public GameObject shootingPoint;

	void Start () {

        buildingLocationType = BuildingLocationType.Planet;

        // Some InvokeRepeating are in the turret class !
        InvokeRepeating("UpdateTarget", 0f, 0.5f);
        InvokeRepeating("LockOnTarget", 0f, 0.1f); 
	}


	
    public LaserTurret(string name) :  base(name)
    {
        Debug.Log("LaserTurret constructor");
    }

    public void LockOnTarget()
    {
        if (hasEnoughEnergy)
        {
            RotateCanonTowardsTarget();
            

            LineRenderer lineRenderer = gameObject.GetComponent<LineRenderer>();
            if (meteorTarget != null)
            {
                lineRenderer.enabled = true;
                GameObject target = meteorTarget;
                lineRenderer.SetPosition(0, shootingPoint.transform.position);
                lineRenderer.SetPosition(1, target.transform.position);

                DealDamageToMeteorTarget();
            }
            else
            {
                lineRenderer.enabled = false;
            }

            // Direction to shoot towards
            //Vector3 dir = meteorTarget.transform.position - transform.position;
            //Quaternion lookRotation = Quaternion.LookRotation(dir);
            //Vector3 rotation = Quaternion.Lerp(transform.rotation, lookRotation, Time.deltaTime * 10f);
            //transform.rotation = Quaternion.Euler(0f, rotation.y, 0f);
        }
    }


    // Draw a preview of the turret's range
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, range);
    }

    void RotateCanonTowardsTarget()
    {
        if(meteorTarget != null)
        {
            float canonX = turretHead.transform.position.x;
            float targetX = meteorTarget.transform.position.x;
            float canonY = turretHead.transform.position.y;
            float targetY = meteorTarget.transform.position.y;

            float deltaX = targetX - canonX;
            float deltaY = targetY - canonY;

            float angle = GeometryManager.GetRadAngleFromXY(deltaX, deltaY);
            // To degree
            angle = angle * 180 / Mathf.PI - 90;

            Debug.Log("Angle: " + angle);

            turretHead.transform.localEulerAngles = new Vector3(angle, 0, 0);


        }
    }

}
