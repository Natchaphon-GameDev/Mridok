using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GhostLightBulb : MonoBehaviour
{
    public Rigidbody2D rb;
    public new HingeJoint2D hingeJoint;
    public float velocity;

    public void Start()
    {
        enabled = false;
    }

    public void Active()
    { 
        enabled = true;
    }
    
    private void DropBulb()
    {
        if (rb.velocity.magnitude >= velocity)
        {
            hingeJoint.enabled = false;
        }
    }

    private void FixedUpdate()
    {
        DropBulb();
    }
}
