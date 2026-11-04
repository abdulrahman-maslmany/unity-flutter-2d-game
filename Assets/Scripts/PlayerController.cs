using System;
using GoogleMobileAds.Api;
using MyGame;
using MyGame.Animation;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float jumpForce = 7f;

    [Header("Combat & Ranged Attack")]
    [SerializeField] private GameObject arrowPrefab;
    [SerializeField] private Transform arrowSpawnPoint;
    [SerializeField] private float bowAttackCooldown = 0.5f;

    [Header("Hazard & Fall Bounds")]
    [SerializeField] private float fallThreshold = -10f;

    private Rigidbody2D rb;
    private Animator animator;
    private PlayerInputSimulation input;
    private float lastBowAttackTime;
    private Vector3 respawnPoint;
    private bool isRespawning;
    private bool hasAppliedFallDamage;

    public bool IsFacingRight => transform.localScale.x > 0;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

#if UNITY_6000_0_OR_NEWER
        input = FindFirstObjectByType<PlayerInputSimulation>();
#else
        input = FindObjectOfType<PlayerInputSimulation>();
#endif

        respawnPoint = transform.position;

        if (PlayerDamageable.Instance != null)
        {
            PlayerDamageable.Instance.OnDeath += HandleDeath;
            PlayerDamageable.Instance.OnRevive += HandleRevive;
        }
    }

    private void OnDestroy()
    {
        if (PlayerDamageable.Instance != null)
        {
            PlayerDamageable.Instance.OnDeath -= HandleDeath;
            PlayerDamageable.Instance.OnRevive -= HandleRevive;
        }
    }

    private void Update()
    {
        if (!enabled || isRespawning)
        {
            return;
        }

        CheckFallBoundary();
        HandleMovement();
        HandleActions();
    }

    private void CheckFallBoundary()
    {
        if (transform.position.y < fallThreshold && !hasAppliedFallDamage)
        {
            hasAppliedFallDamage = true;

            if (PlayerDamageable.Instance != null && PlayerDamageable.Instance.IsAlive)
            {
                int lethalDamage = PlayerDamageable.Instance.MaxHealth + 1;
                PlayerDamageable.Instance.TakeDamage(lethalDamage);
            }
        }
    }

    private void HandleMovement()
    {
        if (input == null) return;

        float moveInput = 0f;
        if (input.MoveLeft) moveInput = -1f;
        else if (input.MoveRight) moveInput = 1f;

        rb.linearVelocity = new Vector2(moveInput * moveSpeed, rb.linearVelocity.y);

        // ✅ إصلاح خطأ Codacy: فحص المدى العشري بدلاً من != 0
        bool isMoving = Mathf.Abs(moveInput) > 0.001f;

        if (isMoving)
        {
            transform.localScale = new Vector3(moveInput > 0 ? 1 : -1, 1, 1);
        }

        animator.SetBool(AnimationStrings.canMove, isMoving);
        animator.SetFloat(AnimationStrings.speed, Mathf.Abs(moveInput));
    }

    private void HandleActions()
    {
        if (input == null) return;

        if (input.JumpPressed)
        {
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            animator.SetTrigger(AnimationStrings.jumpTrigger);
            input.ResetJump();
        }

        if (input.AttackPressed && Time.time - lastBowAttackTime >= bowAttackCooldown)
        {
            lastBowAttackTime = Time.time;
            animator.SetTrigger(AnimationStrings.bowAttackTrigger);
            FireArrow();
            input.ResetAttack();
        }
    }

    private void FireArrow()
    {
        if (!arrowPrefab || !arrowSpawnPoint) return;

        GameObject arrow = Instantiate(arrowPrefab, arrowSpawnPoint.position, Quaternion.identity);
        if (arrow.TryGetComponent(out Projectile projectile))
        {
            Vector2 direction = IsFacingRight ? Vector2.right : Vector2.left;
            projectile.SetDirection(direction);
        }
    }

    public void HandleDeath()
    {
        rb.linearVelocity = Vector2.zero;
        rb.angularVelocity = 0f;
        rb.bodyType = RigidbodyType2D.Kinematic;
        rb.simulated = false;

        animator.SetBool(AnimationStrings.canMove, false);
        animator.SetTrigger(AnimationStrings.deathTrigger);

        enabled = false;
    }

    public void HandleRevive()
    {
        isRespawning = false;
        rb.linearVelocity = Vector2.zero;
        rb.angularVelocity = 0f;
        rb.bodyType = RigidbodyType2D.Dynamic;
        rb.simulated = true;

        enabled = true;
        animator.Rebind();
        animator.Update(0f);

        hasAppliedFallDamage = false;
    }

    public void ResetPlayer(Vector3 position)
    {
        isRespawning = true;
        rb.simulated = false;
        rb.linearVelocity = Vector2.zero;
        rb.angularVelocity = 0f;

        transform.position = position;

        rb.simulated = true;
        HandleRevive();
    }

    public void SetRespawnPoint(Vector3 newRespawnPoint)
    {
        respawnPoint = newRespawnPoint;
    }

    public void RespawnPlayer()
    {
        ResetPlayer(respawnPoint);
    }

    public void ShowRewardedAd(RewardedAd rewardedAd)
    {
        if (rewardedAd != null && rewardedAd.CanShowAd())
        {
            rewardedAd.Show(reward =>
            {
                RespawnPlayer();
            });
        }
        else
        {
            RespawnPlayer();
        }
    }

    public void SetMoveDirection(float direction)
    {
        if (input == null) return;
        input.MoveLeft = direction < -0.001f;
        input.MoveRight = direction > 0.001f;
    }

    public void Jump()
    {
        if (input == null) return;
        input.JumpPressed = true;
    }
}
