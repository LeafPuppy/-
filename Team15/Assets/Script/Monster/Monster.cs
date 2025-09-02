using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : Entity
{
    public MonsterDataSO data;

    private Transform dropPosition;
    public GameObject dropWeapon;
    public EntityAnimationController animationController;
    public MonsterStateMachine stateMachine;
    public MonsterController monsterController;
    public SpriteRenderer sprite;

    //임시 사거리
    public int attackRange;

    private void Awake()
    {
        monsterController = GetComponent<MonsterController>();

        animationController = GetComponent<EntityAnimationController>();
        stateMachine = new MonsterStateMachine(this);

    }
    protected override void Start()
    {
        currentHealth = data.maxHealth;

    }

    protected override void Die()
    {
        dropPosition = transform;

        // 사망시 무기 드롭
        if (dropWeapon != null)
        {
            Instantiate(dropWeapon, dropPosition);
        }

        base.Die();
    }
}
