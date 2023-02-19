using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    private Transform DoorTransform;

    private void Awake()
    {
        DoorTransform = GetComponent<Transform>();
    }

    public void OpenTheDoor()
    {
        DoorTransform.position += new Vector3(0, 3, 0);
    }
}
