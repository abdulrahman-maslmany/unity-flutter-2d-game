using UnityEngine;

namespace MyGame.Enemies
{
    public abstract class EnemyAIBase : MonoBehaviour
    {
        [Header("Base AI Settings")]
        [SerializeField] protected float detectionRadius = 8f;
        [SerializeField] protected float attackRange = 1.5f;
        [SerializeField] protected float moveSpeed = 3f;

        protected Transform playerTransform;
        protected bool isPlayerDetected;

        protected virtual void Start()
        {
            FindPlayerSafely();
        }

        protected bool CheckPlayerExists()
        {
            if (playerTransform == null)
            {
                FindPlayerSafely();
            }
            return playerTransform != null;
        }

        protected void FindPlayerSafely()
        {
            var playerObj = GameObject.FindGameObjectWithTag("Player");
            if (playerObj != null)
            {
                playerTransform = playerObj.transform;
            }
        }

        public bool IsPlayerInAttackRange(Vector3 enemyPosition)
        {
            if (!CheckPlayerExists()) return false;
            return Vector2.Distance(enemyPosition, playerTransform.position) <= attackRange;
        }

        public abstract void Attack();
        public abstract void MoveTowardsTarget();
    }
}
