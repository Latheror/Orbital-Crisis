using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchManager : MonoBehaviour {

    public static TouchManager instance;

    public float perspectiveZoomSpeed = 0.5f;        // The rate of change of the field of view in perspective mode.
    public float orthoZoomSpeed = 0.5f;              // The rate of change of the orthographic size in orthographic mode.
    public float minFieldOfView = 20;
    public float maxFieldOfView = 100;
    public GameObject topPanel;
    public GameObject bottomPanel;

    public Camera camera;

    public Vector3 lastTouch;

    void Awake(){ 
        if (instance != null){ Debug.LogError("More than one TouchManager in scene !"); return; } instance = this;
    }

	// Use this for initialization
	void Start () {
        camera = Camera.main;
	}
	
	// Update is called once per frame
	void Update () {

        // Debug.Log("TouchCount :" + Input.touchCount);

        // Are we touching the screen ?
        if(Input.touchCount > 0)
        {
            //StyleManager.instance.TouchCountToPlanetColor(Input.touchCount);

            // First element of touch array
            Touch touch1 = Input.GetTouch(0);
            Vector3 touch1Pos = touch1.position;

            //Debug.Log("Touch position : " + touch1Pos)

            // Only one touch
            if(Input.touchCount == 1)
            {
                lastTouch = touch1Pos;

                if(BuildingManager.instance.buildingState == BuildingManager.BuildingState.BuildingSelected || BuildingManager.instance.buildingState == BuildingManager.BuildingState.LocationSelected)
                {
                    // The touch is not on the menu panels
                    if(IsTouchWithinGameArea(lastTouch))
                    {
                        Debug.Log("Touching the screen while building prefab is selected | Displaying a building preview ");
                        BuildingManager.instance.SelectBuildingLocation();
                        //BuildingManager.instance.DisplayBuildingPreview();
                    }
                }
                else if(BuildingManager.instance.buildingState == BuildingManager.BuildingState.Default)
                {
                    // Touching while in default state
                    if(IsTouchWithinGameArea(lastTouch))
                    {
                        //Debug.Log("1 Touch during default state.");
                        if (GameManager.instance.selectionState == GameManager.SelectionState.SpaceShipSelected)
                        {
                            if(SpaceshipManager.instance.selectedSpaceship != null)
                            {
                                Vector3 destPos = GeometryManager.instance.GetLocationFromTouchPointOnPlanetPlane(lastTouch);
                                if (! GeometryManager.instance.IsTouchWithinSpaceshipInfoPanelArea(destPos))
                                {

                                    Debug.Log("DestPos: " + destPos);
                                    SpaceshipManager.instance.selectedSpaceship.GetComponent<Spaceship>().SetManualDestination(destPos);
                                }
                            }
                        }
                        else
                        {

                            // Cast a ray
                            Ray ray = Camera.main.ScreenPointToRay(Input.GetTouch(0).position);
                            RaycastHit hit;
                            Debug.DrawRay(ray.origin, ray.direction * 300, Color.yellow, 100f);
                            if (Physics.Raycast(ray, out hit))
                            {
                                Debug.Log(hit.transform.name);
                                if (hit.collider != null)
                                {

                                    GameObject touchedObject = hit.transform.gameObject;
                                    Debug.Log("Touched " + touchedObject.transform.name);
                                    switch (hit.collider.gameObject.tag)
                                    {
                                        case ("meteor"):
                                            {
                                                Debug.Log("Touched a meteor !");
                                                //hit.collider.gameObject.GetComponent<Meteor>().TouchedByPlayer();
                                                break;
                                            }
                                        case ("meteorColliderHolder"):
                                            {
                                                Debug.Log("Touched a meteor collider holder !");
                                                hit.collider.gameObject.transform.parent.GetComponent<Meteor>().TouchedByPlayer();
                                                break;
                                            }
                                        case ("spaceship"):
                                            {
                                                Debug.Log("Touched a spaceship !");
                                                hit.collider.gameObject.GetComponent<Spaceship>().Select(true);
                                                break;
                                            }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        // Pinch to zoom
        if(Input.touchCount == 2)
        {
            // Store both touches.
            Touch touchZero = Input.GetTouch(0);
            Touch touchOne = Input.GetTouch(1);

            // Find the position in the previous frame of each touch.
            Vector2 touchZeroPrevPos = touchZero.position - touchZero.deltaPosition;
            Vector2 touchOnePrevPos = touchOne.position - touchOne.deltaPosition;

            // Find the magnitude of the vector (the distance) between the touches in each frame.
            float prevTouchDeltaMag = (touchZeroPrevPos - touchOnePrevPos).magnitude;
            float touchDeltaMag = (touchZero.position - touchOne.position).magnitude;

            // Find the difference in the distances between each frame.
            float deltaMagnitudeDiff = prevTouchDeltaMag - touchDeltaMag;

            // If the camera is orthographic...
            if (camera.orthographic)
            {
                // ... change the orthographic size based on the change in distance between the touches.
                camera.orthographicSize += deltaMagnitudeDiff * orthoZoomSpeed;

                // Make sure the orthographic size never drops below zero.
                camera.orthographicSize = Mathf.Max(camera.orthographicSize, 0.1f);
            }
            // If the camera is in perspective
            else
            {
                // Otherwise change the field of view based on the change in distance between the touches.
                camera.fieldOfView += deltaMagnitudeDiff * perspectiveZoomSpeed;

                // Clamp the field of view to make sure it's between 0 and 180.
                camera.fieldOfView = Mathf.Clamp(camera.fieldOfView, minFieldOfView, maxFieldOfView);
            }
        }   
    }


    public bool IsTouchWithinGameArea(Vector3 touchPos)
    {
        Debug.Log("Touchpos Y: " + touchPos.y);
        Debug.Log("TopPanel Height: " + topPanel.GetComponent<RectTransform>().sizeDelta.y);
        Debug.Log("BottomPanel Height: " + bottomPanel.GetComponent<RectTransform>().sizeDelta.y);
        Debug.Log("Screen Hight: " + Screen.height);
        return (((touchPos.y) >= bottomPanel.GetComponent<RectTransform>().sizeDelta.y)) && ((touchPos.y) <= (Screen.height - topPanel.GetComponent<RectTransform>().sizeDelta.y));
    }


}
