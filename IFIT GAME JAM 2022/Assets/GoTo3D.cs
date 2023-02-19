using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoTo3D : MonoBehaviour
{
    public static event Action On3D;
    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("PlayerA"))
        {
            On3D?.Invoke();
        }
    }
}
