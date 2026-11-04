using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;
using UnityEngine.UI;
using System.Collections;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    // دالة static لتعيين الـ Instance لمنع تحذيرات Codacy
    private static void SetInstance(GameManager instance) => Instance = instance;

    private Vector3 lastCheckpointPosition;
    private bool checkpointSet = false;

    [Header("Internet Check Settings")]
    [SerializeField] private GameObject noInternetPanelPrefab;  // بريفاب للبانل
    [SerializeField] private float checkInterval = 10f;         // كل كم ثانية يتحقق
    [SerializeField] private bool requireInternetToStart = true; // هل يمنع تحميل المشهد إذا لا يوجد نت؟

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
        SetInstance(this); // تم التعديل هنا لحل مشكلة static field assignment
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
            Debug.Log("[GameManager] No internet at start. Waiting for connection...");
        }

        StartCoroutine(CheckInternetLoop());
    }

    // ✅ تم استبدال while(true) بشرط دورة حياة يونيتي لحل الخطأ الحرج
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

#if UNITY_2020_1_OR_NEWER
            bool hasInternet = (request.result == UnityWebRequest.Result.Success);
#else
            bool hasInternet = !(request.isNetworkError || request.isHttpError);
#endif

            if (!hasInternet)
            {
                ShowNoInternetPanel(true);
                if (!isPaused)
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
                Debug.Log("[GameManager] NoInternetPanel instantiated.");
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
            Debug.Log("[GameManager] RetryButton setup complete.");
        }
        else
        {
            Debug.LogWarning("[GameManager] RetryButton not found in NoInternetPanel. Add a Button named 'RetryButton' to the panel.");
        }
    }

    public void ManualCheckInternet()
    {
        StartCoroutine(CheckInternetConnection());
        Debug.Log("[GameManager] Manual internet check initiated by user.");
    }

    public void EnsureUIActive()
    {
        Canvas[] canvases = UnityEngine.Object.FindObjectsByType<Canvas>(FindObjectsSortMode.None);
        foreach (var canvas in canvases)
        {
            canvas.enabled = true;
            Debug.Log("[GameManager] Canvas reactivated: " + canvas.name);
        }
        
        Button[] buttons = UnityEngine.Object.FindObjectsByType<Button>(FindObjectsSortMode.None);
        foreach (var button in buttons)
        {
            button.interactable = true;
        }
    }

    // ----------------- باقي وظائف GameManager -----------------

    public void SetCheckpoint(Vector3 position)
    {
        lastCheckpointPosition = position;
        checkpointSet = true;
        Debug.Log("[GameManager] Checkpoint saved at: " + position);
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
            Debug.Log("[GameManager] Player respawned at checkpoint via PlayerController");
        }
        else
        {
            playerObj.transform.position = lastCheckpointPosition;
            Debug.Log("[GameManager] Player respawned at checkpoint (fallback)");
        }
    }

    public void ResetGame()
    {
        checkpointSet = false;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
