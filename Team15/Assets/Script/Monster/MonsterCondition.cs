using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MonsterCondition : MonoBehaviour, IDamageable
{
    public event System.Action<MonsterCondition> OnDie;

    private Transform dropPosition;
    public float currentHealth;
    public Monster monster;
    public GameObject dropWeapon;

    private bool isDead = false;

    private void Start()
    {
        if(this.gameObject.name != "Back")
            currentHealth = monster.data.maxHealth;
    }

    public void TakeDamage(float damage)
    {
        if (isDead) return;

        if (this.gameObject.name != "Back")
            currentHealth -= damage / 2;
        else 
            monster.condition.currentHealth -= damage;

        monster.animationController.ChangeAnimation(AnimationState.Damage);

        if (currentHealth <= 0f && !isDead)
        {
            StartCoroutine(Die());
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
