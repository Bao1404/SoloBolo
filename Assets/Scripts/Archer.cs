using UnityEngine;

public class Archer : Character
{
    [SerializeField] private GameObject arrowPrefab;
    [SerializeField] private Transform firePoint;
    [SerializeField] private float arrowFlightTime = 0.6f;
    private float shootCooldown = 1f;  // Cooldown between shots (in seconds)
    private float lastShootTime = 0f;  // Time of last shot

    private AudioSource audioSource;  
    public AudioClip attackSound; 

    protected override void Start()
    {
        base.Start();
        // Optionally, find the closest enemy character to shoot at
        target = FindClosestTarget();
    }

    protected override void Update()
    {
        // Nếu không có mục tiêu hoặc mục tiêu đã bị tiêu diệt, tìm mục tiêu mới
        if (target == null || currentHp <= 0)
        {
            target = FindClosestTarget();  // Cập nhật lại target nếu không có mục tiêu
            animator.SetBool("isAttack", false);  // Dừng hoạt ảnh tấn công khi không còn mục tiêu
        }

        UpdateHealthBar();

        // Kiểm tra nếu mục tiêu trong phạm vi phát hiện
        if (DetectTargetInRange())
        {
            FlipCharacter();  // Quay về hướng kẻ địch

            // Kiểm tra nếu mục tiêu trong phạm vi tấn công và cooldown đã hết
            if (AttackTargetInRange() && Time.time >= lastShootTime + shootCooldown)
            {
                animator.SetBool("isAttack", true);
                ShootArrow();  // Bắn mũi tên
                lastShootTime = Time.time; // Reset last shoot time after shooting
                FindNextTarget();  // Tìm mục tiêu mới sau khi tấn công xong
            }
            else
            {
                animator.SetBool("isAttack", false);
                Move();  // Di chuyển nếu không trong phạm vi tấn công
            }
        }
        else
        {
            transform.localScale = initialScale;
            Move();  // Di chuyển về phía mục tiêu nếu không có mục tiêu trong phạm vi
        }
    }

    public void ShootArrow()
    {
        if (target == null) return;

        Debug.Log("Shooting arrow at enemy: " + target.name);  // Log để kiểm tra mũi tên được bắn

        // Kiểm tra xem mục tiêu có tag phù hợp không
        if ((CompareTag("Character") && target.CompareTag("Enemy")) || (CompareTag("Enemy") && target.CompareTag("Character")))
        {
            // Tính toán hướng từ nhân vật đến kẻ địch
            Vector3 directionToEnemy = (target.transform.position - firePoint.position).normalized;

            // Instantiate the arrow at the firePoint
            GameObject arrowObj = Instantiate(arrowPrefab, firePoint.position, Quaternion.identity);
            Arrow arrow = arrowObj.GetComponent<Arrow>();

            PlayAttackSound();

            if (arrow != null)
            {
                // Tính toán thời gian bay của mũi tên
                float timeToTarget = Vector3.Distance(firePoint.position, target.transform.position) / arrow.speed; // speed của mũi tên có thể được đặt trong class Arrow

                // Bắn mũi tên về phía kẻ địch
                arrow.Launch(target.transform.position, timeToTarget, CompareTag("Character") ? "Enemy" : "Character");

                // Cập nhật thời gian bắn cuối cùng
                lastShootTime = Time.time; // Đảm bảo thời gian bắn được cập nhật sau mỗi lần tấn công
            }
            else
            {
                Debug.LogError("Arrow object is missing the Arrow component!");
            }
        }
    }

    // Kiểm tra xem mục tiêu có trong phạm vi để tấn công hay không
    protected bool AttackTargetInRange()
    {
        if (target != null)
        {
            float distance = Vector3.Distance(transform.position, target.transform.position);
            return distance <= attackRange;
        }
        return false;
    }

    private void PlayAttackSound()
    {
        if (audioSource != null && attackSound != null)
        {
            audioSource.PlayOneShot(attackSound);  // Phát âm thanh một lần
        }
    }

    private void Awake()
    {
        // Tìm AudioSource nếu chưa có
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();  // Nếu không có, tạo một AudioSource mới
        }
    }
}
