using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraDecress : MonoBehaviour
{
    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (Camera.main.orthographicSize > 5)
            {
                Camera.main.orthographicSize -= 0.1f;
            }
        }
    }
}
