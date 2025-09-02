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

    //�ӽ� ��Ÿ�
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

        // ����� ���� ���
        if (dropWeapon != null)
        {
            Instantiate(dropWeapon, dropPosition);
        }

        base.Die();
    }
}
