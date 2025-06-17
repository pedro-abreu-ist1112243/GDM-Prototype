using UnityEngine;

public class TextTriggerZone : MonoBehaviour
{
    [SerializeField] private string message = "You found a secret!";
    [SerializeField] private MessageDisplayer messageDisplayer;

    private bool hasTriggered = false;

    private void OnTriggerEnter(Collider other)
    {
        if (hasTriggered) return;

        if (other.CompareTag("Player"))
        {
            messageDisplayer.ShowMessage(message);
            hasTriggered = true; // Optional: so it triggers only once
        }
    }
}
