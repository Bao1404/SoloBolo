using UnityEngine;

public class ArcherCharacter : Character
{
    [SerializeField] private GameObject arrowPrefab;
    [SerializeField] private Transform firePoint;
    private GameObject enemy;
    [SerializeField] private float arrowFlightTime = 0.6f;
    private float shootCooldown = 1f;  // Cooldown between shots (in seconds)
    private float lastShootTime = 0f;  // Time of last shot

    protected override void Start()
    {
        base.Start();
        // Optionally, find the closest enemy character to shoot at
        enemy = FindClosestTarget();
    }

    protected override void Update()
    {
        if (enemy == null)
        {
            enemy = FindClosestTarget();
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
        if (enemy == null) return;

        // Instantiate the arrow at the firePoint
        GameObject arrowObj = Instantiate(arrowPrefab, firePoint.position, Quaternion.identity);
        Arrow arrow = arrowObj.GetComponent<Arrow>();

        // Calculate time to target (you can adjust this based on your game mechanics)
        float timeToTarget = 0.6f;

        // Launch the arrow towards the character's position
        arrow.Launch(enemy.transform.position, timeToTarget, "Enemy");

        // Update the last shoot time
        lastShootTime = Time.time;
    }
}
