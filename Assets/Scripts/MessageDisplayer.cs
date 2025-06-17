using System.Collections;
using UnityEngine;
using TMPro;

public class MessageDisplayer : MonoBehaviour
{
    [SerializeField] private GameObject messagePanel;
    [SerializeField] private TMP_Text messageText;
    [SerializeField] private float displayDuration = 5f;

    private Coroutine currentMessage;

    public void ShowMessage(string message)
    {
        if (currentMessage != null)
            StopCoroutine(currentMessage);

        currentMessage = StartCoroutine(ShowMessageRoutine(message));
    }

    private IEnumerator ShowMessageRoutine(string message)
    {
        messageText.text = message;
        messagePanel.SetActive(true);
        yield return new WaitForSeconds(displayDuration);
        messagePanel.SetActive(false);
        currentMessage = null;
    }
}
