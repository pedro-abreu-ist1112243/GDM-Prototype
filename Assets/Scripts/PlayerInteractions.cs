using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteractions : MonoBehaviour
{
    public float checkRadius = 1.5f; // Radius to check for collectibles
    public List<string> inventory = new List<string>(); // Player's inventory

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
        // Use the new input system action for interaction (replace with your actual action name if different)
        if (controls.Actions.Interact.WasPressedThisFrame())
        {
            TryCollectNearby();
            TryInteractWithInventoryChecker();
        }
    }

    private void TryCollectNearby()
    {
        // Check for nearby objects with tag "Collectible"
        Collider[] colliders = Physics.OverlapSphere(transform.position, checkRadius);
        foreach (Collider col in colliders)
        {
            if (col.CompareTag("Collectible"))
            {
                var collectible = col.GetComponent<MonoBehaviour>();
                if (collectible != null)
                {
                    var method = collectible.GetType().GetMethod("PlayerInteract");
                    if (method != null)
                    {
                        object result = method.Invoke(collectible, null);
                        if (result is string str)
                        {
                            inventory.Add(str);
                            Debug.Log("Added to inventory: " + str);
                        }
                    }
                }
                break; // Only interact with the first collectible found
            }
        }
    }

    // Scans nearby for an object with the tag "InventoryChecker" and calls its PlayerInteract() method
    public void TryInteractWithInventoryChecker()
{
    Collider[] colliders = Physics.OverlapSphere(transform.position, checkRadius);
    Debug.Log($"[PlayerInteractions] Checking for InventoryChecker within radius {checkRadius}...");
    foreach (Collider col in colliders)
    {
        Debug.Log($"[PlayerInteractions] Found collider: {col.name} (tag: {col.tag})");
        if (col.CompareTag("InventoryChecker"))
        {
            Debug.Log("[PlayerInteractions] InventoryChecker found! Attempting to interact...");
            var checker = col.GetComponent<MonoBehaviour>();
            if (checker != null)
            {
                var method = checker.GetType().GetMethod("PlayerInteract");
                if (method != null)
                {
                    Debug.Log("[PlayerInteractions] Calling PlayerInteract on InventoryChecker.");
                    method.Invoke(checker, null);
                }
                else
                {
                    Debug.LogWarning("[PlayerInteractions] No PlayerInteract method found on InventoryChecker.");
                }
            }
            else
            {
                Debug.LogWarning("[PlayerInteractions] No MonoBehaviour found on InventoryChecker collider.");
            }
            break; // Only interact with the first InventoryChecker found
        }
    }
}
}