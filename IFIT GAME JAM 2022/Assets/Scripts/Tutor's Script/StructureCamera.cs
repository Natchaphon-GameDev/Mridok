using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class StructureCamera : MonoBehaviour
{
    [SerializeField] private CinemachineVirtualCamera mainCam;
    [SerializeField] private CinemachineVirtualCamera sideCam;
    [SerializeField] private CinemachineBrain activeCamera;
    [SerializeField] private float facingTime;

    public void Focus(SpriteRenderer character)
    {
        sideCam.Priority = 15;
        StartCoroutine(SwitchDirection(character));
    }

    public void DeFocus(SpriteRenderer character)
    {
        sideCam.Priority = 0;
        StartCoroutine(SwitchDirection(character));
    }

    IEnumerator SwitchDirection(SpriteRenderer character)
    {
        float _time = 0;
        do
        {
            _time += Time.deltaTime;
            
            character.transform.LookAt(activeCamera.transform);
            yield return 0;
        } while (_time < facingTime);
    }
}
