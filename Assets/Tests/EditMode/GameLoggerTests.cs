using NUnit.Framework;
using MyGame.Core;

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
}
