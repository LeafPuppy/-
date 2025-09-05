using System.Threading;
using Unity.VisualScripting;
using UnityEngine;

public class WeaponDropDamage : MonoBehaviour
{
    public bool canDamage = true;
    public float damage;

    private void OnTriggerEnter2D(Collider2D collision)
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
        else if(collision.gameObject.CompareTag("Spikes"))
        {
            Destroy(this.gameObject);
        }
    }
}
