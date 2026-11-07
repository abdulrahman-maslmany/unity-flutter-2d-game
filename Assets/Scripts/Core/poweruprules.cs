namespace Game.Core;

public enum PowerUpState
{
    Small = 0,
    Super = 1,
    Fire = 2
}

public static class PowerUpRules
{
    public const float InvulnerabilityDuration = 2.0f;

    /// <summary>
    /// Evaluates state progression when collecting a power-up item.
    /// </summary>
    public static PowerUpState UpgradeState(PowerUpState currentState, PowerUpState collectedType)
    {
        if (collectedType == PowerUpState.Fire)
        {
            return PowerUpState.Fire;
        }

        if (collectedType == PowerUpState.Super && currentState == PowerUpState.Small)
        {
            return PowerUpState.Super;
        }

        return currentState;
    }

    /// <summary>
    /// Processes enemy hit damage. If player has Super or Fire state, downgrades to Small
    /// and grants invulnerability frames instead of dying.
    /// </summary>
    public static bool ProcessDamage(
        PowerUpState currentState,
        bool isInvincible,
        out PowerUpState nextState,
        out float invulnerabilityGranted)
    {
        // Player is immune to damage during star power or post-hit i-frames
        if (isInvincible)
        {
            nextState = currentState;
            invulnerabilityGranted = 0f;
            return false; // Did not take damage
        }

        // Powered up player absorbs hit and downgrades
        if (currentState != PowerUpState.Small)
        {
            nextState = PowerUpState.Small;
            invulnerabilityGranted = InvulnerabilityDuration;
            return true; // Damage absorbed safely
        }

        // Small player suffers lethal damage
        nextState = PowerUpState.Small;
        invulnerabilityGranted = 0f;
        return true; // Lethal hit
    }

    /// <summary>
    /// Checks if a hit on small player is fatal.
    /// </summary>
    public static bool IsFatalHit(PowerUpState state, bool isInvincible)
    {
        if (isInvincible) return false;
        return state == PowerUpState.Small;
    }
}