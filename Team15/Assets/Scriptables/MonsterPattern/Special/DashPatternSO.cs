using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(fileName = "Dash", menuName = "Pattern/Dash")]
public class DashPatternSO : PatternDataSO
{
    public float dashSpeed;
    public float sturnTime;
    public Vector2 hitBox;


    public override IEnumerator Execute(Monster monster)
    {
        monster._collider.isTrigger = true;
        monster.isMaintain = true;
        monster.inPattern = true;
        monster.rg.gravityScale = 0;
        if (monster.transform.localScale.x > 0)
        {
            while (monster.isMaintain)
            {
                monster.transform.position += Vector3.right * dashSpeed * Time.fixedDeltaTime;

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
                //부딪힌 오브젝트 태그가 MapEndWall이면 돌진 끝, 잠시 스턴 후 재개
                Collider2D[] walls = Physics2D.OverlapBoxAll(monster.transform.position, hitBox, 0);
                foreach (var wall in walls)
                {
                    if (wall.CompareTag("MapEndWall"))
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
                }
                yield return null;
            }
        }
        else
        {
            while (monster.isMaintain)
            {
                monster.transform.position += Vector3.left * dashSpeed * Time.fixedDeltaTime;

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
                //부딪힌 오브젝트 태그가 MapEndWall이면 돌진 끝, 잠시 스턴 후 재개
                Collider2D[] walls = Physics2D.OverlapBoxAll(monster.transform.position, hitBox, 0);
                foreach (var wall in walls)
                {
                    if (wall.CompareTag("MapEndWall"))
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
                }
                yield return null;
            }
        }
    }
}
