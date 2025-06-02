using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UponInteractSpawnObjectPlayer : MonoBehaviour
{
    [SerializeField] private GameObject objectToSpawn; // Assign the prefab in the Inspector
    [SerializeField] private float interactRange = 2f; // Range to interact
    [SerializeField] private Vector3 spawnOffset = Vector3.zero; // Optional offset from this object's position

    private bool hasSpawned = false;
    private GameObject spawnedPlayer; // Reference to the spawned player

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
        if (hasSpawned) return;

        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        foreach (GameObject player in players)
        {
            float distance = Vector3.Distance(player.transform.position, transform.position);
            if (distance <= interactRange && controls.Actions.Interact.WasPressedThisFrame())
            {
                if (objectToSpawn != null)
                {
                    spawnedPlayer = Instantiate(objectToSpawn, transform.position + spawnOffset, Quaternion.identity);
                    hasSpawned = true;

                    // Find PlayerManager and set player2
                    PlayerManager pm = FindObjectOfType<PlayerManager>();
                    if (pm != null)
                    {
                        // Assuming player2 is the second player in the PlayerManager
                        pm.SetPlayer2(spawnedPlayer);
                    }
                }
                break;
            }
        }
    }
}