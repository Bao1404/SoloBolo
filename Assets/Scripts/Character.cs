using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public abstract class Character : MonoBehaviour
{
    [SerializeField] protected float walkSpeed = 0.5f;
    [SerializeField] protected float attackRange = 1f;
    [SerializeField] public float attackDamage = 10f;
    [SerializeField] protected float detectRange = 5f;
    [SerializeField] public float maxHp = 100f;
    public float currentHp;
    [SerializeField] protected Image healthBar;
    [SerializeField] protected CapsuleCollider2D attackCollider;
    [SerializeField] protected BoxCollider2D hitBoxCollider;
    [SerializeField] public string characterType = "None";
    protected GameObject target;
    protected Animator animator;
    public Vector3 initialScale;
    private bool isAttacking = false;

    private AudioSource audioDieSource;  
    public AudioClip dieSound; 

    [SerializeField] private string characterTag = "Character";  // Tag mặc định

    protected virtual void Start()
    {
        animator = GetComponent<Animator>();
        target = FindClosestTarget();

        if (attackCollider != null) attackCollider.enabled = false;
        hitBoxCollider.enabled = true;
        currentHp = maxHp;

        // Lưu hướng ban đầu của nhân vật
        initialScale = transform.localScale;

        // Thiết lập hướng mặc định dựa trên tag của nhân vật
        if (CompareTag("Character"))
        {
            initialScale = new Vector3(1, 1, 1);  // Nhân vật quay phải
        }
        else if (CompareTag("Enemy"))
        {
            initialScale = new Vector3(-1, 1, 1);  // Kẻ địch quay trái
        }

        transform.localScale = initialScale;  // Đặt lại hướng ban đầu
    }

    protected virtual void Update()
    {
        UpdateHealthBar();

        // Nếu chưa đang tấn công, luôn tìm mục tiêu gần nhất
        if (!isAttacking)
        {
            target = FindClosestTarget();
            animator.SetBool("isAttack", false);
        }

        if (DetectTargetInRange())
        {
            FlipCharacter();

            if (AttackTargetInRange())
            {
                if (!isAttacking)
                    Attack();
            }
            else
            {
                Move();
            }
        }
        else
        {
            transform.localScale = initialScale;
            Move();
        }
    }

    protected void Move()
    {
        // Kiểm tra xem có kẻ địch trong phạm vi phát hiện không
        if (DetectTargetInRange())
        {
            float distance = Vector3.Distance(transform.position, target.transform.position);

            // Di chuyển về phía mục tiêu nếu không trong tầm đánh
            if (distance > attackRange)
            {
                Vector3 direction = (target.transform.position - transform.position).normalized;
                transform.position += direction * walkSpeed * Time.deltaTime;

                FlipCharacter();  // Quay về hướng kẻ địch
            }
            else
            {
                SetAttackAnimation(true); // Bắt đầu hoạt ảnh tấn công nếu trong phạm vi
            }
        }
        else
        {
            // Di chuyển mặc định nếu không có mục tiêu trong phạm vi
            if (CompareTag("Character"))
            {
                transform.position += Vector3.right * walkSpeed * Time.deltaTime;
            }
            else if (CompareTag("Enemy"))
            {
                transform.position += Vector3.left * walkSpeed * Time.deltaTime;
            }

            transform.localScale = initialScale;  // Quay lại hướng ban đầu khi không có mục tiêu
        }
    }

    protected void FindNextTarget()
    {
        // Tìm mục tiêu mới sau khi tấn công xong
        target = FindClosestTarget();
    }

    protected void FlipCharacter()
    {
        // Tính toán hướng từ nhân vật đến mục tiêu
        Vector3 directionToTarget = target.transform.position - transform.position;

        // Kiểm tra xem mục tiêu nằm bên trái hay bên phải của nhân vật
        if (directionToTarget.x > 0)
        {
            // Nếu kẻ địch nằm bên phải, nhân vật sẽ quay sang phải
            transform.localScale = new Vector3(1, 1, 1);  // Lật phải
        }
        else
        {
            // Nếu kẻ địch nằm bên trái, nhân vật sẽ quay sang trái
            transform.localScale = new Vector3(-1, 1, 1);  // Lật trái
        }
    }

    protected void SetAttackAnimation(bool isAttacking)
    {
        if (animator.GetBool("isAttack") != isAttacking)
        {
            animator.SetBool("isAttack", isAttacking);
        }
    }

    protected bool DetectTargetInRange()
    {
        return target != null && Vector3.Distance(transform.position, target.transform.position) <= detectRange;
    }

    protected bool AttackTargetInRange()
    {
        if (target == null) return false;
        float distance = Vector3.Distance(transform.position, target.transform.position);
        return distance <= attackRange;
    }

    protected virtual void Attack()
    {
        if (attackCollider == null) return;

        isAttacking = true;
        SetAttackAnimation(true);
        StartCoroutine(AttackRoutine());
    }

    private IEnumerator AttackRoutine()
    {
        attackCollider.enabled = true;
        yield return new WaitForSeconds(1f);

        attackCollider.enabled = false;
        SetAttackAnimation(false);
        isAttacking = false;
        // Ngay khi kết thúc attack, sẽ tự động trong Update() tìm lại target gần nhất
    }

    public void DisableAttackCollider()
    {
        if (attackCollider != null) attackCollider.enabled = false;
    }

    protected GameObject FindClosestTarget()
    {
        GameObject[] targets;

        if (CompareTag("Character"))
        {
            // Nhân vật sẽ tìm Enemy và EnemyBase
            targets = GameObject
                .FindGameObjectsWithTag("Enemy")
                .Concat(GameObject.FindGameObjectsWithTag("EnemyBase"))
                .ToArray();
        }
        else // CompareTag("Enemy")
        {
            // Kẻ địch sẽ tìm Character và Base
            targets = GameObject
                .FindGameObjectsWithTag("Character")
                .Concat(GameObject.FindGameObjectsWithTag("Base"))
                .ToArray();
        }

        GameObject closest = null;
        float minDist = Mathf.Infinity;

        foreach (GameObject t in targets)
        {
            if (t == null) continue;
            float dist = Vector3.Distance(transform.position, t.transform.position);
            if (dist < minDist)
            {
                minDist = dist;
                closest = t;
            }
        }

        return closest;
    }

    protected void UpdateHealthBar()
    {
        if (healthBar != null)
        {
            healthBar.fillAmount = currentHp / maxHp;
        }
    }

    public virtual void TakeDamage(float damage)
    {
        currentHp -= damage;
        currentHp = Mathf.Max(currentHp, 0);
        UpdateHealthBar();

        if (currentHp <= 0)
        {
            Die();
        }
    }

    public void Die()
    {
        transform.localScale = initialScale;
        Destroy(gameObject);  // Tiêu diệt đối tượng
        DieSound();
        if (CompareTag("Enemy"))
        {
            GameManager.instance.AddCoin(2);
        }
    }

    private void DieSound()
    {
        if (audioDieSource != null && dieSound != null)
        {
            audioDieSource.PlayOneShot(dieSound);  // Phát âm thanh một lần
        }
    }

    private void Awake()
    {
        // Tìm AudioSource nếu chưa có
        audioDieSource = GetComponent<AudioSource>();
        if (audioDieSource == null)
        {
            audioDieSource = gameObject.AddComponent<AudioSource>();  // Nếu không có, tạo một AudioSource mới
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        TryIgnoreCharacterCollision(collision);
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        TryIgnoreCharacterCollision(collision);
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if ((collision.gameObject.CompareTag("Character") || collision.gameObject.CompareTag("Enemy")) && hitBoxCollider != null)
        {
            Collider2D otherCol = collision.collider;
            Physics2D.IgnoreCollision(hitBoxCollider, otherCol, false);
        }
    }

    private void TryIgnoreCharacterCollision(Collision2D collision)
    {
        if ((collision.gameObject.CompareTag("Character") || collision.gameObject.CompareTag("Enemy")) && hitBoxCollider != null)
        {
            float y1 = transform.position.y;
            float y2 = collision.transform.position.y;

            if (Mathf.Abs(y1 - y2) > 0.1f)
            {
                Collider2D otherCol = collision.collider;
                Physics2D.IgnoreCollision(hitBoxCollider, otherCol, true);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (attackCollider != null && attackCollider.enabled)
        {
            HandleAttackCollision(collision);
        }
    }

    private void HandleAttackCollision(Collider2D collision)
    {
        if (CompareTag("Character") && collision.CompareTag("Enemy"))
        {
            Character enemyTarget = collision.GetComponent<Character>();
            if (enemyTarget != null)
            {
                enemyTarget.TakeDamage(attackDamage);
            }
        }
        else if (CompareTag("Enemy") && collision.CompareTag("Character"))
        {
            Character characterTarget = collision.GetComponent<Character>();
            if (characterTarget != null)
            {
                characterTarget.TakeDamage(attackDamage);
            }
        }
    }

    protected void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, detectRange);
    }
}
