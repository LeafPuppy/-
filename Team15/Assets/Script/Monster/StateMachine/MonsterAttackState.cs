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
                //�ʿ� ���Ⱑ ������(�÷��̾� �������� ����) �������� ����
                //������ ������ ������ ���� ��ȯ
                if (stateMachine.Monster.canSpawn)
                    stateMachine.Monster.StartCoroutine(stateMachine.Monster.patterns[stateMachine.Monster.patterns.Length - 1].Execute(stateMachine.Monster));

                //�ʿ� ���Ⱑ �ִٸ� ��Ÿ ���� ����
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
