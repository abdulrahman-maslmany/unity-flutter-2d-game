using NUnit.Framework;

namespace Tests;
    // اختبارات منطق فتح المستويات وحساب الجواهر
    public class LevelProgressionCore
    {
        public static int GetRequiredGemsForLevel(int level) => level switch
        {
            1 => 3,
            2 => 5,
            3 => 8,
            4 => 12,
            _ => 15
        };

        public static bool CanUnlockNextLevel(int currentGems, int level)
        {
            return currentGems >= GetRequiredGemsForLevel(level);
        }
    }

    [TestFixture]
    public class LevelManagerTests
    {
        [TestCase(1, 3)]
        [TestCase(2, 5)]
        [TestCase(3, 8)]
        [TestCase(4, 12)]
        [TestCase(99, 15)]
        public void GetRequiredGemsForLevel_ReturnsCorrectThreshold(int level, int expected)
        {
            Assert.AreEqual(expected, LevelProgressionCore.GetRequiredGemsForLevel(level));
        }

        [Test]
        public void CanUnlockNextLevel_EvaluatesCollectionTarget()
        {
            Assert.IsTrue(LevelProgressionCore.CanUnlockNextLevel(5, 2));
            Assert.IsFalse(LevelProgressionCore.CanUnlockNextLevel(4, 2));
        }
    }

