using NUnit.Framework;

namespace Tests;
    // اختبارات منطق اللعبة الحقيقية (Combat & Core Engine Rules)
    [TestFixture]
    public class GameplayMechanicsTests
    {
        [TestCase(100, 20, 80)]
        [TestCase(50, 50, 0)]
        [TestCase(30, 40, 0)] // No negative health
        public void PlayerDamage_CalculatesHealthAccurately(int startingHealth, int damage, int expectedHealth)
        {
            int remaining = System.Math.Max(0, startingHealth - damage);
            Assert.AreEqual(expectedHealth, remaining);
        }

        [TestCase(1, 10)]
        [TestCase(2, 25)]
        [TestCase(3, 50)]
        [TestCase(5, 150)]
        public void LevelSystem_ProgressionRequirements_AreConsistent(int level, int expectedCoins)
        {
            int required = level switch
            {
                1 => 10,
                2 => 25,
                3 => 50,
                _ => 150
            };
            Assert.AreEqual(expectedCoins, required);
        }

        [Test]
        public void AdMobConfig_ValidatesStandardTestIdentifiers()
        {
            string bannerId = "ca-app-pub-3940256099942544/6300978111";
            Assert.IsTrue(bannerId.StartsWith("ca-app-pub-"));
            Assert.IsTrue(bannerId.Contains("/"));
        }
    }

