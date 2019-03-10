using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    public static MenuManager instance;
    void Awake()
    {
        if (instance != null) { Debug.LogError("More than one MenuManager in scene !"); return; }
        instance = this;
    }

    [Header("Settings")]
    public float skyBoxRotationSpeed = 5f;

    [Header("UI")]
    public GameObject mainCanvas;
    public GameObject mainCameraPosition;
    public Vector3 mainCameraRotation;

    public GameObject saveCanvas;
    public GameObject playCameraPosition;
    public Vector3 playCameraRotation;

    public GameObject settingsPanel;

    public GameObject cameraPivotPoint;

    public GameObject signInButton;
    public Image signInButtonImage;

    public Sprite signInSuccessImage;
    public Sprite signInFailImage;

    public Camera mainCam;
    public float cameraSpeed;

    float pathFactor = 0f;

    // Use this for initialization
    void Start()
    {
        Screen.orientation = ScreenOrientation.AutoRotation;
        Initialize();
    }

    // Update is called once per frame
    void Update()
    {
        RotateSkyBox();
    }

    public void Initialize()
    {
        mainCanvas.SetActive(true);
        saveCanvas.SetActive(true);
        settingsPanel.SetActive(false);
    }

    public void RotateSkyBox()
    {
        RenderSettings.skybox.SetFloat("_Rotation", Time.time * skyBoxRotationSpeed);
    }

    public void OnPlayButtonClick()
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
        Debug.Log("MoveCameraFromMainToSaveCanvas | Setting Forward Trigger");
        cameraPivotPoint.GetComponent<Animator>().SetTrigger("ForwardTrigger");
        //StartCoroutine("CameraMainToPlayMovement"); // Obsolete
    }

    public void MoveCameraFromSaveToMainCanvas()
    {
        Debug.Log("MoveCameraFromSaveToMainCanvas | Setting Backward Trigger");
        cameraPivotPoint.GetComponent<Animator>().SetTrigger("BackwardTrigger");
        //StartCoroutine("CameraPlayToMainMovement"); // Obsolete
    }

    public void OnRateMeButton()
    {
        Debug.Log("OnRateMeButton");
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

    public void OnSettingsButtonClick()
    {
        Debug.Log("OnSettingsButtonClick");
        DisplaySettingsPanel(true);
    }

    public void DisplaySettingsPanel(bool display)
    {
        settingsPanel.SetActive(display);
    }

    public void OnSettingsBackButtonClick()
    {
        Debug.Log("OnSettingsBackButtonClick");
        DisplaySettingsPanel(false);
    }

    public void SetSignButtonImage(bool success)
    {
        signInButtonImage.sprite = (success) ? signInSuccessImage : signInFailImage;
    }

}
