using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using System;

public class MonsterCondition : MonoBehaviour, IDamageable
{
    public event Action<MonsterCondition> OnDie;

    private Transform dropPosition;
    public float currentHealth;
    public Monster monster;
    public GameObject dropWeapon;

    float baseMaxHealth;

    private bool isDead = false;

    private void Start()
    {
        if(this.gameObject.name != "Back")
            currentHealth = monster.data.maxHealth;

        baseMaxHealth = monster.data.maxHealth;

        if (this.gameObject.name != "Back")
        {
            float mul = GameState.EnemyHpMul;
            currentHealth = baseMaxHealth * mul;
        }
    }

    public void TakeDamage(float damage)
    {
        if (isDead) return;

        if (this.gameObject.name != "Back")
        {
            currentHealth -= damage / 2;
            monster.animationController.ChangeAnimation(AnimationState.Damage);

            if (currentHealth <= 0f)
                StartCoroutine(Die());
        }
        else
        {
            monster.condition.currentHealth -= damage;
            monster.animationController.ChangeAnimation(AnimationState.Damage);

            if (monster.condition.currentHealth <= 0f && !monster.condition.isDead)
                monster.condition.StartCoroutine(monster.condition.Die());
        }
    }

    public IEnumerator Die()
    {
        if (isDead) yield break;
        isDead = true;

        dropPosition = transform;
        monster.animationController.ChangeAnimation(AnimationState.Die);

        OnDie?.Invoke(this);

        yield return new WaitForSeconds(1f);

        Destroy(gameObject);
        // 사망시 무기 드롭
        if (dropWeapon != null)
        {
            Instantiate(dropWeapon, dropPosition);
        }
    }
}
