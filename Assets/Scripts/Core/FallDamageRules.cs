namespace Game.Core
{
    public static class FallDamageRules
    {
        public const float MinimumFallThreshold = 10f;
        public const float FatalFallThreshold = 25f;
        public const float DamageMultiplier = 5f;

        public static int CalculateDamage(float fallDistance)
        {
            if (fallDistance <= MinimumFallThreshold) return 0;
            if (fallDistance >= FatalFallThreshold) return 100;

            return (int)((fallDistance - MinimumFallThreshold) * DamageMultiplier);
        }

        public static bool IsLethalFall(float fallDistance, int currentHealth)
        {
            return CalculateDamage(fallDistance) >= currentHealth;
        }
    }
}
