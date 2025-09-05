using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

public enum WeaponState
{
    Idle,
    Attack,
}

public class WeaponAnimationController : MonoBehaviour
{
    private readonly int isAttack = Animator.StringToHash("Attack");

    [SerializeField] private Animator animator;

    public void ChangeAnimation(WeaponState state)
    {
        switch (state)
        {
            case WeaponState.Idle:
                animator.SetBool(isAttack, false);
                break;
            case WeaponState.Attack:
                animator.SetBool(isAttack, true);
                break;
        }
    }
    public void OnAttackAnimationEnd()
    {
        animator.SetBool(isAttack, false);
    }
}
