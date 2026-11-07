using NUnit.Framework;
using Game.Core;

namespace Game.Tests.EditMode;

[TestFixture]
public class JumpPhysicsRulesTests
{
    [TestCase(true, 0f, ExpectedResult = true)]
    [TestCase(true, 1f, ExpectedResult = true)]
    [TestCase(false, 0.05f, ExpectedResult = true)]
    [TestCase(false, 0.15f, ExpectedResult = true)]
    [TestCase(false, 0.20f, ExpectedResult = false)]
    public bool CanJump_EvaluatesCoyoteTimeCorrectly(bool isGrounded, float timeSinceLeftLedge)
    {
        return JumpPhysicsRules.CanJump(isGrounded, timeSinceLeftLedge);
    }

    [TestCase(0.02f, ExpectedResult = true)]
    [TestCase(0.10f, ExpectedResult = true)]
    [TestCase(0.15f, ExpectedResult = false)]
    [TestCase(-0.01f, ExpectedResult = false)]
    public bool ShouldExecuteBufferedJump_EvaluatesBufferWindowCorrectly(float timeSincePressed)
    {
        return JumpPhysicsRules.ShouldExecuteBufferedJump(timeSincePressed);
    }

    [TestCase(10f, ExpectedResult = 5f)]
    [TestCase(20f, ExpectedResult = 10f)]
    [TestCase(0f, ExpectedResult = 0f)]
    [TestCase(-5f, ExpectedResult = -5f)]
    public float ApplyJumpCut_HalvesUpwardVelocityOnly(float currentVelocity)
    {
        return JumpPhysicsRules.ApplyJumpCut(currentVelocity);
    }

    [TestCase(-10f, ExpectedResult = -10f)]
    [TestCase(-20f, ExpectedResult = -20f)]
    [TestCase(-35f, ExpectedResult = -20f)]
    [TestCase(15f, ExpectedResult = 15f)]
    public float ClampFallVelocity_ClampsToTerminalLimit(float velocity)
    {
        return JumpPhysicsRules.ClampFallVelocity(velocity);
    }
}