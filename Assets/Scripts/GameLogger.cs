using UnityEngine;
using System.IO;
using System;

public static class GameLogger
{
    private static string logFilePath;
    private static bool initialized = false;

    static GameLogger()
    {
        Initialize();
    }

    private static void Initialize()
    {
        if (initialized) return;

        string folderPath = Path.Combine(Application.persistentDataPath, "Logs");
        if (!Directory.Exists(folderPath))
            Directory.CreateDirectory(folderPath);

        logFilePath = Path.Combine(folderPath, $"Log_{DateTime.Now:yyyy-MM-dd_HH-mm-ss}.csv");

        // Write CSV header
        try
        {
            File.WriteAllText(logFilePath, "Time,X,Y,Z,Description\n");
            initialized = true;
        }
        catch (Exception e)
        {
            Debug.LogError("Logger initialization error: " + e.Message);
        }
    }

    public static void Log(Vector3 playerPosition, string description)
    {
        if (!initialized)
            Initialize();

        string time = DateTime.Now.ToString("HH:mm:ss");
        string logLine = $"{time},{playerPosition.x:F2},{playerPosition.y:F2},{playerPosition.z:F2},\"{description}\"";

        Debug.Log("[Log] " + logLine); // Also logs to Unity Console

        try
        {
            File.AppendAllText(logFilePath, logLine + Environment.NewLine);
        }
        catch (Exception e)
        {
            Debug.LogError("Logger write error: " + e.Message);
        }
    }
}
