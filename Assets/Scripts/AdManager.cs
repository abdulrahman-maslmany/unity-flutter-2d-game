using System;
using GoogleMobileAds.Api;
using MyGame;
using UnityEngine;

public class AdManager : MonoBehaviour
{
    public static AdManager Instance { get; private set; }

    // Official Google AdMob test identifiers to prevent invalid traffic and secret leak flags
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
            Debug.Log($"[AdManager] SDK Initialized: {status}");
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
                Debug.LogError($"[AdManager] Interstitial load failed: {error.GetMessage()}");
                return;
            }

            if (ad == null) return;

            interstitialAd = ad;
            interstitialAd.OnAdFullScreenContentClosed += HandleAdClosed;
            interstitialAd.OnAdFullScreenContentFailed += adError =>
                Debug.LogError($"[AdManager] Interstitial display failed: {adError.GetMessage()}");
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
            Debug.LogWarning("[AdManager] Interstitial not ready. Invoking callback.");
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
                Debug.LogError($"[AdManager] Rewarded load failed: {error.GetMessage()}");
                return;
            }

            if (ad == null) return;

            rewardedAd = ad;
            rewardedAd.OnAdFullScreenContentClosed += HandleAdClosed;
            rewardedAd.OnAdFullScreenContentFailed += adError =>
                Debug.LogError($"[AdManager] Rewarded display failed: {adError.GetMessage()}");
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
                Debug.LogError($"[AdManager] Rewarded show error: {error.GetMessage()}");
                ResumeGame();
                onClosed?.Invoke();
                LoadRewardedAd();
            };

            rewardedAd.Show(reward =>
            {
                Debug.Log($"[AdManager] User earned reward. Amount: {reward.Amount}");
                onUserRewarded?.Invoke();
            });
        }
        else
        {
            Debug.LogWarning("[AdManager] Rewarded ad not ready. Triggering fallback.");
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
                Debug.LogError($"[AdManager] Rewarded Interstitial load failed: {error.GetMessage()}");
                return;
            }

            if (ad == null) return;

            rewardedInterstitialAd = ad;
            rewardedInterstitialAd.OnAdFullScreenContentClosed += HandleAdClosed;
            rewardedInterstitialAd.OnAdFullScreenContentFailed += adError =>
                Debug.LogError($"[AdManager] Rewarded Interstitial show error: {adError.GetMessage()}");
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
                Debug.Log("[AdManager] User rewarded from interstitial.");
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
            Debug.LogWarning("[AdManager] No AudioListener found for pausing.");
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
            Debug.LogWarning("[AdManager] Player reference not found in LevelManager during resume.");
        }
    }
    #endregion
}
