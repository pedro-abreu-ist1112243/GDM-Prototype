using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportUponInteract : MonoBehaviour
{
    [SerializeField] private Transform teleportTarget; // Assign the target node in the Inspector
    [SerializeField] private float interactRange = 2f; // Range to interact
    [SerializeField] private PortalState portalState; // Reference to the PortalState script

    private Controls controls;

    void Awake()
    {
        controls = new Controls();

        // Try to find PortalState if not assigned
        if (portalState == null)
        {
            portalState = FindObjectOfType<PortalState>();
            if (portalState == null)
            {
                Debug.LogWarning("PortalState script not found! Please assign it in the Inspector or add it to a GameObject in the scene.");
            }
        }
    }

    void OnEnable()
    {
        controls.Enable();
    }

    void OnDisable()
    {
        controls.Disable();
    }

    void Update()
    {
        // Find all objects tagged "Player"
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        foreach (GameObject player in players)
        {
            float distance = Vector3.Distance(player.transform.position, transform.position);
            if (distance <= interactRange && controls.Actions.EnterPortal.WasPressedThisFrame())
            {
                player.transform.position = teleportTarget.position;

                // Update the portal state (toggle it)
                if (portalState != null)
                {
                    portalState.SetPortaled(!portalState.hasPortaled);
                    Debug.Log($"Player has portaled! PortalState toggled to: {portalState.hasPortaled}");
                }
                else
                {
                    Debug.LogWarning("PortalState reference is null! Cannot update portal state.");
                }

                break; // Only teleport the first player found in range
            }
        }
    }
}