using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CyclingObject : MonoBehaviour
{
    public GameObject[] objectsToCycle; // Assign in Inspector
    public float cycleInterval = 2f;    // Time between cycles

    public Collider detectionTrigger;    // Assign a trigger collider in Inspector

    private int currentIndex = 0;
    private float timer = 0f;

    void Start()
    {
        SetActiveObject(currentIndex);
    }

    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= cycleInterval)
        {
            timer = 0f;
            currentIndex = (currentIndex + 1) % objectsToCycle.Length;
            SetActiveObject(currentIndex);
        }
    }

    void SetActiveObject(int index)
{
    for (int i = 0; i < objectsToCycle.Length; i++)
    {
        if (objectsToCycle[i] != null)
            objectsToCycle[i].SetActive(i == index);
    }

    // Enable detection trigger only when the first object is active
    if (detectionTrigger != null)
        detectionTrigger.enabled = (index == 0);
}

    public void OnPlayerTriggerEnter(Collider other)
{
    if (other.CompareTag("Player"))
    {
        Debug.Log("Player caught! Resetting scene.");
        GameLogger.Log(other.transform.position, "Player died during stealth segment");
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
}