using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lightbulb : MonoBehaviour
{
    private Rigidbody rb;

    [SerializeField]
    private string disappearMessage; // Editable in the Unity Inspector

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        if (rb != null)
            rb.isKinematic = true; // Start as non-moving
    }

    public void Interact()
    {
        Drop();
    }

    public string PlayerInteract()
    {
        return Disappear();
    }

    // Call this to make the lightbulb fall
    public void Drop()
    {
        if (rb != null)
            rb.isKinematic = false;
    }

    // Call this to make the lightbulb disappear and return the message
    public string Disappear()
    {
        GameLogger.Log(transform.position,"Interacted with " +  disappearMessage);
        gameObject.SetActive(false);
        return disappearMessage;
    }
}