using System;

namespace MyGame.Core;

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
