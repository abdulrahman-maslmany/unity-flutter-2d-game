namespace MyGame.Core;
public static class EnemyAICore
{
    public static bool IsInDetectionRange(float distance, float range) => distance <= range;
    public static bool IsInAttackRange(float distance, float attackRange) => distance <= attackRange;
}
