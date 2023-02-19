using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Respawner : MonoBehaviour
{
   [SerializeField] private Transform respawnPoint;
   private void OnTriggerEnter2D(Collider2D col)
   {
      if (col.CompareTag("PlayerA") || col.CompareTag("PlayerB"))
      {
         col.transform.position = respawnPoint.transform.position;
      }
   }
}
