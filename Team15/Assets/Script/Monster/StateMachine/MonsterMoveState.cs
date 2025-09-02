using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterMoveState : MonsterBaseState
{
    public MonsterMoveState(MonsterStateMachine stateMachine) : base(stateMachine)
    {
    }

    public override void Enter()
    {
        stateMachine.Monster.animationController.ChangeAnimation(AnimationState.Move);
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        if(stateMachine.Player != null)
        {
            if (Vector2.Distance(stateMachine.Player.transform.position, stateMachine.Monster.transform.position) < stateMachine.Monster.attackRange)
            {
                stateMachine.ChangeState(stateMachine.AttackState);
            }
        }
        else
        {
            stateMachine.ChangeState(stateMachine.IdleState);
        }
    }
}
