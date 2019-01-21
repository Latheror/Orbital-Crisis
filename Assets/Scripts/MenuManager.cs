using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{

    [Header("Settings")]
    public float skyBoxRotationSpeed = 5f;

    public GameObject mainCanvas;
    public GameObject mainCameraPosition;
    public Vector3 mainCameraRotation;

    public GameObject saveCanvas;
    public GameObject playCameraPosition;
    public Vector3 playCameraRotation;

    public GameObject cameraPivotPoint;

    public Camera mainCam;
    public float cameraSpeed;

    float pathFactor = 0f;

    // Use this for initialization
    void Start()
    {
        Screen.orientation = ScreenOrientation.AutoRotation;
        Init();
    }

    // Update is called once per frame
    void Update()
    {
        RotateSkyBox();
    }

    public void Init()
    {
        mainCanvas.SetActive(true);
        saveCanvas.SetActive(true);
    }

    public void RotateSkyBox()
    {
        RenderSettings.skybox.SetFloat("_Rotation", Time.time * skyBoxRotationSpeed);
    }

    public void PlayButton()
    {
        SwitchViewFromMainToSaveCanvas();
    }

    public void BackFromSaveButton()
    {
        SwitchViewFromPlayToMainCanvas();
    }

    public void QuitButton()
    {
        Application.Quit();
    }

    public void PlayCanvas_BackButton()
    {
        SwitchViewFromPlayToMainCanvas();
    }

    public void SwitchViewFromMainToSaveCanvas()
    {
        mainCanvas.SetActive(true);
        saveCanvas.SetActive(true);
        MoveCameraFromMainToSaveCanvas();
    }

    public void SwitchViewFromPlayToMainCanvas()
    {
        mainCanvas.SetActive(true);
        saveCanvas.SetActive(true);
        MoveCameraFromSaveToMainCanvas();
    }

    public void MoveCameraFromMainToSaveCanvas()
    {
        StartCoroutine("CameraMainToPlayMovement");
    }

    public void MoveCameraFromSaveToMainCanvas()
    {
        StartCoroutine("CameraPlayToMainMovement");
    }

    public void OnRateMeButton()
    {
        Application.OpenURL("https://play.google.com/store/apps/details?id=com.LathGames.OrbitalCrisis");
    }

    IEnumerator CameraMainToPlayMovement()
    {
        while ((cameraPivotPoint.transform.rotation.y) > -0.69f)
        {
            cameraPivotPoint.transform.Rotate(Vector3.up, - Time.deltaTime * cameraSpeed);
            //Debug.Log("CameraPivotAngle: " + cameraPivotPoint.transform.rotation.y);

            yield return null;
        }
        yield return null;
    }

    IEnumerator CameraPlayToMainMovement()
    {
        while ((cameraPivotPoint.transform.rotation.y) < 0f)
        {
            cameraPivotPoint.transform.Rotate(Vector3.up, Time.deltaTime * cameraSpeed);
            yield return null;
        }
        yield return null;
    }

}
