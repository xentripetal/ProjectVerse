using System;

public static class Debug {
    public static void Log(String message) {
        UnityEngine.Debug.Log(message);
    }

    public static void LogError(String message) {
        UnityEngine.Debug.LogError(message);
    }

    public static void LogWarning(String message) {
        UnityEngine.Debug.LogWarning(message);
    }
}