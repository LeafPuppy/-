using UnityEngine;
public enum AnimationState
{
    Idle,
    Move,
    Jump,
    Damage,
    Die,
    Dash
}
public class EntityAnimationController : MonoBehaviour
{
    private readonly int isMove = Animator.StringToHash("Move");
    private readonly int isJump = Animator.StringToHash("Jump");
    private readonly int isDash = Animator.StringToHash("Dash");
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
            case AnimationState.Dash:
                animator.SetTrigger(isDash);
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
