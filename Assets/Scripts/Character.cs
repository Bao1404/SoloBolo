using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public abstract class Character : MonoBehaviour
{
    [SerializeField] protected float walkSpeed = 0.5f;
    [SerializeField] protected float attackRange = 1f;
    [SerializeField] protected float attackDamage = 10f;
    [SerializeField] protected float detectRange = 5f;
    [SerializeField] protected float maxHp = 100f;
    protected float currentHp;
    [SerializeField] protected Image healthBar;
    [SerializeField] protected CapsuleCollider2D attackCollider;
    [SerializeField] protected BoxCollider2D hitBoxCollider;
    protected Enemy enemy;
    protected Animator animator;
    protected virtual void Start()
    {
        animator = GetComponent<Animator>();
        enemy = FindClosestEnemy();
        attackCollider.enabled = false;
        hitBoxCollider.enabled = true;
        currentHp = maxHp;
    }
    protected virtual void Update()
    {
        if (enemy == null || enemy.gameObject == null)
        {
            enemy = FindClosestEnemy();
        }

        UpdateHealthBar();

        if (AttackEnemyInRange())
        {
            Attack();
        }
        else
        {
            Move();
        }
    }
    protected void Move()
    {
        animator.SetBool("isAttack", false);

        if (enemy != null)
        {
            float distance = Vector3.Distance(transform.position, enemy.transform.position);
            if (distance <= detectRange && distance > attackRange)
            {
                Vector3 direction = (enemy.transform.position - transform.position).normalized;
                transform.position += direction * walkSpeed * Time.deltaTime;
                FlipCharacter();
                return;
            }
        }
        transform.position += Vector3.right * walkSpeed * Time.deltaTime;
        FlipCharacter();
    }
    protected bool DetectEnemyInRange()
    {
        return enemy != null && enemy.gameObject != null &&
               Vector3.Distance(transform.position, enemy.transform.position) <= detectRange;
    }
    protected bool AttackEnemyInRange()
    {
        return enemy != null && enemy.gameObject != null &&
               Vector3.Distance(transform.position, enemy.transform.position) <= attackRange;
    }
    protected void Attack()
    {
        animator.SetBool("isAttack", true);
        StartCoroutine(AttackRoutine());
    }
    private IEnumerator AttackRoutine()
    {
        attackCollider.enabled = true;
        yield return new WaitForSeconds(0.4f);
        attackCollider.enabled = false;
    }
    public void DisableAttackCollider()
    {
        attackCollider.enabled = false;
    }
    protected void FlipCharacter()
    {
        if (enemy != null && DetectEnemyInRange())
        {
            float direction = enemy.transform.position.x - transform.position.x;
            transform.localScale = new Vector3(direction < 0 ? -1 : 1, 1, 1);
        }
        else
        {
            transform.localScale = new Vector3(1, 1, 1);
        }
    }
    protected Enemy FindClosestEnemy()
    {
        Enemy[] enemies = FindObjectsByType<Enemy>(FindObjectsSortMode.None);
        Enemy closest = null;
        float minDist = Mathf.Infinity;

        foreach (Enemy e in enemies)
        {
            if (e == null) continue;
            float dist = Vector3.Distance(transform.position, e.transform.position);
            if (dist < minDist)
            {
                minDist = dist;
                closest = e;
            }
        }

        return closest;
    }
    protected void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, detectRange);
    }
    public void TakeDame(float damage)
    {
        currentHp -= damage;
        currentHp = Mathf.Max(currentHp, 0);
        UpdateHealthBar();
        if (currentHp <= 0)
        {
            Die();
        }
    }
    private void Die()
    {
        Destroy(gameObject);
    }
    protected void UpdateHealthBar()
    {
        if (healthBar != null)
        {
            healthBar.fillAmount = currentHp / maxHp;
        }
    }
}