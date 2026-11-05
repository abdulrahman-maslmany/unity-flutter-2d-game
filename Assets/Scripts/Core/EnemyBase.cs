using UnityEngine;

namespace MyGame.Core;

public abstract class EnemyBase : MonoBehaviour
{
    [Header("Base Settings")]
    [SerializeField] protected float moveSpeed = 2f;
    [SerializeField] protected float detectionRange = 5f;
    [SerializeField] protected float attackRange = 1f;

    protected Transform player;
    protected Rigidbody2D rb;
    protected Animator animator;
    protected bool hasTarget;

    protected virtual void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    protected virtual void FindPlayerSafely()
    {
        var playerObj = UnityEngine.Object.FindFirstObjectByType<PlayerController>();
        if (playerObj != null)
        {
            player = playerObj.transform;
        }
    }

    protected virtual void StopMoving()
    {
        if (rb != null)
        {
            rb.linearVelocity = Vector2.zero;
        }
        if (animator != null)
        {
            animator.SetBool("canMove", false);
            animator.SetFloat("speed", 0f);
        }
    }
}
