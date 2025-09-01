using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public enum AnimationState
{
    Idle,
    Move,
    Jump,
    Damage,
    Die,
}

public class EntityAnimationController : MonoBehaviour
{
    private readonly int isMove = Animator.StringToHash("Move");
    private readonly int isJump = Animator.StringToHash("Jump");
    private readonly int isDamage = Animator.StringToHash("Damage");
    private readonly int isDie = Animator.StringToHash("Die");

    [SerializeField] private Animator animator;

    private void Update()
    {
        //테스트용 코드
        if (Input.GetKeyDown(KeyCode.Alpha1))
            ChangeAnimation(AnimationState.Idle);
        else if(Input.GetKeyDown(KeyCode.Alpha2))
            ChangeAnimation(AnimationState.Move);
        else if( Input.GetKeyDown(KeyCode.Alpha3))
            ChangeAnimation(AnimationState.Jump);
        else if(Input.GetKeyDown (KeyCode.Alpha4))
            ChangeAnimation(AnimationState.Damage);
        else if(Input.GetKeyDown (KeyCode.Alpha5))
            ChangeAnimation(AnimationState.Die);
    }

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
