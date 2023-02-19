using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionRays : MonoBehaviour
{
    [SerializeField] private GameObject currentHitObject;
    
    [SerializeField] private float sphereRadius;
    [SerializeField] private float maxDistance;
    [SerializeField] private LayerMask layerMask;

    private Vector3 origin;
    private Vector3 direction;

    private float currentHitDistance;

    private void Start()
    {
        direction = transform.forward;
    }

    private void Update()
    {
        origin = transform.position;
        RaycastHit hit;
        if (Physics.SphereCast(origin, sphereRadius, direction, out hit, maxDistance, layerMask,
                QueryTriggerInteraction.UseGlobal))
        {
            currentHitObject = hit.transform.gameObject;
            currentHitDistance = hit.distance;
        }
        else
        {
            currentHitDistance = maxDistance;
            currentHitObject = null;
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Debug.DrawLine(origin, origin + direction * currentHitDistance);
        Gizmos.DrawWireSphere(origin + direction * currentHitDistance, sphereRadius);
    }

    public void Facing(Vector3 dir)
    {
        direction = dir;
    }
}
