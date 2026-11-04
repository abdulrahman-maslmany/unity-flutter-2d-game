namespace MyGame.Core;

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
