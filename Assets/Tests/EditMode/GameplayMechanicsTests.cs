using NUnit.Framework;

namespace Tests
{
    // فئة منطق مجردة بدون MonoBehaviour لقياس حسابات ضرر اللاعب ومستويات اللعبة
    public class CombatCalculator
    {
        public static int CalculateDamage(int baseDamage, int armorMultiplier)
        {
            return System.Math.Max(1, baseDamage - armorMultiplier);
        }

        public static bool CheckLethalDamage(int currentHealth, int incomingDamage)
        {
            return (currentHealth - incomingDamage) <= 0;
        }
    }

    public class LevelProgressionSystem
    {
        public static int GetTargetCoinsForLevel(int level) => level switch
        {
            1 => 10,
            2 => 25,
            3 => 50,
            4 => 100,
            _ => 150
        };

        public static bool IsLevelUnlockable(int collectedCoins, int level)
        {
            return collectedCoins >= GetTargetCoinsForLevel(level);
        }
    }

    [TestFixture]
    public class GameplayMechanicsTests
    {
        [TestCase(50, 10, 40)]
        [TestCase(10, 20, 1)] // Min damage floor check
        public void CombatCalculator_CalculatesCorrectArmorMitigation(int baseDmg, int armor, int expected)
        {
            int actualDamage = CombatCalculator.CalculateDamage(baseDmg, armor);
            Assert.AreEqual(expected, actualDamage);
        }

        [Test]
        public void CombatCalculator_DetectsLethalBlowCorrectly()
        {
            Assert.IsTrue(CombatCalculator.CheckLethalDamage(30, 35));
            Assert.IsFalse(CombatCalculator.CheckLethalDamage(100, 25));
        }

        [TestCase(1, 10, true)]
        [TestCase(2, 20, false)] // Needs 25
        [TestCase(3, 50, true)]
        public void LevelProgression_ValidatesGemsRequirement(int level, int coins, bool expectedUnlock)
        {
            bool canUnlock = LevelProgressionSystem.IsLevelUnlockable(coins, level);
            Assert.AreEqual(expectedUnlock, canUnlock);
        }
    }
}
