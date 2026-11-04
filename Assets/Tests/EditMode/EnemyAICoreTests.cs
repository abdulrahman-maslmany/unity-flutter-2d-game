using NUnit.Framework;
using MyGame.Core;

namespace Tests;

[TestFixture]
public class EnemyAICoreTests
{
    [TestCase(3.0f, 5.0f, ExpectedResult = true)]
    [TestCase(5.0f, 5.0f, ExpectedResult = true)]
    [TestCase(5.1f, 5.0f, ExpectedResult = false)]
    [TestCase(0.0f, 5.0f, ExpectedResult = false)]
    public bool IsInDetectionRange_EvaluatesBoundaryConditions(float distance, float detectionRange)
    {
        return EnemyAICore.IsInDetectionRange(distance, detectionRange);
    }

    [TestCase(0.8f, 1.0f, ExpectedResult = true)]
    [TestCase(1.0f, 1.0f, ExpectedResult = true)]
    [TestCase(1.2f, 1.0f, ExpectedResult = false)]
    public bool IsInAttackRange_EvaluatesBoundaryConditions(float distance, float attackRange)
    {
        return EnemyAICore.IsInAttackRange(distance, attackRange);
    }

    [TestCase(5.0f, 3.0f, 1.0f, ExpectedResult = true)]  // 2s elapsed >= 1s cooldown
    [TestCase(3.5f, 3.0f, 1.0f, ExpectedResult = false)] // 0.5s elapsed < 1s cooldown
    [TestCase(4.0f, 3.0f, 1.0f, ExpectedResult = true)]  // Exactly cooldown reached
    public bool CanAttack_ValidatesCooldownAccurately(float currentTime, float lastAttackTime, float cooldown)
    {
        return EnemyAICore.CanAttack(currentTime, lastAttackTime, cooldown);
    }

    [TestCase(2.5f, ExpectedResult = 1f)]
    [TestCase(-3.0f, ExpectedResult = -1f)]
    [TestCase(0.0001f, ExpectedResult = 1f)]
    public float CalculateFacingDirection_FlipsCorrectly(float directionX)
    {
        return EnemyAICore.CalculateFacingDirection(directionX);
    }
}
