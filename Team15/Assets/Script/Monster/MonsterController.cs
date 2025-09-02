using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterController : MonoBehaviour
{
    public Monster monster;

    private void Start()
    {
        monster.stateMachine.ChangeState(monster.stateMachine.IdleState);
    }
    void Update()
    {
        monster.stateMachine.Update();
    }
}
