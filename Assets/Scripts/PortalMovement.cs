using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalMovement : MonoBehaviour
{
    public PortalState portalState;

    public float moveSpeed = 5f;
    public float portalSpeed = 2f;
    public float jumpForce = 5f;
    public Transform platformBelow;
    public Transform platformAbove;
    public float interactionRange = 1f;

    private bool isOnBelowPlatform = false;
    private bool isPortaling = false;
    private Vector3 targetPosition;
    public bool isGrounded = true;
    private MoveableObject moveableObject;
    private Vector3 moveableObjectOffset;
    private bool isHandlingMoveableObject = false;
    public bool canHandleMoveableObject = false;

    public bool isTelekinetic = true;

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
        // Handle left and right movement
        float horizontalInput = 0f;
        if (controls.Actions.MoveLeft.ReadValue<float>() > 0)
            horizontalInput -= 1f;
        if (controls.Actions.MoveRight.ReadValue<float>() > 0)
            horizontalInput += 1f;
        transform.Translate(Vector3.right * horizontalInput * moveSpeed * Time.deltaTime);

        // Handle portal behavior when 'EnterPortal' is pressed
        if (controls.Actions.EnterPortal.WasPressedThisFrame() && !isPortaling && IsNearPortal())
        {
            StartPortalTransition();
        }

        // Handle jumping when 'Jump' is pressed
        if (controls.Actions.Jump.WasPressedThisFrame() && isGrounded)
        {
            Jump();
        }

        // Detect and handle moveable objects
        if (!isHandlingMoveableObject) { DetectMoveableObject(); }
        HandleMoveableObjects();

        // Smoothly move the object during the portal transition
        if (isPortaling)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, portalSpeed * Time.deltaTime);

            if (Vector3.Distance(transform.position, targetPosition) < 0.01f)
            {
                isPortaling = false;
            }
        }
    }

    void StartPortalTransition()
    {
        Vector3 currentPosition = transform.position;
        float offset = 0.1f;

        if (isOnBelowPlatform)
        {
            transform.position = new Vector3(currentPosition.x, platformAbove.position.y + offset, currentPosition.z);
            if (portalState != null) portalState.SetPortaled(true);
        }
        else
        {
            transform.position = new Vector3(currentPosition.x, platformBelow.position.y + offset, currentPosition.z);
            if (portalState != null) portalState.SetPortaled(false);
        }

        isOnBelowPlatform = !isOnBelowPlatform;
    }


    void Jump()
    {
        GetComponent<Rigidbody>().AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        isGrounded = false;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground") || collision.gameObject.CompareTag("Moveable_Object"))
        {
            // Only set isGrounded if the collision normal is mostly upwards
            foreach (ContactPoint contact in collision.contacts)
            {
                if (contact.normal.y > 0.5f)
                {
                    isGrounded = true;
                    break;
                }
            }
        }

        if (collision.gameObject.CompareTag("Ground"))
        {
            canHandleMoveableObject = true;
        }
        


    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            canHandleMoveableObject = false;
        }
    }

    void DetectMoveableObject()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, interactionRange);

        foreach (Collider collider in colliders)
        {
            MoveableObject detectedObject = collider.GetComponent<MoveableObject>();
            if (detectedObject != null)
            {
                moveableObject = detectedObject;
                return;
            }
        }
        moveableObject = null;
    }

    void HandleMoveableObjects()
    {
        if (!isTelekinetic)
            return;

        if (moveableObject != null)
        {
            // Use HoldObject action (mapped to M) for telekinesis
            if (controls.Actions.HoldObject.WasReleasedThisFrame())
            {
                moveableObject.SetIsMovable(true);
                isHandlingMoveableObject = false;
            }
            if (controls.Actions.HoldObject.WasPressedThisFrame())
            {
                isHandlingMoveableObject = true;
            }
            if (controls.Actions.HoldObject.ReadValue<float>() > 0 && canHandleMoveableObject && moveableObject.GetIsMovable())
            {
                if (moveableObjectOffset == Vector3.zero)
                {
                    moveableObjectOffset = moveableObject.transform.position - transform.position;
                }

                if (moveableObject.IsTouchingObject())
                {
                    if (moveableObject.GetIsMovable())
                    {
                        moveableObject.transform.position = transform.position + 0.98f * moveableObjectOffset;
                        moveableObject.SetIsMovable(false);
                        return;
                    }
                    moveableObject.SetIsMovable(false);
                }

                if (moveableObject.GetIsMovable())
                {
                    moveableObject.transform.position = transform.position + moveableObjectOffset;
                }
            }
            else
            {
                moveableObjectOffset = Vector3.zero;
            }
        }
    }

    bool IsNearPortal(float checkRange = 2f)
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, checkRange);
        foreach (Collider col in colliders)
        {
            if (col.CompareTag("Portal"))
            {
                return true;
            }
        }
        return false;
    }
}