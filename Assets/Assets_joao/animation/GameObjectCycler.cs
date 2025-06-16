using UnityEngine;

public class GameObjectCycler : MonoBehaviour
{
    [Header("Game Objects to Cycle Through")]
    public GameObject[] gameObjects = new GameObject[5];

    [Header("Character Rotation")]
    public Transform characterTransform;

    [Header("Cycling Settings")]
    public float cycleDelay = 0.2f; // Time between cycles when holding key

    private int currentIndex = 0;
    private Vector3 originalRotation;
    private float lastCycleTime = 0f;

    void Start()
    {
        // Make sure we have exactly 5 game objects
        if (gameObjects.Length != 5)
        {
            Debug.LogWarning("Please assign exactly 5 GameObjects to the array!");
            return;
        }

        // Store the original rotation (left-facing)
        if (characterTransform != null)
        {
            originalRotation = characterTransform.eulerAngles;
        }

        // Deactivate all objects at start
        DeactivateAllObjects();

        // Activate the first object
        if (gameObjects[currentIndex] != null)
        {
            gameObjects[currentIndex].SetActive(true);
        }
    }

    void Update()
    {
        // Check for A key being held (keep left-facing rotation)
        if (Input.GetKey(KeyCode.A))
        {
            SetCharacterRotation(false); // Face left

            // Cycle if enough time has passed
            if (Time.time - lastCycleTime >= cycleDelay)
            {
                CyclePrevious();
                lastCycleTime = Time.time;
            }
        }

        // Check for D key being held (rotate to face right)
        if (Input.GetKey(KeyCode.D))
        {
            SetCharacterRotation(true); // Face right

            // Cycle if enough time has passed
            if (Time.time - lastCycleTime >= cycleDelay)
            {
                CycleNext();
                lastCycleTime = Time.time;
            }
        }
    }

    void CycleNext()
    {
        // Deactivate current object
        if (gameObjects[currentIndex] != null)
        {
            gameObjects[currentIndex].SetActive(false);
        }

        // Move to next index (wrap around to 0 if at end)
        currentIndex = (currentIndex + 1) % gameObjects.Length;

        // Activate new current object
        if (gameObjects[currentIndex] != null)
        {
            gameObjects[currentIndex].SetActive(true);
        }

        Debug.Log($"Activated GameObject {currentIndex + 1}");
    }

    void CyclePrevious()
    {
        // Deactivate current object
        if (gameObjects[currentIndex] != null)
        {
            gameObjects[currentIndex].SetActive(false);
        }

        // Move to previous index (wrap around to end if at beginning)
        currentIndex = (currentIndex - 1 + gameObjects.Length) % gameObjects.Length;

        // Activate new current object
        if (gameObjects[currentIndex] != null)
        {
            gameObjects[currentIndex].SetActive(true);
        }

        Debug.Log($"Activated GameObject {currentIndex + 1}");
    }

    void SetCharacterRotation(bool faceRight)
    {
        if (characterTransform != null)
        {
            if (faceRight)
            {
                // Rotate 180 degrees on Y-axis to face right
                characterTransform.eulerAngles = new Vector3(
                    originalRotation.x,
                    originalRotation.y + 180f,
                    originalRotation.z
                );
            }
            else
            {
                // Use original rotation (face left)
                characterTransform.eulerAngles = originalRotation;
            }
        }
    }

    void DeactivateAllObjects()
    {
        foreach (GameObject obj in gameObjects)
        {
            if (obj != null)
            {
                obj.SetActive(false);
            }
        }
    }
}