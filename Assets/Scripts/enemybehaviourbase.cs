using UnityEngine;
using System.Collections;
using MyGame.Animation;

namespace MyGame;

[RequireComponent(typeof(Rigidbody2D), typeof(EnemyAttack))]
public abstract class EnemyBehaviourBase : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 3f;
    public float detectionRange = 5f;
    public float attackRange = 1f;

    [Header("Attack")]
    public float attackCooldown = 0.5f;
    protected float lastAttackTime = 0f;

    protected Transform player;
    protected Animator animator;
    protected Rigidbody2D rb;
    protected EnemyAttack enemyAttack;
    protected bool hasTarget = false;

    protected Vector3 startPosition;
    protected Damageable damageable;

    // اسم الـ Tag المميز لكل عدو في الـ Logs
    protected abstract string LogTag { get; }

    protected virtual void Awake()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        enemyAttack = GetComponent<EnemyAttack>();
        damageable = GetComponent<Damageable>();
        startPosition = transform.position;
    }

    protected virtual void Start()
    {
        StartCoroutine(FindPlayerSafely());
    }

    protected virtual void OnEnable()
    {
        ResetEnemy();
    }

    protected virtual void OnDisable()
    {
        CancelInvoke(nameof(CheckPlayerExists));
    }

    protected virtual IEnumerator FindPlayerSafely()
    {
        yield return null;

        if (player == null && PlayerSingleton.Instance != null)
        {
            player = PlayerSingleton.Instance.PlayerTransform;
            GameLogger.Log($"[{LogTag}] Player found via Singleton.");
        }
        else
        {
            PlayerSingleton singleton = FindFirstObjectByType<PlayerSingleton>();
            if (singleton != null)
            {
                player = singleton.PlayerTransform;
                GameLogger.Log($"[{LogTag}] Player found via FindFirstObjectByType.");
            }
            else
            {
                PlayerController playerController = FindFirstObjectByType<PlayerController>();
                if (playerController != null)
                {
                    player = playerController.transform;
                    GameLogger.Log($"[{LogTag}] Player found via PlayerController.");
                }
                else
                {
                    GameLogger.LogWarning($"[{LogTag}] Player not found yet. Retrying in 0.5s...");
                    yield return new WaitForSeconds(0.5f);
                    StartCoroutine(FindPlayerSafely());
                    yield break;
                }
            }
        }

        if (player != null)
        {
            CancelInvoke(nameof(CheckPlayerExists));
            InvokeRepeating(nameof(CheckPlayerExists), 1f, 1f);
        }
    }

    protected virtual void CheckPlayerExists()
    {
        if (player == null || player.gameObject == null)
        {
            GameLogger.Log($"[{LogTag}] Player lost. Re-finding...");
            CancelInvoke(nameof(CheckPlayerExists));
            StopCoroutine(FindPlayerSafely());
            StartCoroutine(FindPlayerSafely());
        }
    }

    protected virtual void FixedUpdate()
    {
        if (player == null) return;

        float distanceToPlayer = Vector2.Distance(transform.position, player.position);
        hasTarget = distanceToPlayer <= detectionRange;
        bool isInAttackRange = distanceToPlayer <= attackRange;

        animator.SetBool(AnimationStrings.hasTarget, hasTarget);
        animator.SetBool(AnimationStrings.isInAttackRange, isInAttackRange);

        if (hasTarget)
        {
            if (isInAttackRange)
            {
                StopMoving();
                if (Time.time - lastAttackTime >= attackCooldown)
                {
                    animator.SetTrigger(AnimationStrings.attackTrigger);
                    lastAttackTime = Time.time;
                }
            }
            else
            {
                FollowPlayer();
            }
        }
        else
        {
            StopMoving();
        }
    }

    // تُترك لكل نوع وحش ليحدد حركته (طائر أو أرضي)
    protected abstract void FollowPlayer();
    protected abstract void StopMoving();

    public virtual void ResetEnemy()
    {
        damageable?.ResetHealth();
        transform.position = startPosition;
        rb.linearVelocity = Vector2.zero;

        animator.Rebind();
        animator.Update(0f);

        lastAttackTime = 0f;
        hasTarget = false;
        animator.SetBool(AnimationStrings.hasTarget, false);
        animator.SetBool(AnimationStrings.isInAttackRange, false);
        animator.SetBool(AnimationStrings.canMove, false);
        animator.SetFloat(AnimationStrings.speed, 0f);

        gameObject.SetActive(true);

        player = null;
        CancelInvoke(nameof(CheckPlayerExists));
        StartCoroutine(FindPlayerSafely());
    }

    public virtual void DealDamage()
    {
        GameLogger.Log($"[{LogTag}] DealDamage called from animation event!");
        enemyAttack?.PerformAttack();
    }
}
