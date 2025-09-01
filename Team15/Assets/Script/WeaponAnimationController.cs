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


    void Update()
    {
        //테스트 코드
        if (Input.GetMouseButton(0))
            ChangeAnimation(WeaponState.Attack);
        else if(Input.GetMouseButtonUp(0))
            ChangeAnimation(WeaponState.Idle);
    }

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
}
