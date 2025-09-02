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
    void FixedUpdate()
    {
        monster.stateMachine.Update();
    }

    public Coroutine StartCo(IEnumerator coroutine)
    {
        var co = StartCoroutine(coroutine);

        return co;
    }

    public void StopCo(Coroutine co)
    {
        if(co != null)
            StopCoroutine(co);
    }
}
