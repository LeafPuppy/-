using System.Collections;
using System.Collections.Generic;
using UnityEditor;
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
    public PatternDataSO[] patterns;
    public BoxCollider2D _collider;

    public float speed;
    public bool inPattern;
    public bool isMaintain;

    //�ӽ� ��Ÿ�
    public int attackRange;

    private void Awake()
    {
        monsterController = GetComponent<MonsterController>();
        animationController = GetComponent<EntityAnimationController>();
        stateMachine = new MonsterStateMachine(this);
        _collider = GetComponent<BoxCollider2D>();
        speed = data.speed;
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

    public IEnumerator CheckInPattern()
    {
        yield return new WaitForSeconds(1f);
        inPattern = false;
    }
}
