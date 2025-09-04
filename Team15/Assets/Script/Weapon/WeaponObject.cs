using UnityEngine;

public class WeaponObject : MonoBehaviour
{
    [Header("Weapon Settings")]
    public bool canAttack = false;
    public float damage = 10f;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!canAttack) return;

        if (collision.gameObject.CompareTag("Damageable"))
        {
            // IDamageable 인터페이스 구현체가 있으면 데미지 처리
            var damageable = collision.gameObject.GetComponent<IDamageable>();
            if (damageable != null)
            {
                damageable.TakeDamage(damage);
            }
            // 추가적으로 데미지 이펙트, 사운드 등 처리 가능
        }
    }

    // 공격 가능 상태를 외부에서 제어할 수 있도록 메서드 제공
    public void SetAttackState(bool state)
    {
        canAttack = state;
    }
}
