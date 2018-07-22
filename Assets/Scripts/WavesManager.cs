using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WavesManager : MonoBehaviour {

    public int currentWaveNumber;


	// Use this for initialization
	void Start () {
        currentWaveNumber = 0;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void NextWaveButton()
    {
        LaunchNewWave();
    }

    public void LaunchNewWave()
    {
        currentWaveNumber++;
        int nbMeteorsInWave = currentWaveNumber * 10;

        StartCoroutine(MeteorSpawnCouroutine(nbMeteorsInWave, 1f));
    }


    public IEnumerator MeteorSpawnCouroutine(int nb, float delay) {
        while(nb > 0)
        {
            Debug.Log("Meteor Spawn Coroutine.");
            MeteorsManager.instance.SpawnNewMeteor();
            nb--;
            yield return new WaitForSeconds(delay); // waits 3 seconds
        }
    }


}
