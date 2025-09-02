using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;


public class EntityAnimationController : MonoBehaviour
{
    private readonly int isMove = Animator.StringToHash("Move");
    private readonly int isJump = Animator.StringToHash("Jump");
    private readonly int isDamage = Animator.StringToHash("Damage");
    private readonly int isDie = Animator.StringToHash("Die");

    [SerializeField] private Animator animator;
    public virtual void ChangeAnimation(PlayerState state)
    {
        switch (state)
        {
            case PlayerState.Idle:
                animator.SetBool(isMove, false);
                break;
            case PlayerState.Move:
                animator.SetBool(isMove, true);
                break;
            case PlayerState.Jump:
                animator.SetTrigger(isJump);
                break;
            case PlayerState.Damage:
                animator.SetTrigger(isDamage);
                break;
            case PlayerState.Die:
                animator.SetTrigger(isDie);
                break;
        }
    }
}
