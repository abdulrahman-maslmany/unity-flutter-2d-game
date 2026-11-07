namespace Game.Core
{
    public static class AdAvailabilityRules
    {
        public static bool CanShowInterstitial(bool isAdLoaded, bool isCooldownActive, bool isPlayerInBossFight)
        {
            if (!isAdLoaded) return false;
            if (isCooldownActive) return false;
            if (isPlayerInBossFight) return false;
            return true;
        }

        public static bool CanShowRewardAd(bool isRewardedLoaded, int playerLives, int maxLives)
        {
            if (!isRewardedLoaded) return false;
            return playerLives < maxLives;
        }
    }
}
