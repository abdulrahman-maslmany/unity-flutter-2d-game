using System;
using GoogleMobileAds.Api;
using MyGame;
using MyGame.Utils;
using UnityEngine;

public class AdManager : MonoBehaviour
{
    private const string Tag = "AdManager";

    public static AdManager Instance { get; private set; }

    // Official Google AdMob test identifiers
    private const string InterstitialAdUnitId = "ca-app-pub-3940256099942544/1033173712";
    private const string RewardedAdUnitId = "ca-app-pub-3940256099942544/5224354917";
    private const string BannerAdUnitId = "ca-app-pub-3940256099942544/6300978111";
    private const string RewardedInterstitialAdUnitId = "ca-app-pub-3940256099942544/5354046379";

    private InterstitialAd interstitialAd;
    private RewardedAd rewardedAd;
    private RewardedInterstitialAd rewardedInterstitialAd;
    private BannerView bannerView;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        MobileAds.Initialize(status =>
        {
            GameLogger.Log(Tag, $"SDK Initialized successfully: {status}");
            LoadInterstitialAd();
            LoadRewardedAd();
            LoadRewardedInterstitialAd();
            LoadBannerAd();
        });
    }

    private void OnDestroy()
    {
        bannerView?.Destroy();
        interstitialAd?.Destroy();
    }

    #region Interstitial
    private void LoadInterstitialAd()
    {
        interstitialAd?.Destroy();

        var request = new AdRequest();
        InterstitialAd.Load(InterstitialAdUnitId, request, (InterstitialAd ad, LoadAdError error) =>
        {
            if (error != null)
            {
                GameLogger.LogError(Tag, $"Interstitial load failed: {error.GetMessage()}");
                return;
            }

            if (ad == null) return;

            interstitialAd = ad;
            interstitialAd.OnAdFullScreenContentClosed += HandleAdClosed;
            interstitialAd.OnAdFullScreenContentFailed += adError =>
                GameLogger.LogError(Tag, $"Interstitial display failed: {adError.GetMessage()}");
        });
    }

    public void ShowInterstitialAd(Action onAdClosed = null)
    {
        if (interstitialAd != null && interstitialAd.CanShowAd())
        {
            PauseGame();
            interstitialAd.OnAdFullScreenContentClosed -= HandleAdClosed;
            interstitialAd.OnAdFullScreenContentClosed += () =>
            {
                HandleAdClosed();
                onAdClosed?.Invoke();
                LoadInterstitialAd();
            };
            interstitialAd.Show();
        }
        else
        {
            GameLogger.LogWarning(Tag, "Interstitial ad not ready. Invoking callback fallback.");
            onAdClosed?.Invoke();
            LoadInterstitialAd();
        }
    }

    private void HandleAdClosed()
    {
        ResumeGame();
    }
    #endregion

    #region Rewarded
    private void LoadRewardedAd()
    {
        rewardedAd = null;

        var request = new AdRequest();
        RewardedAd.Load(RewardedAdUnitId, request, (RewardedAd ad, LoadAdError error) =>
        {
            if (error != null)
            {
                GameLogger.LogError(Tag, $"Rewarded load failed: {error.GetMessage()}");
                return;
            }

            if (ad == null) return;

            rewardedAd = ad;
            rewardedAd.OnAdFullScreenContentClosed += HandleAdClosed;
            rewardedAd.OnAdFullScreenContentFailed += adError =>
                GameLogger.LogError(Tag, $"Rewarded display failed: {adError.GetMessage()}");
        });
    }

    public void ShowRewardedAd(Action onUserRewarded = null, Action onClosed = null)
    {
        if (rewardedAd != null && rewardedAd.CanShowAd())
        {
            PauseGame();

            rewardedAd.OnAdFullScreenContentClosed += () =>
            {
                ResumeGame();
                onClosed?.Invoke();
                LoadRewardedAd();
            };

            rewardedAd.OnAdFullScreenContentFailed += error =>
            {
                GameLogger.LogError(Tag, $"Rewarded display error: {error.GetMessage()}");
                ResumeGame();
                onClosed?.Invoke();
                LoadRewardedAd();
            };

            rewardedAd.Show(reward =>
            {
                GameLogger.Log(Tag, $"User earned reward. Amount: {reward.Amount}");
                onUserRewarded?.Invoke();
            });
        }
        else
        {
            GameLogger.LogWarning(Tag, "Rewarded ad not ready. Triggering fallback.");
            onClosed?.Invoke();
            LoadRewardedAd();
        }
    }

    private void LoadRewardedInterstitialAd()
    {
        rewardedInterstitialAd = null;

        var request = new AdRequest();
        RewardedInterstitialAd.Load(RewardedInterstitialAdUnitId, request, (RewardedInterstitialAd ad, LoadAdError error) =>
        {
            if (error != null)
            {
                GameLogger.LogError(Tag, $"Rewarded Interstitial load failed: {error.GetMessage()}");
                return;
            }

            if (ad == null) return;

            rewardedInterstitialAd = ad;
            rewardedInterstitialAd.OnAdFullScreenContentClosed += HandleAdClosed;
            rewardedInterstitialAd.OnAdFullScreenContentFailed += adError =>
                GameLogger.LogError(Tag, $"Rewarded Interstitial show error: {adError.GetMessage()}");
        });
    }

    public void ShowRewardedInterstitialAd(Action onUserRewarded = null, Action onClosed = null)
    {
        if (rewardedInterstitialAd != null && rewardedInterstitialAd.CanShowAd())
        {
            PauseGame();
            rewardedInterstitialAd.OnAdFullScreenContentClosed -= HandleAdClosed;
            rewardedInterstitialAd.OnAdFullScreenContentClosed += () =>
            {
                HandleAdClosed();
                onClosed?.Invoke();
                LoadRewardedInterstitialAd();
            };
            rewardedInterstitialAd.Show(reward =>
            {
                onUserRewarded?.Invoke();
                GameLogger.Log(Tag, "User rewarded from interstitial.");
            });
        }
        else
        {
            onUserRewarded?.Invoke();
            onClosed?.Invoke();
            LoadRewardedInterstitialAd();
        }
    }
    #endregion

    #region Banner
    private void LoadBannerAd()
    {
        bannerView?.Destroy();
        bannerView = new BannerView(BannerAdUnitId, AdSize.Banner, AdPosition.Bottom);
        var request = new AdRequest();
        bannerView.LoadAd(request);
    }

    public void HideBanner() => bannerView?.Hide();
    public void ShowBanner() => bannerView?.Show();
    #endregion

    #region Pause / Resume
    private void PauseGame()
    {
        Time.timeScale = 0f;
        var audioListener = UnityEngine.Object.FindFirstObjectByType<AudioListener>();
        if (audioListener != null)
        {
            AudioListener.pause = true;
        }
        else
        {
            GameLogger.LogWarning(Tag, "No AudioListener found for pausing.");
        }
    }

    private void ResumeGame()
    {
        Time.timeScale = 1f;
        AudioListener.pause = false;

        if (LevelManager.Instance?.player != null)
        {
            var player = LevelManager.Instance.player;
            player.gameObject.SetActive(true);
            var rb = player.GetComponent<Rigidbody2D>();
            if (rb != null) rb.linearVelocity = Vector2.zero;

            LevelManager.Instance.RestorePlayerInput();
            LevelManager.Instance.ReLinkMovementButtons();
        }
        else
        {
            GameLogger.LogWarning(Tag, "Player reference not found in LevelManager during resume.");
        }
    }
    #endregion
}
