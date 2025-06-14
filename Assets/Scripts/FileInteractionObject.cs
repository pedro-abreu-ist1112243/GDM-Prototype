using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using TMPro;

public class FileInteractionObject : MonoBehaviour
{
    [SerializeField] private GameObject fileUIPanel; // The UI panel to display
    [SerializeField] private TMP_Text fileText; // Text component to show file content
    [TextArea]
    [SerializeField] private string fileContent; // File content to show

    private bool isPlayerNearby = false;
    private bool isReading = false;
    private Controls controls;
    private PlayerInteractions playerInteractions;

    void Awake()
    {
        controls = new Controls();
    }

    void OnEnable()
    {
        controls.Enable();
        controls.Actions.Interact.performed += OnInteract;
    }

    void OnDisable()
    {
        controls.Actions.Interact.performed -= OnInteract;
        controls.Disable();
    }

    void Update()
    {
        if (isReading && Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            CloseFile();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player entered interaction zone");
            isPlayerNearby = true;
            playerInteractions = other.GetComponent<PlayerInteractions>();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerNearby = false;
        }
    }

    private void OnInteract(InputAction.CallbackContext context)
    {
        if (isPlayerNearby && !isReading)
        {
            OpenFile();
        }
    }

    private void OpenFile()
    {
        isReading = true;

        if (fileUIPanel != null && fileText != null)
        {
            fileText.text = fileContent;
            fileUIPanel.SetActive(true);
        }

        Time.timeScale = 0f; // Pause game
        controls.Disable(); // Optional: disable movement if it's bound to same input system
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    private void CloseFile()
    {
        isReading = false;

        if (fileUIPanel != null)
        {
            fileUIPanel.SetActive(false);
        }

        Time.timeScale = 1f; // Resume game
        controls.Enable(); // Re-enable input
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
}
