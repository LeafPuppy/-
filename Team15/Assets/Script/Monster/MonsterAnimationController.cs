using UnityEngine;


public class MonsterAnimationController : MonoBehaviour
{
    private readonly int isMove = Animator.StringToHash("Move");
    private readonly int isJump = Animator.StringToHash("Jump");
    private readonly int isDamage = Animator.StringToHash("Damage");
    private readonly int isDie = Animator.StringToHash("Die");

    [SerializeField] private Animator animator;

    public void ChangeAnimation(AnimationState state)
    {
        switch (state)
        {
            case AnimationState.Idle:
                animator.SetBool(isMove, false);
                break;
            case AnimationState.Move:
                animator.SetBool(isMove, true);
                break;
            case AnimationState.Jump:
                animator.SetTrigger(isJump);
                break;
            case AnimationState.Damage:
                animator.SetTrigger(isDamage);
                break;
            case AnimationState.Die:
                animator.SetTrigger(isDie);
                break;
        }
    }
}