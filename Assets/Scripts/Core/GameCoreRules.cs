using System;

namespace MyGame.Core
{
    public static class CombatRules
    {
        public static int ApplyDamage(int currentHealth, int incomingDamage)
        {
            return Math.Max(0, currentHealth - incomingDamage);
        }

        public static bool IsLethal(int currentHealth, int incomingDamage)
        {
            return (currentHealth - incomingDamage) <= 0;
        }
    }

    public static class LevelProgressionRules
    {
        public static int GetRequiredGems(int level) => level switch
        {
            1 => 3,
            2 => 5,
            3 => 8,
            4 => 12,
            _ => 15
        };

        public static bool CanUnlock(int gemsCollected, int level)
        {
            return gemsCollected >= GetRequiredGems(level);
        }
    }
}
