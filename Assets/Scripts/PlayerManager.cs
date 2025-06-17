using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    [Header("Player References")]
    public GameObject player1;
    public GameObject player2;

    [Header("Camera")]
    public CameraFollow cameraFollow; // Reference to your CameraFollow script

    [Header("Player Detection")]
    public string player2Tag = "Player2"; // Tag to identify player 2 when it spawns

    private GameObject activePlayer;
    private bool isTwoPlayerMode = false;
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
        // Start with player1 as active
        SetActivePlayer(player1);

        // If player2 is already assigned in inspector, enable two-player mode
        if (player2 != null)
        {
            isTwoPlayerMode = true;
        }
    }

    void Update()
    {
        // Auto-detect player2 if not already assigned
        if (!isTwoPlayerMode && player2 == null)
        {
            DetectPlayer2();
        }

        // Debug: Check if we're in two-player mode
        if (Input.GetKeyDown(KeyCode.Q))
        {
            Debug.Log($"Q pressed! Two-player mode: {isTwoPlayerMode}, Player2: {(player2 != null ? player2.name : "null")}");
        }

        // Handle character switching with Q key - using both input methods
        bool switchPressed = false;

        // Try new Input System first
        try
        {
            if (controls != null && controls.Actions.SwitchCharacter.WasPressedThisFrame())
            {
                switchPressed = true;
                Debug.Log("Switch detected via new Input System");
            }
        }
        catch (System.Exception e)
        {
            Debug.LogWarning($"New Input System failed: {e.Message}");
        }

        // Fallback to old Input Manager
        if (!switchPressed && Input.GetKeyDown(KeyCode.Q))
        {
            switchPressed = true;
            Debug.Log("Switch detected via old Input Manager");
        }

        if (switchPressed)
        {
            if (!isTwoPlayerMode)
            {
                Debug.Log("Cannot switch - not in two-player mode");
                return;
            }

            // Only allow switch if the active player is grounded
            PortalMovement activeMovement = GetPortalMovement(activePlayer);
            if (activeMovement != null && activeMovement.isGrounded)
            {
                Debug.Log("Switching character...");
                SwitchCharacter();
            }
            else
            {
                if (activeMovement == null)
                    Debug.Log("Cannot switch - no PortalMovement component found");
                else
                    Debug.Log("Cannot switch - player not grounded");
            }
        }
    }

    void DetectPlayer2()
    {
        // Look for a GameObject with the specified tag
        GameObject foundPlayer2 = GameObject.FindGameObjectWithTag(player2Tag);

        if (foundPlayer2 != null && foundPlayer2 != player1)
        {
            SetPlayer2(foundPlayer2);
        }
    }

    void SwitchCharacter()
    {
        if (activePlayer == player1) {
            SetActivePlayer(player2);
            GameLogger.Log(player1.transform.position, "Swicthed to character 2");
        }
        else
        {
            SetActivePlayer(player1);
            GameLogger.Log(player2.transform.position, "Swicthed to character 1");
        }
            
    }

    void SetActivePlayer(GameObject player)
    {
        if (player == null)
        {
            Debug.LogError("Trying to set null player as active!");
            return;
        }

        Debug.Log($"Setting active player to: {player.name}");
        activePlayer = player;

        // Get PortalMovement components (check both the object itself and its children)
        PortalMovement movement1 = GetPortalMovement(player1);
        PortalMovement movement2 = player2 != null ? GetPortalMovement(player2) : null;

        if (movement1 != null)
        {
            movement1.enabled = (player == player1);
            Debug.Log($"Player1 PortalMovement enabled: {movement1.enabled}");
        }
        else
        {
            Debug.LogError($"Player1 ({player1.name}) has no PortalMovement component in itself or children!");
        }

        if (movement2 != null)
        {
            movement2.enabled = (player == player2);
            Debug.Log($"Player2 PortalMovement enabled: {movement2.enabled}");
        }
        else if (player2 != null)
        {
            Debug.LogError($"Player2 ({player2.name}) has no PortalMovement component in itself or children!");
        }

        // Get Rigidbody components (check both the object itself and its children)
        Rigidbody rb1 = GetRigidbody(player1);
        Rigidbody rb2 = player2 != null ? GetRigidbody(player2) : null;

        if (rb1 != null)
        {
            rb1.isKinematic = (player != player1);
            Debug.Log($"Player1 Rigidbody kinematic: {rb1.isKinematic}");
        }

        if (rb2 != null)
        {
            rb2.isKinematic = (player != player2);
            Debug.Log($"Player2 Rigidbody kinematic: {rb2.isKinematic}");
        }

        // Handle cameras - enable/disable ALL child cameras
        Camera[] cameras1 = GetAllCameras(player1);
        Camera[] cameras2 = player2 != null ? GetAllCameras(player2) : new Camera[0];

        // Enable/disable all cameras for Player1
        foreach (Camera cam in cameras1)
        {
            if (cam != null)
            {
                cam.enabled = (player == player1);
                Debug.Log($"Player1 Camera '{cam.name}' enabled: {cam.enabled}");
            }
        }

        // Enable/disable all cameras for Player2
        foreach (Camera cam in cameras2)
        {
            if (cam != null)
            {
                cam.enabled = (player == player2);
                Debug.Log($"Player2 Camera '{cam.name}' enabled: {cam.enabled}");
            }
        }

        if (cameras1.Length == 0)
        {
            Debug.LogWarning($"Player1 ({player1.name}) has no Camera components in itself or children!");
        }

        if (cameras2.Length == 0 && player2 != null)
        {
            Debug.LogWarning($"Player2 ({player2.name}) has no Camera components in itself or children!");
        }

        // Update camera follow target (if you're using CameraFollow script)
        if (cameraFollow != null)
        {
            Transform characterTransform = GetCharacterTransform(activePlayer);
            cameraFollow.player = characterTransform;
            Debug.Log($"CameraFollow now following: {characterTransform.name}");
        }

        Debug.Log($"Active player switched to: {activePlayer.name}");
    }

    public void SetPlayer2(GameObject newPlayer2)
    {
        if (newPlayer2 == null || newPlayer2 == player1) return;

        player2 = newPlayer2;
        isTwoPlayerMode = true;

        // Immediately disable Player2's components since Player1 should still be active
        PortalMovement movement2 = GetPortalMovement(player2);
        if (movement2 != null)
        {
            movement2.enabled = false;
            Debug.Log("Disabled Player2 PortalMovement on detection");
        }

        Rigidbody rb2 = GetRigidbody(player2);
        if (rb2 != null)
        {
            rb2.isKinematic = true;
            Debug.Log("Set Player2 Rigidbody to kinematic on detection");
        }

        // Immediately disable ALL of Player2's cameras
        Camera[] cameras2 = GetAllCameras(player2);
        foreach (Camera cam in cameras2)
        {
            if (cam != null)
            {
                cam.enabled = false;
                Debug.Log($"Disabled Player2 Camera '{cam.name}' on detection");
            }
        }

        // Store the current active player before calling SetActivePlayer
        GameObject currentlyActive = activePlayer;

        // Make sure the current active player setup is correct
        // This will refresh all the component states but keep the same active player
        SetActivePlayer(currentlyActive);

        Debug.Log($"Player 2 detected and assigned: {player2.name}");
        Debug.Log($"Active player should still be: {currentlyActive.name}");
    }

    // Public method to manually enable/disable two-player mode
    public void SetTwoPlayerMode(bool enabled)
    {
        isTwoPlayerMode = enabled && player2 != null;

        if (!isTwoPlayerMode)
        {
            // If disabling two-player mode, make sure player1 is active
            SetActivePlayer(player1);
        }
    }

    // Helper methods to find components in the GameObject or its children
    private PortalMovement GetPortalMovement(GameObject player)
    {
        if (player == null) return null;

        // First check the GameObject itself
        PortalMovement movement = player.GetComponent<PortalMovement>();
        if (movement != null) return movement;

        // Then check children
        movement = player.GetComponentInChildren<PortalMovement>();
        return movement;
    }

    private Rigidbody GetRigidbody(GameObject player)
    {
        if (player == null) return null;

        // First check the GameObject itself
        Rigidbody rb = player.GetComponent<Rigidbody>();
        if (rb != null) return rb;

        // Then check children
        rb = player.GetComponentInChildren<Rigidbody>();
        return rb;
    }

    private Transform GetCharacterTransform(GameObject player)
    {
        if (player == null) return null;

        // If the player itself has PortalMovement, use that transform
        if (player.GetComponent<PortalMovement>() != null)
            return player.transform;

        // Otherwise, find the child with PortalMovement
        PortalMovement movement = player.GetComponentInChildren<PortalMovement>();
        if (movement != null)
            return movement.transform;

        // Fallback to the player's own transform
        return player.transform;
    }

    private Camera GetCamera(GameObject player)
    {
        if (player == null) return null;

        // First check the GameObject itself
        Camera cam = player.GetComponent<Camera>();
        if (cam != null) return cam;

        // Then check children
        cam = player.GetComponentInChildren<Camera>();
        return cam;
    }

    private Camera[] GetAllCameras(GameObject player)
    {
        if (player == null) return new Camera[0];

        // Get all cameras in the GameObject and its children
        Camera[] cameras = player.GetComponentsInChildren<Camera>(true); // true includes inactive components
        return cameras;
    }

    // Getter methods for debugging/external access
    public GameObject GetActivePlayer() => activePlayer;
    public bool IsTwoPlayerMode() => isTwoPlayerMode;

    // Manual method for testing - you can call this from inspector or other scripts
    [ContextMenu("Force Switch Character")]
    public void ForceSwitchCharacter()
    {
        Debug.Log("Force switch called!");
        if (player2 != null)
        {
            isTwoPlayerMode = true;
            SwitchCharacter();
        }
        else
        {
            Debug.Log("Cannot force switch - Player2 is null");
        }
    }

    // Manual method to test player2 assignment
    [ContextMenu("Force Detect Player2")]
    public void ForceDetectPlayer2()
    {
        DetectPlayer2();
        Debug.Log($"After detection - Player2: {(player2 != null ? player2.name : "null")}, Two-player mode: {isTwoPlayerMode}");
    }
}