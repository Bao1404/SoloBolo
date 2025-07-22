using UnityEngine;

public class ArcherEnemy : Character
{
    [SerializeField] private GameObject arrowPrefab;
    [SerializeField] private Transform firePoint;
    private GameObject character;
    [SerializeField] private float arrowFlightTime = 0.6f;
    private float shootCooldown = 1f;  // Cooldown between shots (in seconds)
    private float lastShootTime = 0f;  // Time of last shot

    protected override void Start()
    {
        base.Start();
        // Optionally, find the closest enemy character to shoot at
        character = FindClosestTarget();
    }

    protected override void Update()
    {
        // If target is null, find a new one
        if (character == null)
        {
            character = FindClosestTarget();
            animator.SetBool("isAttack", false);  // Dừng hoạt ảnh tấn công khi không còn mục tiêu
        }

        UpdateHealthBar();

        // Allow attack if within range
        if (AttackTargetInRange() && Time.time >= lastShootTime + shootCooldown)
        {
            animator.SetBool("isAttack", true);
            ShootArrow();
        }
        else
        {
            animator.SetBool("isAttack", false);
            Move();  // Move the character based on the target's position
        }
    }

    public void ShootArrow()
    {
        if (character == null) return;

        // Instantiate the arrow at the firePoint
        GameObject arrowObj = Instantiate(arrowPrefab, firePoint.position, Quaternion.identity);
        Arrow arrow = arrowObj.GetComponent<Arrow>();

        // Calculate time to target (you can adjust this based on your game mechanics)
        float timeToTarget = 0.6f;

        // Launch the arrow towards the character's position
        arrow.Launch(character.transform.position, timeToTarget, "Character");

        // Update the last shoot time
        lastShootTime = Time.time;
    }
}
