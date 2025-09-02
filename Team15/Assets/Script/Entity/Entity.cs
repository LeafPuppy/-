using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamageable
{
    void TakeDamage(float damage);
}

public class Entity : MonoBehaviour, IDamageable
{
    public float maxHealth = 100f;
    public float currentHealth;
    protected virtual void Start()
    {
        currentHealth = maxHealth;
    }
    public virtual void TakeDamage(float damage)
    {
        currentHealth -= damage;
        if (currentHealth <= 0)
        {
            Die();
        }
    }
    protected virtual void Die()
    {
        // Handle entity death (e.g., play animation, drop loot, etc.)
        Destroy(gameObject);
    }
}