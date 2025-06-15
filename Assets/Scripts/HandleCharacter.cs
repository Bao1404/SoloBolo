using UnityEngine;

public class CharacterMoverment : MonoBehaviour
{
    [SerializeField] private float walkSpeed = 0.5f;
    [SerializeField] private Transform attackRange;
    [SerializeField] private float attackRangeRadius = 0.2f;
    private Animator animator;
    [SerializeField] private LayerMask enemyLayer;
    private bool isInRange;

    void Start()
    {
       animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        isInRange = CheckIfInRange();
        if (isInRange)
        {
            AttackEnemy();
        }
        else
        {
            Mover();
        }
    }
    private void Mover()
    {
        transform.position += Vector3.right * walkSpeed * Time.deltaTime;
    }
    private void AttackEnemy()
    {
        if (isInRange)
        {
            animator.SetBool("isAttack", true);
        }
        else
        {
            animator.SetBool("isAttack", false);
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (attackRange != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(attackRange.position, attackRangeRadius);
        }
    }
    private bool CheckIfInRange()
    {
        return Physics2D.OverlapCircle(attackRange.position, attackRangeRadius, enemyLayer);
    }
}
