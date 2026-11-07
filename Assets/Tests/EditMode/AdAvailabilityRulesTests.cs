using NUnit.Framework;
using Game.Core;

namespace Game.Tests.EditMode
{
    [TestFixture]
    public class AdAvailabilityRulesTests
    {
        [TestCase(true, false, false, ExpectedResult = true)]
        [TestCase(false, false, false, ExpectedResult = false)]
        [TestCase(true, true, false, ExpectedResult = false)]
        [TestCase(true, false, true, ExpectedResult = false)]
        public bool CanShowInterstitial_EvaluatesCorrectly(bool isLoaded, bool isCooldown, bool inBoss)
        {
            return AdAvailabilityRules.CanShowInterstitial(isLoaded, isCooldown, inBoss);
        }

        [TestCase(true, 1, 3, ExpectedResult = true)]
        [TestCase(true, 3, 3, ExpectedResult = false)]
        [TestCase(false, 1, 3, ExpectedResult = false)]
        public bool CanShowRewardAd_EvaluatesCorrectly(bool isLoaded, int lives, int maxLives)
        {
            return AdAvailabilityRules.CanShowRewardAd(isLoaded, lives, maxLives);
        }
    }
}
