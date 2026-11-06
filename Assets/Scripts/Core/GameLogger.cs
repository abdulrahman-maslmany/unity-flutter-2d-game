using System;

namespace MyGame.Core
{
    public enum LogLevel
    {
        Info,
        Warning,
        Error
    }

    /// <summary>
    /// Pure C# logging and formatting engine decoupled from UnityEngine.
    /// Provides structured logging capabilities verified by automated NUnit tests.
    /// </summary>
    public static class GameLogger
    {
        // Handlers allowing Unity MonoBehaviours to subscribe to logs (e.g. Debug.Log)
        public static Action<string> LogHandler { get; set; }
        public static Action<string> WarningHandler { get; set; }
        public static Action<string> ErrorHandler { get; set; }

        public static string FormatLog(string tag, string message, LogLevel level = LogLevel.Info)
        {
            string cleanTag = string.IsNullOrWhiteSpace(tag) ? "General" : tag.Trim();
            return $"[{level.ToString().ToUpperInvariant()}] [{cleanTag}] {message}";
        }

        public static string FormatStructured(LogLevel level, string tag, string eventName, string details = "")
        {
            string cleanTag = string.IsNullOrWhiteSpace(tag) ? "General" : tag.Trim();
            string cleanEvent = string.IsNullOrWhiteSpace(eventName) ? "Unknown" : eventName.Trim();
            string cleanDetails = !string.IsNullOrWhiteSpace(details) ? $" details={details.Trim()}" : "";
            return $"[{level.ToString().ToUpperInvariant()}] tag={cleanTag} event={cleanEvent}{cleanDetails}";
        }

        public static void Log(string tag, string message)
        {
            string entry = FormatLog(tag, message, LogLevel.Info);
            LogHandler?.Invoke(entry);
        }

        public static void LogWarning(string tag, string message)
        {
            string entry = FormatLog(tag, message, LogLevel.Warning);
            WarningHandler?.Invoke(entry);
        }

        public static void LogError(string tag, string message)
        {
            string entry = FormatLog(tag, message, LogLevel.Error);
            ErrorHandler?.Invoke(entry);
        }
    }
}
