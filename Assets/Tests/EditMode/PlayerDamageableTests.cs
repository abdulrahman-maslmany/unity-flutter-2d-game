using NUnit.Framework;
using MyGame.Core;

namespace Tests
{
    [TestFixture]
    public class PlayerDamageableTests
    {
        [Test]
        public void CombatRules_ApplyDamage_ReducesHealthAccurately()
        {
            int remaining = CombatRules.ApplyDamage(100, 35);
            Assert.AreEqual(65, remaining);
        }

        [Test]
        public void CombatRules_ApplyDamage_ClampsAtZeroOnOverkill()
        {
            int remaining = CombatRules.ApplyDamage(50, 120);
            Assert.AreEqual(0, remaining);
        }

        [Test]
        public void CombatRules_IsLethal_CorrectlyFlagsFatalBlow()
        {
            Assert.IsTrue(CombatRules.IsLethal(30, 45));
            Assert.IsFalse(CombatRules.IsLethal(100, 20));
        }

        [TestCase(1, 3)]
        [TestCase(2, 5)]
        [TestCase(3, 8)]
        [TestCase(4, 12)]
        [TestCase(99, 15)]
        public void LevelProgressionRules_ReturnsCorrectThreshold(int level, int expectedGems)
        {
            int required = LevelProgressionRules.GetRequiredGems(level);
            Assert.AreEqual(expectedGems, required);
        }

        [Test]
        public void LevelProgressionRules_CanUnlock_ValidatesCollectionTarget()
        {
            Assert.IsTrue(LevelProgressionRules.CanUnlock(5, 2));
            Assert.IsFalse(LevelProgressionRules.CanUnlock(4, 2));
        }
    }
}
