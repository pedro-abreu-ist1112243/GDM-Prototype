using UnityEngine;

public class PortalState : MonoBehaviour
{
    // Public bool that other scripts can access
    public bool hasPortaled = false;

    // Call this when the player portals
    public void SetPortaled(bool state)
    {
        hasPortaled = state;
    }
}
