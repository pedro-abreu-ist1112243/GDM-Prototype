using UnityEngine;
using UnityEngine.SceneManagement;

public class Door : MonoBehaviour
{
    public enum DoorDirection { Next, Previous }
    public DoorDirection direction = DoorDirection.Next;
    public float interactionRange = 2f; // Set in Inspector

    void Update()
    {
        string sceneName = SceneManager.GetActiveScene().name;

        // Always get current players in case one spawns during gameplay
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");

        if ((sceneName == "Level_3 2" || sceneName == "Level_4 1") && players.Length >= 2)
        {
            float dist1 = Vector3.Distance(transform.position, players[0].transform.position);
            float dist2 = Vector3.Distance(transform.position, players[1].transform.position);

            if (dist1 <= interactionRange && dist2 <= interactionRange && Input.GetKeyDown(KeyCode.E))
            {
                Debug.Log("Both players are in range. Proceeding through door.");
                if (direction == DoorDirection.Next)
                {
                    SceneController.instance.NextLevel();
                }
                else if (direction == DoorDirection.Previous)
                {
                    SceneController.instance.PreviousLevel();
                }
            }
        }
        else if (!(sceneName == "Level_3 2" || sceneName == "Level_4 1") && players.Length > 0)
        {
            float dist = Vector3.Distance(transform.position, players[0].transform.position);
            if (dist <= interactionRange && Input.GetKeyDown(KeyCode.E))
            {
                Debug.Log("Player is in range. Proceeding through door.");
                if (direction == DoorDirection.Next)
                {
                    SceneController.instance.NextLevel();
                }
                else if (direction == DoorDirection.Previous)
                {
                    SceneController.instance.PreviousLevel();
                }
            }
        }
    }
}