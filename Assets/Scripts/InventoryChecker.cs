using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryChecker : MonoBehaviour
{
    // Reference to the PlayerInteractions script
    [SerializeField] private PlayerInteractions playerInteractions;

    // Reference to the Door object
    [SerializeField] private Door door;
    [SerializeField] private GameObject greenLed;
    [SerializeField] private GameObject redLed;
    [SerializeField] private GameObject yellowLed;

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
        Debug.LogWarning("ITEMSSS");
        if (playerInteractions == null)
        {
            Debug.LogWarning("PlayerInteractions reference not set on InventoryChecker.");
            return false;
        }

        // Debug message showing both inventories

        foreach (string item in itemsToCheck)
        {
            if (!playerInteractions.inventory.Contains(item))
            {
                return false;
            }
        }
        return true;
    }

    // Call this to compare the player's inventory to the provided list from the inspector
    public bool HasRequiredItemsFromInspector()
    {
        return HasItems(requiredItemsList);
    }

    public void PlayerInteract()
    {
        if (door != null && HasRequiredItemsFromInspector())
        {
            //Doesn't have to be a door, can be any object
            door.GetComponent<BoxCollider>().enabled = true;
            greenLed.SetActive(true);
            redLed.SetActive(true);
            yellowLed.SetActive(true);
        }
    }
}