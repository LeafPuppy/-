using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UIElements;

public class MobRangeWeapon : MobWeaponBase
{
    public GameObject projectile;
    public Transform pivot;
    public float delay = 1.5f;
    public float lastAttackTime;

    public override void Attack(Transform target)
    {
        base.Attack();
        if (Time.time - lastAttackTime > delay)
        {
            CreateProjectile(projectile, pivot, target);
            lastAttackTime = Time.time;
        }

    }

    public void CreateProjectile(GameObject projectile, Transform pivot, Transform target)
    {
        //각도 계산해줘야함
        var obj = Instantiate(projectile, pivot.position, quaternion.identity);
        var pro = obj.GetComponent<Projectile>();
        pro.Attack(target);
    }
}
