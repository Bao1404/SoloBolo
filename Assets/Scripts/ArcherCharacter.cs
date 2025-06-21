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
        // Kiểm tra mục tiêu và tấn công nếu mục tiêu ở trong phạm vi tấn công
        if (AttackTargetInRange() && Time.time >= lastShootTime + shootCooldown)
        {
            animator.SetBool("isAttack", true);
            Attack();  // Gọi Attack để bắn mũi tên
        }
        else
        {
            animator.SetBool("isAttack", false);
            Move();  // Nếu không, di chuyển về phía mục tiêu
        }
    }

    protected override void Attack()
    {
        ShootArrow();
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
