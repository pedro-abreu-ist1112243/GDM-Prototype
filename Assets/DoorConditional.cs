using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DoorConditional : MonoBehaviour
{
    [SerializeField] private string sceneToLoad; // Set this in the Inspector
    private bool isUnlocked = false; // By default false

    public void Unlock()
    {
        isUnlocked = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (isUnlocked && other.CompareTag("Player") && !string.IsNullOrEmpty(sceneToLoad))
        {
            GameLogger.Log(transform.position, $"Loading next level: ");
            SceneManager.LoadScene(sceneToLoad);
        }
    }
}