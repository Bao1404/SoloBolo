using UnityEngine;

public abstract class Character : MonoBehaviour
{
    [SerializeField] protected float walkSpeed = 0.5f;
    [SerializeField] protected float attackRange = 1f;
    [SerializeField] protected float attackDamage = 10f;
    [SerializeField] protected float detectRange = 5f;
    protected Enemy enemy;
    protected Animator animator;
    protected virtual void Start()
    {
        animator = GetComponent<Animator>();
        enemy = FindAnyObjectByType<Enemy>();
    }
    protected virtual void Update()
    {
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
        transform.position += Vector3.right * walkSpeed * Time.deltaTime;
        FlipCharacter();
        if(DetectEnemyInRange() && !AttackEnemyInRange())
        {

        }
    }
    protected bool DetectEnemyInRange()
    {
        return Vector3.Distance(transform.position, enemy.transform.position) <= detectRange;
    }
    protected bool AttackEnemyInRange()
    {
        return Vector3.Distance(transform.position, enemy.transform.position) <= attackRange;
    }
    protected void Attack()
    {
        animator.SetBool("isAttack", true);
    }
    protected void FlipCharacter()
    {
        transform.localScale = new Vector3(enemy.transform.position.x < transform.position.x ? -1 : 1, 1, 1);
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, detectRange);
    }
}