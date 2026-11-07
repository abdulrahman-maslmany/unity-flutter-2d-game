using NUnit.Framework;
using Game.Core;

namespace Game.Tests.EditMode
{
    [TestFixture]
    public class FallDamageRulesTests
    {
        [TestCase(5f, ExpectedResult = 0)]
        [TestCase(10f, ExpectedResult = 0)]
        [TestCase(12f, ExpectedResult = 10)]
        [TestCase(25f, ExpectedResult = 100)]
        [TestCase(30f, ExpectedResult = 100)]
        public int CalculateDamage_EvaluatesCorrectly(float fallDistance)
        {
            return FallDamageRules.CalculateDamage(fallDistance);
        }

        [TestCase(30f, 100, ExpectedResult = true)]
        [TestCase(12f, 100, ExpectedResult = false)]
        [TestCase(12f, 5, ExpectedResult = true)]
        public bool IsLethalFall_EvaluatesCorrectly(float fallDistance, int currentHealth)
        {
            return FallDamageRules.IsLethalFall(fallDistance, currentHealth);
        }
    }
}
