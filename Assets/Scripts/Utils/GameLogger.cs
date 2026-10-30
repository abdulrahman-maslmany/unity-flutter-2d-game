using UnityEngine;

namespace MyGame.Utils
{
    public static class GameLogger
    {
        [System.Diagnostics.Conditional("UNITY_EDITOR")]
        [System.Diagnostics.Conditional("DEVELOPMENT_BUILD")]
        public static void Log(string tag, string message)
        {
            Debug.Log($"[{tag}] {message}");
        }

        public static void LogWarning(string tag, string message)
        {
            Debug.LogWarning($"[{tag}] {message}");
        }

        public static void LogError(string tag, string message)
        {
            Debug.LogError($"[{tag}] {message}");
        }
    }
}
