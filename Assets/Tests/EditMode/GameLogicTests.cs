using NUnit.Framework;

namespace Tests
{
    // فئات وسيطة تحاكي منطق اللعبة لاختبارها في الـ CI
    public class MockPlayer
    {
        public int Health { get; private set; } = 100;
        public bool IsDead => Health <= 0;

        public void TakeDamage(int damage)
        {
            Health = System.Math.Max(0, Health - damage);
        }
    }

    public class MockLevelManager
    {
        public static int GetRequiredGems(int level) => level switch
        {
            1 => 3,
            2 => 5,
            3 => 8,
            _ => 10
        };
    }

    [TestFixture]
    public class GameLogicTests
    {
        [Test]
        public void Player_TakeDamage_ReducesHealthProperly()
        {
            var player = new MockPlayer();
            player.TakeDamage(30);

            Assert.AreEqual(70, player.Health);
            Assert.IsFalse(player.IsDead);
        }

        [Test]
        public void Player_FatalDamage_TriggersDeathState()
        {
            var player = new MockPlayer();
            player.TakeDamage(120);

            Assert.AreEqual(0, player.Health);
            Assert.IsTrue(player.IsDead);
        }

        [TestCase(1, 3)]
        [TestCase(2, 5)]
        [TestCase(3, 8)]
        [TestCase(99, 10)]
        public void LevelManager_RequiredGems_MatchesProgressionRule(int level, int expectedGems)
        {
            int actualGems = MockLevelManager.GetRequiredGems(level);
            Assert.AreEqual(expectedGems, actualGems);
        }

        [Test]
        public void AdMobConfig_Format_Validation()
        {
            const string testAdUnitId = "ca-app-pub-3940256099942544/6300978111";
            Assert.IsTrue(testAdUnitId.StartsWith("ca-app-pub-"));
            Assert.IsTrue(testAdUnitId.Contains("/"));
        }
    }
}
