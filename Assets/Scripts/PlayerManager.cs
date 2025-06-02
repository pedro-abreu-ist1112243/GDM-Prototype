using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public GameObject player1;
    public GameObject player2;
    public CameraFollow cameraFollow; // Reference to your CameraFollow script

    private GameObject activePlayer;
    [SerializeField] private bool isTwoPlayerMode = false;

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
        SetActivePlayer(player1);
    }

    void Update()
    {
        // Only allow switch if two-player mode is enabled
        if (!isTwoPlayerMode) return;

        // Only allow switch if the active player is grounded
        PortalMovement activeMovement = activePlayer.GetComponent<PortalMovement>();
        if (controls.Actions.SwitchCharacter.WasPressedThisFrame() && activeMovement != null && activeMovement.isGrounded)
        {
            if (activePlayer == player1)
                SetActivePlayer(player2);
            else
                SetActivePlayer(player1);
        }
    }

    void SetActivePlayer(GameObject player)
    {
        activePlayer = player;

        // Enable input on the active player, disable on the other
        player1.GetComponent<PortalMovement>().enabled = (player == player1);
        if (player2 != null)
            player2.GetComponent<PortalMovement>().enabled = (player == player2);

        // Set Rigidbody kinematic state: inactive player is kinematic, active is not
        Rigidbody rb1 = player1.GetComponent<Rigidbody>();
        if (player2 != null)
        {
            Rigidbody rb2 = player2.GetComponent<Rigidbody>();
            if (rb2 != null) rb2.isKinematic = (player != player2);
        }
        if (rb1 != null) rb1.isKinematic = (player != player1);

        // Update camera follow target
        cameraFollow.player = activePlayer.transform;
    }

    public void SetPlayer2(GameObject newPlayer2)
    {
        player2 = newPlayer2;
        isTwoPlayerMode = true;
        SetActivePlayer(activePlayer);
    }
}