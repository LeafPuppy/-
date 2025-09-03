using System.Collections;
using UnityEngine;

public class Monster : MonoBehaviour
{
    public MonsterDataSO data;

    public MonsterAnimationController animationController;
    public MonsterStateMachine stateMachine;
    public MonsterController monsterController;
    public SpriteRenderer sprite;
    public PatternDataSO[] patterns;
    public BoxCollider2D _collider;
    public BoxCollider2D back;
    public Rigidbody2D rg;
    public MonsterCondition condition;

    public float speed;
    public bool inPattern;
    public bool isMaintain;
    public bool canSpawn;
    public Coroutine co;

    //�ӽ� ��Ÿ�
    public int attackRange;

    private void Awake()
    {
        monsterController = GetComponent<MonsterController>();
        animationController = GetComponent<MonsterAnimationController>();
        stateMachine = new MonsterStateMachine(this);
        _collider = GetComponent<BoxCollider2D>();
        rg = GetComponent<Rigidbody2D>();
        condition = GetComponent<MonsterCondition>();
        speed = data.speed;
    }

    public IEnumerator CheckInPattern()
    {
        yield return new WaitForSeconds(1f);
        inPattern = false;
    }
}
