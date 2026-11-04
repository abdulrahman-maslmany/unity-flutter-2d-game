using UnityEngine;
using MyGame;

public class PlayerSingleton : MonoBehaviour
{
    public static PlayerSingleton Instance { get; private set; }

    // دالة static مساعدة لتعيين النسخة لمنع تحذير Codacy
    private static void SetInstance(PlayerSingleton instance) => Instance = instance;

    public Transform PlayerTransform { get; private set; }
    
    // ✅ نوع صحيح دلوقتي
    public PlayerDamageable PlayerDamageable { get; private set; }
    
    public PlayerController PlayerController { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        SetInstance(this); // تم التعديل هنا
        DontDestroyOnLoad(gameObject);

        InitializeComponents();
    }

    private void OnEnable()
    {
        if (Instance == this && PlayerTransform == null)
        {
            InitializeComponents();
        }
    }

    private void InitializeComponents()
    {
        if (!gameObject.activeInHierarchy) return;

        PlayerTransform = transform;

        PlayerDamageable = GetComponent<PlayerDamageable>();
        PlayerController = GetComponent<PlayerController>();

        if (PlayerDamageable == null)
            Debug.LogError("[PlayerSingleton] PlayerDamageable component is missing on Player!");

        if (PlayerController == null)
            Debug.LogError("[PlayerSingleton] PlayerController component is missing on Player!");
    }
}
