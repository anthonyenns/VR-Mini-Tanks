using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HiFi.Platform;

public class debugger : MonoBehaviour
{

    public HiFi_PresetAxisInput axis;
    public HiFi_PresetButtonInput button;


    void Update()
    {
        if (HiFi_Platform.instance.Preset(button))
            Debug.Log("button");

        if (HiFi_Platform.instance.Preset(axis) != 0)
            Debug.Log("axis");

    }
}
