using NUnit.Framework;
using Game.Core;

namespace Game.Tests.EditMode;

[TestFixture]
public class ScoreRulesTests
{
    [TestCase(0, ExpectedResult = 0)]
    [TestCase(1, ExpectedResult = 100)]
    [TestCase(2, ExpectedResult = 200)]
    [TestCase(3, ExpectedResult = 400)]
    [TestCase(4, ExpectedResult = 800)]
    [TestCase(5, ExpectedResult = 1000)]
    [TestCase(10, ExpectedResult = 1000)]
    public int CalculateStompScore_EvaluatesComboMultipliersCorrectly(int stompCount)
    {
        return ScoreRules.CalculateStompScore(stompCount);
    }

    [TestCase(99, 1, true, 0)]
    [TestCase(50, 10, false, 60)]
    [TestCase(95, 10, true, 5)]
    [TestCase(0, 0, false, 0)]
    public void TryAwardCoinLife_HandlesThresholdAndRemainderCorrectly(int current, int add, bool expectLife, int expectBalance)
    {
        bool awarded = ScoreRules.TryAwardCoinLife(current, add, out int newBalance);

        Assert.That(awarded, Is.EqualTo(expectLife));
        Assert.That(newBalance, Is.EqualTo(expectBalance));
    }

    [TestCase(0, ExpectedResult = 0)]
    [TestCase(-10, ExpectedResult = 0)]
    [TestCase(10, ExpectedResult = 500)]
    [TestCase(60, ExpectedResult = 3000)]
    public int CalculateTimeBonus_CalculatesAccurately(int remainingSeconds)
    {
        return ScoreRules.CalculateTimeBonus(remainingSeconds);
    }
}