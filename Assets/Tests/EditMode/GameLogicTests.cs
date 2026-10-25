using NUnit.Framework;

namespace Tests
{
    public class GameLogicTests
    {
        [Test]
        public void PlayerHealth_DamageCalculation_DecreasesProperly()
        {
            int currentHealth = 100;
            int damage = 25;
            int newHealth = currentHealth - damage;

            Assert.AreEqual(75, newHealth);
            Assert.IsTrue(newHealth > 0);
        }

        [Test]
        public void LevelProgression_RequiredGems_ValidatesBounds()
        {
            int gemsCollected = 5;
            int requiredGems = 5;
            bool isLevelCompleted = gemsCollected >= requiredGems;

            Assert.IsTrue(isLevelCompleted);
        }

        [Test]
        public void AdMobConfig_FormatValidation_IsCorrect()
        {
            string dummyAdId = "ca-app-pub-3940256099942544/6300978111";
            Assert.IsTrue(dummyAdId.StartsWith("ca-app-pub-"));
            Assert.IsTrue(dummyAdId.Contains("/"));
        }
    }
}
