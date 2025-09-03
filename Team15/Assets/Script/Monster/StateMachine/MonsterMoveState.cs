using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterMoveState : MonsterBaseState
{
    Vector3 moveDir;

    public MonsterMoveState(MonsterStateMachine stateMachine) : base(stateMachine)
    {
    }

    public override void Enter()
    {
        if (stateMachine.Monster.data.type != MonsterType.Range)
        {
            stateMachine.Monster.animationController.ChangeAnimation(AnimationState.Move);
        }
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {

        if (stateMachine.Player != null)
        {
            if (!stateMachine.Monster.inPattern)
            {
                moveDir = (stateMachine.Player.transform.position - stateMachine.Monster.transform.position).normalized;
                var pos = stateMachine.Monster.transform.position;
                pos.x += moveDir.x * stateMachine.Monster.data.speed * Time.fixedDeltaTime;
                stateMachine.Monster.transform.position = pos;
            }

            if (moveDir.x < 0)
            {
                stateMachine.Monster.sprite.flipX = true;
            }
            else
                stateMachine.Monster.sprite.flipX = false;

            if (Vector2.Distance(stateMachine.Player.transform.position, stateMachine.Monster.transform.position) < stateMachine.Monster.attackRange)
            {
                stateMachine.ChangeState(stateMachine.AttackState);
            }

            if(Vector2.Distance(stateMachine.Player.transform.position, stateMachine.Monster.transform.position) > stateMachine.Monster.data.chasingRange)
            {
                stateMachine.Player = null;
            }
        }
        else
        {
            stateMachine.ChangeState(stateMachine.IdleState);
        }
    }
}
