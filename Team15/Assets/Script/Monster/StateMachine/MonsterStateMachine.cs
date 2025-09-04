using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterStateMachine : StateMachine
{
    public Monster Monster { get; }
    public GameObject Player { get; set; }

    public MonsterIdleState IdleState { get; private set; }
    public MonsterMoveState MoveState { get; private set; }
    public MonsterAttackState AttackState { get; private set; }
    public MonsterSturnState SturnState { get; private set; }


    public MonsterStateMachine(Monster monster)
    {
        this.Monster = monster;

        IdleState = new MonsterIdleState(this);
        MoveState = new MonsterMoveState(this);
        AttackState = new MonsterAttackState(this);
        SturnState = new MonsterSturnState(this);
    }
}
