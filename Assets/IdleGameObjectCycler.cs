using UnityEngine;

public class IdleGameObjectCycler : MonoBehaviour
{
    [Header("Game Objects to Cycle Through")]
    public GameObject[] idleGameObjects = new GameObject[5];

    [Header("Idle Cycling Settings")]
    public float idleCycleDelay = 1.0f; // Time between cycles in idle
    public bool startCyclingOnStart = true; // Start cycling immediately

    private int currentIdleIndex = 0;
    private float lastIdleCycleTime = 0f;
    private bool isIdleCycling = false;

    void Start()
    {
        // Make sure we have exactly 5 game objects
        if (idleGameObjects.Length != 5)
        {
            //Debug.LogWarning("Please assign exactly 5 GameObjects to the idle array!");
            return;
        }

        // Deactivate all idle objects at start
        DeactivateAllIdleObjects();

        // Start idle cycling if enabled
        if (startCyclingOnStart)
        {
            StartIdleCycling();
        }
    }

    void Update()
    {
        // Check if A or D keys are being pressed
        bool movementKeysPressed = Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D);

        if (movementKeysPressed)
        {
            // If movement keys are pressed, pause idle cycling and deactivate all idle objects
            if (isIdleCycling)
            {
                PauseIdleCycling();
                DeactivateAllIdleObjects();
            }
        }
        else
        {
            // If no movement keys are pressed, resume idle cycling
            if (!isIdleCycling && startCyclingOnStart)
            {
                ResumeIdleCycling();
                // Activate current idle object when resuming
                if (idleGameObjects[currentIdleIndex] != null)
                {
                    idleGameObjects[currentIdleIndex].SetActive(true);
                }
            }

            // Continue idle cycling if active
            if (isIdleCycling)
            {
                // Cycle if enough time has passed
                if (Time.time - lastIdleCycleTime >= idleCycleDelay)
                {
                    CycleNextIdle();
                    lastIdleCycleTime = Time.time;
                }
            }
        }
    }

    public void StartIdleCycling()
    {
        if (!isIdleCycling)
        {
            isIdleCycling = true;
            lastIdleCycleTime = Time.time;

            // Activate the first idle object
            if (idleGameObjects[currentIdleIndex] != null)
            {
                idleGameObjects[currentIdleIndex].SetActive(true);
            }

            //Debug.Log("Started idle cycling");
        }
    }

    public void StopIdleCycling()
    {
        if (isIdleCycling)
        {
            isIdleCycling = false;
            DeactivateAllIdleObjects();
            //Debug.Log("Stopped idle cycling");
        }
    }

    public void PauseIdleCycling()
    {
        isIdleCycling = false;
        //Debug.Log("Paused idle cycling");
    }

    public void ResumeIdleCycling()
    {
        if (!isIdleCycling)
        {
            isIdleCycling = true;
            lastIdleCycleTime = Time.time;
            //Debug.Log("Resumed idle cycling");
        }
    }

    void CycleNextIdle()
    {
        // Deactivate current idle object
        if (idleGameObjects[currentIdleIndex] != null)
        {
            idleGameObjects[currentIdleIndex].SetActive(false);
        }

        // Move to next index (wrap around to 0 if at end)
        currentIdleIndex = (currentIdleIndex + 1) % idleGameObjects.Length;

        // Activate new current idle object
        if (idleGameObjects[currentIdleIndex] != null)
        {
            idleGameObjects[currentIdleIndex].SetActive(true);
        }

        //Debug.Log($"Idle: Activated GameObject {currentIdleIndex + 1}");
    }

    void DeactivateAllIdleObjects()
    {
        foreach (GameObject obj in idleGameObjects)
        {
            if (obj != null)
            {
                obj.SetActive(false);
            }
        }
    }

    // Public method to change cycling speed during runtime
    public void SetIdleCycleDelay(float newDelay)
    {
        idleCycleDelay = Mathf.Max(0.1f, newDelay); // Minimum 0.1 seconds
        //Debug.Log($"Idle cycle delay set to {idleCycleDelay} seconds");
    }

    // Public method to get current cycling state
    public bool IsIdleCycling()
    {
        return isIdleCycling;
    }

    // Public method to get current active idle object index
    public int GetCurrentIdleIndex()
    {
        return currentIdleIndex;
    }
}