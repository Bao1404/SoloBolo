using UnityEngine;

public class Shield : Character
{
    [SerializeField] private float shieldBlockDame = 5f;  // Sức chống chịu của khiên

    private bool isShieldActive = false;  // Kiểm tra khiên có đang hoạt động không

    protected override void Start()
    {
        base.Start();
        // Khởi tạo khiên
        isShieldActive = false;
    }

    protected override void Update()
    {
        // Kiểm tra nếu có kẻ địch trong phạm vi phát hiện
        if (DetectTargetInRange())
        {
            // Kiểm tra nếu kẻ địch trong phạm vi tấn công
            if (AttackTargetInRange())
            {
                ActivateShield();  // Kích hoạt khiên khi vào tầm tấn công
            }
            else
            {
                DeactivateShield();  // Nếu không trong tầm tấn công, tắt khiên
            }
        }
        else
        {
            DeactivateShield();  // Nếu không có kẻ địch trong phạm vi phát hiện, tắt khiên
        }

        UpdateHealthBar();

        // Kiểm tra nếu có mục tiêu trong phạm vi phát hiện và khiên hoạt động
        if (AttackTargetInRange() && isShieldActive)
        {
            // Khiên đang hoạt động, nhưng cũng cần di chuyển nếu không có mục tiêu
            Move();  // Di chuyển về phía mục tiêu
        }
        else if (!AttackTargetInRange())
        {
            // Di chuyển mặc định khi không có kẻ địch trong tầm tấn công
            Move();
        }
    }

    // Kích hoạt khiên
    private void ActivateShield()
    {
        isShieldActive = true;
        Debug.Log("Shield activated");
    }

    // Tắt khiên
    private void DeactivateShield()
    {
        isShieldActive = false;
        Debug.Log("Shield deactivated");
    }

    // Override phương thức TakeDamage để giảm sát thương nếu khiên đang hoạt động
    public override void TakeDamage(float damage)
    {
        if (isShieldActive)
        {
            damage -= shieldBlockDame;  // Giảm bớt lượng sát thương nhận vào
            damage = Mathf.Max(damage, 0);  // Đảm bảo sát thương không giảm xuống dưới 0
            Debug.Log("Damage reduced by shield: " + shieldBlockDame);
        }

        currentHp -= damage;
        currentHp = Mathf.Max(currentHp, 0);
        UpdateHealthBar();

        if (currentHp <= 0)
        {
            Die();
        }
    }

    // Các phương thức khác của lớp Character (FindClosestTarget, UpdateHealthBar, Die...)
}
