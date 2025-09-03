using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterSturnState : MonsterBaseState
{
    public MonsterSturnState(MonsterStateMachine stateMachine) : base(stateMachine)
    {
    }

    public override void Enter()
    {
        stateMachine.Player = null;
        stateMachine.Monster.animationController.ChangeAnimation(AnimationState.Idle);
    }
}
