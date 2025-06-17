using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Reset : MonoBehaviour
{
    void Update()
    {
        // When the "0" key is pressed, reload the current scene
        if (Input.GetKeyDown(KeyCode.Alpha0))
        {
            Vector3 player = FindPlayerPosition();
            GameLogger.Log(player, "Level reset");
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
    private Vector3 FindPlayerPosition()
    {
        GameObject player = GameObject.FindWithTag("Player");
        return player != null ? player.transform.position : Vector3.zero;
    }

}