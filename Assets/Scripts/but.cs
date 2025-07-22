using UnityEngine;

public class But : MonoBehaviour
{
    [SerializeField] private float healAmount = 10f;  // Lượng máu hồi
    [SerializeField] private float healRange = 5f;    // Phạm vi hồi máu
    [SerializeField] private float moveSpeed = 2f;    // Tốc độ di chuyển của Bụt
    private Animator animator;       // Animator để điều khiển các hoạt ảnh
    private float currentHp = 100f;  // Máu hiện tại của Bụt

    private bool isHealing = false; // Kiểm tra xem Bụt có đang hồi máu hay không

    void Start()
    {
        animator = GetComponent<Animator>();  // Lấy Animator từ đối tượng
        currentHp = 100f;  // Khởi tạo máu ban đầu
    }
    void Update()
    {
        // Tìm kiếm đồng minh có cùng tag trong phạm vi hồi máu
        GameObject ally = FindLowestHealthAllyInRange();

        // Nếu có đồng minh cần hồi máu, Bụt đứng yên và thực hiện hành động hồi máu
        if (ally != null)
        {
            isHealing = true;  // Đang hồi máu
            HealAlly(ally);  // Hồi máu cho đồng minh
            animator.SetBool("isHeal", true);  // Kích hoạt animation hồi máu
        }
        else
        {
            isHealing = false;  // Không hồi máu, có thể di chuyển
            animator.SetBool("isHeal", false);  // Tắt animation hồi máu
            Move();  // Di chuyển Bụt
        }
    }

    // Tìm đồng minh có cùng tag trong phạm vi hồi máu, ưu tiên đồng minh có HP thấp nhất
    private GameObject FindLowestHealthAllyInRange()
    {
        GameObject[] allies = GameObject.FindGameObjectsWithTag(tag); // Tìm các đồng minh có tag giống với But

        GameObject lowestHpAlly = null;
        float minHp = Mathf.Infinity;

        foreach (GameObject ally in allies)
        {
            if (ally == null) continue;

            Character allyCharacter = ally.GetComponent<Character>(); // Kiểm tra nếu đối tượng là một nhân vật

            // Kiểm tra xem đồng minh có trong phạm vi hồi máu không
            if (allyCharacter != null && Vector3.Distance(transform.position, ally.transform.position) <= healRange)
            {
                if (allyCharacter.currentHp < minHp)  // Nếu máu của đồng minh thấp hơn máu thấp nhất đã tìm thấy
                {
                    minHp = allyCharacter.currentHp;
                    lowestHpAlly = ally;  // Cập nhật đồng minh có máu thấp nhất
                }
            }
        }

        return lowestHpAlly;  // Trả về đồng minh có máu thấp nhất
    }

    // Hồi máu cho đồng minh
    private void HealAlly(GameObject ally)
    {
        Character allyCharacter = ally.GetComponent<Character>();
        if (allyCharacter != null)
        {
            // Tính toán lượng máu sau khi hồi
            float newHealth = allyCharacter.currentHp + healAmount;

            // Đảm bảo máu không vượt quá maxHp
            newHealth = Mathf.Min(newHealth, allyCharacter.maxHp);

            // Cập nhật máu cho đồng minh
            allyCharacter.currentHp = newHealth;
            Debug.Log("Healed ally: " + ally.name);
        }
    }

    // Di chuyển của Bụt
    private void Move()
    {
        // Kiểm tra nếu Bụt có tag "Character" thì di chuyển phải, ngược lại di chuyển trái
        if (CompareTag("Character"))
        {
            transform.position += Vector3.right * moveSpeed * Time.deltaTime;  // Di chuyển phải
        }
        else if (CompareTag("Enemy"))
        {
            transform.position += Vector3.left * moveSpeed * Time.deltaTime;  // Di chuyển trái
        }
    }

    // Phương thức nhận sát thương
    public void TakeDamage(float damage)
    {
        // Kiểm tra khiên đang hoạt động (nếu có) hoặc giảm sát thương theo logic của bạn
        currentHp -= damage;
        currentHp = Mathf.Max(currentHp, 0);  // Đảm bảo máu không âm
        Debug.Log("Current HP: " + currentHp);

        if (currentHp <= 0)
        {
            Die();  // Nếu máu <= 0, chết
        }
    }

    // Phương thức chết
    public void Die()
    {
        Debug.Log("But is dead!");
        Destroy(gameObject);  // Tiêu diệt đối tượng
    }

    // Vẽ Gizmo để hiển thị phạm vi hồi máu
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;  // Màu sắc của phạm vi hồi máu
        Gizmos.DrawWireSphere(transform.position, healRange);  // Vẽ hình cầu xung quanh But để biểu thị phạm vi hồi máu
    }
}
