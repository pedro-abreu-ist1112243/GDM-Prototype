using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Door : MonoBehaviour
{
    public enum DoorDirection { Next, Previous }
    public DoorDirection direction = DoorDirection.Next;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
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