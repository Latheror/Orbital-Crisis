using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gatherable : MonoBehaviour {

    public enum GatherableType { Heal, Energy };

    [Header("Settings")]
    public new string name;
    public GatherableType type;
    public int healingPower = 50;
    public float timeBeforeDespawn = 30f;

	// Use this for initialization
	void Start () {
        InvokeRepeating("SelfDestruct", timeBeforeDespawn, 0f);
    }

    // Update is called once per frame
    void Update () {
		
	}

    public void Remove()
    {
        Destroy(this.gameObject);
    }

    public void ActOnSpaceship(Spaceship spaceship)
    {
        if(type == GatherableType.Heal)
        {
            Debug.Log("Applying healing gatherable effects to Spaceship");
            spaceship.Heal(healingPower);
        }

        Remove();
    }

    public void SelfDestruct()
    {
        Destroy(gameObject);
    }
}
