using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UponInteractSpawnObject : MonoBehaviour
{
    [SerializeField] private GameObject objectToSpawn; // Assign the prefab in the Inspector
    [SerializeField] private float interactRange = 2f; // Range to interact
    [SerializeField] private Vector3 spawnOffset = Vector3.zero; // Optional offset from this object's position

    [Header("Optional: Timed Despawn")]
    [SerializeField] private bool useTimer = false;      // Enable timer functionality
    [SerializeField] private float timerDuration = 2f;   // Duration before despawning

    private GameObject spawnedObject;
    private float timer = 0f;
    private bool timerActive = false;

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
        if (spawnedObject == null)
        {
            GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
            foreach (GameObject player in players)
            {
                float distance = Vector3.Distance(player.transform.position, transform.position);
                if (distance <= interactRange && controls.Actions.Interact.WasPressedThisFrame())
                {
                    if (objectToSpawn != null)
                    {
                        spawnedObject = Instantiate(objectToSpawn, transform.position + spawnOffset, Quaternion.identity);
                        if (useTimer)
                        {
                            timer = timerDuration;
                            timerActive = true;
                        }
                    }
                    break;
                }
            }
        }

        // Handle timer countdown and despawn
        if (useTimer && timerActive && spawnedObject != null)
        {
            timer -= Time.deltaTime;
            if (timer <= 0f)
            {
                Destroy(spawnedObject);
                spawnedObject = null;
                timerActive = false;
            }
        }
    }
}