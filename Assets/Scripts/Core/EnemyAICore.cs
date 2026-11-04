using UnityEngine;

namespace MyGame.Core;

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

    // ✅ الدالة الناقصة الأولى
    public static bool CanAttack(float currentTime, float lastAttackTime, float attackCooldown)
    {
        return (currentTime - lastAttackTime) >= attackCooldown;
    }

    // ✅ الدالة الناقصة الثانية
    public static float CalculateFacingDirection(float directionX)
    {
        if (Mathf.Abs(directionX) <= 0.001f)
        {
            return 1f;
        }

        return directionX > 0f ? 1f : -1f;
    }
}
