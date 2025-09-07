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
    [SerializeField] private GameObject magicPrefab; // [추가] Magic 프리팹 Inspector에서 할당
    [SerializeField] WeaponAnimationController weaponAnimationController;
    [Header("Weapon Settings")]
    public bool canAttack = false;
    public float damage = 10f;
    public float attackRange = 1.5f;
    public float attackDamage = 10f;

    public WeaponType weaponType = WeaponType.Sword;

    [Header("Attack Cooldown")]
    public float attackCooldown = 0.5f;
    private float lastAttackTime = -999f;

    [Header("Staff Settings")]
    public float staffCooldown = 1f;
    private float lastStaffTime = -999f;
    public float staffExplosionRadius = 2f;

    public void TakeAttack(Vector2 attackOrigin, Vector2 attackDirection)
    {
        if (weaponType == WeaponType.Staff)
        {
            if (Time.time < lastStaffTime + staffCooldown)
                return;
            lastStaffTime = Time.time;
        }
        else
        {
            if (Time.time < lastAttackTime + attackCooldown)
                return;
            lastAttackTime = Time.time;
        }

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
                    float orbOffset = 0.5f;
                    Vector2 spawnPos = transform.position + (Vector3)(attackDirection.normalized * orbOffset);

                    float angle = Mathf.Atan2(attackDirection.y, attackDirection.x) * Mathf.Rad2Deg;
                    GameObject orb = Instantiate(
                        magicPrefab,
                        spawnPos,
                        Quaternion.Euler(0, 0, angle)
                    );
                    var magicScript = orb.GetComponent<Magic>();
                    if (magicScript != null)
                    {
                        magicScript.Init(attackDirection.normalized, attackDamage, staffExplosionRadius);
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
