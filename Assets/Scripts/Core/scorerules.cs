namespace Game.Core;

public static class ScoreRules
{
    public const int BaseStompScore = 100;
    public const int MaxStompScore = 1000;
    public const int CoinsPerExtraLife = 100;

    /// <summary>
    /// Calculates score for consecutive enemy stomps without touching the ground.
    /// Multiplier doubles up to MaxStompScore (100, 200, 400, 800, 1000).
    /// </summary>
    public static int CalculateStompScore(int consecutiveStompCount)
    {
        if (consecutiveStompCount <= 0) return 0;
        if (consecutiveStompCount >= 5) return MaxStompScore;

        int score = BaseStompScore;
        for (int i = 1; i < consecutiveStompCount; i++)
        {
            score *= 2;
        }

        return System.Math.Min(score, MaxStompScore);
    }

    /// <summary>
    /// Checks if coin increment grants an extra life and returns remaining coins.
    /// </summary>
    public static bool TryAwardCoinLife(int currentCoins, int coinsToAdd, out int newCoinBalance)
    {
        int totalCoins = currentCoins + coinsToAdd;
        if (totalCoins >= CoinsPerExtraLife)
        {
            newCoinBalance = totalCoins - CoinsPerExtraLife;
            return true;
        }

        newCoinBalance = totalCoins;
        return false;
    }

    /// <summary>
    /// Calculates completion time bonus based on remaining seconds.
    /// </summary>
    public static int CalculateTimeBonus(int remainingSeconds, int pointsPerSecond = 50)
    {
        if (remainingSeconds <= 0) return 0;
        return remainingSeconds * pointsPerSecond;
    }
}