using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ViewManager : MonoBehaviour {

    public static ViewManager instance;
    void Awake()
    {
        if (instance != null) { Debug.LogError("More than one ViewManager in scene !"); return; }
        instance = this;
    }

    public enum ViewState { Dezoom, Zoom }

    [Header("World")]
    public Camera mainCamera;

    [Header("Settings")]
    public float zoomOrthographicSize;
    public float dezoomOrthographicSize;
    public Vector2 centerPosition;
    public float triggerZoomPanelDisplayCameraDistance = 70f;
    public float triggerDezoomPanelDisplayCameraDistance = 100f;

    [Header("Operation")]
    public ViewState currentViewState;

    private void Start()
    {
        Initialize();
    }

    public void Initialize()
    {
        currentViewState = ViewState.Dezoom;
        ApplyViewState();
    }

    public void SetCurrentViewState(ViewState viewState)
    {
        currentViewState = viewState;
        ApplyViewState();
    }

    public void ApplyViewState()
    {
        mainCamera.transform.position = new Vector3(centerPosition.x, centerPosition.y, mainCamera.transform.position.z);
        switch (currentViewState)
        {
            case ViewState.Dezoom:
            {
                mainCamera.orthographicSize = dezoomOrthographicSize;
                PlanetCanvasManager.instance.DisplayZoomInfoPanel(false);
                break;
            }
            case ViewState.Zoom:
            {
                mainCamera.orthographicSize = zoomOrthographicSize;
                PlanetCanvasManager.instance.DisplayZoomInfoPanel(true);
                break;
            }
        }
    }

    public void OnZoomButton()
    {
        Debug.Log("OnZoomButton");
        SetCurrentViewState(ViewState.Zoom);
    }

    public void OnDezoomButton()
    {
        Debug.Log("OnDezoomButton");
        SetCurrentViewState(ViewState.Dezoom);
    }

    public void OnZoomLevelUpdate()
    {
        float camDistance = Camera.main.orthographicSize;
        //Debug.Log("OnZoomLevelUpdate | CamDistance [" + camDistance + "]");
        if (camDistance <= triggerZoomPanelDisplayCameraDistance)
        {
            PlanetCanvasManager.instance.DisplayZoomInfoPanel(true);
        }
        else if (camDistance >= triggerDezoomPanelDisplayCameraDistance)
        {
            PlanetCanvasManager.instance.DisplayZoomInfoPanel(false);
        }
    }
}
