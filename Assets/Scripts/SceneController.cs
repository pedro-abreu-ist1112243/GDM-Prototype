using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    public static SceneController instance;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void NextLevel()
    {
        int nextIndex = SceneManager.GetActiveScene().buildIndex + 1;

        if (nextIndex < SceneManager.sceneCountInBuildSettings)
        {
            Vector3 playerPos = FindPlayerPosition();
            GameLogger.Log(playerPos, $"Loading next level: {SceneManager.GetSceneByBuildIndex(nextIndex).name}");
            SceneManager.LoadSceneAsync(nextIndex);
        }
    }


    public void PreviousLevel()
    {
        int prevIndex = SceneManager.GetActiveScene().buildIndex - 1;

        if (prevIndex >= 0)
        {
            SceneManager.LoadSceneAsync(prevIndex);
        }
    }

    private Vector3 FindPlayerPosition()
    {
        GameObject player = GameObject.FindWithTag("Player");
        return player != null ? player.transform.position : Vector3.zero;
    }

}
