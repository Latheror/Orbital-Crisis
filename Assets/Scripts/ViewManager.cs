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

    [Header("UI")]
    public GameObject dezoomButton;

    [Header("Settings")]
    public float zoomOrthographicSize;
    public float dezoomOrthographicSize;

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
        switch(currentViewState)
        {
            case ViewState.Dezoom:
            {
                dezoomButton.SetActive(false);
                mainCamera.orthographicSize = dezoomOrthographicSize;
                break;
            }
            case ViewState.Zoom:
            {
                dezoomButton.SetActive(true);
                mainCamera.orthographicSize = zoomOrthographicSize;
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

}
