using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pl_worck : MonoBehaviour
{
    [Header("Target to Follow")]
    public Transform target;

    [Header("Follow Settings")]
    public float followSpeed = 5f; // Adjust for smoothness
    private float yOffset = 0f; // Offset on the Y axis (calculated dynamically)

    [Header("Follow Axes")]
    public bool followXY = false; // Toggle to enable X and Y axis following
    public bool isBottom = false; // Determines which camera this is

    [Header("Portal State Reference")]
    public PortalState portalState; // Reference to portal state script

    void LateUpdate()
    {
        if (target == null || portalState == null) return;

        // Determine yOffset based on portal state and isBottom flag
        if (!portalState.hasPortaled)
        {
            yOffset = isBottom ? 1f : 24f;
        }
        else
        {
            yOffset = isBottom ? -21f : 1f;
        }

        Vector3 currentPosition = transform.position;
        float newX = currentPosition.x;
        float newY = currentPosition.y;

        if (followXY)
        {
            newX = Mathf.Lerp(currentPosition.x, target.position.x, followSpeed * Time.deltaTime);
            newY = Mathf.Lerp(currentPosition.y, target.position.y + yOffset, followSpeed * Time.deltaTime);
        }

        transform.position = new Vector3(newX, newY, currentPosition.z);
    }
}
