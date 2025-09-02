using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterAttackState : MonsterBaseState
{
    public MonsterAttackState(MonsterStateMachine stateMachine) : base(stateMachine)
    {
    }

    public override void Enter()
    {
        base.Enter();
        stateMachine.Monster.animationController.ChangeAnimation(AnimationState.Idle);
    }
    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        if (stateMachine.Monster.patterns.Length != 0 && !stateMachine.Monster.inPattern)
        {
            for (int i = 0; i < stateMachine.Monster.patterns.Length; i++)
            {
                stateMachine.Monster.StartCoroutine(stateMachine.Monster.patterns[i].Execute(stateMachine.Monster));
            }
        }

        if (Vector2.Distance(stateMachine.Player.transform.position, stateMachine.Monster.transform.position) > stateMachine.Monster.attackRange)
        {
            stateMachine.ChangeState(stateMachine.MoveState);
        }
    }
}
