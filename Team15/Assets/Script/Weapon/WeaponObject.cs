using UnityEngine;

public enum WeaponType
{
    Sword,
    Bow,
    Staff,
}
public class WeaponObject : MonoBehaviour
{
    [SerializeField] private GameObject arrowPrefab;
    [SerializeField] WeaponAnimationController weaponAnimationController;
    [Header("Weapon Settings")]
    public bool canAttack = false;
    public float damage = 10f;
    public float attackRange = 1.5f;
    public float attackDamage = 10f;

    public WeaponType weaponType = WeaponType.Sword;

    // [추가] 발사 쿨타임 변수
    [Header("Attack Cooldown")]
    public float attackCooldown = 0.5f;
    private float lastAttackTime = -999f;

    public void TakeAttack(Vector2 attackOrigin, Vector2 attackDirection)
    {
        // [추가] 쿨타임 체크
        if (Time.time < lastAttackTime + attackCooldown)
            return;

        lastAttackTime = Time.time;
        weaponAnimationController.ChangeAnimation(WeaponState.Attack);

        switch (weaponType)
        {
            case WeaponType.Sword:
                {
                    Vector2 attackDir = (transform.right + transform.up).normalized;
                    Collider2D[] hits = Physics2D.OverlapCircleAll(attackOrigin, attackRange);
                    float halfAngle = 45f;
                    foreach (var hit in hits)
                    {
                        if (hit == null || !hit.CompareTag("Damageable")) continue;
                        Vector2 toTarget = ((Vector2)hit.transform.position - attackOrigin).normalized;
                        float angle = Vector2.Angle(attackDir, toTarget);
                        if (angle <= halfAngle)
                        {
                            var damageable = hit.GetComponent<IDamageable>();
                            if (damageable != null)
                                damageable.TakeDamage(attackDamage);
                        }
                    }
                }
                break;

            case WeaponType.Bow:
                {
                    float arrowOffset = 0.5f;
                    Vector2 spawnPos = transform.position + (Vector3)(attackDirection.normalized * arrowOffset);

                    float angle = Mathf.Atan2(attackDirection.y, attackDirection.x) * Mathf.Rad2Deg;
                    GameObject arrow = Instantiate(
                        arrowPrefab,
                        spawnPos,
                        Quaternion.Euler(0, 0, angle)
                    );
                    var arrowScript = arrow.GetComponent<Arrow>();
                    if (arrowScript != null)
                    {
                        arrowScript.Init(attackDirection.normalized, attackDamage);
                    }
                }
                break;

            case WeaponType.Staff:
                {
                    Collider2D[] hits = Physics2D.OverlapCircleAll(attackOrigin, attackRange);
                    foreach (var hit in hits)
                    {
                        if (hit == null || !hit.CompareTag("Damageable")) continue;
                        var damageable = hit.GetComponent<IDamageable>();
                        if (damageable != null)
                            damageable.TakeDamage(attackDamage);
                    }
                }
                break;

            default:
                {
                    Vector2 attackDir = (transform.right + transform.up).normalized;
                    Collider2D[] hits = Physics2D.OverlapCircleAll(attackOrigin, attackRange);
                    float halfAngle = 45f;
                    foreach (var hit in hits)
                    {
                        if (hit == null || !hit.CompareTag("Damageable")) continue;
                        Vector2 toTarget = ((Vector2)hit.transform.position - attackOrigin).normalized;
                        float angle = Vector2.Angle(attackDir, toTarget);
                        if (angle <= halfAngle)
                        {
                            var damageable = hit.GetComponent<IDamageable>();
                            if (damageable != null)
                                damageable.TakeDamage(attackDamage);
                        }
                    }
                }
                break;
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
}
