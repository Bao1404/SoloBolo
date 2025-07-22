using UnityEngine;

public class Arrow : MonoBehaviour
{
    [SerializeField] private float damage = 10f;
    [SerializeField] private float lifeTime = 5f;
    [SerializeField] public float speed = 10f;  // Thêm trường speed cho mũi tên

    private Rigidbody2D rb;
    private string targetTag; // Store the target's tag

    public void Launch(Vector2 targetPosition, float timeToTarget, string targetTag)
    {
        this.targetTag = targetTag;  // Store the tag of the target (either "Enemy" or "Character")
        rb = GetComponent<Rigidbody2D>();  // Get the Rigidbody2D of the arrow

        // Calculate direction and velocity
        Vector2 start = transform.position;
        Vector2 dir = targetPosition - start;
        float distance = dir.magnitude;

        // Calculate velocity to hit the target using the speed field
        Vector2 velocity = dir.normalized * speed;

        // Set the arrow's velocity
        rb.linearVelocity = velocity;

        // Rotate the arrow in the direction of the movement
        RotateToDirection(velocity);

        // Destroy the arrow after 'lifeTime' seconds to prevent infinite objects in the scene
        Destroy(gameObject, lifeTime);
    }

    private void RotateToDirection(Vector2 dir)
    {
        // Calculate the angle from the direction vector and set the rotation of the arrow
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle); // Apply the rotation in 2D space
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Check if the collision target matches the specified target tag
        if (collision.CompareTag(targetTag))
        {
            bool didDamage = false;

            // Apply damage to the "Enemy" if it's an enemy character
            if (targetTag == "Enemy" && collision.TryGetComponent<Character>(out Character enemy))
            {
                enemy.TakeDamage(damage);  // Use the TakeDamage method in the Character class
                didDamage = true;
            }
            // Apply damage to the "Character" if it's a player character
            else if (targetTag == "Character" && collision.TryGetComponent<Character>(out Character character))
            {
                character.TakeDamage(damage);  // Use the TakeDamage method in the Character class
                didDamage = true;
            }

            // Destroy the arrow after it has damaged a target
            if (didDamage)
            {
                Destroy(gameObject);
            }
        }

        // Destroy the arrow if it falls below a certain height (optional, for cleanup)
        if (transform.position.y < -2.2f)
        {
            Destroy(gameObject);
        }
    }
}
