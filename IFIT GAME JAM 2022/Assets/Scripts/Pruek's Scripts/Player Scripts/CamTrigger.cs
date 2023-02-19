using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class CamTrigger : MonoBehaviour
{
    [SerializeField] private CinemachineVirtualCamera fontCam;
    [SerializeField] private CinemachineVirtualCamera backCam;
    
    [SerializeField] private GameObject backStage;
    [SerializeField] private GameObject fontStage;

    [SerializeField] private GameObject playerB;
    [SerializeField] private Transform stone;

    public static bool isFontStage = false;

    private IEnumerator Pause(float time)
    {
        Time.timeScale = 0f;
        
        float pauseEndTime = Time.realtimeSinceStartup + time;
        while (Time.realtimeSinceStartup < pauseEndTime)
        {
            var count = Time.realtimeSinceStartup; ;
            
            if (count >= 5.5f)
            {
                Debug.Log(count);
                if (isFontStage)
                {
                    backStage.SetActive(false);
                    fontStage.SetActive(true);
                }
                else
                {
                    fontStage.SetActive(false);
                    backStage.SetActive(true);
                }
            }
            
            yield return 0;
        }
        Time.timeScale = 1;
    }
    
    private void Warp(float pos)
    {
        transform.position = new Vector3(transform.position.x, transform.position.y,pos);
        if (pos == 11)
        {
            playerB.transform.position = new Vector3(transform.position.x - 1, transform.position.y,pos);
        }
        else
        {
            playerB.transform.position = new Vector3(transform.position.x + 1, transform.position.y,pos);
        }
        
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Edge"))
        {
            var camInfo = other.GetComponent<EdgeSetup>().cameraSetup;

            if (camInfo == CamMode.Font && !isFontStage)
            {
                isFontStage = true;
                fontCam.Priority = 1;
                backCam.Priority = 0;
                Warp(11);
                fontCam.LookAt = stone;
                while (fontCam.m_Lens.FieldOfView <= 10)
                {
                    fontCam.m_Lens.FieldOfView += 0.5f;
                }
                
                StartCoroutine(Pause(1.5f));
                fontCam.LookAt = transform;
                fontCam.m_Lens.FieldOfView = 3;
                while (fontCam.m_Lens.FieldOfView >= 3)
                {
                    fontCam.m_Lens.FieldOfView -= 0.5f;
                }
            }
            else if (camInfo == CamMode.Back && isFontStage)
            {
                isFontStage = false;
                fontCam.Priority = 0;
                backCam.Priority = 1;
                Warp(-3);
                fontCam.LookAt = stone;
                while (fontCam.m_Lens.FieldOfView <= 10)
                {
                    fontCam.m_Lens.FieldOfView += 0.5f;
                }
                StartCoroutine(Pause(1.5f));
                fontCam.LookAt = transform;
                while (fontCam.m_Lens.FieldOfView >= 3)
                {
                    fontCam.m_Lens.FieldOfView -= 0.5f;
                }
            }
        }
    }
}
