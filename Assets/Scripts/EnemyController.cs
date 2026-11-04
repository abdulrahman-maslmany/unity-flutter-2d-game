using UnityEngine;
using MyGame.Core;

namespace MyGame;

public class EnemyController : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 2f;
    public float detectionRange = 5f;
    public float attackRange = 1f;

    [Header("Attack")]
    public float attackCooldown = 1f;
    private float lastAttackTime = 0f;

    private Transform player;
    private Animator animator;
    private Rigidbody2D rb;
    private EnemyAttack enemyAttack;
    private bool hasTarget = false;

    private Vector3 startPosition;
    private Damageable damageable;

    private static readonly int HasTargetHash = Animator.StringToHash("hasTarget");
    private static readonly int IsInAttackRangeHash = Animator.StringToHash("isInAttackRange");
    private static readonly int AttackTriggerHash = Animator.StringToHash("attackTrigger");
    private static readonly int CanMoveHash = Animator.StringToHash("canMove");
    private static readonly int SpeedHash = Animator.StringToHash("speed");

    private void Awake()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        enemyAttack = GetComponent<EnemyAttack>();
        damageable = GetComponent<Damageable>();
        startPosition = transform.position;
    }

    private void Start()
    {
        FindPlayer();
    }

    private void OnEnable()
    {
        ResetEnemy();
    }

    private void FindPlayer()
    {
        var playerObj = UnityEngine.Object.FindFirstObjectByType<PlayerController>();
        if (playerObj != null)
        {
            player = playerObj.transform;
        }
    }

    private void FixedUpdate()
    {
        if (player == null) 
        { 
            FindPlayer(); 
            return; 
        }

        float distanceToPlayer = Vector2.Distance(transform.position, player.position);

        // ✅ استخدام منطق EnemyAICore الخالص والمختبر
        hasTarget = EnemyAICore.IsInDetectionRange(distanceToPlayer, detectionRange);
        bool isInAttackRange = EnemyAICore.IsInAttackRange(distanceToPlayer, attackRange);

        animator.SetBool(HasTargetHash, hasTarget);
        animator.SetBool(IsInAttackRangeHash, isInAttackRange);

        if (hasTarget)
        {
            if (isInAttackRange)
            {
                StopMoving();
                if (EnemyAICore.CanAttack(Time.time, lastAttackTime, attackCooldown))
                {
                    animator.SetTrigger(AttackTriggerHash);
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

    private void FollowPlayer()
    {
        Vector2 direction = (player.position - transform.position).normalized;
        rb.linearVelocity = direction * moveSpeed;
        animator.SetBool(CanMoveHash, true);
        animator.SetFloat(SpeedHash, direction.magnitude);

        // ✅ تجنب مقارنة الفاصلة العائمة مع الصفر لحل تحذيرات Codacy
        if (Mathf.Abs(direction.x) > 0.001f)
        {
            float facing = EnemyAICore.CalculateFacingDirection(direction.x);
            transform.localScale = new Vector3(facing, 1, 1);
        }
    }

    private void StopMoving()
    {
        rb.linearVelocity = Vector2.zero;
        animator.SetBool(CanMoveHash, false);
        animator.SetFloat(SpeedHash, 0f);
    }

    public void ResetEnemy()
    {
        damageable?.ResetHealth();
        transform.position = startPosition;
        rb.linearVelocity = Vector2.zero;

        animator.Rebind();
        animator.Update(0f);

        lastAttackTime = 0f;
        hasTarget = false;
        animator.SetBool(HasTargetHash, false);
        animator.SetBool(IsInAttackRangeHash, false);
        animator.SetBool(CanMoveHash, false);
        animator.SetFloat(SpeedHash, 0f);

        gameObject.SetActive(true);
    }
}
