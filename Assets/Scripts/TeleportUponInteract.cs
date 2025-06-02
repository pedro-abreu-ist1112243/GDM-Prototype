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
        // Find all objects tagged "Player"
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        foreach (GameObject player in players)
        {
            float distance = Vector3.Distance(player.transform.position, transform.position);
            if (distance <= interactRange && controls.Actions.EnterPortal.WasPressedThisFrame())
            {
                player.transform.position = teleportTarget.position;
                break; // Only teleport the first player found in range
            }
        }
    }
}