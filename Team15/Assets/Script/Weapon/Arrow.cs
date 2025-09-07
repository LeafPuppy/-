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
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle);
    }

    void Update()
    {
        transform.position += (Vector3)direction * speed * Time.deltaTime;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Weapon 레이어는 무시
        if (collision.gameObject.layer == LayerMask.NameToLayer("Weapon"))
            return;

        if (collision.CompareTag("Player"))
            return;

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
            Destroy(gameObject);
        }
    }
}

