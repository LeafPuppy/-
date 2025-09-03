using System.Collections;
using UnityEngine;

[CreateAssetMenu(fileName = "Explosion", menuName = "Pattern/Explosion")]
public class ExplosionPatternSO : PatternDataSO
{
    public GameObject pointEffect;
    public GameObject exEffect;
    public Vector2 hitBox;
    public float delayTime;

    private float exPosX;

    public override IEnumerator Execute(Monster monster)
    {
        //랜덤 위치에 폭발지점 표시
        exPosX = Random.Range(-4f, 4f);
        var point = Instantiate(pointEffect, new Vector2(exPosX, -2), Quaternion.identity);

        //delayTime만큼 대기
        yield return new WaitForSeconds(delayTime);

        //폭발지점 삭제, 폭발이펙트 표시
        Destroy(point);
        var ex = Instantiate(exEffect, new Vector2(exPosX, -1.5f), Quaternion.identity);
        Collider2D[] objects = Physics2D.OverlapBoxAll(new Vector2(exPosX, -1.5f), hitBox, 0);
        foreach (Collider2D obj in objects)
        {
            if(obj.CompareTag("Player"))
            {
                Debug.Log("플레이어 데미지");
                //obj.TryGetComponent<IDamageable>(out IDamageable damageable);
                //damageable.TakeDamage(damage);
            }
        }

        monster.StartCoroutine(monster.CheckInPattern());

        //1초 뒤 폭발이펙트 삭제
        yield return new WaitForSeconds(1f);
        Destroy(ex);
    }
}
