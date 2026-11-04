using NUnit.Framework;
using MyGame.Services;

namespace Tests;

[TestFixture]
public class ProgressionStoreTests
{
    // ✅ تم وضع = null!; لمنع خطأ CS8618
    private ISaveStore _mockStore = null!;
    private ProgressionManager _progression = null!;

    [SetUp]
    public void Setup()
    {
        _mockStore = new InMemorySaveStore();
        _progression = new ProgressionManager(_mockStore);
    }

    [Test]
    public void LevelOne_IsUnlockedByDefault()
    {
        Assert.IsTrue(_progression.IsLevelUnlocked(1));
        Assert.IsFalse(_progression.IsLevelUnlocked(2));
    }

    [Test]
    public void UnlockLevel_PersistsUnlockedStateSuccessfully()
    {
        _progression.UnlockLevel(2);
        Assert.IsTrue(_progression.IsLevelUnlocked(2));
        Assert.IsFalse(_progression.IsLevelUnlocked(3));
    }

    [Test]
    public void SaveHighScore_UpdatesOnlyWhenScoreIsHigher()
    {
        _progression.SaveHighScore(500);
        Assert.AreEqual(500, _progression.GetHighScore());

        _progression.SaveHighScore(300); // Should not overwrite with lower score
        Assert.AreEqual(500, _progression.GetHighScore());

        _progression.SaveHighScore(1200); // Should update
        Assert.AreEqual(1200, _progression.GetHighScore());
    }
}
