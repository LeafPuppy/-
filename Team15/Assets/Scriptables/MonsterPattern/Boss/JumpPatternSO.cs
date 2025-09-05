using System.Collections;
using UnityEngine;

[CreateAssetMenu(fileName = "Jump", menuName = "Pattern/Jump")]
public class JumpPatternSO : PatternDataSO
{
    public float jumpPower;

    public override IEnumerator Execute(Monster monster)
    {
        yield return Jump(monster);

        //50퍼센트 확률로 2타 실행
        if (Random.Range(0, 2) == 1)
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
            if (monster.transform.position.y < 2)
            {
                AudioManager.Instance.PlaySFX("JumpSFX");
                if (monster.stateMachine.Player.transform.position.y < 1)
                {
                    Debug.Log("플레이어 데미지");
                    monster.stateMachine.Player.TryGetComponent<IDamageable>(out IDamageable damageable);
                    damageable.TakeDamage(damage);
                }
                monster.isMaintain = false;

                monster.co = monster.StartCoroutine(monster.CheckInPattern());
            }
            yield return null;
        }
    }
}
