using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraExplan : MonoBehaviour
{
   private void OnTriggerStay2D(Collider2D other)
   {
      if (other.CompareTag("Player"))
      {
         if (Camera.main.orthographicSize < 8)
         {
            Camera.main.orthographicSize += 0.1f;
         }
      }
   }
}
