using UnityEngine;

public class Arrow : MonoBehaviour
{
    [SerializeField] private float speed = 15f;
    private float damage;
    private Vector2 direction;

    public void Init(Vector2 dir, float dmg)
    {
        direction = dir;
        damage = dmg;
        // 방향에 따라 회전
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle);
    }

    void Update()
    {
        transform.position += (Vector3)direction * speed * Time.deltaTime;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            // 플레이어는 무시
            return;
        }

        if (collision.CompareTag("Damageable"))
        {
            var damageable = collision.GetComponent<IDamageable>();
            if (damageable != null)
            {
                damageable.TakeDamage(damage);
            }
            Destroy(gameObject);
        }
        else if (!collision.isTrigger)
        {
            // 벽 등 맞으면 파괴
            Destroy(gameObject);
        }
    }
}
