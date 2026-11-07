namespace Game.Core;

public static class JumpPhysicsRules
{
    public const float DefaultCoyoteTime = 0.15f;
    public const float DefaultJumpBuffer = 0.1f;
    public const float TerminalVelocity = -20f;
    public const float JumpCutMultiplier = 0.5f;

    /// <summary>
    /// Checks if player can perform a jump based on grounded state or remaining coyote time.
    /// </summary>
    public static bool CanJump(bool isGrounded, float timeSinceLeftLedge, float coyoteDuration = DefaultCoyoteTime)
    {
        if (isGrounded) return true;
        return timeSinceLeftLedge <= coyoteDuration;
    }

    /// <summary>
    /// Checks if a buffered jump input should be consumed upon landing.
    /// </summary>
    public static bool ShouldExecuteBufferedJump(float timeSinceJumpPressed, float bufferDuration = DefaultJumpBuffer)
    {
        return timeSinceJumpPressed >= 0f && timeSinceJumpPressed <= bufferDuration;
    }

    /// <summary>
    /// Reduces upward velocity when the player releases the jump key early (variable jump height).
    /// </summary>
    public static float ApplyJumpCut(float currentVerticalVelocity)
    {
        if (currentVerticalVelocity > 0f)
        {
            return currentVerticalVelocity * JumpCutMultiplier;
        }
        return currentVerticalVelocity;
    }

    /// <summary>
    /// Clamps falling speed to prevent clipping through collision geometry.
    /// </summary>
    public static float ClampFallVelocity(float verticalVelocity, float maxFallSpeed = TerminalVelocity)
    {
        return System.Math.Max(verticalVelocity, maxFallSpeed);
    }
}