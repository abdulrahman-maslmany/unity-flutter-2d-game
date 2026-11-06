using NUnit.Framework;
using MyGame.Core;
using MyGame.Utils;

namespace Tests;

[TestFixture]
public class GameLoggerTests
{
    [Test]
    public void FormatMessage_IncludesLogLevelAndTagCorrectly()
    {
        string formatted = GameLogger.FormatLog("Combat", "Player took damage", LogLevel.Info);
        Assert.IsTrue(formatted.Contains("[Combat]"));
        Assert.IsTrue(formatted.Contains("Player took damage"));
        Assert.IsTrue(formatted.Contains("INFO"));
    }

    [Test]
    public void FormatStructured_IncludesTagEventAndDetailsCorrectly()
    {
        string formatted = GameLogger.FormatStructured(LogLevel.Error, "AdManager", "AdLoadFailed", "code=1001");
        Assert.IsTrue(formatted.Contains("tag=AdManager"));
        Assert.IsTrue(formatted.Contains("event=AdLoadFailed"));
        Assert.IsTrue(formatted.Contains("details=code=1001"));
        Assert.IsTrue(formatted.Contains("ERROR"));
    }
}
