using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(fileName = "Dash", menuName = "Pattern/Dash")]
public class DashPatternSO : PatternDataSO
{
    public float dashSpeed;
    public float sturnTime;
    public Vector2 hitBox;
    private int round;

    public override IEnumerator Execute(Monster monster)
    {
        isHard = GameState.Instance.currentDifficulty == Difficulty.Hard ? true : false;
        round = 1;
        monster._collider.isTrigger = true;
        monster.isMaintain = true;
        monster.inPattern = true;
        monster.rg.gravityScale = 0;
        if (monster.transform.localScale.x > 0)
        {
            monster.animationController.ChangeAnimation(AnimationState.Move);
            yield return new WaitForSeconds(1.5f);
            AudioManager.Instance.PlaySFX("MonsterDashSFX");
            while (monster.isMaintain)
            {
                if (round == 1)
                    monster.transform.position += Vector3.right * dashSpeed * Time.fixedDeltaTime;
                else if (round == 2)
                {
                    var sc = monster.transform.localScale;
                    sc.x *= -1;
                    monster.transform.localScale = sc;
                    monster.transform.position += Vector3.left * dashSpeed * Time.fixedDeltaTime;
                }


                //이동중 플레이어랑 부딪히면 데미지 입힘
                Collider2D[] objects = Physics2D.OverlapBoxAll(monster.transform.position, hitBox, 0);
                foreach (var obj in objects)
                {
                    if (obj.CompareTag("Player"))
                    {
                        Debug.Log("플레이어 데미지");
                        obj.TryGetComponent<IDamageable>(out IDamageable damageable);
                        damageable.TakeDamage(damage);
                    }
                }
                if (monster.transform.position.x >= 12.5f && round == 1 || monster.transform.position.x <= -3.5f && round == 2)
                {
                    AudioManager.Instance.StopSFX();
                    if (!isHard)
                    {
                        monster.isMaintain = false;
                        monster._collider.isTrigger = false;
                        monster.stateMachine.ChangeState(monster.stateMachine.SturnState);
                        monster.rg.gravityScale = 1;
                        yield return new WaitForSeconds(sturnTime);
                        monster.stateMachine.ChangeState(monster.stateMachine.IdleState);
                        monster.speed = monster.data.speed;
                        monster.StartCoroutine(monster.CheckInPattern());
                    }
                    else if (isHard && round == 2)
                    {
                        monster.isMaintain = false;
                        monster._collider.isTrigger = false;
                        monster.stateMachine.ChangeState(monster.stateMachine.SturnState);
                        monster.rg.gravityScale = 1;
                        yield return new WaitForSeconds(sturnTime);
                        monster.stateMachine.ChangeState(monster.stateMachine.IdleState);
                        monster.speed = monster.data.speed;
                        monster.StartCoroutine(monster.CheckInPattern());
                    }
                    else
                    {
                        round++;
                    }
                }
                yield return null;
            }
        }
        else
        {
            monster.animationController.ChangeAnimation(AnimationState.Move);
            yield return new WaitForSeconds(1.5f);
            AudioManager.Instance.PlaySFX("MonsterDashSFX");
            while (monster.isMaintain)
            {
                if (round == 1)
                    monster.transform.position += Vector3.left * dashSpeed * Time.fixedDeltaTime;
                else if (round == 2)
                {
                    var sc = monster.transform.localScale;
                    sc.x *= -1;
                    monster.transform.localScale = sc;
                    monster.transform.position += Vector3.right * dashSpeed * Time.fixedDeltaTime;
                }

                //이동중 플레이어랑 부딪히면 데미지 입힘
                Collider2D[] objects = Physics2D.OverlapBoxAll(monster.transform.position, hitBox, 0);
                foreach (var obj in objects)
                {
                    if (obj.CompareTag("Player"))
                    {
                        Debug.Log("플레이어 데미지");
                        obj.TryGetComponent<IDamageable>(out IDamageable damageable);
                        damageable.TakeDamage(damage);
                    }
                }
                if (monster.transform.position.x >= 12.5f && round == 2 || monster.transform.position.x <= -3.5f && round == 1)
                {
                    AudioManager.Instance.StopSFX();
                    if (!isHard)
                    {
                        monster.isMaintain = false;
                        monster._collider.isTrigger = false;
                        monster.stateMachine.ChangeState(monster.stateMachine.SturnState);
                        monster.rg.gravityScale = 1;
                        yield return new WaitForSeconds(sturnTime);
                        monster.stateMachine.ChangeState(monster.stateMachine.IdleState);
                        monster.speed = monster.data.speed;
                        monster.StartCoroutine(monster.CheckInPattern());
                    }
                    else if (isHard && round == 2)
                    {
                        monster.isMaintain = false;
                        monster._collider.isTrigger = false;
                        monster.stateMachine.ChangeState(monster.stateMachine.SturnState);
                        monster.rg.gravityScale = 1;
                        yield return new WaitForSeconds(sturnTime);
                        monster.stateMachine.ChangeState(monster.stateMachine.IdleState);
                        monster.speed = monster.data.speed;
                        monster.StartCoroutine(monster.CheckInPattern());
                    }
                    else
                    {
                        round++;
                    }
                }
                yield return null;
            }
        }
    }
}
