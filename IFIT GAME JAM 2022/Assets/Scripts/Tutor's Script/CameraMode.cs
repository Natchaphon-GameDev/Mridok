using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class CameraMode : MonoBehaviour
{
    public CamMode _cameraMode;

    [SerializeField] private CinemachineVirtualCamera mainPerCam;
    [SerializeField] private CinemachineVirtualCamera mainIsoCam;
    [SerializeField] private CinemachineVirtualCamera subIsoCam;
    [SerializeField] private float facingTime;
    [SerializeField] private SpriteRenderer characterSprite;

    private bool normalCam = true;

    public enum CamMode
    {
        Perspective,
        Isometric
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            switch (_cameraMode)
            {
                case CamMode.Perspective:
                    _cameraMode = CamMode.Isometric;
                    mainPerCam.Priority = 0;
                    mainIsoCam.Priority = 10;
                    break;
                case CamMode.Isometric:
                    _cameraMode = CamMode.Perspective;
                    mainIsoCam.Priority = 0;
                    mainPerCam.Priority = 10;
                    break;
            }
            StartCoroutine(SwitchDirection());
        }

        if (_cameraMode == CamMode.Isometric && Input.GetKeyDown(KeyCode.LeftAlt))
        {
            if (normalCam)
            {
                mainIsoCam.Priority = 0;
                subIsoCam.Priority = 10;
            }
            else
            {
                subIsoCam.Priority = 0;
                mainIsoCam.Priority = 10;
            }

            StartCoroutine(SwitchDirection());
            
            normalCam = !normalCam;
        }
    }
    
    IEnumerator SwitchDirection()
    {
        float _time = 0;
        do
        {
            _time += Time.deltaTime;
            
            characterSprite.transform.LookAt(transform);
            yield return 0;
        } while (_time < facingTime);
    }
}
