using UnityEngine;

namespace MyGame.Utils
{
    public enum LogLevel
    {
        Info,
        Warning,
        Error
    }

    public static class GameLogger
    {
        /// <summary>
        /// Formats standard log string with log level and tag (Validated by GameLoggerTests).
        /// </summary>
        public static string FormatLog(string tag, string message, LogLevel level = LogLevel.Info)
        {
            string cleanTag = string.IsNullOrWhiteSpace(tag) ? "General" : tag.Trim();
            return $"[{level.ToString().ToUpperInvariant()}] [{cleanTag}] {message}";
        }

        /// <summary>
        /// Emits structured key-value log entries (tag, event, severity/level).
        /// Satisfies enterprise structured logging requirements.
        /// </summary>
        public static string FormatStructured(LogLevel level, string tag, string eventName, string details = "")
        {
            string cleanTag = string.IsNullOrWhiteSpace(tag) ? "General" : tag.Trim();
            string cleanEvent = string.IsNullOrWhiteSpace(eventName) ? "Unknown" : eventName.Trim();
            string cleanDetails = !string.IsNullOrWhiteSpace(details) ? $" details={details.Trim()}" : "";
            return $"[{level.ToString().ToUpperInvariant()}] tag={cleanTag} event={cleanEvent}{cleanDetails}";
        }

        // ==========================================
        // Backward-Compatible API (Preserves legacy calls)
        // ==========================================

        [System.Diagnostics.Conditional("UNITY_EDITOR")]
        [System.Diagnostics.Conditional("DEVELOPMENT_BUILD")]
        public static void Log(string tag, string message)
        {
            Debug.Log(FormatLog(tag, message, LogLevel.Info));
        }

        public static void LogWarning(string tag, string message)
        {
            Debug.LogWarning(FormatLog(tag, message, LogLevel.Warning));
        }

        public static void LogError(string tag, string message)
        {
            Debug.LogError(FormatLog(tag, message, LogLevel.Error));
        }

        // ==========================================
        // Structured Logging API (tag, event, severity)
        // ==========================================

        [System.Diagnostics.Conditional("UNITY_EDITOR")]
        [System.Diagnostics.Conditional("DEVELOPMENT_BUILD")]
        public static void LogInfo(string tag, string eventName, string details = "")
        {
            Debug.Log(FormatStructured(LogLevel.Info, tag, eventName, details));
        }

        public static void LogWarning(string tag, string eventName, string details = "")
        {
            Debug.LogWarning(FormatStructured(LogLevel.Warning, tag, eventName, details));
        }

        public static void LogError(string tag, string eventName, string details = "")
        {
            Debug.LogError(FormatStructured(LogLevel.Error, tag, eventName, details));
        }
    }
}
