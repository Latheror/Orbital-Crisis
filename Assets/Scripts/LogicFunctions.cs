using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LogicFunctions : MonoBehaviour {

    public static bool RandomTrueFalse()
    {
        return (Random.Range(0, 10) < 5 ? true : false);
    }





}

