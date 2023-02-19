using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.Events;

public class Interactive : MonoBehaviour
{
    [SerializeField] private bool playerA;
    [SerializeField] private bool playerB;
    
    private bool playerAOnRange;
    private bool playerBOnRange;
    public UnityEvent playerInteractActive;
    private Controller controller;
    [SerializeField]private GameObject showInteract;

    private void Awake()
    {
        controller = new Controller();
    }

    private void Update()
    {
        if (playerAOnRange)
        {
            if (controller.PlayerA.Interaction.triggered)
            {
                playerInteractActive.Invoke();
                Destroy(showInteract);
                Destroy(this);
            }
        }

        if (playerBOnRange)
        {
            if (controller.PlayerB.Interaction.triggered)
            {
                playerInteractActive.Invoke();
                Destroy(showInteract);
                Destroy(this);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (playerA)
        {
            if (other.gameObject.CompareTag("PlayerA"))
            {
                playerAOnRange = true;
                showInteract.SetActive(true);
            }
        }
        else if (playerB)
        {
            if (other.gameObject.CompareTag("PlayerB"))
            {
                playerBOnRange = true;
                showInteract.SetActive(true);
            }
        }
        // else if (other.gameObject.CompareTag("Interactable"))
        // {
        //     onRange = true;
        //     playerInteractActive?.Invoke();
        //     showInteract.SetActive(false);
        //     Destroy(this);
        // }
    }
    
    private void OnTriggerExit2D(Collider2D other)
    {
        if (playerA)
        {
            if (other.gameObject.CompareTag("Player"))
            {
                playerAOnRange = false;
                showInteract.SetActive(false);
            }
        }
        else if (playerB)
        {
            if (other.gameObject.CompareTag("Ghost"))
            {
                playerBOnRange = false;
                showInteract.SetActive(false);
            }
        }
    }

    private void OnEnable()
    {
        controller.Enable();
    }

    private void OnDisable()
    {
        controller.Disable();
    }
}
