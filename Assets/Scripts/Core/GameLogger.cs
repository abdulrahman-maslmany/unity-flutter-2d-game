namespace MyGame.Core;

public enum LogLevel
{
    Info,
    Warning,
    Error
}

/// <summary>
/// Structured domain logger for game events and telemetry.
/// Pure C# implementation decoupled from UnityEngine runtime.
/// </summary>
public static class GameLogger
{
    public static string FormatLog(string tag, string message, LogLevel level)
    {
        return $"[{level.ToString().ToUpperInvariant()}] [{tag}] {message}";
    }
}
