using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NonPlayerPortal : MonoBehaviour
{
    [SerializeField] private Transform teleportTarget; // Assign the teleport destination in the Inspector
    [SerializeField] private float detectionRadius = 2f; // Set this in the Inspector for a bigger range

    private void Update()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, detectionRadius);
        foreach (Collider col in colliders)
        {
            if (col.CompareTag("Moveable_Object") && teleportTarget != null)
            {
                col.transform.position = teleportTarget.position;
            }
        }
    }
}