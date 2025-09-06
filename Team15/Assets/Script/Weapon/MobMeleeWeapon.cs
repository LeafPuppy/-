using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MobMeleeWeapon : MobWeaponBase
{

    public override void Attack()
    {
        animController.ChangeAnimation(WeaponState.Attack);
        canDamage = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            collision.TryGetComponent<IDamageable>(out IDamageable damageable);
            if(canDamage)
                damageable.TakeDamage(damage);
        }
    }
}
