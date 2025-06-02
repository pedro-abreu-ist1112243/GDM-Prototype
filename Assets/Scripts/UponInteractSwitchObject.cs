using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UponInteractSwitchObject : MonoBehaviour
{
    [SerializeField] private GameObject objectToHide;   // Assign the object to hide in the Inspector
    [SerializeField] private GameObject objectToShow;   // Assign the object to show in the Inspector
    [SerializeField] private float interactRange = 2f;  // Range to interact

    [Header("Optional: Timed Switch Back")]
    [SerializeField] private bool useTimer = false;      // Enable timer functionality
    [SerializeField] private float timerDuration = 2f;   // Duration before switching back

    private bool isShowing = false;
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

    void Start()
    {
        if (objectToShow != null)
            objectToShow.SetActive(false);
        if (objectToHide != null)
            objectToHide.SetActive(true);
        isShowing = false;
    }

    void Update()
    {
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        foreach (GameObject player in players)
        {
            float distance = Vector3.Distance(player.transform.position, transform.position);
            if (distance <= interactRange && controls.Actions.Interact.WasPressedThisFrame())
            {
                isShowing = !isShowing;
                if (objectToHide != null) objectToHide.SetActive(!isShowing);
                if (objectToShow != null) objectToShow.SetActive(isShowing);

                // Start timer if enabled and just switched to showing
                if (useTimer && isShowing)
                {
                    timer = timerDuration;
                    timerActive = true;
                }
                break; // Only switch once per key press and player in range
            }
        }

        // Handle timer countdown and switch back
        if (useTimer && timerActive)
        {
            timer -= Time.deltaTime;
            if (timer <= 0f)
            {
                isShowing = false;
                if (objectToHide != null) objectToHide.SetActive(true);
                if (objectToShow != null) objectToShow.SetActive(false);
                timerActive = false;
            }
        }
    }
}