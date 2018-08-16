﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuPlanet : MonoBehaviour {

    public float rotationSpeed = 10f;
    public float reachedPointDelta = 2f;

    //public GameObject shootingPoint1;
    //public GameObject shootingPoint2;
    //public GameObject meteor1;
    //public GameObject meteor2;

    public List<ShootingPointAndMeteor> shootingPointAndMeteorList;
    public List<TranslatingObjectAndPath> translatingObjectAndPathList;

    // Use this for initialization
    void Start() {

    }

    // Update is called once per frame
    void Update() {
        RotatePlanet();
        UpdateLineRenderers();
        HandleTranslatingObjects();
    }

    public void RotatePlanet()
    {
        transform.Rotate(0, rotationSpeed * Time.deltaTime, 0, Space.World);
    }

    public void UpdateLineRenderers()
    {
        foreach (ShootingPointAndMeteor couple in shootingPointAndMeteorList)
        {
            LineRenderer lr_1 = couple.shootingPoint.GetComponent<LineRenderer>();
            if (CanTargetMeteorFromShootingPoint(couple.meteor, couple.shootingPoint))
            {
                lr_1.enabled = true;
                lr_1.SetPositions(new Vector3[] { couple.shootingPoint.transform.position, couple.meteor.transform.position });
            }
            else
            {
                lr_1.enabled = false;
            }
        }
    }

    public bool CanTargetMeteorFromShootingPoint(GameObject meteor, GameObject shootingPoint)
    {
        bool canTarget = false;
        if (meteor != null && shootingPoint != null)
        {
            Vector3 posMeteor = meteor.transform.position;
            Vector3 posShootingPoint = shootingPoint.transform.position;

            Vector3 planetCenter = gameObject.transform.position;
            float planetRadius = gameObject.transform.localScale.x / 2;


            /* a u2 +b u + c = 0
               where:  

               a = (x2 - x1)^2 + (y2 - y1)^2 + (z2 - z1)^2
               b = 2[(x2 - x1)(x1 - x3) + (y2 - y1)(y1 - y3) + (z2 - z1)(z1 - z3)]
               c = x3^2 + y3^2 + z3^2 + x1^2 + y1^2 + z1^2 - 2[x3 x1 + y3 y1 + z3 z1] - r2
            */

            float a = Mathf.Pow((posMeteor.x - posShootingPoint.x), 2) + Mathf.Pow((posMeteor.y - posShootingPoint.y), 2) + Mathf.Pow((posMeteor.z - posShootingPoint.z), 2);
            float b = 2 * ((posMeteor.x - posShootingPoint.x) * (posShootingPoint.x - planetCenter.x) + (posMeteor.y - posShootingPoint.y) * (posShootingPoint.y - planetCenter.y) + (posMeteor.z - posShootingPoint.z) * (posShootingPoint.z - planetCenter.z));
            float c = Mathf.Pow(planetCenter.x, 2) + Mathf.Pow(planetCenter.y, 2) + Mathf.Pow(planetCenter.z, 2) + Mathf.Pow(posShootingPoint.x, 2) + Mathf.Pow(posShootingPoint.y, 2) + Mathf.Pow(posShootingPoint.z, 2) - 2 * (planetCenter.x * posShootingPoint.x + planetCenter.y * posShootingPoint.y + planetCenter.z * posShootingPoint.z) - Mathf.Pow(planetRadius, 2);

            //Debug.Log("A: " + a + " | b: " + b + " | c: " + c);

            // b^2 - 4*a*c
            float delta = Mathf.Pow(b, 2) - (4 * a * c);

            //Debug.Log("Delta: " + delta);

            canTarget = (delta <= 0);
        }
        else
        {
            Debug.Log("Calling CanTargetMeteorFromShootingPoint without meteor and/or shootingPoint set.");
        }
        return canTarget;
    }

    public void HandleTranslatingObjects()
    {
        foreach (TranslatingObjectAndPath group in translatingObjectAndPathList)
        {
            float step = group.translationSpeed * Time.deltaTime;
            if (group.goesToward)
            {
                // We go towards the path end
                group.translatingObject.transform.position = Vector3.MoveTowards(group.translatingObject.transform.position, group.pathEnd.transform.position, step);
                // Check if we reached end
                if (Vector3.Distance(group.translatingObject.transform.position, group.pathEnd.transform.position) <=  reachedPointDelta)
                {
                    group.goesToward = false;
                }
            }
            else
            {
                // We go back towards the path start
                group.translatingObject.transform.position = Vector3.MoveTowards(group.translatingObject.transform.position, group.pathStart.transform.position, step);
                if (Vector3.Distance(group.translatingObject.transform.position, group.pathStart.transform.position) <= reachedPointDelta)
                {
                    group.goesToward = true;
                }
            }
        }
    }

    [System.Serializable]
    public struct ShootingPointAndMeteor
    {
        public GameObject shootingPoint;
        public GameObject meteor;
    }


    [System.Serializable]
    public class TranslatingObjectAndPath
    {
        public GameObject pathStart;
        public GameObject pathEnd;
        public GameObject translatingObject;
        public bool reachedStart = false;
        public bool reachedEnd = false;
        public bool goesToward = false; // 0: Towards end, 1: Towards start
        public float translationSpeed = 20f;
    }
}