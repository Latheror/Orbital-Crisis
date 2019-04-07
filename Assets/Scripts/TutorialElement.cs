using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialElement : MonoBehaviour
{
    public int id;

    public void OnTouch()
    {
        Debug.Log("On Touch [" + id + "]");
        TutorialManager.instance.TutorialIndicatorTouched(id);
    }
}
