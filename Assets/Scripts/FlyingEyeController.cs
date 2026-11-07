using UnityEngine;
using MyGame.Animation;

namespace MyGame;

public class FlyingEyeController : EnemyBehaviourBase
{
    protected override string LogTag => "FlyingEye";

    protected override void FollowPlayer()
    {
        Vector2 direction = (player.position - transform.position).normalized;
        rb.linearVelocity = direction * moveSpeed; // حركة كاملة في X و Y (طيران)
        animator.SetBool(AnimationStrings.canMove, true);
        animator.SetFloat(AnimationStrings.speed, direction.magnitude);

        if (direction.x != 0)
            transform.localScale = new Vector3(Mathf.Sign(direction.x), 1, 1);
    }

    protected override void StopMoving()
    {
        rb.linearVelocity = Vector2.zero;
        animator.SetBool(AnimationStrings.canMove, false);
        animator.SetFloat(AnimationStrings.speed, 0f);
    }
}
