using UnityEngine;
using MyGame.Animation;

namespace MyGame;

public class GroundEnemyController : EnemyBehaviourBase
{
    protected override string LogTag => "GroundEnemy";

    protected override void FollowPlayer()
    {
        // حركة أفقية في X فقط مع الحفاظ على جاذبية Y
        Vector2 direction = new Vector2(player.position.x - transform.position.x, 0).normalized;
        rb.linearVelocity = new Vector2(direction.x * moveSpeed, rb.linearVelocity.y);

        animator.SetBool(AnimationStrings.canMove, true);
        animator.SetFloat(AnimationStrings.speed, Mathf.Abs(direction.x));

        if (direction.x != 0)
            transform.localScale = new Vector3(Mathf.Sign(direction.x), 1, 1);
    }

    protected override void StopMoving()
    {
        rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);
        animator.SetBool(AnimationStrings.canMove, false);
        animator.SetFloat(AnimationStrings.speed, 0f);
    }
}
