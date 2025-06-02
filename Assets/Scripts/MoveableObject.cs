using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveableObject : MonoBehaviour
{
    private Rigidbody rb; // Reference to the Rigidbody component
    public bool isMovable = true; // Flag to determine if the object is movable

    [SerializeField] private bool isKinematic = false;

    void Start()
    {
        // Get the Rigidbody component
        rb = GetComponent<Rigidbody>();
        rb.isKinematic = isKinematic; // Set the Rigidbody to kinematic based on the flag
    }

    void Update()
    {
        // Make the object kinematic if touching ground, otherwise use isKinematic flag
        if (IsTouchingGround())
        {
            if (rb != null) rb.isKinematic = true;
        }
        else
        {
            if (rb != null) rb.isKinematic = isKinematic;
        }
    }

    public bool IsTouchingObject()
    {
        Collider[] colliders = Physics.OverlapBox(
            transform.position,
            GetComponent<Collider>().bounds.extents * 0.95f, // slightly smaller to avoid self-collision
            transform.rotation
        );

        foreach (Collider col in colliders)
        {
            if (col.gameObject != this.gameObject &&
                (col.CompareTag("Object") || col.CompareTag("Moveable_Object")))
            {
                return true;
            }
        }
        return false;
    }

    // Check if touching ground
    private bool IsTouchingGround()
    {
        Collider[] colliders = Physics.OverlapBox(
            transform.position,
            GetComponent<Collider>().bounds.extents * 1f,
            transform.rotation
        );

        foreach (Collider col in colliders)
        {
            if (col.gameObject != this.gameObject && col.CompareTag("Ground"))
            {
                return true;
            }
        }
        return false;
    }

    // Getter for isMovable
    public bool GetIsMovable()
    {
        return isMovable;
    }

    // Setter for isMovable
    public void SetIsMovable(bool value)
    {
        isMovable = value;
    }
}