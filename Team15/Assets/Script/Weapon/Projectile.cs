using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float damage;
    public float speed;
    private bool isArrive;
    private Rigidbody2D rb;
    Vector2 dir;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    public void Attack(Transform target)
    {
        dir = target.transform.position - this.transform.position;

    }

    private void Update()
    {
        rb.velocity = dir * speed;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isArrive = true;
            collision.TryGetComponent<IDamageable>(out IDamageable damageable);
            damageable.TakeDamage(damage);
            Destroy(gameObject);
        }
        else if (collision.gameObject.layer == LayerMask.GetMask("Enemy"))
        {
            return;
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
