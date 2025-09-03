using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Jump", menuName = "Pattern/Jump")]
public class JumpPatternSO : PatternDataSO
{
    public float jumpPower;
    public Vector2 hitBox;

    public override IEnumerator Execute(Monster monster)
    {
        monster.inPattern = true;
        monster.isMaintain = true;
        monster.rg.AddForce(Vector2.up * jumpPower, ForceMode2D.Impulse);

        yield return new WaitForSeconds(1);

        while (monster.isMaintain)
        {
            //점프 후 착지 체크
            Collider2D[] objects = Physics2D.OverlapBoxAll(monster.transform.position, hitBox, 0);
            foreach (var ground in objects)
            {
                if (ground.CompareTag("Ground"))
                {
                    if (monster.stateMachine.Player.transform.position.y < -1)
                    {
                        monster.stateMachine.Player.TryGetComponent<IDamageable>(out IDamageable damageable);
                        Debug.Log("플레이어 데미지");
                        //damageable.TakeDamage(damage);
                    }
                    monster.isMaintain = false;
                    monster.StartCoroutine(monster.CheckInPattern());
                }
            }
            yield return null;
        }
    }
}
