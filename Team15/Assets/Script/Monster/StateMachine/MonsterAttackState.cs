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
        if (stateMachine.Player != null)
        {
            if (stateMachine.Monster.patterns.Length != 0 && !stateMachine.Monster.inPattern)
            {
                //맵에 무기가 없으면(플레이어 소유무기 제외) 스폰패턴 실행
                //무조건 마지막 패턴이 무기 소환
                if (stateMachine.Monster.canSpawn)
                    stateMachine.Monster.StartCoroutine(stateMachine.Monster.patterns[stateMachine.Monster.patterns.Length - 1].Execute(stateMachine.Monster));

                //맵에 무기가 있다면 기타 패턴 실행
                stateMachine.Monster.StartCoroutine(stateMachine.Monster.patterns[Random.Range(0, stateMachine.Monster.patterns.Length - 1)].Execute(stateMachine.Monster));

            }

            if (Vector2.Distance(stateMachine.Player.transform.position, stateMachine.Monster.transform.position) > stateMachine.Monster.attackRange)
            {
                stateMachine.ChangeState(stateMachine.MoveState);
            }
        }
        else
        {
            stateMachine.ChangeState(stateMachine.IdleState);
        }
    }
}
