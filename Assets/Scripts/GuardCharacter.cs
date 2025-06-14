using UnityEngine;

public class GuardCharacter : Character
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (attackCollider.enabled && collision.CompareTag("Enemy"))
        {
            if (collision.TryGetComponent<Enemy>(out Enemy enemyTarget))
            {
                enemyTarget.TakeDame(attackDamage);
            }
        }
    }
}