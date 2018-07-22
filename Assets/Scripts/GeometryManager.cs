using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeometryManager : MonoBehaviour {

    public static GeometryManager instance;
    void Awake(){ 
        if (instance != null){ Debug.LogError("More than one MeteorsManager in scene !"); return; } instance = this;
    }

    public GameObject mainPlanet;

    // Angle in radians
    public float GetRadAngleFromXY(float x, float y)
    {
        float angle = 0f;
        if(x >= 0)
        {
            angle = Mathf.Atan(y / x);
        }
        else
        {
            if(y >= 0)
            {
                angle = Mathf.PI + Mathf.Atan(y / x);
            }
            else
            {
                angle = Mathf.Atan(y / x) - Mathf.PI;
            }
        }
        return angle;
    }

    public float RadiansToDegrees(float radian)
    {
        return radian * 180 / (Mathf.PI);
    }



    public float NormalizeRadianAngle(float radianAngle)
    {
        if (radianAngle >= Mathf.PI)
        {
            radianAngle -= 2 * Mathf.PI;
        }
        else 
        if (radianAngle <= - Mathf.PI)
        {
            radianAngle += 2 * Mathf.PI;
        }
        return radianAngle;
    }

    public bool IsAngleInRange(float centerAngle, float rangeAngle, float angleToCompare)
    {
        Debug.Log("IsAngleInRange | CenterAngle: " + centerAngle +" | RangeAngle: " + rangeAngle + " | AngleToCompare: " + angleToCompare);
        if(rangeAngle >= 0)
        {
            centerAngle += Mathf.PI;
            angleToCompare += Mathf.PI;
            Debug.Log("IsAngleInRange | CenterAnglePlusPi: " + centerAngle + " | AngleToComparePlusPi: " + angleToCompare);
            return ((angleToCompare <= centerAngle + rangeAngle/2) && (angleToCompare >= centerAngle - rangeAngle/2));
        }
        else
        {   
            Debug.Log("ERROR | IsAngleInRange | rangeAngle is negative !");
            return false;
        }
    }

}
