using System;

namespace MyGame.Core;

/// <summary>
/// Pure AI evaluation logic for enemy detection, attack range, and cooldown math.
/// Decoupled from Unity Engine runtime for sub-second unit testing.
/// </summary>
public static class EnemyAICore
{
    public static bool IsInDetectionRange(float distance, float detectionRange)
    {
        return distance > 0f && distance <= detectionRange;
    }

    public static bool IsInAttackRange(float distance, float attackRange)
    {
        return distance > 0f && distance <= attackRange;
    }

    public static bool CanAttack(float currentTime, float lastAttackTime, float attackCooldown)
    {
        return (currentTime - lastAttackTime) >= attackCooldown;
    }

    public static float CalculateFacingDirection(float directionX)
    {
        // استخدام Math.Abs من مكتبة System القياسية بدلاً من Mathf الخاصة بيونيتي
        if (Math.Abs(directionX) <= 0.001f)
        {
            return 1f;
        }

        return directionX > 0f ? 1f : -1f;
    }
}
