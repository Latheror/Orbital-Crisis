using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour {

    public float skyBoxRotationSpeed = 5f;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        RotateSkyBox();
	}

    public void RotateSkyBox()
    {
        RenderSettings.skybox.SetFloat("_Rotation", Time.time * skyBoxRotationSpeed);
    }

    public void PlayButton()
    {
        SceneManager.LoadScene(1);
    }

    public void QuitButton()
    {
        Application.Quit();
    }
}
