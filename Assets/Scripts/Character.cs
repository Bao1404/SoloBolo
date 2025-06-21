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
    protected GameObject target;
    protected Animator animator;

    [SerializeField] private string characterTag = "Character";  // Default tag

    protected virtual void Start()
    {
        animator = GetComponent<Animator>();
        target = FindClosestTarget();  // Find the closest target (either character or enemy)

        if (attackCollider != null) attackCollider.enabled = false;
        hitBoxCollider.enabled = true;
        currentHp = maxHp;
    }

    protected virtual void Update()
    {
        // If target is null, find a new one
        if (target == null || target.GetComponent<Character>().currentHp <= 0)  // Check if target is dead
        {
            target = FindClosestTarget();  // Find a new target if the current one is dead or null
        }

        UpdateHealthBar();

        // Allow attack if within range
        if (AttackTargetInRange())
        {
            Attack();
        }
        else
        {
            Move();  // Move the character based on the target's position
        }
    }

    protected void Move()
    {
        // Ensure that all characters, regardless of tag, move towards the target
        if (target != null)
        {
            float distance = Vector3.Distance(transform.position, target.transform.position);
            if (distance <= detectRange && distance > attackRange)
            {
                Vector3 direction = (target.transform.position - transform.position).normalized;

                // Move based on tag (Character moves right, Enemy moves left)
                if (CompareTag("Character"))
                {
                    transform.position += Vector3.right * walkSpeed * Time.deltaTime;
                }
                else if (CompareTag("Enemy"))
                {
                    transform.position += Vector3.left * walkSpeed * Time.deltaTime;
                }

                FlipCharacter();  // Flip based on movement direction
                return;
            }
        }

        // Default movement if no target
        if (CompareTag("Character"))
        {
            transform.position += Vector3.right * walkSpeed * Time.deltaTime;
        }
        else if (CompareTag("Enemy"))
        {
            transform.position += Vector3.left * walkSpeed * Time.deltaTime;
        }

        FlipCharacter();  // Ensure correct orientation for the character
    }

    protected void FlipCharacter()
    {
        // Flip the character sprite depending on the tag and direction
        if (CompareTag("Character"))
        {
            transform.localScale = new Vector3(1, 1, 1);  // Flip right for character
        }
        else if (CompareTag("Enemy"))
        {
            transform.localScale = new Vector3(-1, 1, 1);  // Flip left for enemy
        }
    }

    protected virtual void SetAnimation()
    {
        // Placeholder for setting animations (if any)
    }

    // Detect if the target is in range (for movement or attack)
    protected bool DetectTargetInRange()
    {
        return target != null && target.gameObject != null &&
               Vector3.Distance(transform.position, target.transform.position) <= detectRange;
    }

    // Attack logic when in range
    protected bool AttackTargetInRange()
    {
        return target != null && target.gameObject != null &&
               Vector3.Distance(transform.position, target.transform.position) <= attackRange;
    }

    protected virtual void Attack()
    {
        animator.SetBool("isAttack", true);
        StartCoroutine(AttackRoutine());
    }

    private IEnumerator AttackRoutine()
    {
        if (attackCollider != null) attackCollider.enabled = true;
        yield return new WaitForSeconds(0.4f);
        if (attackCollider != null) attackCollider.enabled = false;
    }

    // Disable attack collider
    public void DisableAttackCollider()
    {
        if (attackCollider != null) attackCollider.enabled = false;
    }

    // Find the closest target (Character or Enemy) based on the tag
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

    // Update health bar UI
    protected void UpdateHealthBar()
    {
        if (healthBar != null)
        {
            healthBar.fillAmount = currentHp / maxHp;
        }
    }

    // Method to take damage and reduce health
    public virtual void TakeDamage(float damage)
    {
        currentHp -= damage;
        currentHp = Mathf.Max(currentHp, 0);  // Ensure health doesn't go below 0
        UpdateHealthBar();

        if (currentHp <= 0)
        {
            Die();  // If health is 0 or less, call Die function
        }
    }

    // Method for the character to die
    private void Die()
    {
        Debug.Log($"{gameObject.name} has died!");
        Destroy(gameObject);  // Destroy the character object
    }

    // Collision detection with other objects (such as enemies or characters)
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
        if (attackCollider != null && attackCollider.enabled && CompareTag("Character"))
        {
            if (collision.CompareTag("Enemy"))
            {
                Character enemyTarget = collision.GetComponent<Character>();
                if (enemyTarget != null)
                {
                    enemyTarget.TakeDamage(attackDamage);
                }
            }
        }

        if (attackCollider != null && attackCollider.enabled && CompareTag("Enemy"))
        {
            if (collision.CompareTag("Character"))
            {
                Character characterTarget = collision.GetComponent<Character>();
                if (characterTarget != null)
                {
                    characterTarget.TakeDamage(attackDamage);
                }
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
