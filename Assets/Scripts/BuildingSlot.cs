using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingSlot : MonoBehaviour {

	public Building attachedBuilding = null;

    public Color defaultColor;
    public Color selectionColor;
    public float angle;
    public bool hasBuilding = false;


    public void ChangeColor(Color color)
    {
        gameObject.GetComponent<Renderer>().material.color = color;
    }

    public void SetDefaultColor()
    {
        ChangeColor(defaultColor);
    }

    public void SetSelectionColor()
    {
        ChangeColor(selectionColor);
    }

    public void SetAngle(float angle)
    {
        this.angle = angle;
    }

    public void SetBuilding(Building building)
    {
        hasBuilding = true;
        attachedBuilding = building;
    }

    public bool CanBuildHere()
    {
        return (hasBuilding == false && attachedBuilding == null);
    }

}
