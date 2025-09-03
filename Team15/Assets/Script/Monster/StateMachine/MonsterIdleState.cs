using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterIdleState : MonsterBaseState
{
    public MonsterIdleState(MonsterStateMachine stateMachine) : base(stateMachine)
    {
    }

    public override void Enter()
    {
        stateMachine.Monster.animationController.ChangeAnimation(AnimationState.Idle);
    }
    public override void Exit()
    {
    }

    public override void Update()
    {
        Collider2D[] objects = Physics2D.OverlapCircleAll(stateMachine.Monster.transform.position, stateMachine.Monster.data.chasingRange);
        foreach (var obj in objects)
        {
            if (obj.CompareTag("Player"))
            {
                stateMachine.Player = obj.gameObject;
                stateMachine.ChangeState(stateMachine.MoveState);
            }
        }
    }
}
