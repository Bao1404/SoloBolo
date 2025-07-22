using System.Collections;
using UnityEngine;
using UnityEngine.UI;

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
        // Nếu không có mục tiêu, tìm mục tiêu mới
        if (target == null)
        {
            target = FindClosestTarget();
            animator.SetBool("isAttack", false);  // Dừng hoạt ảnh tấn công khi không còn mục tiêu
        }

        UpdateHealthBar();

        // Kiểm tra nếu mục tiêu trong phạm vi phát hiện
        if (DetectTargetInRange())
        {
            FlipCharacter();  // Quay về hướng kẻ địch
            // Tấn công nếu mục tiêu trong phạm vi
            if (AttackTargetInRange())
            {
                Attack();
                FindNextTarget();  // Tìm kiếm mục tiêu mới sau khi tấn công xong
            }
            else
            {
                Move();
            }
        }
        else
        {
            transform.localScale = initialScale;
            Move();  // Di chuyển về phía mục tiêu nếu không có mục tiêu trong phạm vi
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
        if (target != null)
        {
            float distance = Vector3.Distance(transform.position, target.transform.position);

            // Nếu là nhân vật "Character"
            if (CompareTag("Character"))
            {
                // Kiểm tra nếu target là Base hoặc EnemyBase
                if (target.GetComponent<Base>().CompareTag("EnemyBase"))
                {
                    // Tấn công đối tượng là Base hoặc EnemyBase
                    return distance <= attackRange;
                }
                else if (target.CompareTag("Enemy"))
                {
                    // Tấn công kẻ địch có tag "Enemy"
                    return distance <= attackRange;
                }
            }
            // Nếu là nhân vật "Enemy"
            else if (CompareTag("Enemy"))
            {
                // Kiểm tra nếu target là Base hoặc Base của đối phương
                if (target.GetComponent<Base>().CompareTag("Base"))
                {
                    // Tấn công đối tượng là Base hoặc EnemyBase
                    return distance <= attackRange;
                }
                else if (target.CompareTag("Character"))
                {
                    // Tấn công nhân vật có tag "Character"
                    return distance <= attackRange;
                }
            }
        }

        return false;  // Nếu không phải mục tiêu hợp lệ, trả về false
    }

    protected virtual void Attack()
    {
        if (target != null)
        {
            SetAttackAnimation(true);
            StartCoroutine(AttackRoutine());
        }
        else
        {
            SetAttackAnimation(false);
        }
    }

    private IEnumerator AttackRoutine()
    {
        if (attackCollider != null) attackCollider.enabled = true;
        yield return new WaitForSeconds(0.4f);
        if (attackCollider != null) attackCollider.enabled = false;

        // Dừng hoạt ảnh sau khi tấn công xong
        SetAttackAnimation(false);
    }

    public void DisableAttackCollider()
    {
        if (attackCollider != null) attackCollider.enabled = false;
    }

    protected GameObject FindClosestTarget()
    {
        GameObject[] characters = GameObject.FindGameObjectsWithTag(characterTag);
        GameObject closest = null;
        float minDist = Mathf.Infinity;

        foreach (GameObject c in characters)
        {
            if (c == null) continue;
            float dist = Vector3.Distance(transform.position, c.transform.position);
            if (dist < minDist)
            {
                minDist = dist;
                closest = c;
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
