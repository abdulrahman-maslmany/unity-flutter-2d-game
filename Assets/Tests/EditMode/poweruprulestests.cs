using NUnit.Framework;
using Game.Core;

namespace Game.Tests.EditMode;

[TestFixture]
public class PowerUpRulesTests
{
    [TestCase(PowerUpState.Small, PowerUpState.Super, ExpectedResult = PowerUpState.Super)]
    [TestCase(PowerUpState.Small, PowerUpState.Fire, ExpectedResult = PowerUpState.Fire)]
    [TestCase(PowerUpState.Super, PowerUpState.Fire, ExpectedResult = PowerUpState.Fire)]
    [TestCase(PowerUpState.Fire, PowerUpState.Super, ExpectedResult = PowerUpState.Fire)]
    public PowerUpState UpgradeState_HandlesProgressionCorrectly(PowerUpState current, PowerUpState collected)
    {
        return PowerUpRules.UpgradeState(current, collected);
    }

    [Test]
    public void ProcessDamage_WhenInvincible_IgnoresHitCompletely()
    {
        bool tookDamage = PowerUpRules.ProcessDamage(
            PowerUpState.Fire,
            isInvincible: true,
            out PowerUpState nextState,
            out float iFrames
        );

        Assert.That(tookDamage, Is.False);
        Assert.That(nextState, Is.EqualTo(PowerUpState.Fire));
        Assert.That(iFrames, Is.EqualTo(0f));
    }

    [Test]
    public void ProcessDamage_WhenPoweredUp_DowngradesToSmallAndGrantsIFrames()
    {
        bool tookDamage = PowerUpRules.ProcessDamage(
            PowerUpState.Fire,
            isInvincible: false,
            out PowerUpState nextState,
            out float iFrames
        );

        Assert.That(tookDamage, Is.True);
        Assert.That(nextState, Is.EqualTo(PowerUpState.Small));
        Assert.That(iFrames, Is.EqualTo(PowerUpRules.InvulnerabilityDuration));
    }

    [TestCase(PowerUpState.Small, false, ExpectedResult = true)]
    [TestCase(PowerUpState.Super, false, ExpectedResult = false)]
    [TestCase(PowerUpState.Fire, false, ExpectedResult = false)]
    [TestCase(PowerUpState.Small, true, ExpectedResult = false)]
    public bool IsFatalHit_EvaluatesFatalityCorrectly(PowerUpState state, bool isInvincible)
    {
        return PowerUpRules.IsFatalHit(state, isInvincible);
    }
}