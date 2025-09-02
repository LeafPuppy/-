using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(fileName = "Dash", menuName = "Pattern/Dash")]
public class DashPatternSO : PatternDataSO
{
    public float dashSpeed;
    public float sturnTiem;
    public Vector2 hitBox;


    public override IEnumerator Execute(Monster monster)
    {
        monster._collider.isTrigger = true;
        monster.isMaintain = true;
        monster.inPattern = true;
        if (!monster.sprite.flipX)
        {
            while (monster.isMaintain)
            {
                monster.transform.position += Vector3.right * dashSpeed * Time.fixedDeltaTime;

                //�̵��� �÷��̾�� �ε����� ������ ����
                Collider2D[] objects = Physics2D.OverlapBoxAll(monster.transform.position, hitBox, 0);
                foreach (var obj in objects)
                {
                    if (obj.CompareTag("Player"))
                    {
                        Debug.Log("�÷��̾� ������");
                        //obj.TryGetComponent<IDamageable>(out IDamageable damageable);
                        //damageable.TakeDamage(damage);
                    }
                }

                //�ε��� ������Ʈ �±װ� MapEndWall�̸� ���� ��, ��� ���� �� �簳
                Collider2D[] walls = Physics2D.OverlapBoxAll(monster.transform.position, hitBox, 0);
                foreach (var wall in walls)
                {
                    if (wall.CompareTag("MapEndWall"))
                    {
                        monster.isMaintain = false;
                        monster.speed = 0;
                        monster.stateMachine.ChangeState(monster.stateMachine.IdleState);
                        monster._collider.isTrigger = false;
                        yield return new WaitForSeconds(sturnTiem);
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

                //�̵��� �÷��̾�� �ε����� ������ ����
                Collider2D[] objects = Physics2D.OverlapBoxAll(monster.transform.position, hitBox, 0);
                foreach (var obj in objects)
                {
                    if (obj.CompareTag("Player"))
                    {
                        Debug.Log("�÷��̾� ������");
                        //obj.TryGetComponent<IDamageable>(out IDamageable damageable);
                        //damageable.TakeDamage(damage);
                    }
                }

                //�ε��� ������Ʈ �±װ� MapEndWall�̸� ���� ��, ��� ���� �� �簳
                Collider2D[] walls = Physics2D.OverlapBoxAll(monster.transform.position, hitBox, 0);
                foreach (var wall in walls)
                {
                    if (wall.CompareTag("MapEndWall"))
                    {
                        monster.isMaintain = false;
                        monster.speed = 0;
                        monster.stateMachine.ChangeState(monster.stateMachine.IdleState);
                        monster._collider.isTrigger = false;
                        yield return new WaitForSeconds(sturnTiem);
                        monster.speed = monster.data.speed;
                        monster.StartCoroutine(monster.CheckInPattern());
                    }
                }
                yield return null;
            }
        }
    }
}
