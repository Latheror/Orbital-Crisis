using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeometryManager : MonoBehaviour {

    public static GeometryManager instance;
    void Awake(){ 
        if (instance != null){ Debug.LogError("More than one MeteorsManager in scene !"); return; } instance = this;
    }

    public GameObject mainPlanet;
    public float circleFactor = 100f;

    // Angle in radians
    public static float GetRadAngleFromXY(float x, float y)
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
        //Debug.Log("IsAngleInRange | CenterAngle: " + centerAngle +" | RangeAngle: " + rangeAngle + " | AngleToCompare: " + angleToCompare);
        if(rangeAngle >= 0)
        {
            // DOESNT WORK !
            if(centerAngle <= 0)
            {
                centerAngle = centerAngle + 2*Mathf.PI;
            }
            if(angleToCompare <= 0)
            {
                angleToCompare += 2*Mathf.PI;
            }
            //Debug.Log("IsAngleInRange | CenterAnglePlusPi: " + centerAngle + " | AngleToComparePlusPi: " + angleToCompare);
            return ((angleToCompare <= centerAngle + rangeAngle/2) && (angleToCompare >= centerAngle - rangeAngle/2));
        }
        else
        {   
            Debug.Log("ERROR | IsAngleInRange | rangeAngle is negative !");
            return false;
        }
    }

    public Vector3 GetLocationFromTouchPointOnPlanetPlane(Vector3 touchPos)
    {
        //Debug.Log("GetLocationFromTouchPointOnPlanetPlane | TouchPos: " + touchPos);

        Ray ray = Camera.main.ScreenPointToRay(touchPos);

        Plane planetPlane = new Plane(Vector3.forward, mainPlanet.transform.position);
        float distance = 0;
        Vector3 intersectPointPos = new Vector3(0f, 0f, 0f);

        if (planetPlane.Raycast(ray, out distance))
        {
            intersectPointPos = ray.GetPoint(distance);
        }

        //Debug.Log("GetLocationFromTouchPointOnPlanetPlane | IntersectPointPos: " + intersectPointPos);

        return intersectPointPos;
    }

    public Vector3 GetLocationFromTouchPointOnPlanetPlaneWithOffset(Vector3 touchPos)
    {
        //Debug.Log("GetLocationFromTouchPointOnPlanetPlane | TouchPos: " + touchPos);

        Ray ray = Camera.main.ScreenPointToRay(touchPos);

        Plane planetPlane = new Plane(Vector3.forward, mainPlanet.transform.position);
        float distance = 0;
        Vector3 intersectPointPos = new Vector3(0f, 0f, 0f);

        if (planetPlane.Raycast(ray, out distance))
        {
            intersectPointPos = ray.GetPoint(distance);
        }

        //Debug.Log("GetLocationFromTouchPointOnPlanetPlane | IntersectPointPos: " + intersectPointPos);
        intersectPointPos = new Vector3(intersectPointPos.x, intersectPointPos.y, intersectPointPos.z - 50);

        return intersectPointPos;
    }

    public bool IsTouchWithinSpaceshipInfoPanelArea(Vector3 touchPos)
    {
        float touchPosX = touchPos.x;
        float touchPosY = touchPos.y;
        float margin = 15;
        GameObject infoPanel = SpaceshipManager.instance.currentSelectedSpaceshipInfoPanel;
        float infoPanelLeftBorder = infoPanel.GetComponent<RectTransform>().position.x - infoPanel.GetComponent<RectTransform>().sizeDelta.x / 2;
        float infoPanelRightBorder = infoPanel.GetComponent<RectTransform>().position.x + infoPanel.GetComponent<RectTransform>().sizeDelta.x / 2;
        float infoPanelTopBorder = infoPanel.GetComponent<RectTransform>().position.y + infoPanel.GetComponent<RectTransform>().sizeDelta.y / 2;
        float infoPanelBottomBorder = infoPanel.GetComponent<RectTransform>().position.y - infoPanel.GetComponent<RectTransform>().sizeDelta.y / 2;

        Debug.Log("IsTouchWithinSpaceshipInfoPanelArea | Left: " + infoPanelLeftBorder + " | Right: " + infoPanelRightBorder + " | Top: " + infoPanelTopBorder + " | Bottom: " + infoPanelBottomBorder);


        return ((touchPosX >= (infoPanelLeftBorder - margin)) && (touchPosX <= (infoPanelRightBorder + margin)) && (touchPosY >= (infoPanelBottomBorder - margin)) && (touchPosY <= (infoPanelTopBorder + margin)));
    }

    public Vector3 RandomSpawnPosition()
    {
        Vector2 randomCirclePos = Random.insideUnitCircle.normalized;
        Vector3 pos = new Vector3(randomCirclePos.x * circleFactor, randomCirclePos.y * circleFactor, GameManager.instance.objectsDepthOffset);

        return pos;
    }

}
