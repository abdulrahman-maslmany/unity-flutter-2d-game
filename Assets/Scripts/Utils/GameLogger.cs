using UnityEngine;

namespace MyGame.Utils
{
    public enum LogSeverity
    {
        Info,
        Warning,
        Error
    }

    public static class GameLogger
    {
        /// <summary>
        /// Pure formatter producing structured key-value log entries (tag, event, severity).
        /// Ideal for log aggregation, analytics, and headless unit testing.
        /// </summary>
        public static string FormatEntry(LogSeverity severity, string tag, string eventName, string details = "")
        {
            string cleanTag = string.IsNullOrWhiteSpace(tag) ? "General" : tag.Trim();
            string cleanEvent = string.IsNullOrWhiteSpace(eventName) ? "Unknown" : eventName.Trim();
            string cleanDetails = details != null ? details.Trim() : string.Empty;

            string baseEntry = $"[{severity.ToString().ToUpperInvariant()}] tag={cleanTag} event={cleanEvent}";
            return string.IsNullOrEmpty(cleanDetails) ? baseEntry : $"{baseEntry} details={cleanDetails}";
        }

        // ==========================================
        // Backward-Compatible API (Preserves legacy calls)
        // ==========================================

        [System.Diagnostics.Conditional("UNITY_EDITOR")]
        [System.Diagnostics.Conditional("DEVELOPMENT_BUILD")]
        public static void Log(string tag, string message)
        {
            Debug.Log(FormatEntry(LogSeverity.Info, tag, "General", message));
        }

        public static void LogWarning(string tag, string message)
        {
            Debug.LogWarning(FormatEntry(LogSeverity.Warning, tag, "Warning", message));
        }

        public static void LogError(string tag, string message)
        {
            Debug.LogError(FormatEntry(LogSeverity.Error, tag, "Error", message));
        }

        // ==========================================
        // New Structured Logging API (tag, event, severity)
        // ==========================================

        [System.Diagnostics.Conditional("UNITY_EDITOR")]
        [System.Diagnostics.Conditional("DEVELOPMENT_BUILD")]
        public static void LogInfo(string tag, string eventName, string details = "")
        {
            Debug.Log(FormatEntry(LogSeverity.Info, tag, eventName, details));
        }

        public static void LogWarning(string tag, string eventName, string details = "")
        {
            Debug.LogWarning(FormatEntry(LogSeverity.Warning, tag, eventName, details));
        }

        public static void LogError(string tag, string eventName, string details = "")
        {
            Debug.LogError(FormatEntry(LogSeverity.Error, tag, eventName, details));
        }
    }
}
