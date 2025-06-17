using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryChecker : MonoBehaviour
{
    // Reference to the PlayerInteractions script
    [SerializeField] private PlayerInteractions playerInteractions;

    // Reference to the DoorConditional object
    [SerializeField] private GameObject doorObject;

    [SerializeField] private GameObject greenLed;
    [SerializeField] private GameObject redLed;
    [SerializeField] private GameObject yellowLed;

    // New string fields for LED names
    [SerializeField] private string greenLedName;
    [SerializeField] private string redLedName;
    [SerializeField] private string yellowLedName;

    // String input in the Inspector, e.g. "a,b,c"
    [SerializeField] private string requiredItemsString;

    // The parsed list of required items
    private List<string> requiredItemsList = new List<string>();

    void Awake()
    {
        // Parse the string into a list, splitting by comma and trimming whitespace
        if (!string.IsNullOrEmpty(requiredItemsString))
        {
            string[] items = requiredItemsString.Split(',');
            foreach (var item in items)
            {
                string trimmed = item.Trim();
                if (!string.IsNullOrEmpty(trimmed))
                    requiredItemsList.Add(trimmed);
            }
        }
    }

    // Call this function to check if all required items are in the player's inventory
    public bool HasItems(List<string> itemsToCheck)
    {
        Debug.LogWarning("Checking items: " + string.Join(", ", itemsToCheck));
        if (playerInteractions == null)
        {
            Debug.LogWarning("PlayerInteractions reference not set on InventoryChecker.");
            return false;
        }

        Debug.Log("Player inventory: " + string.Join(", ", playerInteractions.inventory));
        Debug.Log("Required items: " + string.Join(", ", itemsToCheck));

        foreach (string item in itemsToCheck)
        {
            if (!playerInteractions.inventory.Contains(item))
            {
                Debug.Log($"Missing item: {item}");
                return false;
            }
            else
            {
                Debug.Log($"Has item: {item}");
            }
        }
        Debug.Log("All required items found!");
        return true;
    }

    // Call this to compare the player's inventory to the provided list from the inspector
    public bool HasRequiredItemsFromInspector()
    {
        return HasItems(requiredItemsList);
    }

    public bool PlayerHasItem(string itemName)
{
    if (playerInteractions == null)
    {
        Debug.LogWarning("PlayerInteractions reference not set on InventoryChecker.");
        return false;
    }

    bool hasItem = playerInteractions.inventory.Contains(itemName);
    Debug.Log($"Checking for item '{itemName}': {(hasItem ? "FOUND" : "NOT FOUND")}");
    return hasItem;
}

    public void PlayerInteract()
    {
        if (PlayerHasItem(greenLedName)){greenLed.SetActive(true);}
        if (PlayerHasItem(redLedName)){redLed.SetActive(true);}
        if (PlayerHasItem(yellowLedName)){yellowLed.SetActive(true);}

        if (HasRequiredItemsFromInspector())
        {
            // Unlock the door if reference is set and script is found
            if (doorObject != null)
            {
                DoorConditional doorScript = doorObject.GetComponent<DoorConditional>();
                if (doorScript != null)
                {
                    doorScript.Unlock();
                }
                else
                {
                    Debug.LogWarning("DoorConditional script not found on doorObject!");
                }
            }
            else
            {
                Debug.LogWarning("doorObject reference not set in InventoryChecker!");
            }


        }
    }
}