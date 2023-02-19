using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckFalling : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            var playerVariable = other.GetComponent<PlayerVariable>();
            playerVariable.isStopCamera = true;
        }
    }
}
