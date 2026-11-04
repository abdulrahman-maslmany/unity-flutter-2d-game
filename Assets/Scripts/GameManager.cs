using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;
using UnityEngine.UI;
using System.Collections;
using MyGame.Core;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    private static void SetInstance(GameManager instance) => Instance = instance;

    private Vector3 lastCheckpointPosition;
    private bool checkpointSet = false;

    [Header("Internet Check Settings")]
    [SerializeField] private GameObject noInternetPanelPrefab;
    [SerializeField] private float checkInterval = 10f;
    [SerializeField] private bool requireInternetToStart = true;

    private GameObject noInternetPanelInstance;
    private bool isPaused = false;
    private Button retryButton;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        SetInstance(this);
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        StartCoroutine(CheckInternetAtStart());
    }

    private IEnumerator CheckInternetAtStart()
    {
        yield return CheckInternetConnection();

        if (requireInternetToStart && isPaused)
        {
            Debug.Log("[GameManager] Waiting for initial internet connection...");
        }

        StartCoroutine(CheckInternetLoop());
    }

    private IEnumerator CheckInternetLoop()
    {
        while (enabled && Application.isPlaying)
        {
            yield return CheckInternetConnection();
            yield return new WaitForSecondsRealtime(checkInterval);
        }
    }

    private IEnumerator CheckInternetConnection()
    {
        if (retryButton != null)
            retryButton.interactable = false;

        using (UnityWebRequest request = new UnityWebRequest("https://connectivitycheck.gstatic.com/generate_204"))
        {
            request.downloadHandler = new DownloadHandlerBuffer();
            request.timeout = 5;

            yield return request.SendWebRequest();

            bool isNetworkError;
            bool isHttpError;

#if UNITY_2020_1_OR_NEWER
            isNetworkError = request.result == UnityWebRequest.Result.ConnectionError;
            isHttpError = request.result == UnityWebRequest.Result.ProtocolError;
#else
            isNetworkError = request.isNetworkError;
            isHttpError = request.isHttpError;
#endif

            // ✅ استخدام المنطق المعزول والمختبر عبر ConnectivityRules
            bool hasInternet = ConnectivityRules.EvaluateConnectivity((int)request.responseCode, isNetworkError, isHttpError);

            if (!hasInternet)
            {
                ShowNoInternetPanel(true);
                if (!isPaused && ConnectivityRules.ShouldPauseGame(hasInternet, requireInternetToStart))
                {
                    Time.timeScale = 0f;
                    isPaused = true;
                }
            }
            else
            {
                ShowNoInternetPanel(false);
                if (isPaused)
                {
                    Time.timeScale = 1f;
                    isPaused = false;
                    EnsureUIActive();
                }
            }
        }

        if (retryButton != null)
            retryButton.interactable = true;
    }

    private void ShowNoInternetPanel(bool show)
    {
        GameObject panel = GameObject.Find("NoInternetPanel");

        if (panel != null)
        {
            panel.SetActive(show);
            if (show) SetupRetryButton(panel);
            return;
        }

        if (show && noInternetPanelPrefab != null && noInternetPanelInstance == null)
        {
            Canvas canvas = UnityEngine.Object.FindFirstObjectByType<Canvas>();
            if (canvas != null)
            {
                noInternetPanelInstance = Instantiate(noInternetPanelPrefab, canvas.transform);
                noInternetPanelInstance.SetActive(true);
                SetupRetryButton(noInternetPanelInstance);
            }
            else
            {
                Debug.LogWarning("[GameManager] No Canvas found to parent NoInternetPanel.");
            }
        }
        else if (!show && noInternetPanelInstance != null)
        {
            Destroy(noInternetPanelInstance);
            noInternetPanelInstance = null;
            retryButton = null;
        }
    }

    private void SetupRetryButton(GameObject panel)
    {
        retryButton = panel.GetComponentInChildren<Button>();
        if (retryButton != null)
        {
            retryButton.onClick.RemoveAllListeners();
            retryButton.onClick.AddListener(ManualCheckInternet);
        }
    }

    public void ManualCheckInternet()
    {
        StartCoroutine(CheckInternetConnection());
    }

    public void EnsureUIActive()
    {
        Canvas[] canvases = UnityEngine.Object.FindObjectsByType<Canvas>(FindObjectsSortMode.None);
        foreach (var canvas in canvases)
        {
            canvas.enabled = true;
        }

        Button[] buttons = UnityEngine.Object.FindObjectsByType<Button>(FindObjectsSortMode.None);
        foreach (var button in buttons)
        {
            button.interactable = true;
        }
    }

    public void SetCheckpoint(Vector3 position)
    {
        lastCheckpointPosition = position;
        checkpointSet = true;
    }

    public Vector3 LastCheckpointPosition => lastCheckpointPosition;
    public bool HasCheckpoint => checkpointSet;

    public void RespawnPlayer(GameObject playerObj)
    {
        if (!checkpointSet)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            return;
        }

        PlayerController player = playerObj.GetComponent<PlayerController>();
        if (player != null)
        {
            player.RespawnPlayer();
        }
        else
        {
            playerObj.transform.position = lastCheckpointPosition;
        }
    }

    public void ResetGame()
    {
        checkpointSet = false;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
