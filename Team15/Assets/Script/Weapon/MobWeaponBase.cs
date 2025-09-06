using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MobWeaponBase : MonoBehaviour
{
    public WeaponAnimationController animController;
    public float damage;
    public bool canDamage;


    public virtual void Attack()
    {
        animController.ChangeAnimation(WeaponState.Attack);
        canDamage = true;
    }

    public virtual void Attack(Transform target)
    {
        Debug.Log("원공");
        animController.ChangeAnimation(WeaponState.Attack);
    }
}
