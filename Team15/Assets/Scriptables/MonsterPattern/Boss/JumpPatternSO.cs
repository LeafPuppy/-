using System.Collections;
using UnityEngine;

[CreateAssetMenu(fileName = "Jump", menuName = "Pattern/Jump")]
public class JumpPatternSO : PatternDataSO
{
    public float jumpPower;
    public Vector2 hitBox;

    public override IEnumerator Execute(Monster monster)
    {
        yield return Jump(monster);

        //50퍼센트 확률로 2타 실행
        if(Random.Range(0, 2) < 1)
        {
            monster.StopCoroutine(monster.co);
            yield return Jump(monster);
        }
    }

    private IEnumerator Jump(Monster monster)
    {
        monster.inPattern = true;
        monster.isMaintain = true;
        monster.rg.velocity = new Vector2(monster.rg.velocity.x, 0f);
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

                    monster.co = monster.StartCoroutine(monster.CheckInPattern());
                }
            }
            yield return null;
        }
    }
}
