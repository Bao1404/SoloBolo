using UnityEngine;

public class Arrow : MonoBehaviour
{
    [SerializeField] private float damage = 10f;
    [SerializeField] private float lifeTime = 5f;

    private Rigidbody2D rb;
    private string targetTag; // 👈 lưu tag của mục tiêu

    public void Launch(Vector2 targetPosition, float timeToTarget, string targetTag)
    {
        this.targetTag = targetTag;
        rb = GetComponent<Rigidbody2D>();

        Vector2 start = transform.position;
        Vector2 dir = targetPosition - start;
        float distance = dir.magnitude;

        Vector2 velocity = dir.normalized * (distance / timeToTarget);
        rb.linearVelocity = velocity;

        RotateToDirection(velocity);
        Destroy(gameObject, lifeTime);
    }

    private void RotateToDirection(Vector2 dir)
    {
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(targetTag))
        {
            bool didDamage = false;

            if (targetTag == "Enemy" && collision.TryGetComponent<Enemy>(out Enemy enemy))
            {
                enemy.TakeDame(damage);
                didDamage = true;
            }
            else if (targetTag == "Character" && collision.TryGetComponent<Character>(out Character character))
            {
                character.TakeDame(damage);
                didDamage = true;
            }

            if (didDamage)
                Destroy(gameObject);
            if(transform.position.y < -2.2f)
            {
                Destroy(gameObject);
            }
        }
    }
}