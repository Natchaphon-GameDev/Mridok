using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotatePuzzle : MonoBehaviour
{
    [SerializeField] private float rotateSpeed = 100f;
    private bool isdragging = false;
    [SerializeField] private Rigidbody rb;

    [SerializeField] private Transform targetTransform;

    public event Action OnCorrected;

    private bool toggle = false;

    public bool isCorrected = false;

    private void Awake()
    {
        PuzzleManager.OnLogoCorrected += FinishPuzzle;
    }

    private void FinishPuzzle()
    {
        Debug.Log("Finlish!!");
        //TODO: Animate or Something
    }

    private void OnMouseDrag()
    {
        isdragging = true;
    }

    private void Update()
    {
        if (Input.GetMouseButtonUp(0))
        {
            isdragging = false;
            rb.freezeRotation = true;
            rb.freezeRotation = false;
        }
    }

    private void FixedUpdate()
    {
        if (isdragging)
        {
            var x = Input.GetAxis("Mouse X") * rotateSpeed * Time.deltaTime;
            rb.AddTorque(Vector3.up * x);

            var angle = Quaternion.Angle(transform.rotation, targetTransform.rotation);
            
            if (angle <= 5)
            {
                isCorrected = true;
                OnCorrected?.Invoke();
            }
            else
            {
                isCorrected = false;
            }
        }
    }
}