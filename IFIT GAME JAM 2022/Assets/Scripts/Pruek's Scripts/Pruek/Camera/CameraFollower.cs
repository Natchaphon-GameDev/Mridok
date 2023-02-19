using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

public class CameraFollower : MonoBehaviour {

    public Transform playerA;
    public Transform playerB;
    public bool A;
    public bool B;
    public bool isSplit;
    public float dampTime = 0.4f;
    public Camera camera2;
    private Vector3 cameraPos;
    private Vector3 velocity = Vector3.zero;
    public float offset = -10f;
    public GameObject canvas;
    public GameObject splitBar;
    [SerializeField] private Animator fadeAnimator;

    private PlayerVariable PlayerVariable;

    private void Start()
    {
        if (A)
        {
            PlayerVariable = playerA.GetComponent<PlayerVariable>();
        }
        else
        {
            PlayerVariable = playerB.GetComponent<PlayerVariable>();
        }
    }

    private bool stopFollowing => PlayerVariable.isStopCamera;
    
    private IEnumerator OnFadeBlack()
    {
        fadeAnimator.SetBool("fadeOut", true);
        yield return new WaitForSeconds(2);
        fadeAnimator.SetBool("fadeOut", false);
        
    }

    public void SplitScreen()
    {
        StartCoroutine(OnFadeBlack());
        isSplit = true;
    }

    public void OneScreen()
    {
        StartCoroutine(OnFadeBlack());
        isSplit = false;
    }
    
    private void Update ()
    {
        if (isSplit)
        {
            if (camera2 != null)
            {
                canvas.gameObject.SetActive(true);
                splitBar.gameObject.SetActive(true);
                PlayerVariable.isSplitScreen = true;
                camera2.gameObject.SetActive(true);
                Camera.main.rect = new Rect(0.0f, 0.0f, 0.5f, 1.0f);
            }
        }
        else
        {
            if (camera2 != null)
            {
                canvas.gameObject.SetActive(false);
                splitBar.gameObject.SetActive(false);
                
                // PlayerVariable.isSplitScreen = false; TODO: Refactor this
                
                camera2.gameObject.SetActive(false);
                Camera.main.rect = new Rect(0f, 0.0f, 1.0f, 1.0f);
            }
        }
        
        if (A)
        {
            if (!stopFollowing)
            {
                cameraPos = new Vector3(playerA.position.x, playerA.position.y,  offset);
                transform.position = Vector3.SmoothDamp(gameObject.transform.position, cameraPos, ref velocity, dampTime);
            }
        }
        else if (B)
        {
            if (!stopFollowing)
            {
                cameraPos = new Vector3(playerB.position.x, playerB.position.y,  offset);
                transform.position = Vector3.SmoothDamp(gameObject.transform.position, cameraPos, ref velocity, dampTime);
            }
        }
    }

}
