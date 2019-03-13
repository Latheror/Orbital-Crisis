using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotationOnSelf : MonoBehaviour
{
    public int speed;

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(Vector3.forward * Time.deltaTime*speed);
    }
}
