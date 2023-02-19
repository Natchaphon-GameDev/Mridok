using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InpectAnim : MonoBehaviour
{
    [SerializeField] private Animator uiCam;
    [SerializeField] private PlayerController pc;

    public void RunIn()
    {
        uiCam.SetTrigger("Open");
        pc.enabledInput = false;
        pc.EmergencyBreak();
    }

    public void RunOut()
    {
        uiCam.SetTrigger("Close");
        pc.enabledInput = true;
    }
}
