using System.Threading;
using UnityEngine;

public class WeaponDropDamage : MonoBehaviour
{
    public bool canDamage = true;
    public float damage;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && canDamage)
        {
            Debug.Log("플레이어 데미지");
            collision.gameObject.TryGetComponent<IDamageable>(out IDamageable damageable);
            damageable.TakeDamage(damage);
            Destroy(this.gameObject);
        }
        else if (collision.gameObject.CompareTag("Ground"))
        {
            canDamage = false;
        }
    }
}
