using UnityEngine;

public class WeaponObject : MonoBehaviour
{
    [SerializeField] WeaponAnimationController weaponAnimationController;
    [Header("Weapon Settings")]
    public bool canAttack = false;
    public float damage = 10f;
    public float attackRange = 1.5f;
    public float attackDamage = 10f;

    public void TakeAttack(Vector2 attackOrigin, Vector2 attackDirection)
    {
        weaponAnimationController.ChangeAnimation(WeaponState.Attack);

        // 2. 범위 내 공격 판정
        RaycastHit2D[] hits = Physics2D.CircleCastAll(attackOrigin, attackRange, attackDirection, attackRange);
        foreach (var hit in hits)
        {
            if (hit.collider != null && hit.collider.CompareTag("Damageable"))
            {
                var damageable = hit.collider.GetComponent<IDamageable>();
                if (damageable != null)
                {
                    damageable.TakeDamage(attackDamage);
                    Debug.Log($"{hit.collider.name}에게 {attackDamage} 데미지!");
                }
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!canAttack) return;

        if (collision.gameObject.CompareTag("Damageable"))
        {
            var damageable = collision.gameObject.GetComponent<IDamageable>();
            if (damageable != null)
            {
                damageable.TakeDamage(damage);
            }
        }
    }

    public void SetAttackState(bool state)
    {
        canAttack = state;
    }

    // 공격 판정 범위 Gizmo 표시
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}
