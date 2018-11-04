using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchManager : MonoBehaviour {

    [Header("World")]
    public new Camera camera;

    [Header("Settings")]
    public float perspectiveZoomSpeed = 0.5f;        // The rate of change of the field of view in perspective mode.
    public float orthoZoomSpeed = 0.5f;              // The rate of change of the orthographic size in orthographic mode.
    public float minFieldOfView = 20;
    public float maxFieldOfView = 100;
    public float avoidPanelsMargin = 10f;

    [Header("UI")]
    public GameObject topPanel;
    public GameObject bottomPanel;

    [Header("Operation")]
    public Vector3 lastTouch;

    public static TouchManager instance;
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
        if (Input.touchCount > 0)
        {
            //StyleManager.instance.TouchCountToPlanetColor(Input.touchCount);

            // First element of touch array
            Touch touch1 = Input.GetTouch(0);
            Vector3 touch1Pos = touch1.position;

            //Debug.Log("Touch position : " + touch1Pos)

            // Only one touch
            if (Input.touchCount == 1)
            {
                lastTouch = touch1Pos;

                if (BuildingManager.instance.buildingState == BuildingManager.BuildingState.BuildingSelected || BuildingManager.instance.buildingState == BuildingManager.BuildingState.LocationSelected
                    || BuildingManager.instance.buildingState == BuildingManager.BuildingState.BuildingAndLocationSelected)
                {
                    // The touch is not on the menu panels
                    if (IsTouchWithinGameArea(lastTouch))
                    {
                        //Debug.Log("Touching the screen while building prefab is selected | Displaying a building preview ");
                        BuildingManager.instance.SelectBuildingLocation();
                        //BuildingManager.instance.DisplayBuildingPreview();
                    }
                }
                else if (BuildingManager.instance.buildingState == BuildingManager.BuildingState.Default)    // Touching while in default state
                {
                    if (IsTouchWithinGameArea(lastTouch))
                    {
                        // Cast a ray
                        Ray ray = Camera.main.ScreenPointToRay(Input.GetTouch(0).position);
                        RaycastHit hit;

                        bool otherPriorityElementTouched = false;

                        //Debug.DrawRay(ray.origin, ray.direction * 300, Color.yellow, 100f);
                        if (Physics.Raycast(ray, out hit))
                        {
                            //Debug.Log(hit.transform.name);
                            if (hit.collider != null)
                            {
                                GameObject touchedObject = hit.transform.gameObject;
                                //Debug.Log("Touched " + touchedObject.transform.name);
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
                                            otherPriorityElementTouched = true;
                                            break;
                                        }
                                    case ("building"):
                                        {
                                            Debug.Log("Touched a building !");
                                            InfrastructureManager.instance.BuildingTouched(hit.collider.gameObject);
                                            otherPriorityElementTouched = true;
                                            break;
                                        }
                                }
                            }
                        }

                        if (GameManager.instance.selectionState == GameManager.SelectionState.SpaceshipSelected)
                        {
                            if (!otherPriorityElementTouched)
                            {
                                if (SpaceshipManager.instance.selectedSpaceship != null && !SpaceshipManager.instance.selectedSpaceship.GetComponent<Spaceship>().isInAutomaticMode)
                                {
                                    Vector3 touchedPos = GeometryManager.instance.GetLocationFromTouchPointOnPlanetPlane(lastTouch);

                                    //Debug.Log("Setting Manual Destination | DestPos: " + destPos);
                                    if(!GeometryManager.PosWithinPlanetArea(touchedPos))
                                    {
                                        SpaceshipManager.instance.selectedSpaceship.GetComponent<Spaceship>().SetManualDestination(touchedPos);
                                    }
                                }
                            }
                        }
                        else
                        {

                            
                        }
                    }
                }
            }
            else if (Input.touchCount == 2)        // Pinch to zoom
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
    }


    public bool IsTouchWithinGameArea(Vector3 touchPos)
    {
        bool betweenTopAndBottomPanels = false;
        bool avoidsRightPanel = false;

        //Debug.Log("Touchpos Y: " + touchPos.y);
        //Debug.Log("TopPanel Height: " + topPanel.GetComponent<RectTransform>().sizeDelta.y);
        //Debug.Log("BottomPanel Height: " + bottomPanel.GetComponent<RectTransform>().sizeDelta.y);
        //Debug.Log("Screen Hight: " + Screen.height);
        betweenTopAndBottomPanels = (((touchPos.y) >= bottomPanel.GetComponent<RectTransform>().sizeDelta.y + avoidPanelsMargin)) && ((touchPos.y) <= (Screen.height - topPanel.GetComponent<RectTransform>().sizeDelta.y - avoidPanelsMargin));
        //Debug.Log("Within Game Area : " + withinGameArea);
        avoidsRightPanel = ((touchPos.x) <= (Screen.width - (-2*InfoPanel.instance.GetComponent<RectTransform>().rect.x)));

        //Debug.Log("xTouch: " + touchPos.x + " | Screen width: " + Screen.width + " | InfoPanel deltaX: " + InfoPanel.instance.GetComponent<RectTransform>().rect.x);

        return (betweenTopAndBottomPanels && avoidsRightPanel);
    }


}
