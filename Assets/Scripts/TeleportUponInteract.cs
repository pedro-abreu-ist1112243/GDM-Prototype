using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportUponInteract : MonoBehaviour
{
    [SerializeField] private Transform teleportTarget; // Assign the target node in the Inspector
    [SerializeField] private float interactRange = 2f; // Range to interact

    private Controls controls;

    void Awake()
    {
        controls = new Controls();
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
        // Debug: Check if portal input is being pressed
        if (controls.Actions.EnterPortal.WasPressedThisFrame())
        {
            Debug.Log("Portal input detected!");
        }

        // Check for Player 1
        GameObject[] players1 = GameObject.FindGameObjectsWithTag("Player 1");
        Debug.Log($"Found {players1.Length} Player 1 objects");

        foreach (GameObject player in players1)
        {
            // Find the actual moving character (could be a child)
            Transform movingTransform = FindMovingCharacter(player.transform);
            float distance = Vector3.Distance(movingTransform.position, transform.position);
            Debug.Log($"Player 1 distance: {distance}, Interact range: {interactRange}");

            if (distance <= interactRange && controls.Actions.EnterPortal.WasPressedThisFrame())
            {
                Debug.Log("Player 1 is teleporting!");
                TeleportPlayer(player, movingTransform, "Player 1");
                break; // Only teleport the first player found in range
            }
        }

        // Check for Player 2
        GameObject[] players2 = GameObject.FindGameObjectsWithTag("Player 2");
        Debug.Log($"Found {players2.Length} Player 2 objects");

        foreach (GameObject player in players2)
        {
            // Find the actual moving character (could be a child)
            Transform movingTransform = FindMovingCharacter(player.transform);
            float distance = Vector3.Distance(movingTransform.position, transform.position);
            Debug.Log($"Player 2 distance: {distance}, Interact range: {interactRange}");

            if (distance <= interactRange && controls.Actions.EnterPortal.WasPressedThisFrame())
            {
                Debug.Log("Player 2 is teleporting!");
                TeleportPlayer(player, movingTransform, "Player 2");
                break; // Only teleport the first player found in range
            }
        }
    }

    Transform FindMovingCharacter(Transform parent)
    {
        // First check if the parent itself has a CharacterController, Rigidbody, or movement script
        if (parent.GetComponent<CharacterController>() != null ||
            parent.GetComponent<Rigidbody>() != null ||
            parent.GetComponent<Rigidbody2D>() != null)
        {
            return parent;
        }

        // If not, search through all children for the moving character
        foreach (Transform child in parent.GetComponentsInChildren<Transform>())
        {
            if (child.GetComponent<CharacterController>() != null ||
                child.GetComponent<Rigidbody>() != null ||
                child.GetComponent<Rigidbody2D>() != null)
            {
                return child;
            }
        }

        // If no movement components found, just return the parent
        return parent;
    }

    void TeleportPlayer(GameObject player, Transform movingTransform, string playerTag)
    {
        // Teleport the moving character
        movingTransform.position = teleportTarget.position;

        // Find the PortalState script on the tagged player GameObject or its children
        PortalState portalState = player.GetComponentInChildren<PortalState>();

        if (portalState != null)
        {
            // Toggle the portal state for this specific player
            portalState.SetPortaled(!portalState.hasPortaled);
            Debug.Log($"{playerTag} has portaled! PortalState toggled to: {portalState.hasPortaled}");
        }
        else
        {
            Debug.LogWarning($"PortalState script not found on {playerTag} or its children! Make sure the PortalState component is attached to the player hierarchy.");
        }
    }
}