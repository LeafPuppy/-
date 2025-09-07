using UnityEngine;

public class Magic : MonoBehaviour
{
    [SerializeField] private float speed = 10f;
    [SerializeField] private float explosionRadius = 2f;
    [SerializeField] private Animator animator; // [추가] 애니메이터 연결
    private float damage;
    private Vector2 direction;
    private bool exploded = false;

    public void Init(Vector2 dir, float dmg, float radius)
    {
        direction = dir;
        damage = dmg;
        explosionRadius = radius;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle);
    }

    void Update()
    {
        if (!exploded)
            transform.position += (Vector3)direction * speed * Time.deltaTime;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (exploded) return;

        // Weapon 레이어는 무시
        if (collision.gameObject.layer == LayerMask.NameToLayer("Weapon"))
            return;

        if (collision.CompareTag("Player"))
            return;

        // 폭발: 주변에 데미지
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, explosionRadius);
        foreach (var hit in hits)
        {
            if (hit == null || !hit.CompareTag("Damageable")) continue;
            var damageable = hit.GetComponent<IDamageable>();
            if (damageable != null)
                damageable.TakeDamage(damage);
        }

        exploded = true;

        // 폭발 애니메이션 실행
        if (animator != null)
            Debug.Log("Explosion Animation Triggered");
        animator.SetBool("isTrigger", true);

        // 1초 뒤 오브젝트 삭제
        Destroy(gameObject, 1f);
    }
}
