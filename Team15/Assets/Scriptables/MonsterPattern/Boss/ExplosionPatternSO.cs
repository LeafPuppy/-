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
        //���� ��ġ�� �������� ǥ��
        exPosX = Random.Range(-4f, 4f);
        var point = Instantiate(pointEffect, new Vector2(exPosX, -2), Quaternion.identity);

        //delayTime��ŭ ���
        yield return new WaitForSeconds(delayTime);

        //�������� ����, ��������Ʈ ǥ��
        Destroy(point);
        var ex = Instantiate(exEffect, new Vector2(exPosX, -1.5f), Quaternion.identity);
        Collider2D[] objects = Physics2D.OverlapBoxAll(new Vector2(exPosX, -1.5f), hitBox, 0);
        foreach (Collider2D obj in objects)
        {
            if(obj.CompareTag("Player"))
            {
                Debug.Log("�÷��̾� ������");
                //obj.TryGetComponent<IDamageable>(out IDamageable damageable);
                //damageable.TakeDamage(damage);
            }
        }

        monster.StartCoroutine(monster.CheckInPattern());

        //1�� �� ��������Ʈ ����
        yield return new WaitForSeconds(1f);
        Destroy(ex);
    }
}
