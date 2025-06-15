using UnityEngine;
public class Arrow : MonoBehaviour
{
    [SerializeField] private float damage = 10f;
    [SerializeField] private float lifeTime = 5f;

    private Rigidbody2D rb;

    public void Launch(Vector2 targetPosition, float timeToTarget)
    {
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
        if (collision.CompareTag("Enemy"))
        {
            if (collision.TryGetComponent<Enemy>(out Enemy enemy))
            {
                enemy.TakeDame(damage);
            }
            Destroy(gameObject);
        }
    }
}
