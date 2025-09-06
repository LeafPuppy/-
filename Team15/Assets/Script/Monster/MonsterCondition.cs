using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Timeline;

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
        if (this.gameObject.name != "Back")
        {
            if (monster == null || monster.data == null)
            {
                Debug.LogError($"[{name}] Monster/MonsterDataSO가 비어 있습니다.");
                return;
            }

            float baseHp = monster.data.maxHealth;
            float mul = GameState.EnemyHpMul;
            currentHealth = Mathf.Ceil(baseHp * mul);
        }
    }

    public void TakeDamage(float damage)
    {
        if (isDead) return;

        if (this.gameObject.name != "Back" && monster.data.type == MonsterType.Special)
            currentHealth -= damage / 2;
        else 
            monster.condition.currentHealth -= damage;

        switch(monster.data.type)
        {
            case MonsterType.Melee: case MonsterType.Range:
                AudioManager.Instance.PlaySFX("NormalDamageSFX");
                break;
            case MonsterType.Special:
                AudioManager.Instance.PlaySFX("SpecialDamageSFX");
                break;
            case MonsterType.Boss:
                AudioManager.Instance.PlaySFX("BossDamageSFX");
                break;
        }

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
        // 사망시 무기 드롭
        if (dropWeapon != null)
        {
            Debug.Log("무기드랍");
            Instantiate(dropWeapon, dropPosition.position, Quaternion.identity);
        }

        yield return new WaitForSeconds(.5f);

        Destroy(monster.gameObject);
    }
}
