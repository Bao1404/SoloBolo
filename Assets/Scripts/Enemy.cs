using System.Collections;
using UnityEngine;
using UnityEngine.TextCore.Text;
using UnityEngine.UI;

public abstract class Enemy : MonoBehaviour
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
    protected Character character;
    protected Animator animator;
    protected virtual void Start()
    {
        animator = GetComponent<Animator>();
        character = FindClosestCharacter();
        attackCollider.enabled = false;
        hitBoxCollider.enabled = true;
        currentHp = maxHp;
    }
    protected virtual void Update()
    {
        if (character == null || character.gameObject == null)
        {
            character = FindClosestCharacter();
        }
        UpdateHealthBar();
        if (AttackCharacterInRange())
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

        if (character != null)
        {
            float distance = Vector3.Distance(transform.position, character.transform.position);
            if (distance <= detectRange && distance > attackRange)
            {
                Vector3 direction = (character.transform.position - transform.position).normalized;
                transform.position += direction * walkSpeed * Time.deltaTime;
                FlipEnemy();
                return;
            }
        }
        transform.position += Vector3.left * walkSpeed * Time.deltaTime;
        FlipEnemy();
    }
    protected bool DetectCharacterInRange()
    {
        return character != null && character.gameObject != null &&
               Vector3.Distance(transform.position, character.transform.position) <= detectRange;
    }
    protected bool AttackCharacterInRange()
    {
        return character != null && character.gameObject != null &&
               Vector3.Distance(transform.position, character.transform.position) <= attackRange;
    }
    protected Character FindClosestCharacter()
    {
        Character[] characters = FindObjectsByType<Character>(FindObjectsSortMode.None);
        Character closest = null;
        float minDist = Mathf.Infinity;

        foreach (Character e in characters)
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
    protected void FlipEnemy()
    {
        if (character != null && DetectCharacterInRange())
        {
            float direction = character.transform.position.x - transform.position.x;
            transform.localScale = new Vector3(direction < 0 ? -1 : 1, 1, 1);
        }
        else
        {
            transform.localScale = new Vector3(-1, 1, 1);
        }
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